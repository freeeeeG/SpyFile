using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200061C RID: 1564
public class JetSuitLocker : StateMachineComponent<JetSuitLocker.StatesInstance>, ISecondaryInput
{
	// Token: 0x17000218 RID: 536
	// (get) Token: 0x06002774 RID: 10100 RVA: 0x000D625C File Offset: 0x000D445C
	public float FuelAvailable
	{
		get
		{
			GameObject fuel = this.GetFuel();
			float num = 0f;
			if (fuel != null)
			{
				num = fuel.GetComponent<PrimaryElement>().Mass / 100f;
				num = Math.Min(num, 1f);
			}
			return num;
		}
	}

	// Token: 0x06002775 RID: 10101 RVA: 0x000D62A0 File Offset: 0x000D44A0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.fuel_tag = SimHashes.Petroleum.CreateTag();
		this.fuel_consumer = base.gameObject.AddComponent<ConduitConsumer>();
		this.fuel_consumer.conduitType = this.portInfo.conduitType;
		this.fuel_consumer.consumptionRate = 10f;
		this.fuel_consumer.capacityTag = this.fuel_tag;
		this.fuel_consumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		this.fuel_consumer.forceAlwaysSatisfied = true;
		this.fuel_consumer.capacityKG = 100f;
		this.fuel_consumer.useSecondaryInput = true;
		RequireInputs requireInputs = base.gameObject.AddComponent<RequireInputs>();
		requireInputs.conduitConsumer = this.fuel_consumer;
		requireInputs.SetRequirements(false, true);
		int cell = Grid.PosToCell(base.transform.GetPosition());
		CellOffset rotatedOffset = this.building.GetRotatedOffset(this.portInfo.offset);
		this.secondaryInputCell = Grid.OffsetCell(cell, rotatedOffset);
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
		this.flowNetworkItem = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, this.secondaryInputCell, base.gameObject);
		networkManager.AddToNetworks(this.secondaryInputCell, this.flowNetworkItem, true);
		this.fuel_meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target_1", "meter_petrol", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[]
		{
			"meter_target_1"
		});
		this.o2_meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target_2", "meter_oxygen", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[]
		{
			"meter_target_2"
		});
		base.smi.StartSM();
	}

	// Token: 0x06002776 RID: 10102 RVA: 0x000D6444 File Offset: 0x000D4644
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.portInfo.conduitType).RemoveFromNetworks(this.secondaryInputCell, this.flowNetworkItem, true);
		base.OnCleanUp();
	}

	// Token: 0x06002777 RID: 10103 RVA: 0x000D646E File Offset: 0x000D466E
	private GameObject GetFuel()
	{
		return this.storage.FindFirst(this.fuel_tag);
	}

	// Token: 0x06002778 RID: 10104 RVA: 0x000D6481 File Offset: 0x000D4681
	public bool IsSuitFullyCharged()
	{
		return this.suit_locker.IsSuitFullyCharged();
	}

	// Token: 0x06002779 RID: 10105 RVA: 0x000D648E File Offset: 0x000D468E
	public KPrefabID GetStoredOutfit()
	{
		return this.suit_locker.GetStoredOutfit();
	}

	// Token: 0x0600277A RID: 10106 RVA: 0x000D649C File Offset: 0x000D469C
	private void FuelSuit(float dt)
	{
		KPrefabID storedOutfit = this.suit_locker.GetStoredOutfit();
		if (storedOutfit == null)
		{
			return;
		}
		GameObject fuel = this.GetFuel();
		if (fuel == null)
		{
			return;
		}
		PrimaryElement component = fuel.GetComponent<PrimaryElement>();
		if (component == null)
		{
			return;
		}
		JetSuitTank component2 = storedOutfit.GetComponent<JetSuitTank>();
		float num = 375f * dt / 600f;
		num = Mathf.Min(num, 25f - component2.amount);
		num = Mathf.Min(component.Mass, num);
		component.Mass -= num;
		component2.amount += num;
	}

	// Token: 0x0600277B RID: 10107 RVA: 0x000D6539 File Offset: 0x000D4739
	bool ISecondaryInput.HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x0600277C RID: 10108 RVA: 0x000D6549 File Offset: 0x000D4749
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (this.portInfo.conduitType == type)
		{
			return this.portInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x0600277D RID: 10109 RVA: 0x000D656C File Offset: 0x000D476C
	public bool HasFuel()
	{
		GameObject fuel = this.GetFuel();
		return fuel != null && fuel.GetComponent<PrimaryElement>().Mass > 0f;
	}

	// Token: 0x0600277E RID: 10110 RVA: 0x000D65A0 File Offset: 0x000D47A0
	private void RefreshMeter()
	{
		this.o2_meter.SetPositionPercent(this.suit_locker.OxygenAvailable);
		this.fuel_meter.SetPositionPercent(this.FuelAvailable);
		this.anim_controller.SetSymbolVisiblity("oxygen_yes_bloom", this.IsOxygenTankAboveMinimumLevel());
		this.anim_controller.SetSymbolVisiblity("petrol_yes_bloom", this.IsFuelTankAboveMinimumLevel());
	}

	// Token: 0x0600277F RID: 10111 RVA: 0x000D660C File Offset: 0x000D480C
	public bool IsOxygenTankAboveMinimumLevel()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit != null)
		{
			SuitTank component = storedOutfit.GetComponent<SuitTank>();
			return component == null || component.PercentFull() >= TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE;
		}
		return false;
	}

	// Token: 0x06002780 RID: 10112 RVA: 0x000D6650 File Offset: 0x000D4850
	public bool IsFuelTankAboveMinimumLevel()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit != null)
		{
			JetSuitTank component = storedOutfit.GetComponent<JetSuitTank>();
			return component == null || component.PercentFull() >= TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE;
		}
		return false;
	}

	// Token: 0x040016C9 RID: 5833
	[MyCmpReq]
	private Building building;

	// Token: 0x040016CA RID: 5834
	[MyCmpReq]
	private Storage storage;

	// Token: 0x040016CB RID: 5835
	[MyCmpReq]
	private SuitLocker suit_locker;

	// Token: 0x040016CC RID: 5836
	[MyCmpReq]
	private KBatchedAnimController anim_controller;

	// Token: 0x040016CD RID: 5837
	public const float FUEL_CAPACITY = 100f;

	// Token: 0x040016CE RID: 5838
	[SerializeField]
	public ConduitPortInfo portInfo;

	// Token: 0x040016CF RID: 5839
	private int secondaryInputCell = -1;

	// Token: 0x040016D0 RID: 5840
	private FlowUtilityNetwork.NetworkItem flowNetworkItem;

	// Token: 0x040016D1 RID: 5841
	private ConduitConsumer fuel_consumer;

	// Token: 0x040016D2 RID: 5842
	private Tag fuel_tag;

	// Token: 0x040016D3 RID: 5843
	private MeterController o2_meter;

	// Token: 0x040016D4 RID: 5844
	private MeterController fuel_meter;

	// Token: 0x020012DF RID: 4831
	public class States : GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker>
	{
		// Token: 0x06007EDF RID: 32479 RVA: 0x002EA69C File Offset: 0x002E889C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Update("RefreshMeter", delegate(JetSuitLocker.StatesInstance smi, float dt)
			{
				smi.master.RefreshMeter();
			}, UpdateRate.RENDER_200ms, false);
			this.empty.EventTransition(GameHashes.OnStorageChange, this.charging, (JetSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() != null);
			this.charging.DefaultState(this.charging.notoperational).EventTransition(GameHashes.OnStorageChange, this.empty, (JetSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() == null).Transition(this.charged, (JetSuitLocker.StatesInstance smi) => smi.master.IsSuitFullyCharged(), UpdateRate.SIM_200ms);
			this.charging.notoperational.TagTransition(GameTags.Operational, this.charging.operational, false);
			this.charging.operational.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.nofuel, (JetSuitLocker.StatesInstance smi) => !smi.master.HasFuel(), UpdateRate.SIM_200ms).Update("FuelSuit", delegate(JetSuitLocker.StatesInstance smi, float dt)
			{
				smi.master.FuelSuit(dt);
			}, UpdateRate.SIM_1000ms, false);
			this.charging.nofuel.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.operational, (JetSuitLocker.StatesInstance smi) => smi.master.HasFuel(), UpdateRate.SIM_200ms).ToggleStatusItem(BUILDING.STATUSITEMS.SUIT_LOCKER.NO_FUEL.NAME, BUILDING.STATUSITEMS.SUIT_LOCKER.NO_FUEL.TOOLTIP, "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, default(HashedString), 129022, null, null, null);
			this.charged.EventTransition(GameHashes.OnStorageChange, this.empty, (JetSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() == null);
		}

		// Token: 0x040060F1 RID: 24817
		public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State empty;

		// Token: 0x040060F2 RID: 24818
		public JetSuitLocker.States.ChargingStates charging;

		// Token: 0x040060F3 RID: 24819
		public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State charged;

		// Token: 0x020020E7 RID: 8423
		public class ChargingStates : GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State
		{
			// Token: 0x04009342 RID: 37698
			public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State notoperational;

			// Token: 0x04009343 RID: 37699
			public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State operational;

			// Token: 0x04009344 RID: 37700
			public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State nofuel;
		}
	}

	// Token: 0x020012E0 RID: 4832
	public class StatesInstance : GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.GameInstance
	{
		// Token: 0x06007EE1 RID: 32481 RVA: 0x002EA8F2 File Offset: 0x002E8AF2
		public StatesInstance(JetSuitLocker jet_suit_locker) : base(jet_suit_locker)
		{
		}
	}
}
