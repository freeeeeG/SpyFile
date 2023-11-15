using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200067D RID: 1661
public class RefrigeratorController : GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>
{
	// Token: 0x06002C3D RID: 11325 RVA: 0x000EAD50 File Offset: 0x000E8F50
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational, new StateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.Transition.ConditionCallback(this.IsOperational));
		this.operational.DefaultState(this.operational.steady).EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.Not(new StateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.Transition.ConditionCallback(this.IsOperational))).Enter(delegate(RefrigeratorController.StatesInstance smi)
		{
			smi.operational.SetActive(true, false);
		}).Exit(delegate(RefrigeratorController.StatesInstance smi)
		{
			smi.operational.SetActive(false, false);
		});
		this.operational.cooling.Update("Cooling exhaust", delegate(RefrigeratorController.StatesInstance smi, float dt)
		{
			smi.ApplyCoolingExhaust(dt);
		}, UpdateRate.SIM_200ms, true).UpdateTransition(this.operational.steady, new Func<RefrigeratorController.StatesInstance, float, bool>(this.AllFoodCool), UpdateRate.SIM_4000ms, true).ToggleStatusItem(Db.Get().BuildingStatusItems.FridgeCooling, (RefrigeratorController.StatesInstance smi) => smi, Db.Get().StatusItemCategories.Main);
		this.operational.steady.Update("Cooling exhaust", delegate(RefrigeratorController.StatesInstance smi, float dt)
		{
			smi.ApplySteadyExhaust(dt);
		}, UpdateRate.SIM_200ms, true).UpdateTransition(this.operational.cooling, new Func<RefrigeratorController.StatesInstance, float, bool>(this.AnyWarmFood), UpdateRate.SIM_4000ms, true).ToggleStatusItem(Db.Get().BuildingStatusItems.FridgeSteady, (RefrigeratorController.StatesInstance smi) => smi, Db.Get().StatusItemCategories.Main).Enter(delegate(RefrigeratorController.StatesInstance smi)
		{
			smi.SetEnergySaver(true);
		}).Exit(delegate(RefrigeratorController.StatesInstance smi)
		{
			smi.SetEnergySaver(false);
		});
	}

	// Token: 0x06002C3E RID: 11326 RVA: 0x000EAF80 File Offset: 0x000E9180
	private bool AllFoodCool(RefrigeratorController.StatesInstance smi, float dt)
	{
		foreach (GameObject gameObject in smi.storage.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null) && component.Mass >= 0.01f && component.Temperature >= smi.def.simulatedInternalTemperature + smi.def.activeCoolingStopBuffer)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06002C3F RID: 11327 RVA: 0x000EB020 File Offset: 0x000E9220
	private bool AnyWarmFood(RefrigeratorController.StatesInstance smi, float dt)
	{
		foreach (GameObject gameObject in smi.storage.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null) && component.Mass >= 0.01f && component.Temperature >= smi.def.simulatedInternalTemperature + smi.def.activeCoolingStartBuffer)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06002C40 RID: 11328 RVA: 0x000EB0C0 File Offset: 0x000E92C0
	private bool IsOperational(RefrigeratorController.StatesInstance smi)
	{
		return smi.operational.IsOperational;
	}

	// Token: 0x04001A1D RID: 6685
	public GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.State inoperational;

	// Token: 0x04001A1E RID: 6686
	public RefrigeratorController.OperationalStates operational;

	// Token: 0x02001383 RID: 4995
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x0600815E RID: 33118 RVA: 0x002F5200 File Offset: 0x002F3400
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			list.AddRange(SimulatedTemperatureAdjuster.GetDescriptors(this.simulatedInternalTemperature));
			Descriptor item = default(Descriptor);
			string formattedHeatEnergy = GameUtil.GetFormattedHeatEnergy(this.coolingHeatKW * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATGENERATED, formattedHeatEnergy), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED, formattedHeatEnergy), Descriptor.DescriptorType.Effect);
			list.Add(item);
			return list;
		}

		// Token: 0x040062AF RID: 25263
		public float activeCoolingStartBuffer = 2f;

		// Token: 0x040062B0 RID: 25264
		public float activeCoolingStopBuffer = 0.1f;

		// Token: 0x040062B1 RID: 25265
		public float simulatedInternalTemperature = 274.15f;

		// Token: 0x040062B2 RID: 25266
		public float simulatedInternalHeatCapacity = 400f;

		// Token: 0x040062B3 RID: 25267
		public float simulatedThermalConductivity = 1000f;

		// Token: 0x040062B4 RID: 25268
		public float powerSaverEnergyUsage;

		// Token: 0x040062B5 RID: 25269
		public float coolingHeatKW;

		// Token: 0x040062B6 RID: 25270
		public float steadyHeatKW;
	}

	// Token: 0x02001384 RID: 4996
	public class OperationalStates : GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.State
	{
		// Token: 0x040062B7 RID: 25271
		public GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.State cooling;

		// Token: 0x040062B8 RID: 25272
		public GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.State steady;
	}

	// Token: 0x02001385 RID: 4997
	public class StatesInstance : GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.GameInstance
	{
		// Token: 0x06008161 RID: 33121 RVA: 0x002F52B4 File Offset: 0x002F34B4
		public StatesInstance(IStateMachineTarget master, RefrigeratorController.Def def) : base(master, def)
		{
			this.temperatureAdjuster = new SimulatedTemperatureAdjuster(def.simulatedInternalTemperature, def.simulatedInternalHeatCapacity, def.simulatedThermalConductivity, this.storage);
			this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		}

		// Token: 0x06008162 RID: 33122 RVA: 0x002F5302 File Offset: 0x002F3502
		protected override void OnCleanUp()
		{
			this.temperatureAdjuster.CleanUp();
			base.OnCleanUp();
		}

		// Token: 0x06008163 RID: 33123 RVA: 0x002F5315 File Offset: 0x002F3515
		public float GetSaverPower()
		{
			return base.def.powerSaverEnergyUsage;
		}

		// Token: 0x06008164 RID: 33124 RVA: 0x002F5322 File Offset: 0x002F3522
		public float GetNormalPower()
		{
			return base.GetComponent<EnergyConsumer>().WattsNeededWhenActive;
		}

		// Token: 0x06008165 RID: 33125 RVA: 0x002F5330 File Offset: 0x002F3530
		public void SetEnergySaver(bool energySaving)
		{
			EnergyConsumer component = base.GetComponent<EnergyConsumer>();
			if (energySaving)
			{
				component.BaseWattageRating = this.GetSaverPower();
				return;
			}
			component.BaseWattageRating = this.GetNormalPower();
		}

		// Token: 0x06008166 RID: 33126 RVA: 0x002F5360 File Offset: 0x002F3560
		public void ApplyCoolingExhaust(float dt)
		{
			GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, base.def.coolingHeatKW * dt, BUILDING.STATUSITEMS.OPERATINGENERGY.FOOD_TRANSFER, dt);
		}

		// Token: 0x06008167 RID: 33127 RVA: 0x002F538A File Offset: 0x002F358A
		public void ApplySteadyExhaust(float dt)
		{
			GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, base.def.steadyHeatKW * dt, BUILDING.STATUSITEMS.OPERATINGENERGY.FOOD_TRANSFER, dt);
		}

		// Token: 0x040062B9 RID: 25273
		[MyCmpReq]
		public Operational operational;

		// Token: 0x040062BA RID: 25274
		[MyCmpReq]
		public Storage storage;

		// Token: 0x040062BB RID: 25275
		private HandleVector<int>.Handle structureTemperature;

		// Token: 0x040062BC RID: 25276
		private SimulatedTemperatureAdjuster temperatureAdjuster;
	}
}
