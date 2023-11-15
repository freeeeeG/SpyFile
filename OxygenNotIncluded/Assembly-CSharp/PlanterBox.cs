using System;

// Token: 0x02000671 RID: 1649
[SkipSaveFileSerialization]
public class PlanterBox : StateMachineComponent<PlanterBox.SMInstance>
{
	// Token: 0x06002BAF RID: 11183 RVA: 0x000E85B1 File Offset: 0x000E67B1
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x040019A1 RID: 6561
	[MyCmpReq]
	private PlantablePlot plantablePlot;

	// Token: 0x02001368 RID: 4968
	public class SMInstance : GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.GameInstance
	{
		// Token: 0x060080EF RID: 33007 RVA: 0x002F2E7A File Offset: 0x002F107A
		public SMInstance(PlanterBox master) : base(master)
		{
		}
	}

	// Token: 0x02001369 RID: 4969
	public class States : GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox>
	{
		// Token: 0x060080F0 RID: 33008 RVA: 0x002F2E84 File Offset: 0x002F1084
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			this.empty.EventTransition(GameHashes.OccupantChanged, this.full, (PlanterBox.SMInstance smi) => smi.master.plantablePlot.Occupant != null).PlayAnim("off");
			this.full.EventTransition(GameHashes.OccupantChanged, this.empty, (PlanterBox.SMInstance smi) => smi.master.plantablePlot.Occupant == null).PlayAnim("on");
		}

		// Token: 0x0400625B RID: 25179
		public GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.State empty;

		// Token: 0x0400625C RID: 25180
		public GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.State full;
	}
}
