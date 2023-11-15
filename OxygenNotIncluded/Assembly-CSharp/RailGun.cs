using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000676 RID: 1654
public class RailGun : StateMachineComponent<RailGun.StatesInstance>, ISim200ms, ISecondaryInput
{
	// Token: 0x170002D8 RID: 728
	// (get) Token: 0x06002BE1 RID: 11233 RVA: 0x000E8EFB File Offset: 0x000E70FB
	public float MaxLaunchMass
	{
		get
		{
			return 200f;
		}
	}

	// Token: 0x170002D9 RID: 729
	// (get) Token: 0x06002BE2 RID: 11234 RVA: 0x000E8F02 File Offset: 0x000E7102
	public float EnergyCost
	{
		get
		{
			return base.smi.EnergyCost();
		}
	}

	// Token: 0x170002DA RID: 730
	// (get) Token: 0x06002BE3 RID: 11235 RVA: 0x000E8F0F File Offset: 0x000E710F
	public float CurrentEnergy
	{
		get
		{
			return this.hepStorage.Particles;
		}
	}

	// Token: 0x170002DB RID: 731
	// (get) Token: 0x06002BE4 RID: 11236 RVA: 0x000E8F1C File Offset: 0x000E711C
	public bool AllowLaunchingFromLogic
	{
		get
		{
			return !this.hasLogicWire || (this.hasLogicWire && this.isLogicActive);
		}
	}

	// Token: 0x170002DC RID: 732
	// (get) Token: 0x06002BE5 RID: 11237 RVA: 0x000E8F38 File Offset: 0x000E7138
	public bool HasLogicWire
	{
		get
		{
			return this.hasLogicWire;
		}
	}

	// Token: 0x170002DD RID: 733
	// (get) Token: 0x06002BE6 RID: 11238 RVA: 0x000E8F40 File Offset: 0x000E7140
	public bool IsLogicActive
	{
		get
		{
			return this.isLogicActive;
		}
	}

	// Token: 0x06002BE7 RID: 11239 RVA: 0x000E8F48 File Offset: 0x000E7148
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.destinationSelector = base.GetComponent<ClusterDestinationSelector>();
		this.resourceStorage = base.GetComponent<Storage>();
		this.particleStorage = base.GetComponent<HighEnergyParticleStorage>();
		if (RailGun.noSurfaceSightStatusItem == null)
		{
			RailGun.noSurfaceSightStatusItem = new StatusItem("RAILGUN_PATH_NOT_CLEAR", "BUILDING", "status_item_no_sky", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		}
		if (RailGun.noDestinationStatusItem == null)
		{
			RailGun.noDestinationStatusItem = new StatusItem("RAILGUN_NO_DESTINATION", "BUILDING", "status_item_no_sky", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		}
		this.gasInputCell = Grid.OffsetCell(Grid.PosToCell(this), this.gasPortInfo.offset);
		this.gasConsumer = this.CreateConduitConsumer(ConduitType.Gas, this.gasInputCell, out this.gasNetworkItem);
		this.liquidInputCell = Grid.OffsetCell(Grid.PosToCell(this), this.liquidPortInfo.offset);
		this.liquidConsumer = this.CreateConduitConsumer(ConduitType.Liquid, this.liquidInputCell, out this.liquidNetworkItem);
		this.solidInputCell = Grid.OffsetCell(Grid.PosToCell(this), this.solidPortInfo.offset);
		this.solidConsumer = this.CreateSolidConduitConsumer(this.solidInputCell, out this.solidNetworkItem);
		this.CreateMeters();
		base.smi.StartSM();
		if (RailGun.infoStatusItemLogic == null)
		{
			RailGun.infoStatusItemLogic = new StatusItem("LogicOperationalInfo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			RailGun.infoStatusItemLogic.resolveStringCallback = new Func<string, object, string>(RailGun.ResolveInfoStatusItemString);
		}
		this.CheckLogicWireState();
		base.Subscribe<RailGun>(-801688580, RailGun.OnLogicValueChangedDelegate);
	}

	// Token: 0x06002BE8 RID: 11240 RVA: 0x000E90E8 File Offset: 0x000E72E8
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.liquidPortInfo.conduitType).RemoveFromNetworks(this.liquidInputCell, this.liquidNetworkItem, true);
		Conduit.GetNetworkManager(this.gasPortInfo.conduitType).RemoveFromNetworks(this.gasInputCell, this.gasNetworkItem, true);
		Game.Instance.solidConduitSystem.RemoveFromNetworks(this.solidInputCell, this.solidConsumer, true);
		base.OnCleanUp();
	}

	// Token: 0x06002BE9 RID: 11241 RVA: 0x000E915C File Offset: 0x000E735C
	private void CreateMeters()
	{
		this.resourceMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_storage_target", "meter_storage", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.particleMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_orb_target", "meter_orb", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
	}

	// Token: 0x06002BEA RID: 11242 RVA: 0x000E91AF File Offset: 0x000E73AF
	bool ISecondaryInput.HasSecondaryConduitType(ConduitType type)
	{
		return this.liquidPortInfo.conduitType == type || this.gasPortInfo.conduitType == type || this.solidPortInfo.conduitType == type;
	}

	// Token: 0x06002BEB RID: 11243 RVA: 0x000E91E0 File Offset: 0x000E73E0
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (this.liquidPortInfo.conduitType == type)
		{
			return this.liquidPortInfo.offset;
		}
		if (this.gasPortInfo.conduitType == type)
		{
			return this.gasPortInfo.offset;
		}
		if (this.solidPortInfo.conduitType == type)
		{
			return this.solidPortInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x06002BEC RID: 11244 RVA: 0x000E9240 File Offset: 0x000E7440
	private LogicCircuitNetwork GetNetwork()
	{
		int portCell = base.GetComponent<LogicPorts>().GetPortCell(RailGun.PORT_ID);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
	}

	// Token: 0x06002BED RID: 11245 RVA: 0x000E9270 File Offset: 0x000E7470
	private void CheckLogicWireState()
	{
		LogicCircuitNetwork network = this.GetNetwork();
		this.hasLogicWire = (network != null);
		int value = (network != null) ? network.OutputValue : 1;
		bool flag = LogicCircuitNetwork.IsBitActive(0, value);
		this.isLogicActive = flag;
		base.smi.sm.allowedFromLogic.Set(this.AllowLaunchingFromLogic, base.smi, false);
		base.GetComponent<KSelectable>().ToggleStatusItem(RailGun.infoStatusItemLogic, network != null, this);
	}

	// Token: 0x06002BEE RID: 11246 RVA: 0x000E92E3 File Offset: 0x000E74E3
	private void OnLogicValueChanged(object data)
	{
		if (((LogicValueChanged)data).portID == RailGun.PORT_ID)
		{
			this.CheckLogicWireState();
		}
	}

	// Token: 0x06002BEF RID: 11247 RVA: 0x000E9302 File Offset: 0x000E7502
	private static string ResolveInfoStatusItemString(string format_str, object data)
	{
		RailGun railGun = (RailGun)data;
		Operational operational = railGun.operational;
		return railGun.AllowLaunchingFromLogic ? BUILDING.STATUSITEMS.LOGIC.LOGIC_CONTROLLED_ENABLED : BUILDING.STATUSITEMS.LOGIC.LOGIC_CONTROLLED_DISABLED;
	}

	// Token: 0x06002BF0 RID: 11248 RVA: 0x000E932C File Offset: 0x000E752C
	public void Sim200ms(float dt)
	{
		WorldContainer myWorld = this.GetMyWorld();
		Extents extents = base.GetComponent<Building>().GetExtents();
		int x = extents.x;
		int x2 = extents.x + extents.width - 2;
		int y = extents.y + extents.height;
		int num = Grid.XYToCell(x, y);
		int num2 = Grid.XYToCell(x2, y);
		bool flag = true;
		int num3 = (int)myWorld.maximumBounds.y;
		for (int i = num; i <= num2; i++)
		{
			int num4 = i;
			while (Grid.CellRow(num4) <= num3)
			{
				if (!Grid.IsValidCell(num4) || Grid.Solid[num4])
				{
					flag = false;
					break;
				}
				num4 = Grid.CellAbove(num4);
			}
		}
		this.operational.SetFlag(RailGun.noSurfaceSight, flag);
		this.operational.SetFlag(RailGun.noDestination, this.destinationSelector.GetDestinationWorld() >= 0);
		KSelectable component = base.GetComponent<KSelectable>();
		component.ToggleStatusItem(RailGun.noSurfaceSightStatusItem, !flag, null);
		component.ToggleStatusItem(RailGun.noDestinationStatusItem, this.destinationSelector.GetDestinationWorld() < 0, null);
		this.UpdateMeters();
	}

	// Token: 0x06002BF1 RID: 11249 RVA: 0x000E9444 File Offset: 0x000E7644
	private void UpdateMeters()
	{
		this.resourceMeter.SetPositionPercent(Mathf.Clamp01(this.resourceStorage.MassStored() / this.resourceStorage.capacityKg));
		this.particleMeter.SetPositionPercent(Mathf.Clamp01(this.particleStorage.Particles / this.particleStorage.capacity));
	}

	// Token: 0x06002BF2 RID: 11250 RVA: 0x000E94A0 File Offset: 0x000E76A0
	private void LaunchProjectile()
	{
		Extents extents = base.GetComponent<Building>().GetExtents();
		Vector2I vector2I = Grid.PosToXY(base.transform.position);
		vector2I.y += extents.height + 1;
		int cell = Grid.XYToCell(vector2I.x, vector2I.y);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("RailGunPayload"), Grid.CellToPosCBC(cell, Grid.SceneLayer.Front));
		Storage component = gameObject.GetComponent<Storage>();
		float num = 0f;
		while (num < this.launchMass && this.resourceStorage.MassStored() > 0f)
		{
			num += this.resourceStorage.Transfer(component, GameTags.Stored, this.launchMass - num, false, true);
		}
		component.SetContentsDeleteOffGrid(false);
		this.particleStorage.ConsumeAndGet(base.smi.EnergyCost());
		gameObject.SetActive(true);
		if (this.destinationSelector.GetDestinationWorld() >= 0)
		{
			RailGunPayload.StatesInstance smi = gameObject.GetSMI<RailGunPayload.StatesInstance>();
			smi.takeoffVelocity = 35f;
			smi.StartSM();
			smi.Launch(base.gameObject.GetMyWorldLocation(), this.destinationSelector.GetDestination());
		}
	}

	// Token: 0x06002BF3 RID: 11251 RVA: 0x000E95C1 File Offset: 0x000E77C1
	private ConduitConsumer CreateConduitConsumer(ConduitType inputType, int inputCell, out FlowUtilityNetwork.NetworkItem flowNetworkItem)
	{
		ConduitConsumer conduitConsumer = base.gameObject.AddComponent<ConduitConsumer>();
		conduitConsumer.conduitType = inputType;
		conduitConsumer.useSecondaryInput = true;
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(inputType);
		flowNetworkItem = new FlowUtilityNetwork.NetworkItem(inputType, Endpoint.Sink, inputCell, base.gameObject);
		networkManager.AddToNetworks(inputCell, flowNetworkItem, true);
		return conduitConsumer;
	}

	// Token: 0x06002BF4 RID: 11252 RVA: 0x000E95FB File Offset: 0x000E77FB
	private SolidConduitConsumer CreateSolidConduitConsumer(int inputCell, out FlowUtilityNetwork.NetworkItem flowNetworkItem)
	{
		SolidConduitConsumer solidConduitConsumer = base.gameObject.AddComponent<SolidConduitConsumer>();
		solidConduitConsumer.useSecondaryInput = true;
		flowNetworkItem = new FlowUtilityNetwork.NetworkItem(ConduitType.Solid, Endpoint.Sink, inputCell, base.gameObject);
		Game.Instance.solidConduitSystem.AddToNetworks(inputCell, flowNetworkItem, true);
		return solidConduitConsumer;
	}

	// Token: 0x040019C4 RID: 6596
	[Serialize]
	public float launchMass = 200f;

	// Token: 0x040019C5 RID: 6597
	public float MinLaunchMass = 2f;

	// Token: 0x040019C6 RID: 6598
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040019C7 RID: 6599
	[MyCmpGet]
	private KAnimControllerBase kac;

	// Token: 0x040019C8 RID: 6600
	[MyCmpGet]
	public HighEnergyParticleStorage hepStorage;

	// Token: 0x040019C9 RID: 6601
	public Storage resourceStorage;

	// Token: 0x040019CA RID: 6602
	private MeterController resourceMeter;

	// Token: 0x040019CB RID: 6603
	private HighEnergyParticleStorage particleStorage;

	// Token: 0x040019CC RID: 6604
	private MeterController particleMeter;

	// Token: 0x040019CD RID: 6605
	private ClusterDestinationSelector destinationSelector;

	// Token: 0x040019CE RID: 6606
	public static readonly Operational.Flag noSurfaceSight = new Operational.Flag("noSurfaceSight", Operational.Flag.Type.Requirement);

	// Token: 0x040019CF RID: 6607
	private static StatusItem noSurfaceSightStatusItem;

	// Token: 0x040019D0 RID: 6608
	public static readonly Operational.Flag noDestination = new Operational.Flag("noDestination", Operational.Flag.Type.Requirement);

	// Token: 0x040019D1 RID: 6609
	private static StatusItem noDestinationStatusItem;

	// Token: 0x040019D2 RID: 6610
	[SerializeField]
	public ConduitPortInfo liquidPortInfo;

	// Token: 0x040019D3 RID: 6611
	private int liquidInputCell = -1;

	// Token: 0x040019D4 RID: 6612
	private FlowUtilityNetwork.NetworkItem liquidNetworkItem;

	// Token: 0x040019D5 RID: 6613
	private ConduitConsumer liquidConsumer;

	// Token: 0x040019D6 RID: 6614
	[SerializeField]
	public ConduitPortInfo gasPortInfo;

	// Token: 0x040019D7 RID: 6615
	private int gasInputCell = -1;

	// Token: 0x040019D8 RID: 6616
	private FlowUtilityNetwork.NetworkItem gasNetworkItem;

	// Token: 0x040019D9 RID: 6617
	private ConduitConsumer gasConsumer;

	// Token: 0x040019DA RID: 6618
	[SerializeField]
	public ConduitPortInfo solidPortInfo;

	// Token: 0x040019DB RID: 6619
	private int solidInputCell = -1;

	// Token: 0x040019DC RID: 6620
	private FlowUtilityNetwork.NetworkItem solidNetworkItem;

	// Token: 0x040019DD RID: 6621
	private SolidConduitConsumer solidConsumer;

	// Token: 0x040019DE RID: 6622
	public static readonly HashedString PORT_ID = "LogicLaunching";

	// Token: 0x040019DF RID: 6623
	private bool hasLogicWire;

	// Token: 0x040019E0 RID: 6624
	private bool isLogicActive;

	// Token: 0x040019E1 RID: 6625
	private static StatusItem infoStatusItemLogic;

	// Token: 0x040019E2 RID: 6626
	public bool FreeStartHex;

	// Token: 0x040019E3 RID: 6627
	public bool FreeDestinationHex;

	// Token: 0x040019E4 RID: 6628
	private static readonly EventSystem.IntraObjectHandler<RailGun> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<RailGun>(delegate(RailGun component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x0200136E RID: 4974
	public class StatesInstance : GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.GameInstance
	{
		// Token: 0x060080FB RID: 33019 RVA: 0x002F30D6 File Offset: 0x002F12D6
		public StatesInstance(RailGun smi) : base(smi)
		{
		}

		// Token: 0x060080FC RID: 33020 RVA: 0x002F30DF File Offset: 0x002F12DF
		public bool HasResources()
		{
			return base.smi.master.resourceStorage.MassStored() >= base.smi.master.launchMass;
		}

		// Token: 0x060080FD RID: 33021 RVA: 0x002F310B File Offset: 0x002F130B
		public bool HasEnergy()
		{
			return base.smi.master.particleStorage.Particles > this.EnergyCost();
		}

		// Token: 0x060080FE RID: 33022 RVA: 0x002F312A File Offset: 0x002F132A
		public bool HasDestination()
		{
			return base.smi.master.destinationSelector.GetDestinationWorld() != base.smi.master.GetMyWorldId();
		}

		// Token: 0x060080FF RID: 33023 RVA: 0x002F3156 File Offset: 0x002F1356
		public bool IsDestinationReachable(bool forceRefresh = false)
		{
			if (forceRefresh)
			{
				this.UpdatePath();
			}
			return base.smi.master.destinationSelector.GetDestinationWorld() != base.smi.master.GetMyWorldId() && this.PathLength() != -1;
		}

		// Token: 0x06008100 RID: 33024 RVA: 0x002F3198 File Offset: 0x002F1398
		public int PathLength()
		{
			if (base.smi.m_cachedPath == null)
			{
				this.UpdatePath();
			}
			if (base.smi.m_cachedPath == null)
			{
				return -1;
			}
			int num = base.smi.m_cachedPath.Count;
			if (base.master.FreeStartHex)
			{
				num--;
			}
			if (base.master.FreeDestinationHex)
			{
				num--;
			}
			return num;
		}

		// Token: 0x06008101 RID: 33025 RVA: 0x002F31FC File Offset: 0x002F13FC
		public void UpdatePath()
		{
			this.m_cachedPath = ClusterGrid.Instance.GetPath(base.gameObject.GetMyWorldLocation(), base.smi.master.destinationSelector.GetDestination(), base.smi.master.destinationSelector);
		}

		// Token: 0x06008102 RID: 33026 RVA: 0x002F3249 File Offset: 0x002F1449
		public float EnergyCost()
		{
			return Mathf.Max(0f, 0f + (float)this.PathLength() * 10f);
		}

		// Token: 0x06008103 RID: 33027 RVA: 0x002F3268 File Offset: 0x002F1468
		public bool MayTurnOn()
		{
			return this.HasEnergy() && this.IsDestinationReachable(false) && base.master.operational.IsOperational && base.sm.allowedFromLogic.Get(this);
		}

		// Token: 0x04006262 RID: 25186
		public const int INVALID_PATH_LENGTH = -1;

		// Token: 0x04006263 RID: 25187
		private List<AxialI> m_cachedPath;
	}

	// Token: 0x0200136F RID: 4975
	public class States : GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun>
	{
		// Token: 0x06008104 RID: 33028 RVA: 0x002F32A0 File Offset: 0x002F14A0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			this.root.EventHandler(GameHashes.ClusterDestinationChanged, delegate(RailGun.StatesInstance smi)
			{
				smi.UpdatePath();
			});
			this.off.PlayAnim("off").EventTransition(GameHashes.OnParticleStorageChanged, this.on, (RailGun.StatesInstance smi) => smi.MayTurnOn()).EventTransition(GameHashes.ClusterDestinationChanged, this.on, (RailGun.StatesInstance smi) => smi.MayTurnOn()).EventTransition(GameHashes.OperationalChanged, this.on, (RailGun.StatesInstance smi) => smi.MayTurnOn()).ParamTransition<bool>(this.allowedFromLogic, this.on, (RailGun.StatesInstance smi, bool p) => smi.MayTurnOn());
			this.on.DefaultState(this.on.power_on).EventTransition(GameHashes.OperationalChanged, this.on.power_off, (RailGun.StatesInstance smi) => !smi.master.operational.IsOperational).EventTransition(GameHashes.ClusterDestinationChanged, this.on.power_off, (RailGun.StatesInstance smi) => !smi.IsDestinationReachable(false)).EventTransition(GameHashes.ClusterFogOfWarRevealed, (RailGun.StatesInstance smi) => Game.Instance, this.on.power_off, (RailGun.StatesInstance smi) => !smi.IsDestinationReachable(true)).EventTransition(GameHashes.OnParticleStorageChanged, this.on.power_off, (RailGun.StatesInstance smi) => !smi.MayTurnOn()).ParamTransition<bool>(this.allowedFromLogic, this.on.power_off, (RailGun.StatesInstance smi, bool p) => !p).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null);
			this.on.power_on.PlayAnim("power_on").OnAnimQueueComplete(this.on.wait_for_storage);
			this.on.power_off.PlayAnim("power_off").OnAnimQueueComplete(this.off);
			this.on.wait_for_storage.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.ClusterDestinationChanged, this.on.power_off, (RailGun.StatesInstance smi) => !smi.HasEnergy()).EventTransition(GameHashes.OnStorageChange, this.on.working, (RailGun.StatesInstance smi) => smi.HasResources() && smi.sm.cooldownTimer.Get(smi) <= 0f).EventTransition(GameHashes.OperationalChanged, this.on.working, (RailGun.StatesInstance smi) => smi.HasResources() && smi.sm.cooldownTimer.Get(smi) <= 0f).EventTransition(GameHashes.RailGunLaunchMassChanged, this.on.working, (RailGun.StatesInstance smi) => smi.HasResources() && smi.sm.cooldownTimer.Get(smi) <= 0f).ParamTransition<float>(this.cooldownTimer, this.on.cooldown, (RailGun.StatesInstance smi, float p) => p > 0f);
			this.on.working.DefaultState(this.on.working.pre).Enter(delegate(RailGun.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit(delegate(RailGun.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
			this.on.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.on.working.loop);
			this.on.working.loop.PlayAnim("working_loop").OnAnimQueueComplete(this.on.working.fire);
			this.on.working.fire.Enter(delegate(RailGun.StatesInstance smi)
			{
				if (smi.IsDestinationReachable(false))
				{
					smi.master.LaunchProjectile();
					smi.sm.payloadsFiredSinceCooldown.Delta(1, smi);
					if (smi.sm.payloadsFiredSinceCooldown.Get(smi) >= 6)
					{
						smi.sm.cooldownTimer.Set(30f, smi, false);
					}
				}
			}).GoTo(this.on.working.bounce);
			this.on.working.bounce.ParamTransition<float>(this.cooldownTimer, this.on.working.pst, (RailGun.StatesInstance smi, float p) => p > 0f || !smi.HasResources()).ParamTransition<int>(this.payloadsFiredSinceCooldown, this.on.working.loop, (RailGun.StatesInstance smi, int p) => p < 6 && smi.HasResources());
			this.on.working.pst.PlayAnim("working_pst").OnAnimQueueComplete(this.on.wait_for_storage);
			this.on.cooldown.DefaultState(this.on.cooldown.pre).ToggleMainStatusItem(Db.Get().BuildingStatusItems.RailGunCooldown, null);
			this.on.cooldown.pre.PlayAnim("cooldown_pre").OnAnimQueueComplete(this.on.cooldown.loop);
			this.on.cooldown.loop.PlayAnim("cooldown_loop", KAnim.PlayMode.Loop).ParamTransition<float>(this.cooldownTimer, this.on.cooldown.pst, (RailGun.StatesInstance smi, float p) => p <= 0f).Update(delegate(RailGun.StatesInstance smi, float dt)
			{
				this.cooldownTimer.Delta(-dt, smi);
			}, UpdateRate.SIM_1000ms, false);
			this.on.cooldown.pst.PlayAnim("cooldown_pst").OnAnimQueueComplete(this.on.wait_for_storage).Exit(delegate(RailGun.StatesInstance smi)
			{
				smi.sm.payloadsFiredSinceCooldown.Set(0, smi, false);
			});
		}

		// Token: 0x04006264 RID: 25188
		public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State off;

		// Token: 0x04006265 RID: 25189
		public RailGun.States.OnStates on;

		// Token: 0x04006266 RID: 25190
		public StateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.FloatParameter cooldownTimer;

		// Token: 0x04006267 RID: 25191
		public StateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.IntParameter payloadsFiredSinceCooldown;

		// Token: 0x04006268 RID: 25192
		public StateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.BoolParameter allowedFromLogic;

		// Token: 0x04006269 RID: 25193
		public StateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.BoolParameter updatePath;

		// Token: 0x0200210B RID: 8459
		public class WorkingStates : GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State
		{
			// Token: 0x04009403 RID: 37891
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State pre;

			// Token: 0x04009404 RID: 37892
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State loop;

			// Token: 0x04009405 RID: 37893
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State fire;

			// Token: 0x04009406 RID: 37894
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State bounce;

			// Token: 0x04009407 RID: 37895
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State pst;
		}

		// Token: 0x0200210C RID: 8460
		public class CooldownStates : GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State
		{
			// Token: 0x04009408 RID: 37896
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State pre;

			// Token: 0x04009409 RID: 37897
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State loop;

			// Token: 0x0400940A RID: 37898
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State pst;
		}

		// Token: 0x0200210D RID: 8461
		public class OnStates : GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State
		{
			// Token: 0x0400940B RID: 37899
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State power_on;

			// Token: 0x0400940C RID: 37900
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State wait_for_storage;

			// Token: 0x0400940D RID: 37901
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State power_off;

			// Token: 0x0400940E RID: 37902
			public RailGun.States.WorkingStates working;

			// Token: 0x0400940F RID: 37903
			public RailGun.States.CooldownStates cooldown;
		}
	}
}
