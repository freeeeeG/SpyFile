using System;
using TUNING;
using UnityEngine;

// Token: 0x0200029B RID: 667
public class MissionControlConfig : IBuildingConfig
{
	// Token: 0x06000D9D RID: 3485 RVA: 0x0004C2C4 File Offset: 0x0004A4C4
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_VANILLA_ONLY;
	}

	// Token: 0x06000D9E RID: 3486 RVA: 0x0004C2CC File Offset: 0x0004A4CC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MissionControl";
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

	// Token: 0x06000D9F RID: 3487 RVA: 0x0004C368 File Offset: 0x0004A568
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		BuildingDef def = go.GetComponent<BuildingComplete>().Def;
		Prioritizable.AddRef(go);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGetDef<PoweredController.Def>();
		go.AddOrGetDef<SkyVisibilityMonitor.Def>().skyVisibilityInfo = MissionControlConfig.SKY_VISIBILITY_INFO;
		go.AddOrGetDef<MissionControl.Def>();
		MissionControlWorkable missionControlWorkable = go.AddOrGet<MissionControlWorkable>();
		missionControlWorkable.requiredSkillPerk = Db.Get().SkillPerks.CanMissionControl.Id;
		missionControlWorkable.workLayer = Grid.SceneLayer.BuildingUse;
	}

	// Token: 0x06000DA0 RID: 3488 RVA: 0x0004C3E9 File Offset: 0x0004A5E9
	public override void DoPostConfigureComplete(GameObject go)
	{
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Laboratory.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		MissionControlConfig.AddVisualizer(go);
	}

	// Token: 0x06000DA1 RID: 3489 RVA: 0x0004C417 File Offset: 0x0004A617
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		MissionControlConfig.AddVisualizer(go);
	}

	// Token: 0x06000DA2 RID: 3490 RVA: 0x0004C41F File Offset: 0x0004A61F
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		MissionControlConfig.AddVisualizer(go);
	}

	// Token: 0x06000DA3 RID: 3491 RVA: 0x0004C427 File Offset: 0x0004A627
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.OriginOffset.y = 2;
		skyVisibilityVisualizer.RangeMin = -1;
		skyVisibilityVisualizer.RangeMax = 1;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x040007D8 RID: 2008
	public const string ID = "MissionControl";

	// Token: 0x040007D9 RID: 2009
	public const float EFFECT_DURATION = 600f;

	// Token: 0x040007DA RID: 2010
	public const float SPEED_MULTIPLIER = 1.2f;

	// Token: 0x040007DB RID: 2011
	public const int SCAN_RADIUS = 1;

	// Token: 0x040007DC RID: 2012
	public const int VERTICAL_SCAN_OFFSET = 2;

	// Token: 0x040007DD RID: 2013
	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 2), 1, new CellOffset(0, 2), 1, 0);
}
