﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x020002C3 RID: 707
public class OxyliteRefineryConfig : IBuildingConfig
{
	// Token: 0x06000E64 RID: 3684 RVA: 0x0004F6A8 File Offset: 0x0004D8A8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "OxyliteRefinery";
		int width = 3;
		int height = 4;
		string anim = "oxylite_refinery_kanim";
		int hitpoints = 100;
		float construction_time = 480f;
		string[] array = new string[]
		{
			"RefinedMetal",
			"Plastic"
		};
		float[] construction_mass = new float[]
		{
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER5[0],
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0]
		};
		string[] construction_materials = array;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tier, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 1200f;
		buildingDef.ExhaustKilowattsWhenActive = 8f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 1));
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.UtilityInputOffset = new CellOffset(1, 0);
		return buildingDef;
	}

	// Token: 0x06000E65 RID: 3685 RVA: 0x0004F784 File Offset: 0x0004D984
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Tag tag = SimHashes.Oxygen.CreateTag();
		Tag tag2 = SimHashes.Gold.CreateTag();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		OxyliteRefinery oxyliteRefinery = go.AddOrGet<OxyliteRefinery>();
		oxyliteRefinery.emitTag = SimHashes.OxyRock.CreateTag();
		oxyliteRefinery.emitMass = 10f;
		oxyliteRefinery.dropOffset = new Vector3(0f, 1f);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 1.2f;
		conduitConsumer.capacityTag = tag;
		conduitConsumer.capacityKG = 6f;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 23.2f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.showInUI = true;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = tag2;
		manualDeliveryKG.refillMass = 1.8000001f;
		manualDeliveryKG.capacity = 7.2000003f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(tag, 0.6f, true),
			new ElementConverter.ConsumedElement(tag2, 0.003f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.6f, SimHashes.OxyRock, 303.15f, false, true, 0f, 0.5f, 1f, byte.MaxValue, 0, true)
		};
		Prioritizable.AddRef(go);
	}

	// Token: 0x06000E66 RID: 3686 RVA: 0x0004F909 File Offset: 0x0004DB09
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x04000845 RID: 2117
	public const string ID = "OxyliteRefinery";

	// Token: 0x04000846 RID: 2118
	public const float EMIT_MASS = 10f;

	// Token: 0x04000847 RID: 2119
	public const float INPUT_O2_PER_SECOND = 0.6f;

	// Token: 0x04000848 RID: 2120
	public const float OXYLITE_PER_SECOND = 0.6f;

	// Token: 0x04000849 RID: 2121
	public const float GOLD_PER_SECOND = 0.003f;

	// Token: 0x0400084A RID: 2122
	public const float OUTPUT_TEMP = 303.15f;

	// Token: 0x0400084B RID: 2123
	public const float REFILL_RATE = 2400f;

	// Token: 0x0400084C RID: 2124
	public const float GOLD_STORAGE_AMOUNT = 7.2000003f;

	// Token: 0x0400084D RID: 2125
	public const float O2_STORAGE_AMOUNT = 6f;

	// Token: 0x0400084E RID: 2126
	public const float STORAGE_CAPACITY = 23.2f;
}
