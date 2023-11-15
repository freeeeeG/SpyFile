using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000895 RID: 2197
public class SpeechMonitor : GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>
{
	// Token: 0x06003FD2 RID: 16338 RVA: 0x00164F3C File Offset: 0x0016313C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.Enter(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.CreateMouth)).Exit(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.DestroyMouth));
		this.satisfied.DoNothing();
		this.talking.Enter(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.BeginTalking)).Update(new Action<SpeechMonitor.Instance, float>(SpeechMonitor.UpdateTalking), UpdateRate.RENDER_EVERY_TICK, false).Target(this.mouth).OnAnimQueueComplete(this.satisfied).Exit(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.EndTalking));
	}

	// Token: 0x06003FD3 RID: 16339 RVA: 0x00164FD8 File Offset: 0x001631D8
	private static void CreateMouth(SpeechMonitor.Instance smi)
	{
		smi.mouth = global::Util.KInstantiate(Assets.GetPrefab(MouthAnimation.ID), null, null).GetComponent<KBatchedAnimController>();
		smi.mouth.gameObject.SetActive(true);
		smi.sm.mouth.Set(smi.mouth.gameObject, smi, false);
	}

	// Token: 0x06003FD4 RID: 16340 RVA: 0x00165035 File Offset: 0x00163235
	private static void DestroyMouth(SpeechMonitor.Instance smi)
	{
		if (smi.mouth != null)
		{
			global::Util.KDestroyGameObject(smi.mouth);
			smi.mouth = null;
		}
	}

	// Token: 0x06003FD5 RID: 16341 RVA: 0x00165058 File Offset: 0x00163258
	private static string GetRandomSpeechAnim(string speech_prefix)
	{
		return speech_prefix + UnityEngine.Random.Range(1, TuningData<SpeechMonitor.Tuning>.Get().speechCount).ToString();
	}

	// Token: 0x06003FD6 RID: 16342 RVA: 0x00165084 File Offset: 0x00163284
	public static bool IsAllowedToPlaySpeech(GameObject go)
	{
		if (go.HasTag(GameTags.Dead))
		{
			return false;
		}
		KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
		KAnim.Anim currentAnim = component.GetCurrentAnim();
		return currentAnim == null || (GameAudioSheets.Get().IsAnimAllowedToPlaySpeech(currentAnim) && SpeechMonitor.CanOverrideHead(component));
	}

	// Token: 0x06003FD7 RID: 16343 RVA: 0x001650C8 File Offset: 0x001632C8
	private static bool CanOverrideHead(KBatchedAnimController kbac)
	{
		bool result = true;
		KAnim.Anim currentAnim = kbac.GetCurrentAnim();
		if (currentAnim == null)
		{
			return false;
		}
		int currentFrameIndex = kbac.GetCurrentFrameIndex();
		if (currentFrameIndex <= 0)
		{
			return false;
		}
		KAnim.Anim.Frame frame;
		if (KAnimBatchManager.Instance().GetBatchGroupData(currentAnim.animFile.animBatchTag).TryGetFrame(currentFrameIndex, out frame) && frame.hasHead)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06003FD8 RID: 16344 RVA: 0x0016511C File Offset: 0x0016331C
	public static void BeginTalking(SpeechMonitor.Instance smi)
	{
		smi.ev.clearHandle();
		if (smi.voiceEvent != null)
		{
			smi.ev = VoiceSoundEvent.PlayVoice(smi.voiceEvent, smi.GetComponent<KBatchedAnimController>(), 0f, false, false);
		}
		if (smi.ev.isValid())
		{
			smi.mouth.Play(SpeechMonitor.GetRandomSpeechAnim(smi.speechPrefix), KAnim.PlayMode.Once, 1f, 0f);
			smi.mouth.Queue(SpeechMonitor.GetRandomSpeechAnim(smi.speechPrefix), KAnim.PlayMode.Once, 1f, 0f);
			smi.mouth.Queue(SpeechMonitor.GetRandomSpeechAnim(smi.speechPrefix), KAnim.PlayMode.Once, 1f, 0f);
			smi.mouth.Queue(SpeechMonitor.GetRandomSpeechAnim(smi.speechPrefix), KAnim.PlayMode.Once, 1f, 0f);
		}
		else
		{
			smi.mouth.Play(SpeechMonitor.GetRandomSpeechAnim(smi.speechPrefix), KAnim.PlayMode.Once, 1f, 0f);
			smi.mouth.Queue(SpeechMonitor.GetRandomSpeechAnim(smi.speechPrefix), KAnim.PlayMode.Once, 1f, 0f);
		}
		SpeechMonitor.UpdateTalking(smi, 0f);
	}

	// Token: 0x06003FD9 RID: 16345 RVA: 0x0016525B File Offset: 0x0016345B
	public static void EndTalking(SpeechMonitor.Instance smi)
	{
		smi.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, 3);
	}

	// Token: 0x06003FDA RID: 16346 RVA: 0x00165270 File Offset: 0x00163470
	public static KAnim.Anim.FrameElement GetFirstFrameElement(KBatchedAnimController controller)
	{
		KAnim.Anim.FrameElement result = default(KAnim.Anim.FrameElement);
		result.symbol = HashedString.Invalid;
		int currentFrameIndex = controller.GetCurrentFrameIndex();
		KAnimBatch batch = controller.GetBatch();
		if (currentFrameIndex == -1 || batch == null)
		{
			return result;
		}
		KAnim.Anim.Frame frame;
		if (!controller.GetBatch().group.data.TryGetFrame(currentFrameIndex, out frame))
		{
			return result;
		}
		for (int i = 0; i < frame.numElements; i++)
		{
			int num = frame.firstElementIdx + i;
			if (num < batch.group.data.frameElements.Count)
			{
				KAnim.Anim.FrameElement frameElement = batch.group.data.frameElements[num];
				if (!(frameElement.symbol == HashedString.Invalid))
				{
					result = frameElement;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06003FDB RID: 16347 RVA: 0x00165334 File Offset: 0x00163534
	public static void UpdateTalking(SpeechMonitor.Instance smi, float dt)
	{
		if (smi.ev.isValid())
		{
			PLAYBACK_STATE playback_STATE;
			smi.ev.getPlaybackState(out playback_STATE);
			if (playback_STATE == PLAYBACK_STATE.STOPPING || playback_STATE == PLAYBACK_STATE.STOPPED)
			{
				smi.GoTo(smi.sm.satisfied);
				smi.ev.clearHandle();
				return;
			}
		}
		KAnim.Anim.FrameElement firstFrameElement = SpeechMonitor.GetFirstFrameElement(smi.mouth);
		if (firstFrameElement.symbol == HashedString.Invalid)
		{
			return;
		}
		smi.Get<SymbolOverrideController>().AddSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, smi.mouth.AnimFiles[0].GetData().build.GetSymbol(firstFrameElement.symbol), 3);
	}

	// Token: 0x04002988 RID: 10632
	public GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State satisfied;

	// Token: 0x04002989 RID: 10633
	public GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State talking;

	// Token: 0x0400298A RID: 10634
	public static string PREFIX_SAD = "sad";

	// Token: 0x0400298B RID: 10635
	public static string PREFIX_HAPPY = "happy";

	// Token: 0x0400298C RID: 10636
	public static string PREFIX_SINGER = "sing";

	// Token: 0x0400298D RID: 10637
	public StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.TargetParameter mouth;

	// Token: 0x0400298E RID: 10638
	private static HashedString HASH_SNAPTO_MOUTH = "snapto_mouth";

	// Token: 0x020016B3 RID: 5811
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020016B4 RID: 5812
	public class Tuning : TuningData<SpeechMonitor.Tuning>
	{
		// Token: 0x04006C8B RID: 27787
		public float randomSpeechIntervalMin;

		// Token: 0x04006C8C RID: 27788
		public float randomSpeechIntervalMax;

		// Token: 0x04006C8D RID: 27789
		public int speechCount;
	}

	// Token: 0x020016B5 RID: 5813
	public new class Instance : GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.GameInstance
	{
		// Token: 0x06008C00 RID: 35840 RVA: 0x003178ED File Offset: 0x00315AED
		public Instance(IStateMachineTarget master, SpeechMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06008C01 RID: 35841 RVA: 0x00317902 File Offset: 0x00315B02
		public bool IsPlayingSpeech()
		{
			return base.IsInsideState(base.sm.talking);
		}

		// Token: 0x06008C02 RID: 35842 RVA: 0x00317915 File Offset: 0x00315B15
		public void PlaySpeech(string speech_prefix, string voice_event)
		{
			this.speechPrefix = speech_prefix;
			this.voiceEvent = voice_event;
			this.GoTo(base.sm.talking);
		}

		// Token: 0x06008C03 RID: 35843 RVA: 0x00317938 File Offset: 0x00315B38
		public void DrawMouth()
		{
			KAnim.Anim.FrameElement firstFrameElement = SpeechMonitor.GetFirstFrameElement(base.smi.mouth);
			if (firstFrameElement.symbol == HashedString.Invalid)
			{
				return;
			}
			KAnim.Build.Symbol symbol = base.smi.mouth.AnimFiles[0].GetData().build.GetSymbol(firstFrameElement.symbol);
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			base.GetComponent<SymbolOverrideController>().AddSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, base.smi.mouth.AnimFiles[0].GetData().build.GetSymbol(firstFrameElement.symbol), 3);
			KAnim.Build.Symbol symbol2 = KAnimBatchManager.Instance().GetBatchGroupData(component.batchGroupID).GetSymbol(SpeechMonitor.HASH_SNAPTO_MOUTH);
			KAnim.Build.SymbolFrameInstance symbolFrameInstance = KAnimBatchManager.Instance().GetBatchGroupData(symbol.build.batchTag).symbolFrameInstances[symbol.firstFrameIdx + firstFrameElement.frame];
			symbolFrameInstance.buildImageIdx = base.GetComponent<SymbolOverrideController>().GetAtlasIdx(symbol.build.GetTexture(0));
			component.SetSymbolOverride(symbol2.firstFrameIdx, ref symbolFrameInstance);
		}

		// Token: 0x04006C8E RID: 27790
		public KBatchedAnimController mouth;

		// Token: 0x04006C8F RID: 27791
		public string speechPrefix = "happy";

		// Token: 0x04006C90 RID: 27792
		public string voiceEvent;

		// Token: 0x04006C91 RID: 27793
		public EventInstance ev;
	}
}
