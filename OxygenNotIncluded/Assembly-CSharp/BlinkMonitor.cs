using System;
using UnityEngine;

// Token: 0x02000869 RID: 2153
public class BlinkMonitor : GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>
{
	// Token: 0x06003F03 RID: 16131 RVA: 0x0015F914 File Offset: 0x0015DB14
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.Enter(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.CreateEyes)).Exit(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.DestroyEyes));
		this.satisfied.ScheduleGoTo(new Func<BlinkMonitor.Instance, float>(BlinkMonitor.GetRandomBlinkTime), this.blinking);
		this.blinking.EnterTransition(this.satisfied, GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.Not(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.Transition.ConditionCallback(BlinkMonitor.CanBlink))).Enter(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.BeginBlinking)).Update(new Action<BlinkMonitor.Instance, float>(BlinkMonitor.UpdateBlinking), UpdateRate.RENDER_EVERY_TICK, false).Target(this.eyes).OnAnimQueueComplete(this.satisfied).Exit(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.EndBlinking));
	}

	// Token: 0x06003F04 RID: 16132 RVA: 0x0015F9DE File Offset: 0x0015DBDE
	private static bool CanBlink(BlinkMonitor.Instance smi)
	{
		return SpeechMonitor.IsAllowedToPlaySpeech(smi.gameObject) && smi.Get<Navigator>().CurrentNavType != NavType.Ladder;
	}

	// Token: 0x06003F05 RID: 16133 RVA: 0x0015FA00 File Offset: 0x0015DC00
	private static float GetRandomBlinkTime(BlinkMonitor.Instance smi)
	{
		return UnityEngine.Random.Range(TuningData<BlinkMonitor.Tuning>.Get().randomBlinkIntervalMin, TuningData<BlinkMonitor.Tuning>.Get().randomBlinkIntervalMax);
	}

	// Token: 0x06003F06 RID: 16134 RVA: 0x0015FA1C File Offset: 0x0015DC1C
	private static void CreateEyes(BlinkMonitor.Instance smi)
	{
		smi.eyes = Util.KInstantiate(Assets.GetPrefab(EyeAnimation.ID), null, null).GetComponent<KBatchedAnimController>();
		smi.eyes.gameObject.SetActive(true);
		smi.sm.eyes.Set(smi.eyes.gameObject, smi, false);
	}

	// Token: 0x06003F07 RID: 16135 RVA: 0x0015FA79 File Offset: 0x0015DC79
	private static void DestroyEyes(BlinkMonitor.Instance smi)
	{
		if (smi.eyes != null)
		{
			Util.KDestroyGameObject(smi.eyes);
			smi.eyes = null;
		}
	}

	// Token: 0x06003F08 RID: 16136 RVA: 0x0015FA9C File Offset: 0x0015DC9C
	public static void BeginBlinking(BlinkMonitor.Instance smi)
	{
		string s = "eyes1";
		smi.eyes.Play(s, KAnim.PlayMode.Once, 1f, 0f);
		BlinkMonitor.UpdateBlinking(smi, 0f);
	}

	// Token: 0x06003F09 RID: 16137 RVA: 0x0015FAD6 File Offset: 0x0015DCD6
	public static void EndBlinking(BlinkMonitor.Instance smi)
	{
		smi.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(BlinkMonitor.HASH_SNAPTO_EYES, 3);
	}

	// Token: 0x06003F0A RID: 16138 RVA: 0x0015FAEC File Offset: 0x0015DCEC
	public static void UpdateBlinking(BlinkMonitor.Instance smi, float dt)
	{
		int currentFrameIndex = smi.eyes.GetCurrentFrameIndex();
		KAnimBatch batch = smi.eyes.GetBatch();
		if (currentFrameIndex == -1 || batch == null)
		{
			return;
		}
		KAnim.Anim.Frame frame;
		if (!smi.eyes.GetBatch().group.data.TryGetFrame(currentFrameIndex, out frame))
		{
			return;
		}
		HashedString hash = HashedString.Invalid;
		for (int i = 0; i < frame.numElements; i++)
		{
			int num = frame.firstElementIdx + i;
			if (num < batch.group.data.frameElements.Count)
			{
				KAnim.Anim.FrameElement frameElement = batch.group.data.frameElements[num];
				if (!(frameElement.symbol == HashedString.Invalid))
				{
					hash = frameElement.symbol;
					break;
				}
			}
		}
		smi.GetComponent<SymbolOverrideController>().AddSymbolOverride(BlinkMonitor.HASH_SNAPTO_EYES, smi.eyes.AnimFiles[0].GetData().build.GetSymbol(hash), 3);
	}

	// Token: 0x040028CE RID: 10446
	public GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State satisfied;

	// Token: 0x040028CF RID: 10447
	public GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State blinking;

	// Token: 0x040028D0 RID: 10448
	public StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.TargetParameter eyes;

	// Token: 0x040028D1 RID: 10449
	private static HashedString HASH_SNAPTO_EYES = "snapto_eyes";

	// Token: 0x02001646 RID: 5702
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001647 RID: 5703
	public class Tuning : TuningData<BlinkMonitor.Tuning>
	{
		// Token: 0x04006B2D RID: 27437
		public float randomBlinkIntervalMin;

		// Token: 0x04006B2E RID: 27438
		public float randomBlinkIntervalMax;
	}

	// Token: 0x02001648 RID: 5704
	public new class Instance : GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.GameInstance
	{
		// Token: 0x06008A31 RID: 35377 RVA: 0x0031359D File Offset: 0x0031179D
		public Instance(IStateMachineTarget master, BlinkMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06008A32 RID: 35378 RVA: 0x003135A7 File Offset: 0x003117A7
		public bool IsBlinking()
		{
			return base.IsInsideState(base.sm.blinking);
		}

		// Token: 0x06008A33 RID: 35379 RVA: 0x003135BA File Offset: 0x003117BA
		public void Blink()
		{
			this.GoTo(base.sm.blinking);
		}

		// Token: 0x04006B2F RID: 27439
		public KBatchedAnimController eyes;
	}
}
