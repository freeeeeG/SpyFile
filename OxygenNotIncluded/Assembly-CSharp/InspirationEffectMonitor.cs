using System;
using UnityEngine;

// Token: 0x02000882 RID: 2178
public class InspirationEffectMonitor : GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>
{
	// Token: 0x06003F6A RID: 16234 RVA: 0x0016255C File Offset: 0x0016075C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.EventHandler(GameHashes.CatchyTune, new GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.GameEvent.Callback(this.OnCatchyTune)).ParamTransition<bool>(this.shouldCatchyTune, this.catchyTune, (InspirationEffectMonitor.Instance smi, bool shouldCatchyTune) => shouldCatchyTune);
		this.catchyTune.Exit(delegate(InspirationEffectMonitor.Instance smi)
		{
			this.shouldCatchyTune.Set(false, smi, false);
		}).ToggleEffect("HeardJoySinger").ToggleThought(Db.Get().Thoughts.CatchyTune, null).EventHandler(GameHashes.StartWork, new GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.GameEvent.Callback(this.TryThinkCatchyTune)).ToggleStatusItem(Db.Get().DuplicantStatusItems.JoyResponse_HeardJoySinger, null).Enter(delegate(InspirationEffectMonitor.Instance smi)
		{
			this.SingCatchyTune(smi);
		}).Update(delegate(InspirationEffectMonitor.Instance smi, float dt)
		{
			this.TryThinkCatchyTune(smi, null);
			this.inspirationTimeRemaining.Delta(-dt, smi);
		}, UpdateRate.SIM_4000ms, false).ParamTransition<float>(this.inspirationTimeRemaining, this.idle, (InspirationEffectMonitor.Instance smi, float p) => p <= 0f);
	}

	// Token: 0x06003F6B RID: 16235 RVA: 0x0016267B File Offset: 0x0016087B
	private void OnCatchyTune(InspirationEffectMonitor.Instance smi, object data)
	{
		this.inspirationTimeRemaining.Set(600f, smi, false);
		this.shouldCatchyTune.Set(true, smi, false);
	}

	// Token: 0x06003F6C RID: 16236 RVA: 0x0016269F File Offset: 0x0016089F
	private void TryThinkCatchyTune(InspirationEffectMonitor.Instance smi, object data)
	{
		if (UnityEngine.Random.Range(1, 101) > 66)
		{
			this.SingCatchyTune(smi);
		}
	}

	// Token: 0x06003F6D RID: 16237 RVA: 0x001626B4 File Offset: 0x001608B4
	private void SingCatchyTune(InspirationEffectMonitor.Instance smi)
	{
		smi.master.gameObject.GetSMI<ThoughtGraph.Instance>().AddThought(Db.Get().Thoughts.CatchyTune);
		if (!smi.GetSpeechMonitor().IsPlayingSpeech() && SpeechMonitor.IsAllowedToPlaySpeech(smi.gameObject))
		{
			smi.GetSpeechMonitor().PlaySpeech(Db.Get().Thoughts.CatchyTune.speechPrefix, Db.Get().Thoughts.CatchyTune.sound);
		}
	}

	// Token: 0x0400292F RID: 10543
	public StateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.BoolParameter shouldCatchyTune;

	// Token: 0x04002930 RID: 10544
	public StateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.FloatParameter inspirationTimeRemaining;

	// Token: 0x04002931 RID: 10545
	public GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.State idle;

	// Token: 0x04002932 RID: 10546
	public GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.State catchyTune;

	// Token: 0x02001687 RID: 5767
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001688 RID: 5768
	public new class Instance : GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.GameInstance
	{
		// Token: 0x06008B47 RID: 35655 RVA: 0x0031629C File Offset: 0x0031449C
		public Instance(IStateMachineTarget master, InspirationEffectMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06008B48 RID: 35656 RVA: 0x003162A6 File Offset: 0x003144A6
		public SpeechMonitor.Instance GetSpeechMonitor()
		{
			if (this.speechMonitor == null)
			{
				this.speechMonitor = base.master.gameObject.GetSMI<SpeechMonitor.Instance>();
			}
			return this.speechMonitor;
		}

		// Token: 0x04006C08 RID: 27656
		public SpeechMonitor.Instance speechMonitor;
	}
}
