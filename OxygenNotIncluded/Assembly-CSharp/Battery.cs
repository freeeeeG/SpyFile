using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005BC RID: 1468
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name}")]
[AddComponentMenu("KMonoBehaviour/scripts/Battery")]
public class Battery : KMonoBehaviour, IEnergyConsumer, ICircuitConnected, IGameObjectEffectDescriptor, IEnergyProducer
{
	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x06002400 RID: 9216 RVA: 0x000C4FB1 File Offset: 0x000C31B1
	// (set) Token: 0x06002401 RID: 9217 RVA: 0x000C4FB9 File Offset: 0x000C31B9
	public float WattsUsed { get; private set; }

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x06002402 RID: 9218 RVA: 0x000C4FC2 File Offset: 0x000C31C2
	public float WattsNeededWhenActive
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x06002403 RID: 9219 RVA: 0x000C4FC9 File Offset: 0x000C31C9
	public float PercentFull
	{
		get
		{
			return this.joulesAvailable / this.capacity;
		}
	}

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x06002404 RID: 9220 RVA: 0x000C4FD8 File Offset: 0x000C31D8
	public float PreviousPercentFull
	{
		get
		{
			return this.PreviousJoulesAvailable / this.capacity;
		}
	}

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x06002405 RID: 9221 RVA: 0x000C4FE7 File Offset: 0x000C31E7
	public float JoulesAvailable
	{
		get
		{
			return this.joulesAvailable;
		}
	}

	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x06002406 RID: 9222 RVA: 0x000C4FEF File Offset: 0x000C31EF
	public float Capacity
	{
		get
		{
			return this.capacity;
		}
	}

	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x06002407 RID: 9223 RVA: 0x000C4FF7 File Offset: 0x000C31F7
	// (set) Token: 0x06002408 RID: 9224 RVA: 0x000C4FFF File Offset: 0x000C31FF
	public float ChargeCapacity { get; private set; }

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x06002409 RID: 9225 RVA: 0x000C5008 File Offset: 0x000C3208
	public int PowerSortOrder
	{
		get
		{
			return this.powerSortOrder;
		}
	}

	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x0600240A RID: 9226 RVA: 0x000C5010 File Offset: 0x000C3210
	public string Name
	{
		get
		{
			return base.GetComponent<KSelectable>().GetName();
		}
	}

	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x0600240B RID: 9227 RVA: 0x000C501D File Offset: 0x000C321D
	// (set) Token: 0x0600240C RID: 9228 RVA: 0x000C5025 File Offset: 0x000C3225
	public int PowerCell { get; private set; }

	// Token: 0x170001BA RID: 442
	// (get) Token: 0x0600240D RID: 9229 RVA: 0x000C502E File Offset: 0x000C322E
	public ushort CircuitID
	{
		get
		{
			return Game.Instance.circuitManager.GetCircuitID(this);
		}
	}

	// Token: 0x170001BB RID: 443
	// (get) Token: 0x0600240E RID: 9230 RVA: 0x000C5040 File Offset: 0x000C3240
	public bool IsConnected
	{
		get
		{
			return this.connectionStatus > CircuitManager.ConnectionStatus.NotConnected;
		}
	}

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x0600240F RID: 9231 RVA: 0x000C504B File Offset: 0x000C324B
	public bool IsPowered
	{
		get
		{
			return this.connectionStatus == CircuitManager.ConnectionStatus.Powered;
		}
	}

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x06002410 RID: 9232 RVA: 0x000C5056 File Offset: 0x000C3256
	// (set) Token: 0x06002411 RID: 9233 RVA: 0x000C505E File Offset: 0x000C325E
	public bool IsVirtual { get; protected set; }

	// Token: 0x170001BE RID: 446
	// (get) Token: 0x06002412 RID: 9234 RVA: 0x000C5067 File Offset: 0x000C3267
	// (set) Token: 0x06002413 RID: 9235 RVA: 0x000C506F File Offset: 0x000C326F
	public object VirtualCircuitKey { get; protected set; }

	// Token: 0x06002414 RID: 9236 RVA: 0x000C5078 File Offset: 0x000C3278
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Batteries.Add(this);
		Building component = base.GetComponent<Building>();
		this.PowerCell = component.GetPowerInputCell();
		base.Subscribe<Battery>(-1582839653, Battery.OnTagsChangedDelegate);
		this.OnTagsChanged(null);
		this.meter = (base.GetComponent<PowerTransformer>() ? null : new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		}));
		Game.Instance.circuitManager.Connect(this);
		Game.Instance.energySim.AddBattery(this);
	}

	// Token: 0x06002415 RID: 9237 RVA: 0x000C5138 File Offset: 0x000C3338
	private void OnTagsChanged(object data)
	{
		if (this.HasAllTags(this.connectedTags))
		{
			Game.Instance.circuitManager.Connect(this);
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.JoulesAvailable, this);
			return;
		}
		Game.Instance.circuitManager.Disconnect(this, false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.JoulesAvailable, false);
	}

	// Token: 0x06002416 RID: 9238 RVA: 0x000C51BC File Offset: 0x000C33BC
	protected override void OnCleanUp()
	{
		Game.Instance.energySim.RemoveBattery(this);
		Game.Instance.circuitManager.Disconnect(this, true);
		Components.Batteries.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06002417 RID: 9239 RVA: 0x000C51F0 File Offset: 0x000C33F0
	public virtual void EnergySim200ms(float dt)
	{
		this.dt = dt;
		this.joulesConsumed = 0f;
		this.WattsUsed = 0f;
		this.ChargeCapacity = this.chargeWattage * dt;
		if (this.meter != null)
		{
			float percentFull = this.PercentFull;
			this.meter.SetPositionPercent(percentFull);
		}
		this.UpdateSounds();
		this.PreviousJoulesAvailable = this.JoulesAvailable;
		this.ConsumeEnergy(this.joulesLostPerSecond * dt, true);
	}

	// Token: 0x06002418 RID: 9240 RVA: 0x000C5264 File Offset: 0x000C3464
	private void UpdateSounds()
	{
		float previousPercentFull = this.PreviousPercentFull;
		float percentFull = this.PercentFull;
		if (percentFull == 0f && previousPercentFull != 0f)
		{
			base.GetComponent<LoopingSounds>().PlayEvent(GameSoundEvents.BatteryDischarged);
		}
		if (percentFull > 0.999f && previousPercentFull <= 0.999f)
		{
			base.GetComponent<LoopingSounds>().PlayEvent(GameSoundEvents.BatteryFull);
		}
		if (percentFull < 0.25f && previousPercentFull >= 0.25f)
		{
			base.GetComponent<LoopingSounds>().PlayEvent(GameSoundEvents.BatteryWarning);
		}
	}

	// Token: 0x06002419 RID: 9241 RVA: 0x000C52E0 File Offset: 0x000C34E0
	public void SetConnectionStatus(CircuitManager.ConnectionStatus status)
	{
		this.connectionStatus = status;
		if (status == CircuitManager.ConnectionStatus.NotConnected)
		{
			this.operational.SetActive(false, false);
			return;
		}
		this.operational.SetActive(this.operational.IsOperational && this.JoulesAvailable > 0f, false);
	}

	// Token: 0x0600241A RID: 9242 RVA: 0x000C5330 File Offset: 0x000C3530
	public void AddEnergy(float joules)
	{
		this.joulesAvailable = Mathf.Min(this.capacity, this.JoulesAvailable + joules);
		this.joulesConsumed += joules;
		this.ChargeCapacity -= joules;
		this.WattsUsed = this.joulesConsumed / this.dt;
	}

	// Token: 0x0600241B RID: 9243 RVA: 0x000C5388 File Offset: 0x000C3588
	public void ConsumeEnergy(float joules, bool report = false)
	{
		if (report)
		{
			float num = Mathf.Min(this.JoulesAvailable, joules);
			ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyWasted, -num, StringFormatter.Replace(BUILDINGS.PREFABS.BATTERY.CHARGE_LOSS, "{Battery}", this.GetProperName()), null);
		}
		this.joulesAvailable = Mathf.Max(0f, this.JoulesAvailable - joules);
	}

	// Token: 0x0600241C RID: 9244 RVA: 0x000C53E6 File Offset: 0x000C35E6
	public void ConsumeEnergy(float joules)
	{
		this.ConsumeEnergy(joules, false);
	}

	// Token: 0x0600241D RID: 9245 RVA: 0x000C53F0 File Offset: 0x000C35F0
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.powerTransformer == null)
		{
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.REQUIRESPOWERGENERATOR, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESPOWERGENERATOR, Descriptor.DescriptorType.Requirement, false));
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.BATTERYCAPACITY, GameUtil.GetFormattedJoules(this.capacity, "", GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.BATTERYCAPACITY, GameUtil.GetFormattedJoules(this.capacity, "", GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false));
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.BATTERYLEAK, GameUtil.GetFormattedJoules(this.joulesLostPerSecond, "F1", GameUtil.TimeSlice.PerCycle)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.BATTERYLEAK, GameUtil.GetFormattedJoules(this.joulesLostPerSecond, "F1", GameUtil.TimeSlice.PerCycle)), Descriptor.DescriptorType.Effect, false));
		}
		else
		{
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.TRANSFORMER_INPUT_WIRE, UI.BUILDINGEFFECTS.TOOLTIPS.TRANSFORMER_INPUT_WIRE, Descriptor.DescriptorType.Requirement, false));
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.TRANSFORMER_OUTPUT_WIRE, GameUtil.GetFormattedWattage(this.capacity, GameUtil.WattageFormatterUnit.Automatic, true)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.TRANSFORMER_OUTPUT_WIRE, GameUtil.GetFormattedWattage(this.capacity, GameUtil.WattageFormatterUnit.Automatic, true)), Descriptor.DescriptorType.Requirement, false));
		}
		return list;
	}

	// Token: 0x0600241E RID: 9246 RVA: 0x000C5538 File Offset: 0x000C3738
	[ContextMenu("Refill Power")]
	public void DEBUG_RefillPower()
	{
		this.joulesAvailable = this.capacity;
	}

	// Token: 0x040014A8 RID: 5288
	[SerializeField]
	public float capacity;

	// Token: 0x040014A9 RID: 5289
	[SerializeField]
	public float chargeWattage = float.PositiveInfinity;

	// Token: 0x040014AA RID: 5290
	[Serialize]
	private float joulesAvailable;

	// Token: 0x040014AB RID: 5291
	[MyCmpGet]
	protected Operational operational;

	// Token: 0x040014AC RID: 5292
	[MyCmpGet]
	public PowerTransformer powerTransformer;

	// Token: 0x040014AD RID: 5293
	protected MeterController meter;

	// Token: 0x040014AF RID: 5295
	public float joulesLostPerSecond;

	// Token: 0x040014B1 RID: 5297
	[SerializeField]
	public int powerSortOrder;

	// Token: 0x040014B5 RID: 5301
	private float PreviousJoulesAvailable;

	// Token: 0x040014B6 RID: 5302
	private CircuitManager.ConnectionStatus connectionStatus;

	// Token: 0x040014B7 RID: 5303
	public static readonly Tag[] DEFAULT_CONNECTED_TAGS = new Tag[]
	{
		GameTags.Operational
	};

	// Token: 0x040014B8 RID: 5304
	[SerializeField]
	public Tag[] connectedTags = Battery.DEFAULT_CONNECTED_TAGS;

	// Token: 0x040014B9 RID: 5305
	private static readonly EventSystem.IntraObjectHandler<Battery> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Battery>(delegate(Battery component, object data)
	{
		component.OnTagsChanged(data);
	});

	// Token: 0x040014BA RID: 5306
	private float dt;

	// Token: 0x040014BB RID: 5307
	private float joulesConsumed;
}
