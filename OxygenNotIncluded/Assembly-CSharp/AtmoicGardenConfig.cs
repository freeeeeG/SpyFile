﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x0200001D RID: 29
public class AtmoicGardenConfig : IBuildingConfig
{
	// Token: 0x0600007B RID: 123 RVA: 0x000051E0 File Offset: 0x000033E0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "AtomicGarden";
		int width = 4;
		int height = 3;
		string anim = "fertilizer_maker_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 1f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.PowerInputOffset = new CellOffset(1, 0);
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(-1, 0));
		buildingDef.Deprecated = true;
		return buildingDef;
	}

	// Token: 0x0600007C RID: 124 RVA: 0x000052A0 File Offset: 0x000034A0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		go.AddOrGet<WaterPurifier>();
		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		manualDeliveryKG.RequestedItemTag = new Tag("Dirt");
		manualDeliveryKG.capacity = 136.5f;
		manualDeliveryKG.refillMass = 19.5f;
		ManualDeliveryKG manualDeliveryKG2 = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG2.SetStorage(storage);
		manualDeliveryKG2.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		manualDeliveryKG2.RequestedItemTag = new Tag("Phosphorite");
		manualDeliveryKG2.capacity = 54.6f;
		manualDeliveryKG2.refillMass = 7.7999997f;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
		conduitConsumer.capacityKG = 0.19500001f;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.forceAlwaysSatisfied = true;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(new Tag("DirtyWater"), 0.039f, true),
			new ElementConverter.ConsumedElement(new Tag("Dirt"), 0.065f, true),
			new ElementConverter.ConsumedElement(new Tag("Phosphorite"), 0.025999999f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.12f, SimHashes.Fertilizer, 323.15f, false, true, 0f, 0.5f, 1f, byte.MaxValue, 0, true)
		};
		BuildingElementEmitter buildingElementEmitter = go.AddOrGet<BuildingElementEmitter>();
		buildingElementEmitter.emitRate = 0.01f;
		buildingElementEmitter.temperature = 349.15f;
		buildingElementEmitter.element = SimHashes.Methane;
		buildingElementEmitter.modifierOffset = new Vector2(2f, 2f);
		ElementDropper elementDropper = go.AddComponent<ElementDropper>();
		elementDropper.emitMass = 10f;
		elementDropper.emitTag = new Tag("Fertilizer");
		elementDropper.emitOffset = new Vector3(0f, 1f, 0f);
		Prioritizable.AddRef(go);
	}

	// Token: 0x0600007D RID: 125 RVA: 0x000054D6 File Offset: 0x000036D6
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x0400005C RID: 92
	public const string ID = "AtomicGarden";

	// Token: 0x0400005D RID: 93
	private const float FERTILIZER_PER_LOAD = 10f;

	// Token: 0x0400005E RID: 94
	private const float FERTILIZER_PRODUCTION_RATE = 0.12f;

	// Token: 0x0400005F RID: 95
	private const float METHANE_PRODUCTION_RATE = 0.01f;

	// Token: 0x04000060 RID: 96
	private const float _TOTAL_PRODUCTION = 0.13f;

	// Token: 0x04000061 RID: 97
	private const float DIRT_CONSUMPTION_RATE = 0.065f;

	// Token: 0x04000062 RID: 98
	private const float DIRTY_WATER_CONSUMPTION_RATE = 0.039f;

	// Token: 0x04000063 RID: 99
	private const float PHOSPHORITE_CONSUMPTION_RATE = 0.025999999f;
}
