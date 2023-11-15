using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005D0 RID: 1488
public class Compost : StateMachineComponent<Compost.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x060024F5 RID: 9461 RVA: 0x000CA509 File Offset: 0x000C8709
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Compost>(-1697596308, Compost.OnStorageChangedDelegate);
	}

	// Token: 0x060024F6 RID: 9462 RVA: 0x000CA524 File Offset: 0x000C8724
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<ManualDeliveryKG>().ShowStatusItem = false;
		this.temperatureAdjuster = new SimulatedTemperatureAdjuster(this.simulatedInternalTemperature, this.simulatedInternalHeatCapacity, this.simulatedThermalConductivity, base.GetComponent<Storage>());
		base.smi.StartSM();
	}

	// Token: 0x060024F7 RID: 9463 RVA: 0x000CA571 File Offset: 0x000C8771
	protected override void OnCleanUp()
	{
		this.temperatureAdjuster.CleanUp();
	}

	// Token: 0x060024F8 RID: 9464 RVA: 0x000CA57E File Offset: 0x000C877E
	private void OnStorageChanged(object data)
	{
		(GameObject)data == null;
	}

	// Token: 0x060024F9 RID: 9465 RVA: 0x000CA58D File Offset: 0x000C878D
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return SimulatedTemperatureAdjuster.GetDescriptors(this.simulatedInternalTemperature);
	}

	// Token: 0x04001536 RID: 5430
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001537 RID: 5431
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04001538 RID: 5432
	[SerializeField]
	public float flipInterval = 600f;

	// Token: 0x04001539 RID: 5433
	[SerializeField]
	public float simulatedInternalTemperature = 323.15f;

	// Token: 0x0400153A RID: 5434
	[SerializeField]
	public float simulatedInternalHeatCapacity = 400f;

	// Token: 0x0400153B RID: 5435
	[SerializeField]
	public float simulatedThermalConductivity = 1000f;

	// Token: 0x0400153C RID: 5436
	private SimulatedTemperatureAdjuster temperatureAdjuster;

	// Token: 0x0400153D RID: 5437
	private static readonly EventSystem.IntraObjectHandler<Compost> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<Compost>(delegate(Compost component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x02001271 RID: 4721
	public class StatesInstance : GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.GameInstance
	{
		// Token: 0x06007D65 RID: 32101 RVA: 0x002E429E File Offset: 0x002E249E
		public StatesInstance(Compost master) : base(master)
		{
		}

		// Token: 0x06007D66 RID: 32102 RVA: 0x002E42A7 File Offset: 0x002E24A7
		public bool CanStartConverting()
		{
			return base.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false);
		}

		// Token: 0x06007D67 RID: 32103 RVA: 0x002E42BA File Offset: 0x002E24BA
		public bool CanContinueConverting()
		{
			return base.master.GetComponent<ElementConverter>().CanConvertAtAll();
		}

		// Token: 0x06007D68 RID: 32104 RVA: 0x002E42CC File Offset: 0x002E24CC
		public bool IsEmpty()
		{
			return base.master.storage.IsEmpty();
		}

		// Token: 0x06007D69 RID: 32105 RVA: 0x002E42DE File Offset: 0x002E24DE
		public void ResetWorkable()
		{
			CompostWorkable component = base.master.GetComponent<CompostWorkable>();
			component.ShowProgressBar(false);
			component.WorkTimeRemaining = component.GetWorkTime();
		}
	}

	// Token: 0x02001272 RID: 4722
	public class States : GameStateMachine<Compost.States, Compost.StatesInstance, Compost>
	{
		// Token: 0x06007D6A RID: 32106 RVA: 0x002E4300 File Offset: 0x002E2500
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.empty.Enter("empty", delegate(Compost.StatesInstance smi)
			{
				smi.ResetWorkable();
			}).EventTransition(GameHashes.OnStorageChange, this.insufficientMass, (Compost.StatesInstance smi) => !smi.IsEmpty()).EventTransition(GameHashes.OperationalChanged, this.disabledEmpty, (Compost.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingWaste, null).PlayAnim("off");
			this.insufficientMass.Enter("empty", delegate(Compost.StatesInstance smi)
			{
				smi.ResetWorkable();
			}).EventTransition(GameHashes.OnStorageChange, this.empty, (Compost.StatesInstance smi) => smi.IsEmpty()).EventTransition(GameHashes.OnStorageChange, this.inert, (Compost.StatesInstance smi) => smi.CanStartConverting()).ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingWaste, null).PlayAnim("idle_half");
			this.inert.EventTransition(GameHashes.OperationalChanged, this.disabled, (Compost.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).PlayAnim("on").ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingCompostFlip, null).ToggleChore(new Func<Compost.StatesInstance, Chore>(this.CreateFlipChore), this.composting);
			this.composting.Enter("Composting", delegate(Compost.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).EventTransition(GameHashes.OnStorageChange, this.empty, (Compost.StatesInstance smi) => !smi.CanContinueConverting()).EventTransition(GameHashes.OperationalChanged, this.disabled, (Compost.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).ScheduleGoTo((Compost.StatesInstance smi) => smi.master.flipInterval, this.inert).Exit(delegate(Compost.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
			this.disabled.Enter("disabledEmpty", delegate(Compost.StatesInstance smi)
			{
				smi.ResetWorkable();
			}).PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.inert, (Compost.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.disabledEmpty.Enter("disabledEmpty", delegate(Compost.StatesInstance smi)
			{
				smi.ResetWorkable();
			}).PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.empty, (Compost.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
		}

		// Token: 0x06007D6B RID: 32107 RVA: 0x002E4690 File Offset: 0x002E2890
		private Chore CreateFlipChore(Compost.StatesInstance smi)
		{
			return new WorkChore<CompostWorkable>(Db.Get().ChoreTypes.FlipCompost, smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x04005FBB RID: 24507
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State empty;

		// Token: 0x04005FBC RID: 24508
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State insufficientMass;

		// Token: 0x04005FBD RID: 24509
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State disabled;

		// Token: 0x04005FBE RID: 24510
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State disabledEmpty;

		// Token: 0x04005FBF RID: 24511
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State inert;

		// Token: 0x04005FC0 RID: 24512
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State composting;
	}
}
