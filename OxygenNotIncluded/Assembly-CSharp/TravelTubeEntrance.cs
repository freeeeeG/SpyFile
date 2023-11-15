using System;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006AE RID: 1710
[SerializationConfig(MemberSerialization.OptIn)]
public class TravelTubeEntrance : StateMachineComponent<TravelTubeEntrance.SMInstance>, ISaveLoadable, ISim200ms
{
	// Token: 0x1700032B RID: 811
	// (get) Token: 0x06002E50 RID: 11856 RVA: 0x000F499F File Offset: 0x000F2B9F
	public float AvailableJoules
	{
		get
		{
			return this.availableJoules;
		}
	}

	// Token: 0x1700032C RID: 812
	// (get) Token: 0x06002E51 RID: 11857 RVA: 0x000F49A7 File Offset: 0x000F2BA7
	public float TotalCapacity
	{
		get
		{
			return this.jouleCapacity;
		}
	}

	// Token: 0x1700032D RID: 813
	// (get) Token: 0x06002E52 RID: 11858 RVA: 0x000F49AF File Offset: 0x000F2BAF
	public float UsageJoules
	{
		get
		{
			return this.joulesPerLaunch;
		}
	}

	// Token: 0x1700032E RID: 814
	// (get) Token: 0x06002E53 RID: 11859 RVA: 0x000F49B7 File Offset: 0x000F2BB7
	public bool HasLaunchPower
	{
		get
		{
			return this.availableJoules > this.joulesPerLaunch;
		}
	}

	// Token: 0x1700032F RID: 815
	// (get) Token: 0x06002E54 RID: 11860 RVA: 0x000F49C7 File Offset: 0x000F2BC7
	public bool HasWaxForGreasyLaunch
	{
		get
		{
			return this.storage.GetAmountAvailable(SimHashes.MilkFat.CreateTag()) >= this.waxPerLaunch;
		}
	}

	// Token: 0x17000330 RID: 816
	// (get) Token: 0x06002E55 RID: 11861 RVA: 0x000F49E9 File Offset: 0x000F2BE9
	public int WaxLaunchesAvailable
	{
		get
		{
			return Mathf.FloorToInt(this.storage.GetAmountAvailable(SimHashes.MilkFat.CreateTag()) / this.waxPerLaunch);
		}
	}

	// Token: 0x17000331 RID: 817
	// (get) Token: 0x06002E56 RID: 11862 RVA: 0x000F4A0C File Offset: 0x000F2C0C
	private bool ShouldUseWaxLaunchAnimation
	{
		get
		{
			return this.deliverAndUseWax && this.HasWaxForGreasyLaunch;
		}
	}

	// Token: 0x06002E57 RID: 11863 RVA: 0x000F4A20 File Offset: 0x000F2C20
	public static void SetTravelerGleamEffect(TravelTubeEntrance.SMInstance smi)
	{
		TravelTubeEntrance.Work component = smi.GetComponent<TravelTubeEntrance.Work>();
		if (component.worker != null)
		{
			component.worker.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("gleam", smi.master.ShouldUseWaxLaunchAnimation);
		}
	}

	// Token: 0x06002E58 RID: 11864 RVA: 0x000F4A67 File Offset: 0x000F2C67
	public static string GetLaunchAnimName(TravelTubeEntrance.SMInstance smi)
	{
		if (!smi.master.ShouldUseWaxLaunchAnimation)
		{
			return "working_pre";
		}
		return "wax";
	}

	// Token: 0x06002E59 RID: 11865 RVA: 0x000F4A81 File Offset: 0x000F2C81
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.energyConsumer.OnConnectionChanged += this.OnConnectionChanged;
	}

	// Token: 0x06002E5A RID: 11866 RVA: 0x000F4AA0 File Offset: 0x000F2CA0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.SetWaxUse(this.deliverAndUseWax);
		int x = (int)base.transform.GetPosition().x;
		int y = (int)base.transform.GetPosition().y + 2;
		Extents extents = new Extents(x, y, 1, 1);
		UtilityConnections connections = Game.Instance.travelTubeSystem.GetConnections(Grid.XYToCell(x, y), true);
		this.TubeConnectionsChanged(connections);
		this.tubeChangedEntry = GameScenePartitioner.Instance.Add("TravelTubeEntrance.TubeListener", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[35], new Action<object>(this.TubeChanged));
		base.Subscribe<TravelTubeEntrance>(-592767678, TravelTubeEntrance.OnOperationalChangedDelegate);
		base.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		this.meter = new MeterController(this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.waxMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "wax_meter_target", "wax_meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.CreateNewWaitReactable();
		Grid.RegisterTubeEntrance(Grid.PosToCell(this), Mathf.FloorToInt(this.availableJoules / this.joulesPerLaunch));
		base.smi.StartSM();
		this.UpdateWaxCharge();
		this.UpdateCharge();
		base.Subscribe<TravelTubeEntrance>(493375141, TravelTubeEntrance.OnRefreshUserMenuDelegate);
	}

	// Token: 0x06002E5B RID: 11867 RVA: 0x000F4BF4 File Offset: 0x000F2DF4
	private void OnStorageChanged(object obj)
	{
		this.UpdateWaxCharge();
	}

	// Token: 0x06002E5C RID: 11868 RVA: 0x000F4BFC File Offset: 0x000F2DFC
	protected override void OnCleanUp()
	{
		if (this.travelTube != null)
		{
			this.travelTube.Unsubscribe(-1041684577, new Action<object>(this.TubeConnectionsChanged));
			this.travelTube = null;
		}
		Grid.UnregisterTubeEntrance(Grid.PosToCell(this));
		this.ClearWaitReactable();
		GameScenePartitioner.Instance.Free(ref this.tubeChangedEntry);
		base.OnCleanUp();
	}

	// Token: 0x06002E5D RID: 11869 RVA: 0x000F4C64 File Offset: 0x000F2E64
	private void OnRefreshUserMenu(object data)
	{
		if (!this.deliverAndUseWax)
		{
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_speed_up", UI.USERMENUACTIONS.TRANSITTUBEWAX.NAME, delegate()
			{
				this.SetWaxUse(true);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.TRANSITTUBEWAX.TOOLTIP, true), 1f);
		}
		else
		{
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_speed_up", UI.USERMENUACTIONS.CANCELTRANSITTUBEWAX.NAME, delegate()
			{
				this.SetWaxUse(false);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELTRANSITTUBEWAX.TOOLTIP, true), 1f);
		}
		KSelectable component = base.GetComponent<KSelectable>();
		bool flag = this.deliverAndUseWax && this.WaxLaunchesAvailable > 0;
		if (component != null)
		{
			if (flag)
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.TransitTubeEntranceWaxReady, this);
				return;
			}
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.TransitTubeEntranceWaxReady, false);
		}
	}

	// Token: 0x06002E5E RID: 11870 RVA: 0x000F4D6C File Offset: 0x000F2F6C
	public void SetWaxUse(bool usingWax)
	{
		this.deliverAndUseWax = usingWax;
		this.manualDelivery.AbortDelivery("Switching to new delivery request");
		this.manualDelivery.capacity = (usingWax ? this.storage.capacityKg : 0f);
		this.manualDelivery.refillMass = (usingWax ? this.waxPerLaunch : 0f);
		this.manualDelivery.MinimumMass = (usingWax ? this.waxPerLaunch : 0f);
		if (!usingWax)
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}
		this.OnRefreshUserMenu(null);
	}

	// Token: 0x06002E5F RID: 11871 RVA: 0x000F4E08 File Offset: 0x000F3008
	private void TubeChanged(object data)
	{
		if (this.travelTube != null)
		{
			this.travelTube.Unsubscribe(-1041684577, new Action<object>(this.TubeConnectionsChanged));
			this.travelTube = null;
		}
		GameObject gameObject = data as GameObject;
		if (data == null)
		{
			this.TubeConnectionsChanged(0);
			return;
		}
		TravelTube component = gameObject.GetComponent<TravelTube>();
		if (component != null)
		{
			component.Subscribe(-1041684577, new Action<object>(this.TubeConnectionsChanged));
			this.travelTube = component;
			return;
		}
		this.TubeConnectionsChanged(0);
	}

	// Token: 0x06002E60 RID: 11872 RVA: 0x000F4E9C File Offset: 0x000F309C
	private void TubeConnectionsChanged(object data)
	{
		bool value = (UtilityConnections)data == UtilityConnections.Up;
		this.operational.SetFlag(TravelTubeEntrance.tubeConnected, value);
	}

	// Token: 0x06002E61 RID: 11873 RVA: 0x000F4EC4 File Offset: 0x000F30C4
	private bool CanAcceptMorePower()
	{
		return this.operational.IsOperational && (this.button == null || this.button.IsEnabled) && this.energyConsumer.IsExternallyPowered && this.availableJoules < this.jouleCapacity;
	}

	// Token: 0x06002E62 RID: 11874 RVA: 0x000F4F18 File Offset: 0x000F3118
	public void Sim200ms(float dt)
	{
		if (this.CanAcceptMorePower())
		{
			this.availableJoules = Mathf.Min(this.jouleCapacity, this.availableJoules + this.energyConsumer.WattsUsed * dt);
			this.UpdateCharge();
		}
		this.energyConsumer.SetSustained(this.HasLaunchPower);
		this.UpdateActive();
		this.UpdateConnectionStatus();
	}

	// Token: 0x06002E63 RID: 11875 RVA: 0x000F4F75 File Offset: 0x000F3175
	public void Reserve(TubeTraveller.Instance traveller, int prefabInstanceID)
	{
		Grid.ReserveTubeEntrance(Grid.PosToCell(this), prefabInstanceID, true);
	}

	// Token: 0x06002E64 RID: 11876 RVA: 0x000F4F85 File Offset: 0x000F3185
	public void Unreserve(TubeTraveller.Instance traveller, int prefabInstanceID)
	{
		Grid.ReserveTubeEntrance(Grid.PosToCell(this), prefabInstanceID, false);
	}

	// Token: 0x06002E65 RID: 11877 RVA: 0x000F4F95 File Offset: 0x000F3195
	public bool IsTraversable(Navigator agent)
	{
		return Grid.HasUsableTubeEntrance(Grid.PosToCell(this), agent.gameObject.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x06002E66 RID: 11878 RVA: 0x000F4FB2 File Offset: 0x000F31B2
	public bool HasChargeSlotReserved(Navigator agent)
	{
		return Grid.HasReservedTubeEntrance(Grid.PosToCell(this), agent.gameObject.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x06002E67 RID: 11879 RVA: 0x000F4FCF File Offset: 0x000F31CF
	public bool HasChargeSlotReserved(TubeTraveller.Instance tube_traveller, int prefabInstanceID)
	{
		return Grid.HasReservedTubeEntrance(Grid.PosToCell(this), prefabInstanceID);
	}

	// Token: 0x06002E68 RID: 11880 RVA: 0x000F4FDD File Offset: 0x000F31DD
	public bool IsChargedSlotAvailable(TubeTraveller.Instance tube_traveller, int prefabInstanceID)
	{
		return Grid.HasUsableTubeEntrance(Grid.PosToCell(this), prefabInstanceID);
	}

	// Token: 0x06002E69 RID: 11881 RVA: 0x000F4FEC File Offset: 0x000F31EC
	public bool ShouldWait(GameObject reactor)
	{
		if (!this.operational.IsOperational)
		{
			return false;
		}
		if (!this.HasLaunchPower)
		{
			return false;
		}
		if (this.launch_workable.worker == null)
		{
			return false;
		}
		TubeTraveller.Instance smi = reactor.GetSMI<TubeTraveller.Instance>();
		return this.HasChargeSlotReserved(smi, reactor.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x06002E6A RID: 11882 RVA: 0x000F5040 File Offset: 0x000F3240
	public void ConsumeCharge(GameObject reactor)
	{
		if (this.HasLaunchPower)
		{
			this.availableJoules -= this.joulesPerLaunch;
			if (this.deliverAndUseWax && this.HasWaxForGreasyLaunch)
			{
				TubeTraveller.Instance smi = reactor.GetSMI<TubeTraveller.Instance>();
				if (smi != null)
				{
					Tag tag = SimHashes.MilkFat.CreateTag();
					float num;
					SimUtil.DiseaseInfo diseaseInfo;
					float num2;
					this.storage.ConsumeAndGetDisease(tag, this.waxPerLaunch, out num, out diseaseInfo, out num2);
					GermExposureMonitor.Instance smi2 = reactor.GetSMI<GermExposureMonitor.Instance>();
					if (smi2 != null)
					{
						smi2.TryInjectDisease(diseaseInfo.idx, diseaseInfo.count, tag, Sickness.InfectionVector.Contact);
					}
					smi.SetWaxState(true);
				}
			}
			this.UpdateCharge();
			this.UpdateWaxCharge();
		}
	}

	// Token: 0x06002E6B RID: 11883 RVA: 0x000F50DC File Offset: 0x000F32DC
	private void CreateNewWaitReactable()
	{
		if (this.wait_reactable == null)
		{
			this.wait_reactable = new TravelTubeEntrance.WaitReactable(this);
		}
	}

	// Token: 0x06002E6C RID: 11884 RVA: 0x000F50F2 File Offset: 0x000F32F2
	private void OrphanWaitReactable()
	{
		this.wait_reactable = null;
	}

	// Token: 0x06002E6D RID: 11885 RVA: 0x000F50FB File Offset: 0x000F32FB
	private void ClearWaitReactable()
	{
		if (this.wait_reactable != null)
		{
			this.wait_reactable.Cleanup();
			this.wait_reactable = null;
		}
	}

	// Token: 0x06002E6E RID: 11886 RVA: 0x000F5118 File Offset: 0x000F3318
	private void OnOperationalChanged(object data)
	{
		bool flag = (bool)data;
		Grid.SetTubeEntranceOperational(Grid.PosToCell(this), flag);
		this.UpdateActive();
	}

	// Token: 0x06002E6F RID: 11887 RVA: 0x000F513E File Offset: 0x000F333E
	private void OnConnectionChanged()
	{
		this.UpdateActive();
		this.UpdateConnectionStatus();
	}

	// Token: 0x06002E70 RID: 11888 RVA: 0x000F514C File Offset: 0x000F334C
	private void UpdateActive()
	{
		this.operational.SetActive(this.CanAcceptMorePower(), false);
	}

	// Token: 0x06002E71 RID: 11889 RVA: 0x000F5160 File Offset: 0x000F3360
	private void UpdateCharge()
	{
		base.smi.sm.hasLaunchCharges.Set(this.HasLaunchPower, base.smi, false);
		float positionPercent = Mathf.Clamp01(this.availableJoules / this.jouleCapacity);
		this.meter.SetPositionPercent(positionPercent);
		this.energyConsumer.UpdatePoweredStatus();
		Grid.SetTubeEntranceReservationCapacity(Grid.PosToCell(this), Mathf.FloorToInt(this.availableJoules / this.joulesPerLaunch));
		this.OnRefreshUserMenu(null);
	}

	// Token: 0x06002E72 RID: 11890 RVA: 0x000F51E0 File Offset: 0x000F33E0
	private void UpdateWaxCharge()
	{
		float positionPercent = Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg);
		this.waxMeter.SetPositionPercent(positionPercent);
	}

	// Token: 0x06002E73 RID: 11891 RVA: 0x000F5218 File Offset: 0x000F3418
	private void UpdateConnectionStatus()
	{
		bool flag = this.button != null && !this.button.IsEnabled;
		bool isConnected = this.energyConsumer.IsConnected;
		bool hasLaunchPower = this.HasLaunchPower;
		if (flag || !isConnected || hasLaunchPower)
		{
			this.connectedStatus = this.selectable.RemoveStatusItem(this.connectedStatus, false);
			return;
		}
		if (this.connectedStatus == Guid.Empty)
		{
			this.connectedStatus = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.NotEnoughPower, null);
		}
	}

	// Token: 0x04001B48 RID: 6984
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001B49 RID: 6985
	[MyCmpReq]
	private TravelTubeEntrance.Work launch_workable;

	// Token: 0x04001B4A RID: 6986
	[MyCmpReq]
	private EnergyConsumerSelfSustaining energyConsumer;

	// Token: 0x04001B4B RID: 6987
	[MyCmpGet]
	private BuildingEnabledButton button;

	// Token: 0x04001B4C RID: 6988
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001B4D RID: 6989
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04001B4E RID: 6990
	[MyCmpReq]
	private ManualDeliveryKG manualDelivery;

	// Token: 0x04001B4F RID: 6991
	public float jouleCapacity = 1f;

	// Token: 0x04001B50 RID: 6992
	public float joulesPerLaunch = 1f;

	// Token: 0x04001B51 RID: 6993
	public float waxPerLaunch;

	// Token: 0x04001B52 RID: 6994
	[Serialize]
	private float availableJoules;

	// Token: 0x04001B53 RID: 6995
	[Serialize]
	private bool deliverAndUseWax;

	// Token: 0x04001B54 RID: 6996
	private TravelTube travelTube;

	// Token: 0x04001B55 RID: 6997
	public const string WAX_LAUNCH_ANIM_NAME = "wax";

	// Token: 0x04001B56 RID: 6998
	private TravelTubeEntrance.WaitReactable wait_reactable;

	// Token: 0x04001B57 RID: 6999
	private MeterController meter;

	// Token: 0x04001B58 RID: 7000
	private MeterController waxMeter;

	// Token: 0x04001B59 RID: 7001
	private const int MAX_CHARGES = 3;

	// Token: 0x04001B5A RID: 7002
	private const float RECHARGE_TIME = 10f;

	// Token: 0x04001B5B RID: 7003
	private static readonly Operational.Flag tubeConnected = new Operational.Flag("tubeConnected", Operational.Flag.Type.Functional);

	// Token: 0x04001B5C RID: 7004
	private HandleVector<int>.Handle tubeChangedEntry;

	// Token: 0x04001B5D RID: 7005
	private static readonly EventSystem.IntraObjectHandler<TravelTubeEntrance> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<TravelTubeEntrance>(delegate(TravelTubeEntrance component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04001B5E RID: 7006
	private static readonly EventSystem.IntraObjectHandler<TravelTubeEntrance> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<TravelTubeEntrance>(delegate(TravelTubeEntrance component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04001B5F RID: 7007
	private Guid connectedStatus;

	// Token: 0x020013DE RID: 5086
	private class LaunchReactable : WorkableReactable
	{
		// Token: 0x060082A0 RID: 33440 RVA: 0x002FA419 File Offset: 0x002F8619
		public LaunchReactable(Workable workable, TravelTubeEntrance entrance) : base(workable, "LaunchReactable", Db.Get().ChoreTypes.TravelTubeEntrance, WorkableReactable.AllowedDirection.Any)
		{
			this.entrance = entrance;
		}

		// Token: 0x060082A1 RID: 33441 RVA: 0x002FA444 File Offset: 0x002F8644
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (base.InternalCanBegin(new_reactor, transition))
			{
				Navigator component = new_reactor.GetComponent<Navigator>();
				return component && this.entrance.HasChargeSlotReserved(component);
			}
			return false;
		}

		// Token: 0x040063A4 RID: 25508
		private TravelTubeEntrance entrance;
	}

	// Token: 0x020013DF RID: 5087
	private class WaitReactable : Reactable
	{
		// Token: 0x060082A2 RID: 33442 RVA: 0x002FA47C File Offset: 0x002F867C
		public WaitReactable(TravelTubeEntrance entrance) : base(entrance.gameObject, "WaitReactable", Db.Get().ChoreTypes.TravelTubeEntrance, 2, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
		{
			this.entrance = entrance;
			this.preventChoreInterruption = false;
		}

		// Token: 0x060082A3 RID: 33443 RVA: 0x002FA4D5 File Offset: 0x002F86D5
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (this.entrance == null)
			{
				base.Cleanup();
				return false;
			}
			return this.entrance.ShouldWait(new_reactor);
		}

		// Token: 0x060082A4 RID: 33444 RVA: 0x002FA50C File Offset: 0x002F870C
		protected override void InternalBegin()
		{
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"), 1f);
			component.Play("idle_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
			this.entrance.OrphanWaitReactable();
			this.entrance.CreateNewWaitReactable();
		}

		// Token: 0x060082A5 RID: 33445 RVA: 0x002FA589 File Offset: 0x002F8789
		public override void Update(float dt)
		{
			if (this.entrance == null)
			{
				base.Cleanup();
				return;
			}
			if (!this.entrance.ShouldWait(this.reactor))
			{
				base.Cleanup();
			}
		}

		// Token: 0x060082A6 RID: 33446 RVA: 0x002FA5B9 File Offset: 0x002F87B9
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"));
			}
		}

		// Token: 0x060082A7 RID: 33447 RVA: 0x002FA5E8 File Offset: 0x002F87E8
		protected override void InternalCleanup()
		{
		}

		// Token: 0x040063A5 RID: 25509
		private TravelTubeEntrance entrance;
	}

	// Token: 0x020013E0 RID: 5088
	public class SMInstance : GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.GameInstance
	{
		// Token: 0x060082A8 RID: 33448 RVA: 0x002FA5EA File Offset: 0x002F87EA
		public SMInstance(TravelTubeEntrance master) : base(master)
		{
		}
	}

	// Token: 0x020013E1 RID: 5089
	public class States : GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance>
	{
		// Token: 0x060082A9 RID: 33449 RVA: 0x002FA5F4 File Offset: 0x002F87F4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.notoperational;
			this.root.ToggleStatusItem(Db.Get().BuildingStatusItems.StoredCharge, null);
			this.notoperational.DefaultState(this.notoperational.normal).PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false);
			this.notoperational.normal.EventTransition(GameHashes.OperationalFlagChanged, this.notoperational.notube, (TravelTubeEntrance.SMInstance smi) => !smi.master.operational.GetFlag(TravelTubeEntrance.tubeConnected));
			this.notoperational.notube.EventTransition(GameHashes.OperationalFlagChanged, this.notoperational.normal, (TravelTubeEntrance.SMInstance smi) => smi.master.operational.GetFlag(TravelTubeEntrance.tubeConnected)).ToggleStatusItem(Db.Get().BuildingStatusItems.NoTubeConnected, null);
			this.notready.PlayAnim("off").ParamTransition<bool>(this.hasLaunchCharges, this.ready, (TravelTubeEntrance.SMInstance smi, bool hasLaunchCharges) => hasLaunchCharges).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.DefaultState(this.ready.free).ToggleReactable((TravelTubeEntrance.SMInstance smi) => new TravelTubeEntrance.LaunchReactable(smi.master.GetComponent<TravelTubeEntrance.Work>(), smi.master.GetComponent<TravelTubeEntrance>())).ParamTransition<bool>(this.hasLaunchCharges, this.notready, (TravelTubeEntrance.SMInstance smi, bool hasLaunchCharges) => !hasLaunchCharges).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.free.PlayAnim("on").WorkableStartTransition((TravelTubeEntrance.SMInstance smi) => smi.GetComponent<TravelTubeEntrance.Work>(), this.ready.occupied);
			this.ready.occupied.PlayAnim(new Func<TravelTubeEntrance.SMInstance, string>(TravelTubeEntrance.GetLaunchAnimName), KAnim.PlayMode.Once).QueueAnim("working_loop", true, null).Enter(new StateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State.Callback(TravelTubeEntrance.SetTravelerGleamEffect)).WorkableStopTransition((TravelTubeEntrance.SMInstance smi) => smi.GetComponent<TravelTubeEntrance.Work>(), this.ready.post);
			this.ready.post.PlayAnim("working_pst").OnAnimQueueComplete(this.ready);
		}

		// Token: 0x040063A6 RID: 25510
		public StateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.BoolParameter hasLaunchCharges;

		// Token: 0x040063A7 RID: 25511
		public TravelTubeEntrance.States.NotOperationalStates notoperational;

		// Token: 0x040063A8 RID: 25512
		public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State notready;

		// Token: 0x040063A9 RID: 25513
		public TravelTubeEntrance.States.ReadyStates ready;

		// Token: 0x0200213D RID: 8509
		public class NotOperationalStates : GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State
		{
			// Token: 0x0400950D RID: 38157
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State normal;

			// Token: 0x0400950E RID: 38158
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State notube;
		}

		// Token: 0x0200213E RID: 8510
		public class ReadyStates : GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State
		{
			// Token: 0x0400950F RID: 38159
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State free;

			// Token: 0x04009510 RID: 38160
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State occupied;

			// Token: 0x04009511 RID: 38161
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State post;
		}
	}

	// Token: 0x020013E2 RID: 5090
	[AddComponentMenu("KMonoBehaviour/Workable/Work")]
	public class Work : Workable, IGameObjectEffectDescriptor
	{
		// Token: 0x060082AB RID: 33451 RVA: 0x002FA892 File Offset: 0x002F8A92
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.resetProgressOnStop = true;
			this.showProgressBar = false;
			this.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_tube_launcher_kanim")
			};
			this.workLayer = Grid.SceneLayer.BuildingUse;
		}

		// Token: 0x060082AC RID: 33452 RVA: 0x002FA8CE File Offset: 0x002F8ACE
		protected override void OnStartWork(Worker worker)
		{
			base.SetWorkTime(1f);
		}

		// Token: 0x040063AA RID: 25514
		public const string DEFAULT_LAUNCH_ANIM_NAME = "anim_interacts_tube_launcher_kanim";
	}
}
