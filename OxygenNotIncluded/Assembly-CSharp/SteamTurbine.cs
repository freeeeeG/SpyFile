using System;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000696 RID: 1686
public class SteamTurbine : Generator
{
	// Token: 0x170002FC RID: 764
	// (get) Token: 0x06002D32 RID: 11570 RVA: 0x000EFD79 File Offset: 0x000EDF79
	// (set) Token: 0x06002D33 RID: 11571 RVA: 0x000EFD81 File Offset: 0x000EDF81
	public int BlockedInputs { get; private set; }

	// Token: 0x170002FD RID: 765
	// (get) Token: 0x06002D34 RID: 11572 RVA: 0x000EFD8A File Offset: 0x000EDF8A
	public int TotalInputs
	{
		get
		{
			return this.srcCells.Length;
		}
	}

	// Token: 0x06002D35 RID: 11573 RVA: 0x000EFD94 File Offset: 0x000EDF94
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.accumulator = Game.Instance.accumulators.Add("Power", this);
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		this.simEmitCBHandle = Game.Instance.massEmitCallbackManager.Add(new Action<Sim.MassEmittedCallback, object>(SteamTurbine.OnSimEmittedCallback), this, "SteamTurbineEmit");
		BuildingDef def = base.GetComponent<BuildingComplete>().Def;
		this.srcCells = new int[def.WidthInCells];
		int cell = Grid.PosToCell(this);
		for (int i = 0; i < def.WidthInCells; i++)
		{
			int x = i - (def.WidthInCells - 1) / 2;
			this.srcCells[i] = Grid.OffsetCell(cell, new CellOffset(x, -2));
		}
		this.smi = new SteamTurbine.Instance(this);
		this.smi.StartSM();
		this.CreateMeter();
	}

	// Token: 0x06002D36 RID: 11574 RVA: 0x000EFE74 File Offset: 0x000EE074
	private void CreateMeter()
	{
		this.meter = new MeterController(base.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_OL",
			"meter_frame",
			"meter_fill"
		});
	}

	// Token: 0x06002D37 RID: 11575 RVA: 0x000EFEC4 File Offset: 0x000EE0C4
	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("cleanup");
		}
		Game.Instance.massEmitCallbackManager.Release(this.simEmitCBHandle, "SteamTurbine");
		this.simEmitCBHandle.Clear();
		base.OnCleanUp();
	}

	// Token: 0x06002D38 RID: 11576 RVA: 0x000EFF18 File Offset: 0x000EE118
	private void Pump(float dt)
	{
		float mass = this.pumpKGRate * dt / (float)this.srcCells.Length;
		foreach (int gameCell in this.srcCells)
		{
			HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(SteamTurbine.OnSimConsumeCallback), this, "SteamTurbineConsume");
			SimMessages.ConsumeMass(gameCell, this.srcElem, mass, 1, handle.index);
		}
	}

	// Token: 0x06002D39 RID: 11577 RVA: 0x000EFF86 File Offset: 0x000EE186
	private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		((SteamTurbine)data).OnSimConsume(mass_cb_info);
	}

	// Token: 0x06002D3A RID: 11578 RVA: 0x000EFF94 File Offset: 0x000EE194
	private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
	{
		if (mass_cb_info.mass > 0f)
		{
			this.storedTemperature = SimUtil.CalculateFinalTemperature(this.storedMass, this.storedTemperature, mass_cb_info.mass, mass_cb_info.temperature);
			this.storedMass += mass_cb_info.mass;
			SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(this.diseaseIdx, this.diseaseCount, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
			this.diseaseIdx = diseaseInfo.idx;
			this.diseaseCount = diseaseInfo.count;
			if (this.storedMass > this.minConvertMass && this.simEmitCBHandle.IsValid())
			{
				Game.Instance.massEmitCallbackManager.GetItem(this.simEmitCBHandle);
				this.gasStorage.AddGasChunk(this.srcElem, this.storedMass, this.storedTemperature, this.diseaseIdx, this.diseaseCount, true, true);
				this.storedMass = 0f;
				this.storedTemperature = 0f;
				this.diseaseIdx = byte.MaxValue;
				this.diseaseCount = 0;
			}
		}
	}

	// Token: 0x06002D3B RID: 11579 RVA: 0x000F00A2 File Offset: 0x000EE2A2
	private static void OnSimEmittedCallback(Sim.MassEmittedCallback info, object data)
	{
		((SteamTurbine)data).OnSimEmitted(info);
	}

	// Token: 0x06002D3C RID: 11580 RVA: 0x000F00B0 File Offset: 0x000EE2B0
	private void OnSimEmitted(Sim.MassEmittedCallback info)
	{
		if (info.suceeded != 1)
		{
			this.storedTemperature = SimUtil.CalculateFinalTemperature(this.storedMass, this.storedTemperature, info.mass, info.temperature);
			this.storedMass += info.mass;
			if (info.diseaseIdx != 255)
			{
				SimUtil.DiseaseInfo a = new SimUtil.DiseaseInfo
				{
					idx = this.diseaseIdx,
					count = this.diseaseCount
				};
				SimUtil.DiseaseInfo b = new SimUtil.DiseaseInfo
				{
					idx = info.diseaseIdx,
					count = info.diseaseCount
				};
				SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(a, b);
				this.diseaseIdx = diseaseInfo.idx;
				this.diseaseCount = diseaseInfo.count;
			}
		}
	}

	// Token: 0x06002D3D RID: 11581 RVA: 0x000F0174 File Offset: 0x000EE374
	public static void InitializeStatusItems()
	{
		SteamTurbine.activeStatusItem = new StatusItem("TURBINE_ACTIVE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022, null);
		SteamTurbine.inputBlockedStatusItem = new StatusItem("TURBINE_BLOCKED_INPUT", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		SteamTurbine.inputPartiallyBlockedStatusItem = new StatusItem("TURBINE_PARTIALLY_BLOCKED_INPUT", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		SteamTurbine.inputPartiallyBlockedStatusItem.resolveStringCallback = new Func<string, object, string>(SteamTurbine.ResolvePartialBlockedStatus);
		SteamTurbine.insufficientMassStatusItem = new StatusItem("TURBINE_INSUFFICIENT_MASS", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022, null);
		SteamTurbine.insufficientMassStatusItem.resolveStringCallback = new Func<string, object, string>(SteamTurbine.ResolveStrings);
		SteamTurbine.buildingTooHotItem = new StatusItem("TURBINE_TOO_HOT", "BUILDING", "status_item_plant_temperature", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		SteamTurbine.buildingTooHotItem.resolveTooltipCallback = new Func<string, object, string>(SteamTurbine.ResolveStrings);
		SteamTurbine.insufficientTemperatureStatusItem = new StatusItem("TURBINE_INSUFFICIENT_TEMPERATURE", "BUILDING", "status_item_plant_temperature", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022, null);
		SteamTurbine.insufficientTemperatureStatusItem.resolveStringCallback = new Func<string, object, string>(SteamTurbine.ResolveStrings);
		SteamTurbine.insufficientTemperatureStatusItem.resolveTooltipCallback = new Func<string, object, string>(SteamTurbine.ResolveStrings);
		SteamTurbine.activeWattageStatusItem = new StatusItem("TURBINE_ACTIVE_WATTAGE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022, null);
		SteamTurbine.activeWattageStatusItem.resolveStringCallback = new Func<string, object, string>(SteamTurbine.ResolveWattageStatus);
	}

	// Token: 0x06002D3E RID: 11582 RVA: 0x000F0320 File Offset: 0x000EE520
	private static string ResolveWattageStatus(string str, object data)
	{
		SteamTurbine steamTurbine = (SteamTurbine)data;
		float num = Game.Instance.accumulators.GetAverageRate(steamTurbine.accumulator) / steamTurbine.WattageRating;
		return str.Replace("{Wattage}", GameUtil.GetFormattedWattage(steamTurbine.CurrentWattage, GameUtil.WattageFormatterUnit.Automatic, true)).Replace("{Max_Wattage}", GameUtil.GetFormattedWattage(steamTurbine.WattageRating, GameUtil.WattageFormatterUnit.Automatic, true)).Replace("{Efficiency}", GameUtil.GetFormattedPercent(num * 100f, GameUtil.TimeSlice.None)).Replace("{Src_Element}", ElementLoader.FindElementByHash(steamTurbine.srcElem).name);
	}

	// Token: 0x06002D3F RID: 11583 RVA: 0x000F03B4 File Offset: 0x000EE5B4
	private static string ResolvePartialBlockedStatus(string str, object data)
	{
		SteamTurbine steamTurbine = (SteamTurbine)data;
		return str.Replace("{Blocked}", steamTurbine.BlockedInputs.ToString()).Replace("{Total}", steamTurbine.TotalInputs.ToString());
	}

	// Token: 0x06002D40 RID: 11584 RVA: 0x000F03FC File Offset: 0x000EE5FC
	private static string ResolveStrings(string str, object data)
	{
		SteamTurbine steamTurbine = (SteamTurbine)data;
		str = str.Replace("{Src_Element}", ElementLoader.FindElementByHash(steamTurbine.srcElem).name);
		str = str.Replace("{Dest_Element}", ElementLoader.FindElementByHash(steamTurbine.destElem).name);
		str = str.Replace("{Overheat_Temperature}", GameUtil.GetFormattedTemperature(steamTurbine.maxBuildingTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		str = str.Replace("{Active_Temperature}", GameUtil.GetFormattedTemperature(steamTurbine.minActiveTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		str = str.Replace("{Min_Mass}", GameUtil.GetFormattedMass(steamTurbine.requiredMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		return str;
	}

	// Token: 0x06002D41 RID: 11585 RVA: 0x000F04A3 File Offset: 0x000EE6A3
	public void SetStorage(Storage steamStorage, Storage waterStorage)
	{
		this.gasStorage = steamStorage;
		this.liquidStorage = waterStorage;
	}

	// Token: 0x06002D42 RID: 11586 RVA: 0x000F04B4 File Offset: 0x000EE6B4
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		if (!this.operational.IsOperational)
		{
			return;
		}
		float num = 0f;
		if (this.gasStorage != null && this.gasStorage.items.Count > 0)
		{
			GameObject gameObject = this.gasStorage.FindFirst(ElementLoader.FindElementByHash(this.srcElem).tag);
			if (gameObject != null)
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				float num2 = 0.1f;
				if (component.Mass > num2)
				{
					num2 = Mathf.Min(component.Mass, this.pumpKGRate * dt);
					num = Mathf.Min(this.JoulesToGenerate(component) * (num2 / this.pumpKGRate), base.WattageRating * dt);
					float num3 = this.HeatFromCoolingSteam(component) * (num2 / component.Mass);
					float num4 = num2 / component.Mass;
					int num5 = Mathf.RoundToInt((float)component.DiseaseCount * num4);
					component.Mass -= num2;
					component.ModifyDiseaseCount(-num5, "SteamTurbine.EnergySim200ms");
					float display_dt = (this.lastSampleTime > 0f) ? (Time.time - this.lastSampleTime) : 1f;
					this.lastSampleTime = Time.time;
					GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, num3 * this.wasteHeatToTurbinePercent, BUILDINGS.PREFABS.STEAMTURBINE2.HEAT_SOURCE, display_dt);
					this.liquidStorage.AddLiquid(this.destElem, num2, this.outputElementTemperature, component.DiseaseIdx, num5, true, true);
				}
			}
		}
		num = Mathf.Clamp(num, 0f, base.WattageRating);
		Game.Instance.accumulators.Accumulate(this.accumulator, num);
		if (num > 0f)
		{
			base.GenerateJoules(num, false);
		}
		this.meter.SetPositionPercent(Game.Instance.accumulators.GetAverageRate(this.accumulator) / base.WattageRating);
		this.meter.SetSymbolTint(SteamTurbine.TINT_SYMBOL, Color.Lerp(Color.red, Color.green, Game.Instance.accumulators.GetAverageRate(this.accumulator) / base.WattageRating));
	}

	// Token: 0x06002D43 RID: 11587 RVA: 0x000F0704 File Offset: 0x000EE904
	public float HeatFromCoolingSteam(PrimaryElement steam)
	{
		float temperature = steam.Temperature;
		return -GameUtil.CalculateEnergyDeltaForElement(steam, temperature, this.outputElementTemperature);
	}

	// Token: 0x06002D44 RID: 11588 RVA: 0x000F0728 File Offset: 0x000EE928
	public float JoulesToGenerate(PrimaryElement steam)
	{
		float num = (steam.Temperature - this.outputElementTemperature) / (this.idealSourceElementTemperature - this.outputElementTemperature);
		return base.WattageRating * (float)Math.Pow((double)num, 1.0);
	}

	// Token: 0x170002FE RID: 766
	// (get) Token: 0x06002D45 RID: 11589 RVA: 0x000F0769 File Offset: 0x000EE969
	public float CurrentWattage
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x04001AAA RID: 6826
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04001AAB RID: 6827
	public SimHashes srcElem;

	// Token: 0x04001AAC RID: 6828
	public SimHashes destElem;

	// Token: 0x04001AAD RID: 6829
	public float requiredMass = 0.001f;

	// Token: 0x04001AAE RID: 6830
	public float minActiveTemperature = 398.15f;

	// Token: 0x04001AAF RID: 6831
	public float idealSourceElementTemperature = 473.15f;

	// Token: 0x04001AB0 RID: 6832
	public float maxBuildingTemperature = 373.15f;

	// Token: 0x04001AB1 RID: 6833
	public float outputElementTemperature = 368.15f;

	// Token: 0x04001AB2 RID: 6834
	public float minConvertMass;

	// Token: 0x04001AB3 RID: 6835
	public float pumpKGRate;

	// Token: 0x04001AB4 RID: 6836
	public float maxSelfHeat;

	// Token: 0x04001AB5 RID: 6837
	public float wasteHeatToTurbinePercent;

	// Token: 0x04001AB6 RID: 6838
	private static readonly HashedString TINT_SYMBOL = new HashedString("meter_fill");

	// Token: 0x04001AB7 RID: 6839
	[Serialize]
	private float storedMass;

	// Token: 0x04001AB8 RID: 6840
	[Serialize]
	private float storedTemperature;

	// Token: 0x04001AB9 RID: 6841
	[Serialize]
	private byte diseaseIdx = byte.MaxValue;

	// Token: 0x04001ABA RID: 6842
	[Serialize]
	private int diseaseCount;

	// Token: 0x04001ABB RID: 6843
	private static StatusItem inputBlockedStatusItem;

	// Token: 0x04001ABC RID: 6844
	private static StatusItem inputPartiallyBlockedStatusItem;

	// Token: 0x04001ABD RID: 6845
	private static StatusItem insufficientMassStatusItem;

	// Token: 0x04001ABE RID: 6846
	private static StatusItem insufficientTemperatureStatusItem;

	// Token: 0x04001ABF RID: 6847
	private static StatusItem activeWattageStatusItem;

	// Token: 0x04001AC0 RID: 6848
	private static StatusItem buildingTooHotItem;

	// Token: 0x04001AC1 RID: 6849
	private static StatusItem activeStatusItem;

	// Token: 0x04001AC3 RID: 6851
	private const Sim.Cell.Properties floorCellProperties = (Sim.Cell.Properties)39;

	// Token: 0x04001AC4 RID: 6852
	private MeterController meter;

	// Token: 0x04001AC5 RID: 6853
	private HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle simEmitCBHandle = HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.InvalidHandle;

	// Token: 0x04001AC6 RID: 6854
	private SteamTurbine.Instance smi;

	// Token: 0x04001AC7 RID: 6855
	private int[] srcCells;

	// Token: 0x04001AC8 RID: 6856
	private Storage gasStorage;

	// Token: 0x04001AC9 RID: 6857
	private Storage liquidStorage;

	// Token: 0x04001ACA RID: 6858
	private ElementConsumer consumer;

	// Token: 0x04001ACB RID: 6859
	private Guid statusHandle;

	// Token: 0x04001ACC RID: 6860
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x04001ACD RID: 6861
	private float lastSampleTime = -1f;

	// Token: 0x020013B9 RID: 5049
	public class States : GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine>
	{
		// Token: 0x060081FD RID: 33277 RVA: 0x002F7528 File Offset: 0x002F5728
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			SteamTurbine.InitializeStatusItems();
			default_state = this.operational;
			this.root.Update("UpdateBlocked", delegate(SteamTurbine.Instance smi, float dt)
			{
				smi.UpdateBlocked(dt);
			}, UpdateRate.SIM_200ms, false);
			this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational.active, (SteamTurbine.Instance smi) => smi.master.GetComponent<Operational>().IsOperational).QueueAnim("off", false, null);
			this.operational.DefaultState(this.operational.active).EventTransition(GameHashes.OperationalChanged, this.inoperational, (SteamTurbine.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational).Update("UpdateOperational", delegate(SteamTurbine.Instance smi, float dt)
			{
				smi.UpdateState(dt);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(SteamTurbine.Instance smi)
			{
				smi.DisableStatusItems();
			});
			this.operational.idle.QueueAnim("on", false, null);
			this.operational.active.Update("UpdateActive", delegate(SteamTurbine.Instance smi, float dt)
			{
				smi.master.Pump(dt);
			}, UpdateRate.SIM_200ms, false).ToggleStatusItem((SteamTurbine.Instance smi) => SteamTurbine.activeStatusItem, (SteamTurbine.Instance smi) => smi.master).Enter(delegate(SteamTurbine.Instance smi)
			{
				smi.GetComponent<KAnimControllerBase>().Play(SteamTurbine.States.ACTIVE_ANIMS, KAnim.PlayMode.Loop);
				smi.GetComponent<Operational>().SetActive(true, false);
			}).Exit(delegate(SteamTurbine.Instance smi)
			{
				smi.master.GetComponent<Generator>().ResetJoules();
				smi.GetComponent<Operational>().SetActive(false, false);
			});
			this.operational.tooHot.Enter(delegate(SteamTurbine.Instance smi)
			{
				smi.GetComponent<KAnimControllerBase>().Play(SteamTurbine.States.TOOHOT_ANIMS, KAnim.PlayMode.Loop);
			});
		}

		// Token: 0x04006330 RID: 25392
		public GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.State inoperational;

		// Token: 0x04006331 RID: 25393
		public SteamTurbine.States.OperationalStates operational;

		// Token: 0x04006332 RID: 25394
		private static readonly HashedString[] ACTIVE_ANIMS = new HashedString[]
		{
			"working_pre",
			"working_loop"
		};

		// Token: 0x04006333 RID: 25395
		private static readonly HashedString[] TOOHOT_ANIMS = new HashedString[]
		{
			"working_pre"
		};

		// Token: 0x0200212C RID: 8492
		public class OperationalStates : GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.State
		{
			// Token: 0x040094A9 RID: 38057
			public GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.State idle;

			// Token: 0x040094AA RID: 38058
			public GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.State active;

			// Token: 0x040094AB RID: 38059
			public GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.State tooHot;
		}
	}

	// Token: 0x020013BA RID: 5050
	public class Instance : GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.GameInstance
	{
		// Token: 0x06008200 RID: 33280 RVA: 0x002F77BC File Offset: 0x002F59BC
		public Instance(SteamTurbine master) : base(master)
		{
		}

		// Token: 0x06008201 RID: 33281 RVA: 0x002F7814 File Offset: 0x002F5A14
		public void UpdateBlocked(float dt)
		{
			base.master.BlockedInputs = 0;
			for (int i = 0; i < base.master.TotalInputs; i++)
			{
				int num = base.master.srcCells[i];
				Element element = Grid.Element[num];
				if (element.IsLiquid || element.IsSolid)
				{
					SteamTurbine master = base.master;
					int blockedInputs = master.BlockedInputs;
					master.BlockedInputs = blockedInputs + 1;
				}
			}
			KSelectable component = base.GetComponent<KSelectable>();
			this.inputBlockedHandle = this.UpdateStatusItem(SteamTurbine.inputBlockedStatusItem, base.master.BlockedInputs == base.master.TotalInputs, this.inputBlockedHandle, component);
			this.inputPartiallyBlockedHandle = this.UpdateStatusItem(SteamTurbine.inputPartiallyBlockedStatusItem, base.master.BlockedInputs > 0 && base.master.BlockedInputs < base.master.TotalInputs, this.inputPartiallyBlockedHandle, component);
		}

		// Token: 0x06008202 RID: 33282 RVA: 0x002F78F8 File Offset: 0x002F5AF8
		public void UpdateState(float dt)
		{
			bool flag = this.CanSteamFlow(ref this.insufficientMass, ref this.insufficientTemperature);
			bool flag2 = this.IsTooHot(ref this.buildingTooHot);
			this.UpdateStatusItems();
			StateMachine.BaseState currentState = base.smi.GetCurrentState();
			if (flag2)
			{
				if (currentState != base.sm.operational.tooHot)
				{
					base.smi.GoTo(base.sm.operational.tooHot);
					return;
				}
			}
			else if (flag)
			{
				if (currentState != base.sm.operational.active)
				{
					base.smi.GoTo(base.sm.operational.active);
					return;
				}
			}
			else if (currentState != base.sm.operational.idle)
			{
				base.smi.GoTo(base.sm.operational.idle);
			}
		}

		// Token: 0x06008203 RID: 33283 RVA: 0x002F79C7 File Offset: 0x002F5BC7
		private bool IsTooHot(ref bool building_too_hot)
		{
			building_too_hot = (base.gameObject.GetComponent<PrimaryElement>().Temperature > base.smi.master.maxBuildingTemperature);
			return building_too_hot;
		}

		// Token: 0x06008204 RID: 33284 RVA: 0x002F79F0 File Offset: 0x002F5BF0
		private bool CanSteamFlow(ref bool insufficient_mass, ref bool insufficient_temperature)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < base.master.srcCells.Length; i++)
			{
				int num3 = base.master.srcCells[i];
				float b = Grid.Mass[num3];
				if (Grid.Element[num3].id == base.master.srcElem)
				{
					num = Mathf.Max(num, b);
					float b2 = Grid.Temperature[num3];
					num2 = Mathf.Max(num2, b2);
				}
			}
			insufficient_mass = (num < base.master.requiredMass);
			insufficient_temperature = (num2 < base.master.minActiveTemperature);
			return !insufficient_mass && !insufficient_temperature;
		}

		// Token: 0x06008205 RID: 33285 RVA: 0x002F7AA0 File Offset: 0x002F5CA0
		public void UpdateStatusItems()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			this.insufficientMassHandle = this.UpdateStatusItem(SteamTurbine.insufficientMassStatusItem, this.insufficientMass, this.insufficientMassHandle, component);
			this.insufficientTemperatureHandle = this.UpdateStatusItem(SteamTurbine.insufficientTemperatureStatusItem, this.insufficientTemperature, this.insufficientTemperatureHandle, component);
			this.buildingTooHotHandle = this.UpdateStatusItem(SteamTurbine.buildingTooHotItem, this.buildingTooHot, this.buildingTooHotHandle, component);
			StatusItem status_item = base.master.operational.IsActive ? SteamTurbine.activeWattageStatusItem : Db.Get().BuildingStatusItems.GeneratorOffline;
			this.activeWattageHandle = component.SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, base.master);
		}

		// Token: 0x06008206 RID: 33286 RVA: 0x002F7B5C File Offset: 0x002F5D5C
		private Guid UpdateStatusItem(StatusItem item, bool show, Guid current_handle, KSelectable ksel)
		{
			Guid result = current_handle;
			if (show != (current_handle != Guid.Empty))
			{
				if (show)
				{
					result = ksel.AddStatusItem(item, base.master);
				}
				else
				{
					result = ksel.RemoveStatusItem(current_handle, false);
				}
			}
			return result;
		}

		// Token: 0x06008207 RID: 33287 RVA: 0x002F7B98 File Offset: 0x002F5D98
		public void DisableStatusItems()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			component.RemoveStatusItem(this.buildingTooHotHandle, false);
			component.RemoveStatusItem(this.insufficientMassHandle, false);
			component.RemoveStatusItem(this.insufficientTemperatureHandle, false);
			component.RemoveStatusItem(this.activeWattageHandle, false);
		}

		// Token: 0x04006334 RID: 25396
		public bool insufficientMass;

		// Token: 0x04006335 RID: 25397
		public bool insufficientTemperature;

		// Token: 0x04006336 RID: 25398
		public bool buildingTooHot;

		// Token: 0x04006337 RID: 25399
		private Guid inputBlockedHandle = Guid.Empty;

		// Token: 0x04006338 RID: 25400
		private Guid inputPartiallyBlockedHandle = Guid.Empty;

		// Token: 0x04006339 RID: 25401
		private Guid insufficientMassHandle = Guid.Empty;

		// Token: 0x0400633A RID: 25402
		private Guid insufficientTemperatureHandle = Guid.Empty;

		// Token: 0x0400633B RID: 25403
		private Guid buildingTooHotHandle = Guid.Empty;

		// Token: 0x0400633C RID: 25404
		private Guid activeWattageHandle = Guid.Empty;
	}
}
