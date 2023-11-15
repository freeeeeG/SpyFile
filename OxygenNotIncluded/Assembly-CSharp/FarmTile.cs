using System;

// Token: 0x020005FD RID: 1533
public class FarmTile : StateMachineComponent<FarmTile.SMInstance>
{
	// Token: 0x06002667 RID: 9831 RVA: 0x000D0DF3 File Offset: 0x000CEFF3
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04001601 RID: 5633
	[MyCmpReq]
	private PlantablePlot plantablePlot;

	// Token: 0x04001602 RID: 5634
	[MyCmpReq]
	private Storage storage;

	// Token: 0x020012A1 RID: 4769
	public class SMInstance : GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.GameInstance
	{
		// Token: 0x06007E03 RID: 32259 RVA: 0x002E6456 File Offset: 0x002E4656
		public SMInstance(FarmTile master) : base(master)
		{
		}

		// Token: 0x06007E04 RID: 32260 RVA: 0x002E6460 File Offset: 0x002E4660
		public bool HasWater()
		{
			PrimaryElement primaryElement = base.master.storage.FindPrimaryElement(SimHashes.Water);
			return primaryElement != null && primaryElement.Mass > 0f;
		}
	}

	// Token: 0x020012A2 RID: 4770
	public class States : GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile>
	{
		// Token: 0x06007E05 RID: 32261 RVA: 0x002E649C File Offset: 0x002E469C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			this.empty.EventTransition(GameHashes.OccupantChanged, this.full, (FarmTile.SMInstance smi) => smi.master.plantablePlot.Occupant != null);
			this.empty.wet.EventTransition(GameHashes.OnStorageChange, this.empty.dry, (FarmTile.SMInstance smi) => !smi.HasWater());
			this.empty.dry.EventTransition(GameHashes.OnStorageChange, this.empty.wet, (FarmTile.SMInstance smi) => !smi.HasWater());
			this.full.EventTransition(GameHashes.OccupantChanged, this.empty, (FarmTile.SMInstance smi) => smi.master.plantablePlot.Occupant == null);
			this.full.wet.EventTransition(GameHashes.OnStorageChange, this.full.dry, (FarmTile.SMInstance smi) => !smi.HasWater());
			this.full.dry.EventTransition(GameHashes.OnStorageChange, this.full.wet, (FarmTile.SMInstance smi) => !smi.HasWater());
		}

		// Token: 0x04006034 RID: 24628
		public FarmTile.States.FarmStates empty;

		// Token: 0x04006035 RID: 24629
		public FarmTile.States.FarmStates full;

		// Token: 0x020020CC RID: 8396
		public class FarmStates : GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.State
		{
			// Token: 0x040092C5 RID: 37573
			public GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.State wet;

			// Token: 0x040092C6 RID: 37574
			public GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.State dry;
		}
	}
}
