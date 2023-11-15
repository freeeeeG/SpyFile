using System;
using STRINGS;
using UnityEngine;

// Token: 0x020005E2 RID: 1506
public class ContactConductivePipeBridge : GameStateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>
{
	// Token: 0x0600257F RID: 9599 RVA: 0x000CBCD4 File Offset: 0x000C9ED4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.noLiquid;
		this.noLiquid.PlayAnim("off", KAnim.PlayMode.Once).ParamTransition<float>(this.noLiquidTimer, this.withLiquid, GameStateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>.IsGTZero);
		this.withLiquid.Update(new Action<ContactConductivePipeBridge.Instance, float>(ContactConductivePipeBridge.ExpirationTimerUpdate), UpdateRate.SIM_200ms, false).PlayAnim("on", KAnim.PlayMode.Loop).ParamTransition<float>(this.noLiquidTimer, this.noLiquid, GameStateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>.IsLTEZero);
	}

	// Token: 0x06002580 RID: 9600 RVA: 0x000CBD4C File Offset: 0x000C9F4C
	private static void ExpirationTimerUpdate(ContactConductivePipeBridge.Instance smi, float dt)
	{
		float num = smi.sm.noLiquidTimer.Get(smi);
		num -= dt;
		smi.sm.noLiquidTimer.Set(num, smi, false);
	}

	// Token: 0x06002581 RID: 9601 RVA: 0x000CBD84 File Offset: 0x000C9F84
	private static float CalculateMaxWattsTransfered(float buildingTemperature, float building_thermal_conductivity, float content_temperature, float content_thermal_conductivity)
	{
		float num = 1f;
		float num2 = 1f;
		float num3 = 50f;
		float num4 = content_temperature - buildingTemperature;
		float num5 = (content_thermal_conductivity + building_thermal_conductivity) * 0.5f;
		return num4 * num5 * num * num3 / num2;
	}

	// Token: 0x06002582 RID: 9602 RVA: 0x000CBDB8 File Offset: 0x000C9FB8
	private static float GetKilloJoulesTransfered(float maxWattsTransfered, float dt, float building_Temperature, float building_heat_capacity, float content_temperature, float content_heat_capacity)
	{
		float num = maxWattsTransfered * dt / 1000f;
		float min = Mathf.Min(content_temperature, building_Temperature);
		float max = Mathf.Max(content_temperature, building_Temperature);
		float value = content_temperature - num / content_heat_capacity;
		float num2 = building_Temperature + num / building_heat_capacity;
		float num3 = Mathf.Clamp(value, min, max);
		num2 = Mathf.Clamp(num2, min, max);
		float num4 = Mathf.Abs(num3 - content_temperature);
		float num5 = Mathf.Abs(num2 - building_Temperature);
		float a = num4 * content_heat_capacity;
		float b = num5 * building_heat_capacity;
		return Mathf.Min(a, b) * Mathf.Sign(maxWattsTransfered);
	}

	// Token: 0x06002583 RID: 9603 RVA: 0x000CBE2C File Offset: 0x000CA02C
	private static float GetFinalContentTemperature(float KJT, float building_Temperature, float building_heat_capacity, float content_temperature, float content_heat_capacity)
	{
		float num = -KJT;
		float num2 = Mathf.Max(0f, content_temperature + num / content_heat_capacity);
		float num3 = Mathf.Max(0f, building_Temperature - num / building_heat_capacity);
		if ((content_temperature - building_Temperature) * (num2 - num3) < 0f)
		{
			return content_temperature * content_heat_capacity / (content_heat_capacity + building_heat_capacity) + building_Temperature * building_heat_capacity / (content_heat_capacity + building_heat_capacity);
		}
		return num2;
	}

	// Token: 0x06002584 RID: 9604 RVA: 0x000CBE80 File Offset: 0x000CA080
	private static float GetFinalBuildingTemperature(float content_temperature, float content_final_temperature, float content_heat_capacity, float building_temperature, float building_heat_capacity)
	{
		float num = (content_temperature - content_final_temperature) * content_heat_capacity;
		float min = Mathf.Min(content_temperature, building_temperature);
		float max = Mathf.Max(content_temperature, building_temperature);
		float num2 = num / building_heat_capacity;
		return Mathf.Clamp(building_temperature + num2, min, max);
	}

	// Token: 0x0400156C RID: 5484
	private const string loopAnimName = "on";

	// Token: 0x0400156D RID: 5485
	private const string loopAnim_noWater = "off";

	// Token: 0x0400156E RID: 5486
	private GameStateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>.State withLiquid;

	// Token: 0x0400156F RID: 5487
	private GameStateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>.State noLiquid;

	// Token: 0x04001570 RID: 5488
	private StateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>.FloatParameter noLiquidTimer;

	// Token: 0x02001279 RID: 4729
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005FC7 RID: 24519
		public ConduitType type = ConduitType.Liquid;

		// Token: 0x04005FC8 RID: 24520
		public float pumpKGRate;
	}

	// Token: 0x0200127A RID: 4730
	public new class Instance : GameStateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>.GameInstance
	{
		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06007D8A RID: 32138 RVA: 0x002E480C File Offset: 0x002E2A0C
		public Tag tag
		{
			get
			{
				if (this.type != ConduitType.Liquid)
				{
					return GameTags.Gas;
				}
				return GameTags.Liquid;
			}
		}

		// Token: 0x06007D8B RID: 32139 RVA: 0x002E4822 File Offset: 0x002E2A22
		public Instance(IStateMachineTarget master, ContactConductivePipeBridge.Def def) : base(master, def)
		{
		}

		// Token: 0x06007D8C RID: 32140 RVA: 0x002E4844 File Offset: 0x002E2A44
		public override void StartSM()
		{
			base.StartSM();
			this.inputCell = this.building.GetUtilityInputCell();
			this.outputCell = this.building.GetUtilityOutputCell();
			this.structureHandle = GameComps.StructureTemperatures.GetHandle(base.gameObject);
			Conduit.GetFlowManager(this.type).AddConduitUpdater(new Action<float>(this.Flow), ConduitFlowPriority.Default);
		}

		// Token: 0x06007D8D RID: 32141 RVA: 0x002E48AC File Offset: 0x002E2AAC
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Conduit.GetFlowManager(this.type).RemoveConduitUpdater(new Action<float>(this.Flow));
		}

		// Token: 0x06007D8E RID: 32142 RVA: 0x002E48D0 File Offset: 0x002E2AD0
		private void Flow(float dt)
		{
			ConduitFlow flowManager = Conduit.GetFlowManager(this.type);
			if (flowManager.HasConduit(this.inputCell) && flowManager.HasConduit(this.outputCell))
			{
				ConduitFlow.ConduitContents contents = flowManager.GetContents(this.inputCell);
				ConduitFlow.ConduitContents contents2 = flowManager.GetContents(this.outputCell);
				float num = Mathf.Min(contents.mass, base.def.pumpKGRate * dt);
				if (flowManager.CanMergeContents(contents, contents2, num))
				{
					base.smi.sm.noLiquidTimer.Set(1.5f, base.smi, false);
					float amountAllowedForMerging = flowManager.GetAmountAllowedForMerging(contents, contents2, num);
					if (amountAllowedForMerging > 0f)
					{
						float temperature = this.ExchangeStorageTemperatureWithBuilding(contents, amountAllowedForMerging, dt);
						float num2 = ((base.def.type == ConduitType.Liquid) ? Game.Instance.liquidConduitFlow : Game.Instance.gasConduitFlow).AddElement(this.outputCell, contents.element, amountAllowedForMerging, temperature, contents.diseaseIdx, contents.diseaseCount);
						if (amountAllowedForMerging != num2)
						{
							global::Debug.Log("Mass Differs By: " + (amountAllowedForMerging - num2).ToString());
						}
						flowManager.RemoveElement(this.inputCell, num2);
					}
				}
			}
		}

		// Token: 0x06007D8F RID: 32143 RVA: 0x002E4A0C File Offset: 0x002E2C0C
		private float ExchangeStorageTemperatureWithBuilding(ConduitFlow.ConduitContents content, float mass, float dt)
		{
			PrimaryElement component = this.building.GetComponent<PrimaryElement>();
			float building_thermal_conductivity = component.Element.thermalConductivity * this.building.Def.ThermalConductivity;
			if (mass > 0f)
			{
				Element element = ElementLoader.FindElementByHash(content.element);
				float content_heat_capacity = mass * element.specificHeatCapacity;
				float building_heat_capacity = this.building.Def.MassForTemperatureModification * component.Element.specificHeatCapacity;
				float temperature = component.Temperature;
				float temperature2 = content.temperature;
				float killoJoulesTransfered = ContactConductivePipeBridge.GetKilloJoulesTransfered(ContactConductivePipeBridge.CalculateMaxWattsTransfered(temperature, building_thermal_conductivity, temperature2, element.thermalConductivity), dt, temperature, building_heat_capacity, temperature2, content_heat_capacity);
				float finalContentTemperature = ContactConductivePipeBridge.GetFinalContentTemperature(killoJoulesTransfered, temperature, building_heat_capacity, temperature2, content_heat_capacity);
				float finalBuildingTemperature = ContactConductivePipeBridge.GetFinalBuildingTemperature(temperature2, finalContentTemperature, content_heat_capacity, temperature, building_heat_capacity);
				if ((finalBuildingTemperature >= 0f && finalBuildingTemperature <= 10000f) & (finalContentTemperature >= 0f && finalContentTemperature <= 10000f))
				{
					GameComps.StructureTemperatures.ProduceEnergy(base.smi.structureHandle, killoJoulesTransfered, BUILDING.STATUSITEMS.OPERATINGENERGY.PIPECONTENTS_TRANSFER, Time.time);
					return finalContentTemperature;
				}
			}
			return 0f;
		}

		// Token: 0x04005FC9 RID: 24521
		public ConduitType type = ConduitType.Liquid;

		// Token: 0x04005FCA RID: 24522
		public HandleVector<int>.Handle structureHandle;

		// Token: 0x04005FCB RID: 24523
		public int inputCell = -1;

		// Token: 0x04005FCC RID: 24524
		public int outputCell = -1;

		// Token: 0x04005FCD RID: 24525
		[MyCmpGet]
		public Building building;
	}
}
