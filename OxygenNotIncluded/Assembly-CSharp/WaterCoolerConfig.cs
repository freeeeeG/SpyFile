using System;
using TUNING;
using UnityEngine;

// Token: 0x02000379 RID: 889
public class WaterCoolerConfig : IBuildingConfig
{
	// Token: 0x06001256 RID: 4694 RVA: 0x00062D20 File Offset: 0x00060F20
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WaterCooler";
		int width = 2;
		int height = 2;
		string anim = "watercooler_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = false;
		return buildingDef;
	}

	// Token: 0x06001257 RID: 4695 RVA: 0x00062D80 File Offset: 0x00060F80
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 10f;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.capacity = 10f;
		manualDeliveryKG.refillMass = 9f;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		go.AddOrGet<WaterCooler>();
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06001258 RID: 4696 RVA: 0x00062E31 File Offset: 0x00061031
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040009EF RID: 2543
	public const string ID = "WaterCooler";

	// Token: 0x040009F0 RID: 2544
	public static global::Tuple<Tag, string>[] BEVERAGE_CHOICE_OPTIONS = new global::Tuple<Tag, string>[]
	{
		new global::Tuple<Tag, string>(SimHashes.Water.CreateTag(), ""),
		new global::Tuple<Tag, string>(SimHashes.Milk.CreateTag(), "DuplicantGotMilk")
	};

	// Token: 0x040009F1 RID: 2545
	public const string MilkEffectID = "DuplicantGotMilk";
}
