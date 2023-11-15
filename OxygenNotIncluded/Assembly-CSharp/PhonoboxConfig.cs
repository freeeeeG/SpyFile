using System;
using TUNING;
using UnityEngine;

// Token: 0x020002C9 RID: 713
public class PhonoboxConfig : IBuildingConfig
{
	// Token: 0x06000E7C RID: 3708 RVA: 0x0005006C File Offset: 0x0004E26C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Phonobox";
		int width = 5;
		int height = 3;
		string anim = "jukebot_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.Floodable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 960f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		return buildingDef;
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x000500F4 File Offset: 0x0004E2F4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		go.AddOrGet<Phonobox>();
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06000E7E RID: 3710 RVA: 0x0005014D File Offset: 0x0004E34D
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400085D RID: 2141
	public const string ID = "Phonobox";
}
