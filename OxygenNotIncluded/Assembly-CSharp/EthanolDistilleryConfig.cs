using System;
using TUNING;
using UnityEngine;

// Token: 0x02000080 RID: 128
public class EthanolDistilleryConfig : IBuildingConfig
{
	// Token: 0x0600026C RID: 620 RVA: 0x00011584 File Offset: 0x0000F784
	public override BuildingDef CreateBuildingDef()
	{
		string id = "EthanolDistillery";
		int width = 4;
		int height = 3;
		string anim = "ethanoldistillery_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.PowerInputOffset = new CellOffset(2, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(-1, 0);
		return buildingDef;
	}

	// Token: 0x0600026D RID: 621 RVA: 0x00011630 File Offset: 0x0000F830
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.Ethanol
		};
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1000f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.showInUI = true;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = WoodLogConfig.TAG;
		manualDeliveryKG.capacity = 600f;
		manualDeliveryKG.refillMass = 150f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(WoodLogConfig.TAG, 1f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.5f, SimHashes.Ethanol, 346.5f, false, true, 0f, 0.5f, 1f, byte.MaxValue, 0, true),
			new ElementConverter.OutputElement(0.33333334f, SimHashes.ToxicSand, 366.5f, false, true, 0f, 0.5f, 1f, byte.MaxValue, 0, true),
			new ElementConverter.OutputElement(0.16666667f, SimHashes.CarbonDioxide, 366.5f, false, false, 0f, 0.5f, 1f, byte.MaxValue, 0, true)
		};
		AlgaeDistillery algaeDistillery = go.AddOrGet<AlgaeDistillery>();
		algaeDistillery.emitMass = 20f;
		algaeDistillery.emitTag = new Tag("ToxicSand");
		algaeDistillery.emitOffset = new Vector3(2f, 1f);
		Prioritizable.AddRef(go);
	}

	// Token: 0x0600026E RID: 622 RVA: 0x000117E3 File Offset: 0x0000F9E3
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x0600026F RID: 623 RVA: 0x000117E5 File Offset: 0x0000F9E5
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06000270 RID: 624 RVA: 0x000117E7 File Offset: 0x0000F9E7
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x0400015C RID: 348
	public const string ID = "EthanolDistillery";

	// Token: 0x0400015D RID: 349
	public const float ORGANICS_CONSUME_PER_SECOND = 1f;

	// Token: 0x0400015E RID: 350
	public const float ORGANICS_STORAGE_AMOUNT = 600f;

	// Token: 0x0400015F RID: 351
	public const float ETHANOL_RATE = 0.5f;

	// Token: 0x04000160 RID: 352
	public const float SOLID_WASTE_RATE = 0.33333334f;

	// Token: 0x04000161 RID: 353
	public const float CO2_WASTE_RATE = 0.16666667f;

	// Token: 0x04000162 RID: 354
	public const float OUTPUT_TEMPERATURE = 346.5f;

	// Token: 0x04000163 RID: 355
	public const float WASTE_OUTPUT_TEMPERATURE = 366.5f;
}
