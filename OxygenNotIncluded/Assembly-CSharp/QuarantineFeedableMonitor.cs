using System;

// Token: 0x0200088B RID: 2187
public class QuarantineFeedableMonitor : GameStateMachine<QuarantineFeedableMonitor, QuarantineFeedableMonitor.Instance>
{
	// Token: 0x06003FA8 RID: 16296 RVA: 0x00163768 File Offset: 0x00161968
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.satisfied.EventTransition(GameHashes.AddUrge, this.hungry, (QuarantineFeedableMonitor.Instance smi) => smi.IsHungry());
		this.hungry.EventTransition(GameHashes.RemoveUrge, this.satisfied, (QuarantineFeedableMonitor.Instance smi) => !smi.IsHungry());
	}

	// Token: 0x0400294F RID: 10575
	public GameStateMachine<QuarantineFeedableMonitor, QuarantineFeedableMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002950 RID: 10576
	public GameStateMachine<QuarantineFeedableMonitor, QuarantineFeedableMonitor.Instance, IStateMachineTarget, object>.State hungry;

	// Token: 0x0200169C RID: 5788
	public new class Instance : GameStateMachine<QuarantineFeedableMonitor, QuarantineFeedableMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008B8F RID: 35727 RVA: 0x00316B18 File Offset: 0x00314D18
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06008B90 RID: 35728 RVA: 0x00316B21 File Offset: 0x00314D21
		public bool IsHungry()
		{
			return base.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.Eat);
		}
	}
}
