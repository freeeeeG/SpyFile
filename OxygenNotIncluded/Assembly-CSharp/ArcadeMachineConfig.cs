using System;
using TUNING;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class ArcadeMachineConfig : IBuildingConfig
{
	// Token: 0x06000069 RID: 105 RVA: 0x00004D04 File Offset: 0x00002F04
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ArcadeMachine";
		int width = 3;
		int height = 3;
		string anim = "arcade_cabinet_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.Floodable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 1200f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		return buildingDef;
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00004D8C File Offset: 0x00002F8C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		go.AddOrGet<ArcadeMachine>();
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00004DDE File Offset: 0x00002FDE
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000055 RID: 85
	public const string ID = "ArcadeMachine";

	// Token: 0x04000056 RID: 86
	public const string SPECIFIC_EFFECT = "PlayedArcade";

	// Token: 0x04000057 RID: 87
	public const string TRACKING_EFFECT = "RecentlyPlayedArcade";
}
