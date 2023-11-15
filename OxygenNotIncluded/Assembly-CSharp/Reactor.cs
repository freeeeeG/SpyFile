using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200067B RID: 1659
public class Reactor : StateMachineComponent<Reactor.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x06002C1A RID: 11290 RVA: 0x000EA0DE File Offset: 0x000E82DE
	// (set) Token: 0x06002C1B RID: 11291 RVA: 0x000EA0E6 File Offset: 0x000E82E6
	private float ReactionMassTarget
	{
		get
		{
			return this.reactionMassTarget;
		}
		set
		{
			this.fuelDelivery.capacity = value * 2f;
			this.fuelDelivery.refillMass = value * 0.2f;
			this.fuelDelivery.MinimumMass = value * 0.2f;
			this.reactionMassTarget = value;
		}
	}

	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x06002C1C RID: 11292 RVA: 0x000EA125 File Offset: 0x000E8325
	public float FuelTemperature
	{
		get
		{
			if (this.reactionStorage.items.Count > 0)
			{
				return this.reactionStorage.items[0].GetComponent<PrimaryElement>().Temperature;
			}
			return -1f;
		}
	}

	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x06002C1D RID: 11293 RVA: 0x000EA15C File Offset: 0x000E835C
	public float ReserveCoolantMass
	{
		get
		{
			PrimaryElement storedCoolant = this.GetStoredCoolant();
			if (!(storedCoolant == null))
			{
				return storedCoolant.Mass;
			}
			return 0f;
		}
	}

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x06002C1E RID: 11294 RVA: 0x000EA185 File Offset: 0x000E8385
	public bool On
	{
		get
		{
			return base.smi.IsInsideState(base.smi.sm.on);
		}
	}

	// Token: 0x06002C1F RID: 11295 RVA: 0x000EA1A4 File Offset: 0x000E83A4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.NuclearReactors.Add(this);
		Storage[] components = base.GetComponents<Storage>();
		this.supplyStorage = components[0];
		this.reactionStorage = components[1];
		this.wasteStorage = components[2];
		this.CreateMeters();
		base.smi.StartSM();
		this.fuelDelivery = base.GetComponent<ManualDeliveryKG>();
		this.CheckLogicInputValueChanged(true);
	}

	// Token: 0x06002C20 RID: 11296 RVA: 0x000EA208 File Offset: 0x000E8408
	protected override void OnCleanUp()
	{
		Components.NuclearReactors.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06002C21 RID: 11297 RVA: 0x000EA21B File Offset: 0x000E841B
	private void Update()
	{
		this.CheckLogicInputValueChanged(false);
	}

	// Token: 0x06002C22 RID: 11298 RVA: 0x000EA224 File Offset: 0x000E8424
	public Notification CreateMeltdownNotification()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		return new Notification(MISC.NOTIFICATIONS.REACTORMELTDOWN.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.REACTORMELTDOWN.TOOLTIP + notificationList.ReduceMessages(false), "/t• " + component.GetProperName(), false, 0f, null, null, null, true, false, false);
	}

	// Token: 0x06002C23 RID: 11299 RVA: 0x000EA283 File Offset: 0x000E8483
	public void SetStorages(Storage supply, Storage reaction, Storage waste)
	{
		this.supplyStorage = supply;
		this.reactionStorage = reaction;
		this.wasteStorage = waste;
	}

	// Token: 0x06002C24 RID: 11300 RVA: 0x000EA29C File Offset: 0x000E849C
	private void CheckLogicInputValueChanged(bool onLoad = false)
	{
		int num = 1;
		if (this.logicPorts.IsPortConnected("CONTROL_FUEL_DELIVERY"))
		{
			num = this.logicPorts.GetInputValue("CONTROL_FUEL_DELIVERY");
		}
		if (num == 0 && (this.fuelDeliveryEnabled || onLoad))
		{
			this.fuelDelivery.refillMass = -1f;
			this.fuelDeliveryEnabled = false;
			this.fuelDelivery.AbortDelivery("AutomationDisabled");
			return;
		}
		if (num == 1 && (!this.fuelDeliveryEnabled || onLoad))
		{
			this.fuelDelivery.refillMass = this.reactionMassTarget * 0.2f;
			this.fuelDeliveryEnabled = true;
		}
	}

	// Token: 0x06002C25 RID: 11301 RVA: 0x000EA33C File Offset: 0x000E853C
	private void OnLogicConnectionChanged(int value, bool connection)
	{
	}

	// Token: 0x06002C26 RID: 11302 RVA: 0x000EA340 File Offset: 0x000E8540
	private void CreateMeters()
	{
		this.temperatureMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "temperature_meter_target", "meter_temperature", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"temperature_meter_target"
		});
		this.waterMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "water_meter_target", "meter_water", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"water_meter_target"
		});
	}

	// Token: 0x06002C27 RID: 11303 RVA: 0x000EA3A8 File Offset: 0x000E85A8
	private void TransferFuel()
	{
		PrimaryElement activeFuel = this.GetActiveFuel();
		PrimaryElement storedFuel = this.GetStoredFuel();
		float num = (activeFuel != null) ? activeFuel.Mass : 0f;
		float num2 = (storedFuel != null) ? storedFuel.Mass : 0f;
		float num3 = this.ReactionMassTarget - num;
		num3 = Mathf.Min(num2, num3);
		if (num3 > 0.5f || num3 == num2)
		{
			this.supplyStorage.Transfer(this.reactionStorage, this.fuelTag, num3, false, true);
		}
	}

	// Token: 0x06002C28 RID: 11304 RVA: 0x000EA430 File Offset: 0x000E8630
	private void TransferCoolant()
	{
		PrimaryElement activeCoolant = this.GetActiveCoolant();
		PrimaryElement storedCoolant = this.GetStoredCoolant();
		float num = (activeCoolant != null) ? activeCoolant.Mass : 0f;
		float a = (storedCoolant != null) ? storedCoolant.Mass : 0f;
		float num2 = 30f - num;
		num2 = Mathf.Min(a, num2);
		if (num2 > 0f)
		{
			this.supplyStorage.Transfer(this.reactionStorage, this.coolantTag, num2, false, true);
		}
	}

	// Token: 0x06002C29 RID: 11305 RVA: 0x000EA4AC File Offset: 0x000E86AC
	private PrimaryElement GetStoredFuel()
	{
		GameObject gameObject = this.supplyStorage.FindFirst(this.fuelTag);
		if (gameObject && gameObject.GetComponent<PrimaryElement>())
		{
			return gameObject.GetComponent<PrimaryElement>();
		}
		return null;
	}

	// Token: 0x06002C2A RID: 11306 RVA: 0x000EA4E8 File Offset: 0x000E86E8
	private PrimaryElement GetActiveFuel()
	{
		GameObject gameObject = this.reactionStorage.FindFirst(this.fuelTag);
		if (gameObject && gameObject.GetComponent<PrimaryElement>())
		{
			return gameObject.GetComponent<PrimaryElement>();
		}
		return null;
	}

	// Token: 0x06002C2B RID: 11307 RVA: 0x000EA524 File Offset: 0x000E8724
	private PrimaryElement GetStoredCoolant()
	{
		GameObject gameObject = this.supplyStorage.FindFirst(this.coolantTag);
		if (gameObject && gameObject.GetComponent<PrimaryElement>())
		{
			return gameObject.GetComponent<PrimaryElement>();
		}
		return null;
	}

	// Token: 0x06002C2C RID: 11308 RVA: 0x000EA560 File Offset: 0x000E8760
	private PrimaryElement GetActiveCoolant()
	{
		GameObject gameObject = this.reactionStorage.FindFirst(this.coolantTag);
		if (gameObject && gameObject.GetComponent<PrimaryElement>())
		{
			return gameObject.GetComponent<PrimaryElement>();
		}
		return null;
	}

	// Token: 0x06002C2D RID: 11309 RVA: 0x000EA59C File Offset: 0x000E879C
	private bool CanStartReaction()
	{
		PrimaryElement activeCoolant = this.GetActiveCoolant();
		PrimaryElement activeFuel = this.GetActiveFuel();
		return activeCoolant && activeFuel && activeCoolant.Mass >= 30f && activeFuel.Mass >= 0.5f;
	}

	// Token: 0x06002C2E RID: 11310 RVA: 0x000EA5E4 File Offset: 0x000E87E4
	private void Cool(float dt)
	{
		PrimaryElement activeFuel = this.GetActiveFuel();
		if (activeFuel == null)
		{
			return;
		}
		PrimaryElement activeCoolant = this.GetActiveCoolant();
		if (activeCoolant == null)
		{
			return;
		}
		GameUtil.ForceConduction(activeFuel, activeCoolant, dt * 5f);
		if (activeCoolant.Temperature > 673.15f)
		{
			base.smi.sm.doVent.Trigger(base.smi);
		}
	}

	// Token: 0x06002C2F RID: 11311 RVA: 0x000EA64C File Offset: 0x000E884C
	private void React(float dt)
	{
		PrimaryElement activeFuel = this.GetActiveFuel();
		if (activeFuel != null && activeFuel.Mass >= 0.25f)
		{
			float num = GameUtil.EnergyToTemperatureDelta(-100f * dt * activeFuel.Mass, activeFuel);
			activeFuel.Temperature += num;
			this.spentFuel += dt * 0.016666668f;
		}
	}

	// Token: 0x06002C30 RID: 11312 RVA: 0x000EA6AD File Offset: 0x000E88AD
	private void SetEmitRads(float rads)
	{
		base.smi.master.radEmitter.emitRads = rads;
		base.smi.master.radEmitter.Refresh();
	}

	// Token: 0x06002C31 RID: 11313 RVA: 0x000EA6DC File Offset: 0x000E88DC
	private bool ReadyToCool()
	{
		PrimaryElement activeCoolant = this.GetActiveCoolant();
		return activeCoolant != null && activeCoolant.Mass > 0f;
	}

	// Token: 0x06002C32 RID: 11314 RVA: 0x000EA708 File Offset: 0x000E8908
	private void DumpSpentFuel()
	{
		PrimaryElement activeFuel = this.GetActiveFuel();
		if (activeFuel != null)
		{
			if (this.spentFuel <= 0f)
			{
				return;
			}
			float num = this.spentFuel * 100f;
			if (num > 0f)
			{
				GameObject go = ElementLoader.FindElementByHash(SimHashes.NuclearWaste).substance.SpawnResource(base.transform.position, num, activeFuel.Temperature, Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.id), Mathf.RoundToInt(num * 50f), false, false, false);
				go.AddTag(GameTags.Stored);
				this.wasteStorage.Store(go, true, false, true, false);
			}
			if (this.wasteStorage.MassStored() >= 100f)
			{
				this.wasteStorage.DropAll(true, true, default(Vector3), true, null);
			}
			if (this.spentFuel >= activeFuel.Mass)
			{
				Util.KDestroyGameObject(activeFuel.gameObject);
				this.spentFuel = 0f;
				return;
			}
			activeFuel.Mass -= this.spentFuel;
			this.spentFuel = 0f;
		}
	}

	// Token: 0x06002C33 RID: 11315 RVA: 0x000EA830 File Offset: 0x000E8A30
	private void UpdateVentStatus()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.ClearToVent())
		{
			if (component.HasStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure))
			{
				base.smi.sm.canVent.Set(true, base.smi, false);
				component.RemoveStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure, false);
				return;
			}
		}
		else if (!component.HasStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure))
		{
			base.smi.sm.canVent.Set(false, base.smi, false);
			component.AddStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure, null);
		}
	}

	// Token: 0x06002C34 RID: 11316 RVA: 0x000EA8E8 File Offset: 0x000E8AE8
	private void UpdateCoolantStatus()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.GetStoredCoolant() != null || base.smi.GetCurrentState() == base.smi.sm.meltdown || base.smi.GetCurrentState() == base.smi.sm.dead)
		{
			if (component.HasStatusItem(Db.Get().BuildingStatusItems.NoCoolant))
			{
				component.RemoveStatusItem(Db.Get().BuildingStatusItems.NoCoolant, false);
				return;
			}
		}
		else if (!component.HasStatusItem(Db.Get().BuildingStatusItems.NoCoolant))
		{
			component.AddStatusItem(Db.Get().BuildingStatusItems.NoCoolant, null);
		}
	}

	// Token: 0x06002C35 RID: 11317 RVA: 0x000EA9A4 File Offset: 0x000E8BA4
	private void InitVentCells()
	{
		if (this.ventCells == null)
		{
			this.ventCells = new int[]
			{
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.zero),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.right),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.left),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.right + Vector3.right),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.left + Vector3.left),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.down),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.down + Vector3.right),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.down + Vector3.left),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.down + Vector3.right + Vector3.right),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.down + Vector3.left + Vector3.left)
			};
		}
	}

	// Token: 0x06002C36 RID: 11318 RVA: 0x000EAC10 File Offset: 0x000E8E10
	public int GetVentCell()
	{
		this.InitVentCells();
		for (int i = 0; i < this.ventCells.Length; i++)
		{
			if (Grid.Mass[this.ventCells[i]] < 150f && !Grid.Solid[this.ventCells[i]])
			{
				return this.ventCells[i];
			}
		}
		return -1;
	}

	// Token: 0x06002C37 RID: 11319 RVA: 0x000EAC70 File Offset: 0x000E8E70
	private bool ClearToVent()
	{
		this.InitVentCells();
		for (int i = 0; i < this.ventCells.Length; i++)
		{
			if (Grid.Mass[this.ventCells[i]] < 150f && !Grid.Solid[this.ventCells[i]])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002C38 RID: 11320 RVA: 0x000EACC6 File Offset: 0x000E8EC6
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>();
	}

	// Token: 0x04001A06 RID: 6662
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001A07 RID: 6663
	[MyCmpGet]
	private RadiationEmitter radEmitter;

	// Token: 0x04001A08 RID: 6664
	[MyCmpGet]
	private ManualDeliveryKG fuelDelivery;

	// Token: 0x04001A09 RID: 6665
	private MeterController temperatureMeter;

	// Token: 0x04001A0A RID: 6666
	private MeterController waterMeter;

	// Token: 0x04001A0B RID: 6667
	private Storage supplyStorage;

	// Token: 0x04001A0C RID: 6668
	private Storage reactionStorage;

	// Token: 0x04001A0D RID: 6669
	private Storage wasteStorage;

	// Token: 0x04001A0E RID: 6670
	private Tag fuelTag = SimHashes.EnrichedUranium.CreateTag();

	// Token: 0x04001A0F RID: 6671
	private Tag coolantTag = GameTags.AnyWater;

	// Token: 0x04001A10 RID: 6672
	private Vector3 dumpOffset = new Vector3(0f, 5f, 0f);

	// Token: 0x04001A11 RID: 6673
	public static string MELTDOWN_STINGER = "Stinger_Loop_NuclearMeltdown";

	// Token: 0x04001A12 RID: 6674
	private static float meterFrameScaleHack = 3f;

	// Token: 0x04001A13 RID: 6675
	[Serialize]
	private float spentFuel;

	// Token: 0x04001A14 RID: 6676
	private float timeSinceMeltdownEmit;

	// Token: 0x04001A15 RID: 6677
	private const float reactorMeltDownBonusMassAmount = 10f;

	// Token: 0x04001A16 RID: 6678
	[MyCmpGet]
	private LogicPorts logicPorts;

	// Token: 0x04001A17 RID: 6679
	private LogicEventHandler fuelControlPort;

	// Token: 0x04001A18 RID: 6680
	private bool fuelDeliveryEnabled = true;

	// Token: 0x04001A19 RID: 6681
	public Guid refuelStausHandle;

	// Token: 0x04001A1A RID: 6682
	[Serialize]
	public int numCyclesRunning;

	// Token: 0x04001A1B RID: 6683
	private float reactionMassTarget = 60f;

	// Token: 0x04001A1C RID: 6684
	private int[] ventCells;

	// Token: 0x0200137F RID: 4991
	public class StatesInstance : GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.GameInstance
	{
		// Token: 0x06008152 RID: 33106 RVA: 0x002F493D File Offset: 0x002F2B3D
		public StatesInstance(Reactor smi) : base(smi)
		{
		}
	}

	// Token: 0x02001380 RID: 4992
	public class States : GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor>
	{
		// Token: 0x06008153 RID: 33107 RVA: 0x002F4948 File Offset: 0x002F2B48
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			default_state = this.off;
			this.root.EventHandler(GameHashes.OnStorageChange, delegate(Reactor.StatesInstance smi)
			{
				PrimaryElement storedCoolant = smi.master.GetStoredCoolant();
				if (!storedCoolant)
				{
					smi.master.waterMeter.SetPositionPercent(0f);
					return;
				}
				smi.master.waterMeter.SetPositionPercent(storedCoolant.Mass / 90f);
			});
			this.off_pre.QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.off);
			this.off.PlayAnim("off").Enter(delegate(Reactor.StatesInstance smi)
			{
				smi.master.radEmitter.SetEmitting(false);
				smi.master.SetEmitRads(0f);
			}).ParamTransition<bool>(this.reactionUnderway, this.on, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue).ParamTransition<bool>(this.melted, this.dead, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue).ParamTransition<bool>(this.meltingDown, this.meltdown, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue).Update(delegate(Reactor.StatesInstance smi, float dt)
			{
				smi.master.TransferFuel();
				smi.master.TransferCoolant();
				if (smi.master.CanStartReaction())
				{
					smi.GoTo(this.on);
				}
			}, UpdateRate.SIM_1000ms, false);
			this.on.Enter(delegate(Reactor.StatesInstance smi)
			{
				smi.sm.reactionUnderway.Set(true, smi, false);
				smi.master.operational.SetActive(true, false);
				smi.master.SetEmitRads(2400f);
				smi.master.radEmitter.SetEmitting(true);
			}).EventHandler(GameHashes.NewDay, (Reactor.StatesInstance smi) => GameClock.Instance, delegate(Reactor.StatesInstance smi)
			{
				smi.master.numCyclesRunning++;
			}).Exit(delegate(Reactor.StatesInstance smi)
			{
				smi.sm.reactionUnderway.Set(false, smi, false);
				smi.master.numCyclesRunning = 0;
			}).Update(delegate(Reactor.StatesInstance smi, float dt)
			{
				smi.master.TransferFuel();
				smi.master.TransferCoolant();
				smi.master.React(dt);
				smi.master.UpdateCoolantStatus();
				smi.master.UpdateVentStatus();
				smi.master.DumpSpentFuel();
				if (!smi.master.fuelDeliveryEnabled)
				{
					smi.master.refuelStausHandle = smi.master.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ReactorRefuelDisabled, null);
				}
				else
				{
					smi.master.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ReactorRefuelDisabled, false);
					smi.master.refuelStausHandle = Guid.Empty;
				}
				if (smi.master.GetActiveCoolant() != null)
				{
					smi.master.Cool(dt);
				}
				PrimaryElement activeFuel = smi.master.GetActiveFuel();
				if (activeFuel != null)
				{
					smi.master.temperatureMeter.SetPositionPercent(Mathf.Clamp01(activeFuel.Temperature / 3000f) / Reactor.meterFrameScaleHack);
					if (activeFuel.Temperature >= 3000f)
					{
						smi.sm.meltdownMassRemaining.Set(10f + smi.master.supplyStorage.MassStored() + smi.master.reactionStorage.MassStored() + smi.master.wasteStorage.MassStored(), smi, false);
						smi.master.supplyStorage.ConsumeAllIgnoringDisease();
						smi.master.reactionStorage.ConsumeAllIgnoringDisease();
						smi.master.wasteStorage.ConsumeAllIgnoringDisease();
						smi.GoTo(this.meltdown.pre);
						return;
					}
					if (activeFuel.Mass <= 0.25f)
					{
						smi.GoTo(this.off_pre);
						smi.master.temperatureMeter.SetPositionPercent(0f);
						return;
					}
				}
				else
				{
					smi.GoTo(this.off_pre);
					smi.master.temperatureMeter.SetPositionPercent(0f);
				}
			}, UpdateRate.SIM_200ms, false).DefaultState(this.on.pre);
			this.on.pre.PlayAnim("working_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.on.reacting).OnSignal(this.doVent, this.on.venting);
			this.on.reacting.PlayAnim("working_loop", KAnim.PlayMode.Loop).OnSignal(this.doVent, this.on.venting);
			this.on.venting.ParamTransition<bool>(this.canVent, this.on.venting.vent, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue).ParamTransition<bool>(this.canVent, this.on.venting.ventIssue, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsFalse);
			this.on.venting.ventIssue.PlayAnim("venting_issue", KAnim.PlayMode.Loop).ParamTransition<bool>(this.canVent, this.on.venting.vent, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue);
			this.on.venting.vent.PlayAnim("venting").Enter(delegate(Reactor.StatesInstance smi)
			{
				PrimaryElement activeCoolant = smi.master.GetActiveCoolant();
				if (activeCoolant != null)
				{
					activeCoolant.GetComponent<Dumpable>().Dump(Grid.CellToPos(smi.master.GetVentCell()));
				}
			}).OnAnimQueueComplete(this.on.reacting);
			this.meltdown.ToggleStatusItem(Db.Get().BuildingStatusItems.ReactorMeltdown, null).ToggleNotification((Reactor.StatesInstance smi) => smi.master.CreateMeltdownNotification()).ParamTransition<float>(this.meltdownMassRemaining, this.dead, (Reactor.StatesInstance smi, float p) => p <= 0f).ToggleTag(GameTags.DeadReactor).DefaultState(this.meltdown.loop);
			this.meltdown.pre.PlayAnim("almost_meltdown_pre", KAnim.PlayMode.Once).QueueAnim("almost_meltdown_loop", false, null).QueueAnim("meltdown_pre", false, null).OnAnimQueueComplete(this.meltdown.loop);
			this.meltdown.loop.PlayAnim("meltdown_loop", KAnim.PlayMode.Loop).Enter(delegate(Reactor.StatesInstance smi)
			{
				smi.master.radEmitter.SetEmitting(true);
				smi.master.SetEmitRads(4800f);
				smi.master.temperatureMeter.SetPositionPercent(1f / Reactor.meterFrameScaleHack);
				smi.master.UpdateCoolantStatus();
				if (this.meltingDown.Get(smi))
				{
					MusicManager.instance.PlaySong(Reactor.MELTDOWN_STINGER, false);
					MusicManager.instance.StopDynamicMusic(false);
				}
				else
				{
					MusicManager.instance.PlaySong(Reactor.MELTDOWN_STINGER, false);
					MusicManager.instance.SetSongParameter(Reactor.MELTDOWN_STINGER, "Music_PlayStinger", 1f, true);
					MusicManager.instance.StopDynamicMusic(false);
				}
				this.meltingDown.Set(true, smi, false);
			}).Exit(delegate(Reactor.StatesInstance smi)
			{
				this.meltingDown.Set(false, smi, false);
				MusicManager.instance.SetSongParameter(Reactor.MELTDOWN_STINGER, "Music_NuclearMeltdownActive", 0f, true);
			}).Update(delegate(Reactor.StatesInstance smi, float dt)
			{
				smi.master.timeSinceMeltdownEmit += dt;
				float num = 0.5f;
				float b = 5f;
				if (smi.master.timeSinceMeltdownEmit > num && smi.sm.meltdownMassRemaining.Get(smi) > 0f)
				{
					smi.master.timeSinceMeltdownEmit -= num;
					float num2 = Mathf.Min(smi.sm.meltdownMassRemaining.Get(smi), b);
					smi.sm.meltdownMassRemaining.Delta(-num2, smi);
					for (int i = 0; i < 3; i++)
					{
						if (num2 >= NuclearWasteCometConfig.MASS)
						{
							GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(NuclearWasteCometConfig.ID), smi.master.transform.position + Vector3.up * 2f, Quaternion.identity, null, null, true, 0);
							gameObject.SetActive(true);
							Comet component = gameObject.GetComponent<Comet>();
							component.ignoreObstacleForDamage.Set(smi.master.gameObject.GetComponent<KPrefabID>());
							component.addTiles = 1;
							int num3 = 270;
							while (num3 > 225 && num3 < 335)
							{
								num3 = UnityEngine.Random.Range(0, 360);
							}
							float f = (float)num3 * 3.1415927f / 180f;
							component.Velocity = new Vector2(-Mathf.Cos(f) * 20f, Mathf.Sin(f) * 20f);
							component.GetComponent<KBatchedAnimController>().Rotation = (float)(-(float)num3) - 90f;
							num2 -= NuclearWasteCometConfig.MASS;
						}
					}
					for (int j = 0; j < 3; j++)
					{
						if (num2 >= 0.001f)
						{
							SimMessages.AddRemoveSubstance(Grid.PosToCell(smi.master.transform.position + Vector3.up * 3f + Vector3.right * (float)j * 2f), SimHashes.NuclearWaste, CellEventLogger.Instance.ElementEmitted, num2 / 3f, 3000f, Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.Id), Mathf.RoundToInt(50f * (num2 / 3f)), true, -1);
						}
					}
				}
			}, UpdateRate.SIM_200ms, false);
			this.dead.PlayAnim("dead").ToggleTag(GameTags.DeadReactor).Enter(delegate(Reactor.StatesInstance smi)
			{
				smi.master.temperatureMeter.SetPositionPercent(1f / Reactor.meterFrameScaleHack);
				smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.DeadReactorCoolingOff, smi);
				this.melted.Set(true, smi, false);
			}).Exit(delegate(Reactor.StatesInstance smi)
			{
				smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.DeadReactorCoolingOff, false);
			}).Update(delegate(Reactor.StatesInstance smi, float dt)
			{
				smi.sm.timeSinceMeltdown.Delta(dt, smi);
				smi.master.radEmitter.emitRads = Mathf.Lerp(4800f, 0f, smi.sm.timeSinceMeltdown.Get(smi) / 3000f);
				smi.master.radEmitter.Refresh();
			}, UpdateRate.SIM_200ms, false);
		}

		// Token: 0x0400629F RID: 25247
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.Signal doVent;

		// Token: 0x040062A0 RID: 25248
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter canVent = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter(true);

		// Token: 0x040062A1 RID: 25249
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter reactionUnderway = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter();

		// Token: 0x040062A2 RID: 25250
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.FloatParameter meltdownMassRemaining = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.FloatParameter(0f);

		// Token: 0x040062A3 RID: 25251
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.FloatParameter timeSinceMeltdown = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.FloatParameter(0f);

		// Token: 0x040062A4 RID: 25252
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter meltingDown = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter(false);

		// Token: 0x040062A5 RID: 25253
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter melted = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter(false);

		// Token: 0x040062A6 RID: 25254
		public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State off;

		// Token: 0x040062A7 RID: 25255
		public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State off_pre;

		// Token: 0x040062A8 RID: 25256
		public Reactor.States.ReactingStates on;

		// Token: 0x040062A9 RID: 25257
		public Reactor.States.MeltdownStates meltdown;

		// Token: 0x040062AA RID: 25258
		public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State dead;

		// Token: 0x02002112 RID: 8466
		public class ReactingStates : GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State
		{
			// Token: 0x04009437 RID: 37943
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State pre;

			// Token: 0x04009438 RID: 37944
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State reacting;

			// Token: 0x04009439 RID: 37945
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State pst;

			// Token: 0x0400943A RID: 37946
			public Reactor.States.ReactingStates.VentingStates venting;

			// Token: 0x02002F5F RID: 12127
			public class VentingStates : GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State
			{
				// Token: 0x0400C1A1 RID: 49569
				public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State ventIssue;

				// Token: 0x0400C1A2 RID: 49570
				public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State vent;
			}
		}

		// Token: 0x02002113 RID: 8467
		public class MeltdownStates : GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State
		{
			// Token: 0x0400943B RID: 37947
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State almost_pre;

			// Token: 0x0400943C RID: 37948
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State almost_loop;

			// Token: 0x0400943D RID: 37949
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State pre;

			// Token: 0x0400943E RID: 37950
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State loop;
		}
	}
}
