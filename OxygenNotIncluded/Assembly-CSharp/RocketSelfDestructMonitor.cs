using System;

// Token: 0x02000734 RID: 1844
public class RocketSelfDestructMonitor : GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance>
{
	// Token: 0x060032AD RID: 12973 RVA: 0x0010D208 File Offset: 0x0010B408
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.EventTransition(GameHashes.RocketSelfDestructRequested, this.exploding, null);
		this.exploding.Update(delegate(RocketSelfDestructMonitor.Instance smi, float dt)
		{
			if (smi.timeinstate >= 3f)
			{
				smi.master.Trigger(-1311384361, null);
				smi.GoTo(this.idle);
			}
		}, UpdateRate.SIM_200ms, false);
	}

	// Token: 0x04001E6D RID: 7789
	public GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x04001E6E RID: 7790
	public GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance, IStateMachineTarget, object>.State exploding;

	// Token: 0x020014C5 RID: 5317
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020014C6 RID: 5318
	public new class Instance : GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060085D5 RID: 34261 RVA: 0x0030759A File Offset: 0x0030579A
		public Instance(IStateMachineTarget master, RocketSelfDestructMonitor.Def def) : base(master)
		{
		}

		// Token: 0x04006660 RID: 26208
		public KBatchedAnimController eyes;
	}
}
