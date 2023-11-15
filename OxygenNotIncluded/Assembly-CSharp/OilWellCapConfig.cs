using System;
using TUNING;
using UnityEngine;

// Token: 0x020002AE RID: 686
public class OilWellCapConfig : IBuildingConfig
{
	// Token: 0x06000E01 RID: 3585 RVA: 0x0004DAAC File Offset: 0x0004BCAC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "OilWellCap";
		int width = 4;
		int height = 4;
		string anim = "geyser_oil_cap_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		BuildingTemplates.CreateElectricalBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(0, 1);
		buildingDef.PowerInputOffset = new CellOffset(1, 1);
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.AttachmentSlotTag = GameTags.OilWell;
		buildingDef.BuildLocationRule = BuildLocationRule.BuildingAttachPoint;
		buildingDef.ObjectLayer = ObjectLayer.AttachableBuilding;
		return buildingDef;
	}

	// Token: 0x06000E02 RID: 3586 RVA: 0x0004DB84 File Offset: 0x0004BD84
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		BuildingTemplates.CreateDefaultStorage(go, false).showInUI = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 2f;
		conduitConsumer.capacityKG = 10f;
		conduitConsumer.capacityTag = OilWellCapConfig.INPUT_WATER_TAG;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(OilWellCapConfig.INPUT_WATER_TAG, 1f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(3.3333333f, SimHashes.CrudeOil, 363.15f, false, false, 2f, 1.5f, 0f, byte.MaxValue, 0, true)
		};
		OilWellCap oilWellCap = go.AddOrGet<OilWellCap>();
		oilWellCap.gasElement = SimHashes.Methane;
		oilWellCap.gasTemperature = 573.15f;
		oilWellCap.addGasRate = 0.033333335f;
		oilWellCap.maxGasPressure = 80.00001f;
		oilWellCap.releaseGasRate = 0.44444448f;
	}

	// Token: 0x06000E03 RID: 3587 RVA: 0x0004DC7E File Offset: 0x0004BE7E
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x0400081C RID: 2076
	private const float WATER_INTAKE_RATE = 1f;

	// Token: 0x0400081D RID: 2077
	private const float WATER_TO_OIL_RATIO = 3.3333333f;

	// Token: 0x0400081E RID: 2078
	private const float LIQUID_STORAGE = 10f;

	// Token: 0x0400081F RID: 2079
	private const float GAS_RATE = 0.033333335f;

	// Token: 0x04000820 RID: 2080
	private const float OVERPRESSURE_TIME = 2400f;

	// Token: 0x04000821 RID: 2081
	private const float PRESSURE_RELEASE_TIME = 180f;

	// Token: 0x04000822 RID: 2082
	private const float PRESSURE_RELEASE_RATE = 0.44444448f;

	// Token: 0x04000823 RID: 2083
	private static readonly Tag INPUT_WATER_TAG = SimHashes.Water.CreateTag();

	// Token: 0x04000824 RID: 2084
	public const string ID = "OilWellCap";
}
