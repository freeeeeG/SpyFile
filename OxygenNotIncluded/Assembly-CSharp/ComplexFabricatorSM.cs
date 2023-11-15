using System;

// Token: 0x020005CF RID: 1487
public class ComplexFabricatorSM : StateMachineComponent<ComplexFabricatorSM.StatesInstance>
{
	// Token: 0x060024F3 RID: 9459 RVA: 0x000CA496 File Offset: 0x000C8696
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04001531 RID: 5425
	[MyCmpGet]
	private ComplexFabricator fabricator;

	// Token: 0x04001532 RID: 5426
	public StatusItem idleQueue_StatusItem = Db.Get().BuildingStatusItems.FabricatorIdle;

	// Token: 0x04001533 RID: 5427
	public StatusItem waitingForMaterial_StatusItem = Db.Get().BuildingStatusItems.FabricatorEmpty;

	// Token: 0x04001534 RID: 5428
	public StatusItem waitingForWorker_StatusItem = Db.Get().BuildingStatusItems.PendingWork;

	// Token: 0x04001535 RID: 5429
	public string idleAnimationName = "off";

	// Token: 0x0200126F RID: 4719
	public class StatesInstance : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.GameInstance
	{
		// Token: 0x06007D5E RID: 32094 RVA: 0x002E3D31 File Offset: 0x002E1F31
		public StatesInstance(ComplexFabricatorSM master) : base(master)
		{
		}
	}

	// Token: 0x02001270 RID: 4720
	public class States : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM>
	{
		// Token: 0x06007D5F RID: 32095 RVA: 0x002E3D3C File Offset: 0x002E1F3C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.idle, (ComplexFabricatorSM.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.idle.DefaultState(this.idle.idleQueue).PlayAnim(new Func<ComplexFabricatorSM.StatesInstance, string>(ComplexFabricatorSM.States.GetIdleAnimName), KAnim.PlayMode.Once).EventTransition(GameHashes.OperationalChanged, this.off, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.operating, (ComplexFabricatorSM.StatesInstance smi) => smi.GetComponent<Operational>().IsActive);
			this.idle.idleQueue.ToggleStatusItem((ComplexFabricatorSM.StatesInstance smi) => smi.master.idleQueue_StatusItem, null).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.waitingForMaterial, (ComplexFabricatorSM.StatesInstance smi) => smi.master.fabricator.HasAnyOrder);
			this.idle.waitingForMaterial.ToggleStatusItem((ComplexFabricatorSM.StatesInstance smi) => smi.master.waitingForMaterial_StatusItem, null).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.idleQueue, (ComplexFabricatorSM.StatesInstance smi) => !smi.master.fabricator.HasAnyOrder).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.waitingForWorker, (ComplexFabricatorSM.StatesInstance smi) => smi.master.fabricator.WaitingForWorker).EventHandler(GameHashes.FabricatorOrdersUpdated, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).EventHandler(GameHashes.OnParticleStorageChanged, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).Enter(delegate(ComplexFabricatorSM.StatesInstance smi)
			{
				this.RefreshHEPStatus(smi);
			});
			this.idle.waitingForWorker.ToggleStatusItem((ComplexFabricatorSM.StatesInstance smi) => smi.master.waitingForWorker_StatusItem, null).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.idleQueue, (ComplexFabricatorSM.StatesInstance smi) => !smi.master.fabricator.WaitingForWorker).EnterTransition(this.operating, (ComplexFabricatorSM.StatesInstance smi) => !smi.master.fabricator.duplicantOperated).EventHandler(GameHashes.FabricatorOrdersUpdated, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).EventHandler(GameHashes.OnParticleStorageChanged, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).Enter(delegate(ComplexFabricatorSM.StatesInstance smi)
			{
				this.RefreshHEPStatus(smi);
			});
			this.operating.DefaultState(this.operating.working_pre).ToggleStatusItem((ComplexFabricatorSM.StatesInstance smi) => smi.master.fabricator.workingStatusItem, (ComplexFabricatorSM.StatesInstance smi) => smi.GetComponent<ComplexFabricator>());
			this.operating.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.operating.working_loop).EventTransition(GameHashes.OperationalChanged, this.operating.working_pst, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.operating.working_pst, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsActive);
			this.operating.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.operating.working_pst, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.operating.working_pst, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsActive);
			this.operating.working_pst.PlayAnim("working_pst").WorkableCompleteTransition((ComplexFabricatorSM.StatesInstance smi) => smi.master.fabricator.Workable, this.operating.working_pst_complete).OnAnimQueueComplete(this.idle);
			this.operating.working_pst_complete.PlayAnim("working_pst_complete").OnAnimQueueComplete(this.idle);
		}

		// Token: 0x06007D60 RID: 32096 RVA: 0x002E41F8 File Offset: 0x002E23F8
		public void RefreshHEPStatus(ComplexFabricatorSM.StatesInstance smi)
		{
			if (smi.master.GetComponent<HighEnergyParticleStorage>() != null && smi.master.fabricator.NeedsMoreHEPForQueuedRecipe())
			{
				smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.FabricatorLacksHEP, smi.master.fabricator);
				return;
			}
			smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.FabricatorLacksHEP, false);
		}

		// Token: 0x06007D61 RID: 32097 RVA: 0x002E4277 File Offset: 0x002E2477
		public static string GetIdleAnimName(ComplexFabricatorSM.StatesInstance smi)
		{
			return smi.master.idleAnimationName;
		}

		// Token: 0x04005FB8 RID: 24504
		public ComplexFabricatorSM.States.IdleStates off;

		// Token: 0x04005FB9 RID: 24505
		public ComplexFabricatorSM.States.IdleStates idle;

		// Token: 0x04005FBA RID: 24506
		public ComplexFabricatorSM.States.OperatingStates operating;

		// Token: 0x020020BF RID: 8383
		public class IdleStates : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State
		{
			// Token: 0x04009259 RID: 37465
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State idleQueue;

			// Token: 0x0400925A RID: 37466
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State waitingForMaterial;

			// Token: 0x0400925B RID: 37467
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State waitingForWorker;
		}

		// Token: 0x020020C0 RID: 8384
		public class OperatingStates : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State
		{
			// Token: 0x0400925C RID: 37468
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_pre;

			// Token: 0x0400925D RID: 37469
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_loop;

			// Token: 0x0400925E RID: 37470
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_pst;

			// Token: 0x0400925F RID: 37471
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_pst_complete;
		}
	}
}
