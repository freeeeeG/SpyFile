﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x02000167 RID: 359
public class FlushToiletConfig : IBuildingConfig
{
	// Token: 0x06000701 RID: 1793 RVA: 0x0002D3B0 File Offset: 0x0002B5B0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FlushToilet";
		int width = 2;
		int height = 3;
		string anim = "toiletflush_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.ExhaustKilowattsWhenActive = 0.25f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.DiseaseCellVisName = "FoodPoisoning";
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(1, 1);
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		SoundEventVolumeCache.instance.AddVolume("toiletflush_kanim", "Lavatory_flush", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("toiletflush_kanim", "Lavatory_door_close", NOISE_POLLUTION.NOISY.TIER1);
		SoundEventVolumeCache.instance.AddVolume("toiletflush_kanim", "Lavatory_door_open", NOISE_POLLUTION.NOISY.TIER1);
		return buildingDef;
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x0002D4B0 File Offset: 0x0002B6B0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ToiletType, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.FlushToiletType, false);
		FlushToilet flushToilet = go.AddOrGet<FlushToilet>();
		flushToilet.massConsumedPerUse = 5f;
		flushToilet.massEmittedPerUse = 11.7f;
		flushToilet.newPeeTemperature = 310.15f;
		flushToilet.diseaseId = "FoodPoisoning";
		flushToilet.diseasePerFlush = 100000;
		flushToilet.diseaseOnDupePerFlush = 5000;
		flushToilet.requireOutput = true;
		KAnimFile[] overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_toiletflush_kanim")
		};
		ToiletWorkableUse toiletWorkableUse = go.AddOrGet<ToiletWorkableUse>();
		toiletWorkableUse.overrideAnims = overrideAnims;
		toiletWorkableUse.workLayer = Grid.SceneLayer.BuildingFront;
		toiletWorkableUse.resetProgressOnStop = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
		conduitConsumer.capacityKG = 5f;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.invertElementFilter = true;
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.Water
		};
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 25f;
		storage.doDiseaseTransfer = false;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		Ownable ownable = go.AddOrGet<Ownable>();
		ownable.slotID = Db.Get().AssignableSlots.Toilet.Id;
		ownable.canBePublic = true;
		go.AddOrGet<RequireOutputs>().ignoreFullPipe = true;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x0002D61D File Offset: 0x0002B81D
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040004E0 RID: 1248
	private const float WATER_USAGE = 5f;

	// Token: 0x040004E1 RID: 1249
	public const string ID = "FlushToilet";
}
