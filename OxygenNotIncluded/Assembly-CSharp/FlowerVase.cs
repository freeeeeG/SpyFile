using System;

// Token: 0x02000602 RID: 1538
public class FlowerVase : StateMachineComponent<FlowerVase.SMInstance>
{
	// Token: 0x06002699 RID: 9881 RVA: 0x000D1A81 File Offset: 0x000CFC81
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600269A RID: 9882 RVA: 0x000D1A89 File Offset: 0x000CFC89
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x0400161E RID: 5662
	[MyCmpReq]
	private PlantablePlot plantablePlot;

	// Token: 0x0400161F RID: 5663
	[MyCmpReq]
	private KBoxCollider2D boxCollider;

	// Token: 0x020012AB RID: 4779
	public class SMInstance : GameStateMachine<FlowerVase.States, FlowerVase.SMInstance, FlowerVase, object>.GameInstance
	{
		// Token: 0x06007E1D RID: 32285 RVA: 0x002E6CFA File Offset: 0x002E4EFA
		public SMInstance(FlowerVase master) : base(master)
		{
		}
	}

	// Token: 0x020012AC RID: 4780
	public class States : GameStateMachine<FlowerVase.States, FlowerVase.SMInstance, FlowerVase>
	{
		// Token: 0x06007E1E RID: 32286 RVA: 0x002E6D04 File Offset: 0x002E4F04
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			this.empty.EventTransition(GameHashes.OccupantChanged, this.full, (FlowerVase.SMInstance smi) => smi.master.plantablePlot.Occupant != null).PlayAnim("off");
			this.full.EventTransition(GameHashes.OccupantChanged, this.empty, (FlowerVase.SMInstance smi) => smi.master.plantablePlot.Occupant == null).PlayAnim("on");
		}

		// Token: 0x0400604F RID: 24655
		public GameStateMachine<FlowerVase.States, FlowerVase.SMInstance, FlowerVase, object>.State empty;

		// Token: 0x04006050 RID: 24656
		public GameStateMachine<FlowerVase.States, FlowerVase.SMInstance, FlowerVase, object>.State full;
	}
}
