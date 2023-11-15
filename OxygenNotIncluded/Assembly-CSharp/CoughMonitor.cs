using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x0200086E RID: 2158
public class CoughMonitor : GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>
{
	// Token: 0x06003F20 RID: 16160 RVA: 0x001603CC File Offset: 0x0015E5CC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.EventHandler(GameHashes.PoorAirQuality, new GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.GameEvent.Callback(this.OnBreatheDirtyAir)).ParamTransition<bool>(this.shouldCough, this.coughing, (CoughMonitor.Instance smi, bool bShouldCough) => bShouldCough);
		this.coughing.ToggleStatusItem(Db.Get().DuplicantStatusItems.Coughing, null).ToggleReactable((CoughMonitor.Instance smi) => smi.GetReactable()).ParamTransition<bool>(this.shouldCough, this.idle, (CoughMonitor.Instance smi, bool bShouldCough) => !bShouldCough);
	}

	// Token: 0x06003F21 RID: 16161 RVA: 0x001604A8 File Offset: 0x0015E6A8
	private void OnBreatheDirtyAir(CoughMonitor.Instance smi, object data)
	{
		float timeInCycles = GameClock.Instance.GetTimeInCycles();
		if (timeInCycles > 0.1f && timeInCycles - smi.lastCoughTime <= 0.1f)
		{
			return;
		}
		Sim.MassConsumedCallback massConsumedCallback = (Sim.MassConsumedCallback)data;
		float num = (smi.lastConsumeTime <= 0f) ? 0f : (timeInCycles - smi.lastConsumeTime);
		smi.lastConsumeTime = timeInCycles;
		smi.amountConsumed -= 0.05f * num;
		smi.amountConsumed = Mathf.Max(smi.amountConsumed, 0f);
		smi.amountConsumed += massConsumedCallback.mass;
		if (smi.amountConsumed >= 1f)
		{
			this.shouldCough.Set(true, smi, false);
			smi.lastConsumeTime = 0f;
			smi.amountConsumed = 0f;
		}
	}

	// Token: 0x040028E1 RID: 10465
	private const float amountToCough = 1f;

	// Token: 0x040028E2 RID: 10466
	private const float decayRate = 0.05f;

	// Token: 0x040028E3 RID: 10467
	private const float coughInterval = 0.1f;

	// Token: 0x040028E4 RID: 10468
	public GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.State idle;

	// Token: 0x040028E5 RID: 10469
	public GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.State coughing;

	// Token: 0x040028E6 RID: 10470
	public StateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.BoolParameter shouldCough = new StateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.BoolParameter(false);

	// Token: 0x02001654 RID: 5716
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001655 RID: 5717
	public new class Instance : GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.GameInstance
	{
		// Token: 0x06008A63 RID: 35427 RVA: 0x00313B3A File Offset: 0x00311D3A
		public Instance(IStateMachineTarget master, CoughMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06008A64 RID: 35428 RVA: 0x00313B44 File Offset: 0x00311D44
		public Reactable GetReactable()
		{
			Emote cough_Small = Db.Get().Emotes.Minion.Cough_Small;
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "BadAirCough", Db.Get().ChoreTypes.Cough, 0f, 0f, float.PositiveInfinity, 0f);
			selfEmoteReactable.SetEmote(cough_Small);
			selfEmoteReactable.preventChoreInterruption = true;
			return selfEmoteReactable.RegisterEmoteStepCallbacks("react_small", null, new Action<GameObject>(this.FinishedCoughing));
		}

		// Token: 0x06008A65 RID: 35429 RVA: 0x00313BD0 File Offset: 0x00311DD0
		private void FinishedCoughing(GameObject cougher)
		{
			cougher.GetComponent<Effects>().Add("ContaminatedLungs", true);
			base.sm.shouldCough.Set(false, base.smi, false);
			base.smi.lastCoughTime = GameClock.Instance.GetTimeInCycles();
		}

		// Token: 0x04006B51 RID: 27473
		[Serialize]
		public float lastCoughTime;

		// Token: 0x04006B52 RID: 27474
		[Serialize]
		public float lastConsumeTime;

		// Token: 0x04006B53 RID: 27475
		[Serialize]
		public float amountConsumed;
	}
}
