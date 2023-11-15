using System;

// Token: 0x020007D9 RID: 2009
public class SafeCellMonitor : GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance>
{
	// Token: 0x060038AA RID: 14506 RVA: 0x0013B154 File Offset: 0x00139354
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Never;
		this.root.ToggleUrge(Db.Get().Urges.MoveToSafety);
		this.satisfied.EventTransition(GameHashes.SafeCellDetected, this.danger, (SafeCellMonitor.Instance smi) => smi.IsAreaUnsafe());
		this.danger.EventTransition(GameHashes.SafeCellLost, this.satisfied, (SafeCellMonitor.Instance smi) => !smi.IsAreaUnsafe()).ToggleChore((SafeCellMonitor.Instance smi) => new MoveToSafetyChore(smi.master), this.satisfied);
	}

	// Token: 0x04002592 RID: 9618
	public GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002593 RID: 9619
	public GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, object>.State danger;

	// Token: 0x02001588 RID: 5512
	public new class Instance : GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060087EC RID: 34796 RVA: 0x0030CDD0 File Offset: 0x0030AFD0
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.safeCellSensor = base.GetComponent<Sensors>().GetSensor<SafeCellSensor>();
		}

		// Token: 0x060087ED RID: 34797 RVA: 0x0030CDEA File Offset: 0x0030AFEA
		public bool IsAreaUnsafe()
		{
			return this.safeCellSensor.HasSafeCell();
		}

		// Token: 0x040068E1 RID: 26849
		private SafeCellSensor safeCellSensor;
	}
}
