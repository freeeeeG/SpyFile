using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000315 RID: 789
public class RocketControlStationConfig : IBuildingConfig
{
	// Token: 0x06001003 RID: 4099 RVA: 0x00057237 File Offset: 0x00055437
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06001004 RID: 4100 RVA: 0x00057240 File Offset: 0x00055440
	public override BuildingDef CreateBuildingDef()
	{
		string id = RocketControlStationConfig.ID;
		int width = 2;
		int height = 2;
		string anim = "rocket_control_station_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER2, tier2, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Repairable = false;
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.DefaultAnimState = "off";
		buildingDef.OnePerWorld = true;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(RocketControlStation.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.ROCKETCONTROLSTATION.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.ROCKETCONTROLSTATION.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.ROCKETCONTROLSTATION.LOGIC_PORT_INACTIVE, false, false)
		};
		return buildingDef;
	}

	// Token: 0x06001005 RID: 4101 RVA: 0x00057305 File Offset: 0x00055505
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(GameTags.RocketInteriorBuilding, false);
		component.AddTag(GameTags.UniquePerWorld, false);
	}

	// Token: 0x06001006 RID: 4102 RVA: 0x00057324 File Offset: 0x00055524
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGet<RocketControlStationIdleWorkable>().workLayer = Grid.SceneLayer.BuildingUse;
		go.AddOrGet<RocketControlStationLaunchWorkable>().workLayer = Grid.SceneLayer.BuildingUse;
		go.AddOrGet<RocketControlStation>();
		go.AddOrGetDef<PoweredController.Def>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RocketInterior, false);
	}

	// Token: 0x040008D8 RID: 2264
	public static string ID = "RocketControlStation";

	// Token: 0x040008D9 RID: 2265
	public const float CONSOLE_WORK_TIME = 30f;

	// Token: 0x040008DA RID: 2266
	public const float CONSOLE_IDLE_TIME = 120f;

	// Token: 0x040008DB RID: 2267
	public const float WARNING_COOLDOWN = 30f;

	// Token: 0x040008DC RID: 2268
	public const float DEFAULT_SPEED = 1f;

	// Token: 0x040008DD RID: 2269
	public const float SLOW_SPEED = 0.5f;

	// Token: 0x040008DE RID: 2270
	public const float DEFAULT_PILOT_MODIFIER = 1f;
}
