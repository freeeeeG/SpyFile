using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000624 RID: 1572
public class LiquidCooledRefinery : ComplexFabricator
{
	// Token: 0x060027B7 RID: 10167 RVA: 0x000D78B1 File Offset: 0x000D5AB1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LiquidCooledRefinery>(-1697596308, LiquidCooledRefinery.OnStorageChangeDelegate);
	}

	// Token: 0x060027B8 RID: 10168 RVA: 0x000D78CC File Offset: 0x000D5ACC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.meter_coolant = new MeterController(component, "meter_target", "meter_coolant", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, null);
		this.meter_metal = new MeterController(component, "meter_target_metal", "meter_metal", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, null);
		this.meter_metal.SetPositionPercent(1f);
		this.smi = new LiquidCooledRefinery.StatesInstance(this);
		this.smi.StartSM();
		Game.Instance.liquidConduitFlow.AddConduitUpdater(new Action<float>(this.OnConduitUpdate), ConduitFlowPriority.Default);
		Building component2 = base.GetComponent<Building>();
		this.outputCell = component2.GetUtilityOutputCell();
		this.workable.OnWorkTickActions = delegate(Worker worker, float dt)
		{
			float percentComplete = this.workable.GetPercentComplete();
			this.meter_metal.SetPositionPercent(percentComplete);
		};
	}

	// Token: 0x060027B9 RID: 10169 RVA: 0x000D7991 File Offset: 0x000D5B91
	protected override void OnCleanUp()
	{
		Game.Instance.liquidConduitFlow.RemoveConduitUpdater(new Action<float>(this.OnConduitUpdate));
		base.OnCleanUp();
	}

	// Token: 0x060027BA RID: 10170 RVA: 0x000D79B4 File Offset: 0x000D5BB4
	private void OnConduitUpdate(float dt)
	{
		bool flag = Game.Instance.liquidConduitFlow.GetContents(this.outputCell).mass > 0f;
		this.smi.sm.outputBlocked.Set(flag, this.smi, false);
		this.operational.SetFlag(LiquidCooledRefinery.coolantOutputPipeEmpty, !flag);
	}

	// Token: 0x060027BB RID: 10171 RVA: 0x000D7A18 File Offset: 0x000D5C18
	public bool HasEnoughCoolant()
	{
		return this.inStorage.GetAmountAvailable(this.coolantTag) + this.buildStorage.GetAmountAvailable(this.coolantTag) >= this.minCoolantMass;
	}

	// Token: 0x060027BC RID: 10172 RVA: 0x000D7A48 File Offset: 0x000D5C48
	private void OnStorageChange(object data)
	{
		float amountAvailable = this.inStorage.GetAmountAvailable(this.coolantTag);
		float capacityKG = this.conduitConsumer.capacityKG;
		float positionPercent = Mathf.Clamp01(amountAvailable / capacityKG);
		if (this.meter_coolant != null)
		{
			this.meter_coolant.SetPositionPercent(positionPercent);
		}
	}

	// Token: 0x060027BD RID: 10173 RVA: 0x000D7A8E File Offset: 0x000D5C8E
	protected override bool HasIngredients(ComplexRecipe recipe, Storage storage)
	{
		return storage.GetAmountAvailable(this.coolantTag) >= this.minCoolantMass && base.HasIngredients(recipe, storage);
	}

	// Token: 0x060027BE RID: 10174 RVA: 0x000D7AB0 File Offset: 0x000D5CB0
	protected override void TransferCurrentRecipeIngredientsForBuild()
	{
		base.TransferCurrentRecipeIngredientsForBuild();
		float num = this.minCoolantMass;
		while (this.buildStorage.GetAmountAvailable(this.coolantTag) < this.minCoolantMass && this.inStorage.GetAmountAvailable(this.coolantTag) > 0f && num > 0f)
		{
			float num2 = this.inStorage.Transfer(this.buildStorage, this.coolantTag, num, false, true);
			num -= num2;
		}
	}

	// Token: 0x060027BF RID: 10175 RVA: 0x000D7B24 File Offset: 0x000D5D24
	protected override List<GameObject> SpawnOrderProduct(ComplexRecipe recipe)
	{
		List<GameObject> list = base.SpawnOrderProduct(recipe);
		PrimaryElement component = list[0].GetComponent<PrimaryElement>();
		component.Temperature = this.outputTemperature;
		float num = GameUtil.CalculateEnergyDeltaForElementChange(component.Element.specificHeatCapacity, component.Mass, component.Element.highTemp, this.outputTemperature);
		ListPool<GameObject, LiquidCooledRefinery>.PooledList pooledList = ListPool<GameObject, LiquidCooledRefinery>.Allocate();
		this.buildStorage.Find(this.coolantTag, pooledList);
		float num2 = 0f;
		foreach (GameObject gameObject in pooledList)
		{
			PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
			if (component2.Mass != 0f)
			{
				num2 += component2.Mass * component2.Element.specificHeatCapacity;
			}
		}
		foreach (GameObject gameObject2 in pooledList)
		{
			PrimaryElement component3 = gameObject2.GetComponent<PrimaryElement>();
			if (component3.Mass != 0f)
			{
				float num3 = component3.Mass * component3.Element.specificHeatCapacity / num2;
				float kilowatts = -num * num3 * this.thermalFudge;
				float num4 = GameUtil.CalculateTemperatureChange(component3.Element.specificHeatCapacity, component3.Mass, kilowatts);
				float temperature = component3.Temperature;
				component3.Temperature += num4;
			}
		}
		this.buildStorage.Transfer(this.outStorage, this.coolantTag, float.MaxValue, false, true);
		pooledList.Recycle();
		return list;
	}

	// Token: 0x060027C0 RID: 10176 RVA: 0x000D7CD0 File Offset: 0x000D5ED0
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.COOLANT, this.coolantTag.ProperName(), GameUtil.GetFormattedMass(this.minCoolantMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.COOLANT, this.coolantTag.ProperName(), GameUtil.GetFormattedMass(this.minCoolantMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		return descriptors;
	}

	// Token: 0x060027C1 RID: 10177 RVA: 0x000D7D4C File Offset: 0x000D5F4C
	public override List<Descriptor> AdditionalEffectsForRecipe(ComplexRecipe recipe)
	{
		List<Descriptor> list = base.AdditionalEffectsForRecipe(recipe);
		PrimaryElement component = Assets.GetPrefab(recipe.results[0].material).GetComponent<PrimaryElement>();
		PrimaryElement primaryElement = this.inStorage.FindFirstWithMass(this.coolantTag, 0f);
		string format = UI.BUILDINGEFFECTS.TOOLTIPS.REFINEMENT_ENERGY_HAS_COOLANT;
		if (primaryElement == null)
		{
			primaryElement = Assets.GetPrefab(GameTags.Water).GetComponent<PrimaryElement>();
			format = UI.BUILDINGEFFECTS.TOOLTIPS.REFINEMENT_ENERGY_NO_COOLANT;
		}
		float num = -GameUtil.CalculateEnergyDeltaForElementChange(component.Element.specificHeatCapacity, recipe.results[0].amount, component.Element.highTemp, this.outputTemperature);
		float temp = GameUtil.CalculateTemperatureChange(primaryElement.Element.specificHeatCapacity, this.minCoolantMass, num * this.thermalFudge);
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.REFINEMENT_ENERGY, GameUtil.GetFormattedJoules(num, "F1", GameUtil.TimeSlice.None)), string.Format(format, GameUtil.GetFormattedJoules(num, "F1", GameUtil.TimeSlice.None), primaryElement.GetProperName(), GameUtil.GetFormattedTemperature(temp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false)), Descriptor.DescriptorType.Effect, false));
		return list;
	}

	// Token: 0x04001708 RID: 5896
	[MyCmpReq]
	private ConduitConsumer conduitConsumer;

	// Token: 0x04001709 RID: 5897
	public static readonly Operational.Flag coolantOutputPipeEmpty = new Operational.Flag("coolantOutputPipeEmpty", Operational.Flag.Type.Requirement);

	// Token: 0x0400170A RID: 5898
	private int outputCell;

	// Token: 0x0400170B RID: 5899
	public Tag coolantTag;

	// Token: 0x0400170C RID: 5900
	public float minCoolantMass = 100f;

	// Token: 0x0400170D RID: 5901
	public float thermalFudge = 0.8f;

	// Token: 0x0400170E RID: 5902
	public float outputTemperature = 313.15f;

	// Token: 0x0400170F RID: 5903
	private MeterController meter_coolant;

	// Token: 0x04001710 RID: 5904
	private MeterController meter_metal;

	// Token: 0x04001711 RID: 5905
	private LiquidCooledRefinery.StatesInstance smi;

	// Token: 0x04001712 RID: 5906
	private static readonly EventSystem.IntraObjectHandler<LiquidCooledRefinery> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<LiquidCooledRefinery>(delegate(LiquidCooledRefinery component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x020012ED RID: 4845
	public class StatesInstance : GameStateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery, object>.GameInstance
	{
		// Token: 0x06007F06 RID: 32518 RVA: 0x002EB4C7 File Offset: 0x002E96C7
		public StatesInstance(LiquidCooledRefinery master) : base(master)
		{
		}
	}

	// Token: 0x020012EE RID: 4846
	public class States : GameStateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery>
	{
		// Token: 0x06007F07 RID: 32519 RVA: 0x002EB4D0 File Offset: 0x002E96D0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			if (LiquidCooledRefinery.States.waitingForCoolantStatus == null)
			{
				LiquidCooledRefinery.States.waitingForCoolantStatus = new StatusItem("waitingForCoolantStatus", BUILDING.STATUSITEMS.ENOUGH_COOLANT.NAME, BUILDING.STATUSITEMS.ENOUGH_COOLANT.TOOLTIP, "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
				LiquidCooledRefinery.States.waitingForCoolantStatus.resolveStringCallback = delegate(string str, object obj)
				{
					LiquidCooledRefinery liquidCooledRefinery = (LiquidCooledRefinery)obj;
					return string.Format(str, liquidCooledRefinery.coolantTag.ProperName(), GameUtil.GetFormattedMass(liquidCooledRefinery.minCoolantMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				};
			}
			default_state = this.waiting_for_coolant;
			this.waiting_for_coolant.ToggleStatusItem(LiquidCooledRefinery.States.waitingForCoolantStatus, (LiquidCooledRefinery.StatesInstance smi) => smi.master).EventTransition(GameHashes.OnStorageChange, this.ready, (LiquidCooledRefinery.StatesInstance smi) => smi.master.HasEnoughCoolant()).ParamTransition<bool>(this.outputBlocked, this.output_blocked, GameStateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery, object>.IsTrue);
			this.ready.EventTransition(GameHashes.OnStorageChange, this.waiting_for_coolant, (LiquidCooledRefinery.StatesInstance smi) => !smi.master.HasEnoughCoolant()).ParamTransition<bool>(this.outputBlocked, this.output_blocked, GameStateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery, object>.IsTrue).Enter(delegate(LiquidCooledRefinery.StatesInstance smi)
			{
				smi.master.SetQueueDirty();
			});
			this.output_blocked.ToggleStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, null).ParamTransition<bool>(this.outputBlocked, this.waiting_for_coolant, GameStateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery, object>.IsFalse);
		}

		// Token: 0x0400610F RID: 24847
		public static StatusItem waitingForCoolantStatus;

		// Token: 0x04006110 RID: 24848
		public StateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery, object>.BoolParameter outputBlocked;

		// Token: 0x04006111 RID: 24849
		public GameStateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery, object>.State waiting_for_coolant;

		// Token: 0x04006112 RID: 24850
		public GameStateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery, object>.State ready;

		// Token: 0x04006113 RID: 24851
		public GameStateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery, object>.State output_blocked;
	}
}
