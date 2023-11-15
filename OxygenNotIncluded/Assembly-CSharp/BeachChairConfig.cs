using System;
using TUNING;
using UnityEngine;

// Token: 0x02000027 RID: 39
public class BeachChairConfig : IBuildingConfig
{
	// Token: 0x060000AA RID: 170 RVA: 0x00006130 File Offset: 0x00004330
	public override BuildingDef CreateBuildingDef()
	{
		string id = "BeachChair";
		int width = 2;
		int height = 3;
		string anim = "beach_chair_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] construction_mass = new float[]
		{
			400f,
			2f
		};
		string[] construction_materials = new string[]
		{
			"BuildableRaw",
			"BuildingFiber"
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER4, none, 0.2f);
		buildingDef.Floodable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		return buildingDef;
	}

	// Token: 0x060000AB RID: 171 RVA: 0x000061B4 File Offset: 0x000043B4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		go.AddOrGet<BeachChairWorkable>().basePriority = RELAXATION.PRIORITY.TIER4;
		BeachChair beachChair = go.AddOrGet<BeachChair>();
		beachChair.specificEffectUnlit = "BeachChairUnlit";
		beachChair.specificEffectLit = "BeachChairLit";
		beachChair.trackingEffect = "RecentlyBeachChair";
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGet<AnimTileable>();
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x060000AC RID: 172 RVA: 0x0000623C File Offset: 0x0000443C
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000072 RID: 114
	public const string ID = "BeachChair";

	// Token: 0x04000073 RID: 115
	public const int TAN_LUX = 10000;

	// Token: 0x04000074 RID: 116
	private const float TANK_SIZE_KG = 20f;

	// Token: 0x04000075 RID: 117
	private const float SPILL_RATE_KG = 0.05f;
}
