using System;
using TUNING;
using UnityEngine;

// Token: 0x02000362 RID: 866
public class TelephoneConfig : IBuildingConfig
{
	// Token: 0x060011B3 RID: 4531 RVA: 0x0005FEAF File Offset: 0x0005E0AF
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060011B4 RID: 4532 RVA: 0x0005FEB8 File Offset: 0x0005E0B8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Telephone";
		int width = 1;
		int height = 2;
		string anim = "telephone_kanim";
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
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		return buildingDef;
	}

	// Token: 0x060011B5 RID: 4533 RVA: 0x0005FF4C File Offset: 0x0005E14C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		Telephone telephone = go.AddOrGet<Telephone>();
		telephone.babbleEffect = "TelephoneBabble";
		telephone.chatEffect = "TelephoneChat";
		telephone.longDistanceEffect = "TelephoneLongDistance";
		telephone.trackingEffect = "RecentlyTelephoned";
		go.AddOrGet<TelephoneCallerWorkable>().basePriority = RELAXATION.PRIORITY.TIER5;
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x060011B6 RID: 4534 RVA: 0x0005FFD8 File Offset: 0x0005E1D8
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400099E RID: 2462
	public const string ID = "Telephone";

	// Token: 0x0400099F RID: 2463
	public const float ringTime = 15f;

	// Token: 0x040009A0 RID: 2464
	public const float callTime = 25f;
}
