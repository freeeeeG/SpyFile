﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x020002BB RID: 699
public class OuthouseConfig : IBuildingConfig
{
	// Token: 0x06000E3D RID: 3645 RVA: 0x0004E764 File Offset: 0x0004C964
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Outhouse";
		int width = 2;
		int height = 3;
		string anim = "outhouse_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER4, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.ExhaustKilowattsWhenActive = 0.25f;
		buildingDef.DiseaseCellVisName = "FoodPoisoning";
		buildingDef.AudioCategory = "Metal";
		SoundEventVolumeCache.instance.AddVolume("outhouse_kanim", "Latrine_door_open", NOISE_POLLUTION.NOISY.TIER1);
		SoundEventVolumeCache.instance.AddVolume("outhouse_kanim", "Latrine_door_close", NOISE_POLLUTION.NOISY.TIER1);
		return buildingDef;
	}

	// Token: 0x06000E3E RID: 3646 RVA: 0x0004E804 File Offset: 0x0004CA04
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ToiletType, false);
		Toilet toilet = go.AddOrGet<Toilet>();
		toilet.maxFlushes = 15;
		toilet.dirtUsedPerFlush = 13f;
		toilet.solidWastePerUse = new Toilet.SpawnInfo(SimHashes.ToxicSand, 6.7f, 0f);
		toilet.solidWasteTemperature = 310.15f;
		toilet.diseaseId = "FoodPoisoning";
		toilet.diseasePerFlush = 100000;
		toilet.diseaseOnDupePerFlush = 100000;
		KAnimFile[] overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_outhouse_kanim")
		};
		ToiletWorkableUse toiletWorkableUse = go.AddOrGet<ToiletWorkableUse>();
		toiletWorkableUse.overrideAnims = overrideAnims;
		toiletWorkableUse.workLayer = Grid.SceneLayer.BuildingFront;
		ToiletWorkableClean toiletWorkableClean = go.AddOrGet<ToiletWorkableClean>();
		toiletWorkableClean.workTime = 90f;
		toiletWorkableClean.overrideAnims = overrideAnims;
		toiletWorkableClean.workLayer = Grid.SceneLayer.BuildingFront;
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.showInUI = true;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = new Tag("Dirt");
		manualDeliveryKG.capacity = 200f;
		manualDeliveryKG.refillMass = 0.01f;
		manualDeliveryKG.MinimumMass = 200f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		manualDeliveryKG.operationalRequirement = Operational.State.Functional;
		manualDeliveryKG.FillToCapacity = true;
		Ownable ownable = go.AddOrGet<Ownable>();
		ownable.slotID = Db.Get().AssignableSlots.Toilet.Id;
		ownable.canBePublic = true;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06000E3F RID: 3647 RVA: 0x0004E984 File Offset: 0x0004CB84
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000831 RID: 2097
	public const string ID = "Outhouse";

	// Token: 0x04000832 RID: 2098
	private const int USES_PER_REFILL = 15;

	// Token: 0x04000833 RID: 2099
	private const float DIRT_PER_REFILL = 200f;

	// Token: 0x04000834 RID: 2100
	private const float DIRT_PER_USE = 13f;
}
