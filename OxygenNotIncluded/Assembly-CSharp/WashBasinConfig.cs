using System;
using TUNING;
using UnityEngine;

// Token: 0x02000377 RID: 887
public class WashBasinConfig : IBuildingConfig
{
	// Token: 0x0600124E RID: 4686 RVA: 0x000629F8 File Offset: 0x00060BF8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WashBasin";
		int width = 2;
		int height = 3;
		string anim = "wash_basin_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] construction_materials = raw_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		return BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, tier2, 0.2f);
	}

	// Token: 0x0600124F RID: 4687 RVA: 0x00062A40 File Offset: 0x00060C40
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.WashStation, false);
		HandSanitizer handSanitizer = go.AddOrGet<HandSanitizer>();
		handSanitizer.massConsumedPerUse = 5f;
		handSanitizer.consumedElement = SimHashes.Water;
		handSanitizer.outputElement = SimHashes.DirtyWater;
		handSanitizer.diseaseRemovalCount = 120000;
		handSanitizer.maxUses = 40;
		handSanitizer.dumpWhenFull = true;
		go.AddOrGet<DirectionControl>();
		HandSanitizer.Work work = go.AddOrGet<HandSanitizer.Work>();
		work.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_washbasin_kanim")
		};
		work.workTime = 5f;
		work.trackUses = true;
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = GameTagExtensions.Create(SimHashes.Water);
		manualDeliveryKG.MinimumMass = 5f;
		manualDeliveryKG.capacity = 200f;
		manualDeliveryKG.refillMass = 40f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		go.AddOrGet<LoopingSounds>();
	}

	// Token: 0x06001250 RID: 4688 RVA: 0x00062B4C File Offset: 0x00060D4C
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040009E5 RID: 2533
	public const string ID = "WashBasin";

	// Token: 0x040009E6 RID: 2534
	public const int DISEASE_REMOVAL_COUNT = 120000;

	// Token: 0x040009E7 RID: 2535
	public const float WATER_PER_USE = 5f;

	// Token: 0x040009E8 RID: 2536
	public const int USES_PER_FLUSH = 40;

	// Token: 0x040009E9 RID: 2537
	public const float WORK_TIME = 5f;
}
