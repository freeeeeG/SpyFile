using System;
using TUNING;
using UnityEngine;

// Token: 0x02000620 RID: 1568
public class LeadSuitLocker : StateMachineComponent<LeadSuitLocker.StatesInstance>
{
	// Token: 0x0600278E RID: 10126 RVA: 0x000D6AFC File Offset: 0x000D4CFC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.o2_meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target_top", "meter_oxygen", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[]
		{
			"meter_target_top"
		});
		this.battery_meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target_side", "meter_petrol", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[]
		{
			"meter_target_side"
		});
		base.smi.StartSM();
	}

	// Token: 0x0600278F RID: 10127 RVA: 0x000D6B7C File Offset: 0x000D4D7C
	public bool IsSuitFullyCharged()
	{
		return this.suit_locker.IsSuitFullyCharged();
	}

	// Token: 0x06002790 RID: 10128 RVA: 0x000D6B89 File Offset: 0x000D4D89
	public KPrefabID GetStoredOutfit()
	{
		return this.suit_locker.GetStoredOutfit();
	}

	// Token: 0x06002791 RID: 10129 RVA: 0x000D6B98 File Offset: 0x000D4D98
	private void FillBattery(float dt)
	{
		KPrefabID storedOutfit = this.suit_locker.GetStoredOutfit();
		if (storedOutfit == null)
		{
			return;
		}
		LeadSuitTank component = storedOutfit.GetComponent<LeadSuitTank>();
		if (!component.IsFull())
		{
			component.batteryCharge += dt / this.batteryChargeTime;
		}
	}

	// Token: 0x06002792 RID: 10130 RVA: 0x000D6BE0 File Offset: 0x000D4DE0
	private void RefreshMeter()
	{
		this.o2_meter.SetPositionPercent(this.suit_locker.OxygenAvailable);
		this.battery_meter.SetPositionPercent(this.suit_locker.BatteryAvailable);
		this.anim_controller.SetSymbolVisiblity("oxygen_yes_bloom", this.IsOxygenTankAboveMinimumLevel());
		this.anim_controller.SetSymbolVisiblity("petrol_yes_bloom", this.IsBatteryAboveMinimumLevel());
	}

	// Token: 0x06002793 RID: 10131 RVA: 0x000D6C50 File Offset: 0x000D4E50
	public bool IsOxygenTankAboveMinimumLevel()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit != null)
		{
			SuitTank component = storedOutfit.GetComponent<SuitTank>();
			return component == null || component.PercentFull() >= EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE;
		}
		return false;
	}

	// Token: 0x06002794 RID: 10132 RVA: 0x000D6C94 File Offset: 0x000D4E94
	public bool IsBatteryAboveMinimumLevel()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit != null)
		{
			LeadSuitTank component = storedOutfit.GetComponent<LeadSuitTank>();
			return component == null || component.PercentFull() >= EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE;
		}
		return false;
	}

	// Token: 0x040016DF RID: 5855
	[MyCmpReq]
	private Building building;

	// Token: 0x040016E0 RID: 5856
	[MyCmpReq]
	private Storage storage;

	// Token: 0x040016E1 RID: 5857
	[MyCmpReq]
	private SuitLocker suit_locker;

	// Token: 0x040016E2 RID: 5858
	[MyCmpReq]
	private KBatchedAnimController anim_controller;

	// Token: 0x040016E3 RID: 5859
	private MeterController o2_meter;

	// Token: 0x040016E4 RID: 5860
	private MeterController battery_meter;

	// Token: 0x040016E5 RID: 5861
	private float batteryChargeTime = 60f;

	// Token: 0x020012E7 RID: 4839
	public class States : GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker>
	{
		// Token: 0x06007EF4 RID: 32500 RVA: 0x002EACE8 File Offset: 0x002E8EE8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Update("RefreshMeter", delegate(LeadSuitLocker.StatesInstance smi, float dt)
			{
				smi.master.RefreshMeter();
			}, UpdateRate.RENDER_200ms, false);
			this.empty.EventTransition(GameHashes.OnStorageChange, this.charging, (LeadSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() != null);
			this.charging.DefaultState(this.charging.notoperational).EventTransition(GameHashes.OnStorageChange, this.empty, (LeadSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() == null).Transition(this.charged, (LeadSuitLocker.StatesInstance smi) => smi.master.IsSuitFullyCharged(), UpdateRate.SIM_200ms);
			this.charging.notoperational.TagTransition(GameTags.Operational, this.charging.operational, false);
			this.charging.operational.TagTransition(GameTags.Operational, this.charging.notoperational, true).Update("FillBattery", delegate(LeadSuitLocker.StatesInstance smi, float dt)
			{
				smi.master.FillBattery(dt);
			}, UpdateRate.SIM_1000ms, false);
			this.charged.EventTransition(GameHashes.OnStorageChange, this.empty, (LeadSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() == null);
		}

		// Token: 0x04006106 RID: 24838
		public GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State empty;

		// Token: 0x04006107 RID: 24839
		public LeadSuitLocker.States.ChargingStates charging;

		// Token: 0x04006108 RID: 24840
		public GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State charged;

		// Token: 0x020020E9 RID: 8425
		public class ChargingStates : GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State
		{
			// Token: 0x0400934E RID: 37710
			public GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State notoperational;

			// Token: 0x0400934F RID: 37711
			public GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State operational;
		}
	}

	// Token: 0x020012E8 RID: 4840
	public class StatesInstance : GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.GameInstance
	{
		// Token: 0x06007EF6 RID: 32502 RVA: 0x002EAE8A File Offset: 0x002E908A
		public StatesInstance(LeadSuitLocker lead_suit_locker) : base(lead_suit_locker)
		{
		}
	}
}
