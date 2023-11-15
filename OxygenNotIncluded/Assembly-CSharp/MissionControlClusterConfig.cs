using System;
using TUNING;
using UnityEngine;

// Token: 0x0200029A RID: 666
public class MissionControlClusterConfig : IBuildingConfig
{
	// Token: 0x06000D94 RID: 3476 RVA: 0x0004C112 File Offset: 0x0004A312
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000D95 RID: 3477 RVA: 0x0004C11C File Offset: 0x0004A31C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MissionControlCluster";
		int width = 3;
		int height = 3;
		string anim = "mission_control_station_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 960f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.DefaultAnimState = "off";
		return buildingDef;
	}

	// Token: 0x06000D96 RID: 3478 RVA: 0x0004C1B8 File Offset: 0x0004A3B8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		BuildingDef def = go.GetComponent<BuildingComplete>().Def;
		Prioritizable.AddRef(go);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGetDef<PoweredController.Def>();
		go.AddOrGetDef<SkyVisibilityMonitor.Def>().skyVisibilityInfo = MissionControlClusterConfig.SKY_VISIBILITY_INFO;
		go.AddOrGetDef<MissionControlCluster.Def>();
		MissionControlClusterWorkable missionControlClusterWorkable = go.AddOrGet<MissionControlClusterWorkable>();
		missionControlClusterWorkable.requiredSkillPerk = Db.Get().SkillPerks.CanMissionControl.Id;
		missionControlClusterWorkable.workLayer = Grid.SceneLayer.BuildingUse;
	}

	// Token: 0x06000D97 RID: 3479 RVA: 0x0004C239 File Offset: 0x0004A439
	public override void DoPostConfigureComplete(GameObject go)
	{
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Laboratory.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		MissionControlClusterConfig.AddVisualizer(go);
	}

	// Token: 0x06000D98 RID: 3480 RVA: 0x0004C267 File Offset: 0x0004A467
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		MissionControlClusterConfig.AddVisualizer(go);
	}

	// Token: 0x06000D99 RID: 3481 RVA: 0x0004C26F File Offset: 0x0004A46F
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		MissionControlClusterConfig.AddVisualizer(go);
	}

	// Token: 0x06000D9A RID: 3482 RVA: 0x0004C277 File Offset: 0x0004A477
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.OriginOffset.y = 2;
		skyVisibilityVisualizer.RangeMin = -1;
		skyVisibilityVisualizer.RangeMax = 1;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x040007D1 RID: 2001
	public const string ID = "MissionControlCluster";

	// Token: 0x040007D2 RID: 2002
	public const int WORK_RANGE_RADIUS = 2;

	// Token: 0x040007D3 RID: 2003
	public const float EFFECT_DURATION = 600f;

	// Token: 0x040007D4 RID: 2004
	public const float SPEED_MULTIPLIER = 1.2f;

	// Token: 0x040007D5 RID: 2005
	public const int SCAN_RADIUS = 1;

	// Token: 0x040007D6 RID: 2006
	public const int VERTICAL_SCAN_OFFSET = 2;

	// Token: 0x040007D7 RID: 2007
	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 2), 1, new CellOffset(0, 2), 1, 0);
}
