using System;
using Klei;
using KSerialization;
using UnityEngine;

// Token: 0x020006B1 RID: 1713
[AddComponentMenu("KMonoBehaviour/scripts/Turbine")]
public class Turbine : KMonoBehaviour
{
	// Token: 0x06002E8F RID: 11919 RVA: 0x000F5E60 File Offset: 0x000F4060
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.simEmitCBHandle = Game.Instance.massEmitCallbackManager.Add(new Action<Sim.MassEmittedCallback, object>(Turbine.OnSimEmittedCallback), this, "TurbineEmit");
		BuildingDef def = base.GetComponent<BuildingComplete>().Def;
		this.srcCells = new int[def.WidthInCells];
		this.destCells = new int[def.WidthInCells];
		int cell = Grid.PosToCell(this);
		for (int i = 0; i < def.WidthInCells; i++)
		{
			int x = i - (def.WidthInCells - 1) / 2;
			this.srcCells[i] = Grid.OffsetCell(cell, new CellOffset(x, -1));
			this.destCells[i] = Grid.OffsetCell(cell, new CellOffset(x, def.HeightInCells - 1));
		}
		this.smi = new Turbine.Instance(this);
		this.smi.StartSM();
		this.CreateMeter();
	}

	// Token: 0x06002E90 RID: 11920 RVA: 0x000F5F3C File Offset: 0x000F413C
	private void CreateMeter()
	{
		this.meter = new MeterController(base.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_OL",
			"meter_frame",
			"meter_fill"
		});
		this.smi.UpdateMeter();
	}

	// Token: 0x06002E91 RID: 11921 RVA: 0x000F5F98 File Offset: 0x000F4198
	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("cleanup");
		}
		Game.Instance.massEmitCallbackManager.Release(this.simEmitCBHandle, "Turbine");
		this.simEmitCBHandle.Clear();
		base.OnCleanUp();
	}

	// Token: 0x06002E92 RID: 11922 RVA: 0x000F5FEC File Offset: 0x000F41EC
	private void Pump(float dt)
	{
		float mass = this.pumpKGRate * dt / (float)this.srcCells.Length;
		foreach (int gameCell in this.srcCells)
		{
			HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(Turbine.OnSimConsumeCallback), this, "TurbineConsume");
			SimMessages.ConsumeMass(gameCell, this.srcElem, mass, 1, handle.index);
		}
	}

	// Token: 0x06002E93 RID: 11923 RVA: 0x000F605A File Offset: 0x000F425A
	private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		((Turbine)data).OnSimConsume(mass_cb_info);
	}

	// Token: 0x06002E94 RID: 11924 RVA: 0x000F6068 File Offset: 0x000F4268
	private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
	{
		if (mass_cb_info.mass > 0f)
		{
			this.storedTemperature = SimUtil.CalculateFinalTemperature(this.storedMass, this.storedTemperature, mass_cb_info.mass, mass_cb_info.temperature);
			this.storedMass += mass_cb_info.mass;
			SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(this.diseaseIdx, this.diseaseCount, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
			this.diseaseIdx = diseaseInfo.idx;
			this.diseaseCount = diseaseInfo.count;
			if (this.storedMass > this.minEmitMass && this.simEmitCBHandle.IsValid())
			{
				float mass = this.storedMass / (float)this.destCells.Length;
				int disease_count = this.diseaseCount / this.destCells.Length;
				Game.Instance.massEmitCallbackManager.GetItem(this.simEmitCBHandle);
				int[] array = this.destCells;
				for (int i = 0; i < array.Length; i++)
				{
					SimMessages.EmitMass(array[i], mass_cb_info.elemIdx, mass, this.emitTemperature, this.diseaseIdx, disease_count, this.simEmitCBHandle.index);
				}
				this.storedMass = 0f;
				this.storedTemperature = 0f;
				this.diseaseIdx = byte.MaxValue;
				this.diseaseCount = 0;
			}
		}
	}

	// Token: 0x06002E95 RID: 11925 RVA: 0x000F61B2 File Offset: 0x000F43B2
	private static void OnSimEmittedCallback(Sim.MassEmittedCallback info, object data)
	{
		((Turbine)data).OnSimEmitted(info);
	}

	// Token: 0x06002E96 RID: 11926 RVA: 0x000F61C0 File Offset: 0x000F43C0
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

	// Token: 0x06002E97 RID: 11927 RVA: 0x000F6284 File Offset: 0x000F4484
	public static void InitializeStatusItems()
	{
		Turbine.inputBlockedStatusItem = new StatusItem("TURBINE_BLOCKED_INPUT", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		Turbine.outputBlockedStatusItem = new StatusItem("TURBINE_BLOCKED_OUTPUT", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		Turbine.spinningUpStatusItem = new StatusItem("TURBINE_SPINNING_UP", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022, null);
		Turbine.activeStatusItem = new StatusItem("TURBINE_ACTIVE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022, null);
		Turbine.activeStatusItem.resolveStringCallback = delegate(string str, object data)
		{
			Turbine turbine = (Turbine)data;
			str = string.Format(str, (int)turbine.currentRPM);
			return str;
		};
		Turbine.insufficientMassStatusItem = new StatusItem("TURBINE_INSUFFICIENT_MASS", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022, null);
		Turbine.insufficientMassStatusItem.resolveTooltipCallback = delegate(string str, object data)
		{
			Turbine turbine = (Turbine)data;
			str = str.Replace("{MASS}", GameUtil.GetFormattedMass(turbine.requiredMassFlowDifferential, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			str = str.Replace("{SRC_ELEMENT}", ElementLoader.FindElementByHash(turbine.srcElem).name);
			return str;
		};
		Turbine.insufficientTemperatureStatusItem = new StatusItem("TURBINE_INSUFFICIENT_TEMPERATURE", "BUILDING", "status_item_plant_temperature", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022, null);
		Turbine.insufficientTemperatureStatusItem.resolveStringCallback = new Func<string, object, string>(Turbine.ResolveStrings);
		Turbine.insufficientTemperatureStatusItem.resolveTooltipCallback = new Func<string, object, string>(Turbine.ResolveStrings);
	}

	// Token: 0x06002E98 RID: 11928 RVA: 0x000F6400 File Offset: 0x000F4600
	private static string ResolveStrings(string str, object data)
	{
		Turbine turbine = (Turbine)data;
		str = str.Replace("{SRC_ELEMENT}", ElementLoader.FindElementByHash(turbine.srcElem).name);
		str = str.Replace("{ACTIVE_TEMPERATURE}", GameUtil.GetFormattedTemperature(turbine.minActiveTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		return str;
	}

	// Token: 0x04001B79 RID: 7033
	public SimHashes srcElem;

	// Token: 0x04001B7A RID: 7034
	public float requiredMassFlowDifferential = 3f;

	// Token: 0x04001B7B RID: 7035
	public float activePercent = 0.75f;

	// Token: 0x04001B7C RID: 7036
	public float minEmitMass;

	// Token: 0x04001B7D RID: 7037
	public float minActiveTemperature = 400f;

	// Token: 0x04001B7E RID: 7038
	public float emitTemperature = 300f;

	// Token: 0x04001B7F RID: 7039
	public float maxRPM;

	// Token: 0x04001B80 RID: 7040
	public float rpmAcceleration;

	// Token: 0x04001B81 RID: 7041
	public float rpmDeceleration;

	// Token: 0x04001B82 RID: 7042
	public float minGenerationRPM;

	// Token: 0x04001B83 RID: 7043
	public float pumpKGRate;

	// Token: 0x04001B84 RID: 7044
	private static readonly HashedString TINT_SYMBOL = new HashedString("meter_fill");

	// Token: 0x04001B85 RID: 7045
	[Serialize]
	private float storedMass;

	// Token: 0x04001B86 RID: 7046
	[Serialize]
	private float storedTemperature;

	// Token: 0x04001B87 RID: 7047
	[Serialize]
	private byte diseaseIdx = byte.MaxValue;

	// Token: 0x04001B88 RID: 7048
	[Serialize]
	private int diseaseCount;

	// Token: 0x04001B89 RID: 7049
	[MyCmpGet]
	private Generator generator;

	// Token: 0x04001B8A RID: 7050
	[Serialize]
	private float currentRPM;

	// Token: 0x04001B8B RID: 7051
	private int[] srcCells;

	// Token: 0x04001B8C RID: 7052
	private int[] destCells;

	// Token: 0x04001B8D RID: 7053
	private Turbine.Instance smi;

	// Token: 0x04001B8E RID: 7054
	private static StatusItem inputBlockedStatusItem;

	// Token: 0x04001B8F RID: 7055
	private static StatusItem outputBlockedStatusItem;

	// Token: 0x04001B90 RID: 7056
	private static StatusItem insufficientMassStatusItem;

	// Token: 0x04001B91 RID: 7057
	private static StatusItem insufficientTemperatureStatusItem;

	// Token: 0x04001B92 RID: 7058
	private static StatusItem activeStatusItem;

	// Token: 0x04001B93 RID: 7059
	private static StatusItem spinningUpStatusItem;

	// Token: 0x04001B94 RID: 7060
	private MeterController meter;

	// Token: 0x04001B95 RID: 7061
	private HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle simEmitCBHandle = HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.InvalidHandle;

	// Token: 0x020013EC RID: 5100
	public class States : GameStateMachine<Turbine.States, Turbine.Instance, Turbine>
	{
		// Token: 0x060082D0 RID: 33488 RVA: 0x002FAC90 File Offset: 0x002F8E90
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			Turbine.InitializeStatusItems();
			default_state = this.operational;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational.spinningUp, (Turbine.Instance smi) => smi.master.GetComponent<Operational>().IsOperational).QueueAnim("off", false, null).Enter(delegate(Turbine.Instance smi)
			{
				smi.master.currentRPM = 0f;
				smi.UpdateMeter();
			});
			this.operational.DefaultState(this.operational.spinningUp).EventTransition(GameHashes.OperationalChanged, this.inoperational, (Turbine.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational).Update("UpdateOperational", delegate(Turbine.Instance smi, float dt)
			{
				smi.UpdateState(dt);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(Turbine.Instance smi)
			{
				smi.DisableStatusItems();
			});
			this.operational.idle.QueueAnim("on", false, null);
			this.operational.spinningUp.ToggleStatusItem((Turbine.Instance smi) => Turbine.spinningUpStatusItem, (Turbine.Instance smi) => smi.master).QueueAnim("buildup", true, null);
			this.operational.active.Update("UpdateActive", delegate(Turbine.Instance smi, float dt)
			{
				smi.master.Pump(dt);
			}, UpdateRate.SIM_200ms, false).ToggleStatusItem((Turbine.Instance smi) => Turbine.activeStatusItem, (Turbine.Instance smi) => smi.master).Enter(delegate(Turbine.Instance smi)
			{
				smi.GetComponent<KAnimControllerBase>().Play(Turbine.States.ACTIVE_ANIMS, KAnim.PlayMode.Loop);
				smi.GetComponent<Operational>().SetActive(true, false);
			}).Exit(delegate(Turbine.Instance smi)
			{
				smi.master.GetComponent<Generator>().ResetJoules();
				smi.GetComponent<Operational>().SetActive(false, false);
			});
		}

		// Token: 0x040063C8 RID: 25544
		public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State inoperational;

		// Token: 0x040063C9 RID: 25545
		public Turbine.States.OperationalStates operational;

		// Token: 0x040063CA RID: 25546
		private static readonly HashedString[] ACTIVE_ANIMS = new HashedString[]
		{
			"working_pre",
			"working_loop"
		};

		// Token: 0x02002140 RID: 8512
		public class OperationalStates : GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State
		{
			// Token: 0x0400951A RID: 38170
			public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State idle;

			// Token: 0x0400951B RID: 38171
			public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State spinningUp;

			// Token: 0x0400951C RID: 38172
			public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State active;
		}
	}

	// Token: 0x020013ED RID: 5101
	public class Instance : GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.GameInstance
	{
		// Token: 0x060082D3 RID: 33491 RVA: 0x002FAF1D File Offset: 0x002F911D
		public Instance(Turbine master) : base(master)
		{
		}

		// Token: 0x060082D4 RID: 33492 RVA: 0x002FAF54 File Offset: 0x002F9154
		public void UpdateState(float dt)
		{
			float num = this.CanSteamFlow(ref this.insufficientMass, ref this.insufficientTemperature) ? base.master.rpmAcceleration : (-base.master.rpmDeceleration);
			base.master.currentRPM = Mathf.Clamp(base.master.currentRPM + dt * num, 0f, base.master.maxRPM);
			this.UpdateMeter();
			this.UpdateStatusItems();
			StateMachine.BaseState currentState = base.smi.GetCurrentState();
			if (base.master.currentRPM >= base.master.minGenerationRPM)
			{
				if (currentState != base.sm.operational.active)
				{
					base.smi.GoTo(base.sm.operational.active);
				}
				base.smi.master.generator.GenerateJoules(base.smi.master.generator.WattageRating * dt, false);
				return;
			}
			if (base.master.currentRPM > 0f)
			{
				if (currentState != base.sm.operational.spinningUp)
				{
					base.smi.GoTo(base.sm.operational.spinningUp);
					return;
				}
			}
			else if (currentState != base.sm.operational.idle)
			{
				base.smi.GoTo(base.sm.operational.idle);
			}
		}

		// Token: 0x060082D5 RID: 33493 RVA: 0x002FB0BC File Offset: 0x002F92BC
		public void UpdateMeter()
		{
			if (base.master.meter != null)
			{
				float num = Mathf.Clamp01(base.master.currentRPM / base.master.maxRPM);
				base.master.meter.SetPositionPercent(num);
				base.master.meter.SetSymbolTint(Turbine.TINT_SYMBOL, (num >= base.master.activePercent) ? Color.green : Color.red);
			}
		}

		// Token: 0x060082D6 RID: 33494 RVA: 0x002FB140 File Offset: 0x002F9340
		private bool CanSteamFlow(ref bool insufficient_mass, ref bool insufficient_temperature)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = float.PositiveInfinity;
			this.isInputBlocked = false;
			for (int i = 0; i < base.master.srcCells.Length; i++)
			{
				int num4 = base.master.srcCells[i];
				float b = Grid.Mass[num4];
				if (Grid.Element[num4].id == base.master.srcElem)
				{
					num = Mathf.Max(num, b);
				}
				float b2 = Grid.Temperature[num4];
				num2 = Mathf.Max(num2, b2);
				ushort index = Grid.ElementIdx[num4];
				Element element = ElementLoader.elements[(int)index];
				if (element.IsLiquid || element.IsSolid)
				{
					this.isInputBlocked = true;
				}
			}
			this.isOutputBlocked = false;
			for (int j = 0; j < base.master.destCells.Length; j++)
			{
				int i2 = base.master.destCells[j];
				float b3 = Grid.Mass[i2];
				num3 = Mathf.Min(num3, b3);
				ushort index2 = Grid.ElementIdx[i2];
				Element element2 = ElementLoader.elements[(int)index2];
				if (element2.IsLiquid || element2.IsSolid)
				{
					this.isOutputBlocked = true;
				}
			}
			insufficient_mass = (num - num3 < base.master.requiredMassFlowDifferential);
			insufficient_temperature = (num2 < base.master.minActiveTemperature);
			return !insufficient_mass && !insufficient_temperature;
		}

		// Token: 0x060082D7 RID: 33495 RVA: 0x002FB2BC File Offset: 0x002F94BC
		public void UpdateStatusItems()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			this.inputBlockedHandle = this.UpdateStatusItem(Turbine.inputBlockedStatusItem, this.isInputBlocked, this.inputBlockedHandle, component);
			this.outputBlockedHandle = this.UpdateStatusItem(Turbine.outputBlockedStatusItem, this.isOutputBlocked, this.outputBlockedHandle, component);
			this.insufficientMassHandle = this.UpdateStatusItem(Turbine.insufficientMassStatusItem, this.insufficientMass, this.insufficientMassHandle, component);
			this.insufficientTemperatureHandle = this.UpdateStatusItem(Turbine.insufficientTemperatureStatusItem, this.insufficientTemperature, this.insufficientTemperatureHandle, component);
		}

		// Token: 0x060082D8 RID: 33496 RVA: 0x002FB348 File Offset: 0x002F9548
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

		// Token: 0x060082D9 RID: 33497 RVA: 0x002FB384 File Offset: 0x002F9584
		public void DisableStatusItems()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			component.RemoveStatusItem(this.inputBlockedHandle, false);
			component.RemoveStatusItem(this.outputBlockedHandle, false);
			component.RemoveStatusItem(this.insufficientMassHandle, false);
			component.RemoveStatusItem(this.insufficientTemperatureHandle, false);
		}

		// Token: 0x040063CB RID: 25547
		public bool isInputBlocked;

		// Token: 0x040063CC RID: 25548
		public bool isOutputBlocked;

		// Token: 0x040063CD RID: 25549
		public bool insufficientMass;

		// Token: 0x040063CE RID: 25550
		public bool insufficientTemperature;

		// Token: 0x040063CF RID: 25551
		private Guid inputBlockedHandle = Guid.Empty;

		// Token: 0x040063D0 RID: 25552
		private Guid outputBlockedHandle = Guid.Empty;

		// Token: 0x040063D1 RID: 25553
		private Guid insufficientMassHandle = Guid.Empty;

		// Token: 0x040063D2 RID: 25554
		private Guid insufficientTemperatureHandle = Guid.Empty;
	}
}
