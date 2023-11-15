﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000225 RID: 549
public class LogicLightSensorConfig : IBuildingConfig
{
	// Token: 0x06000B0E RID: 2830 RVA: 0x0003E6FC File Offset: 0x0003C8FC
	public override BuildingDef CreateBuildingDef()
	{
		string id = LogicLightSensorConfig.ID;
		int width = 1;
		int height = 1;
		string anim = "logiclightsensor_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] construction_mass = new float[]
		{
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0],
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0]
		};
		string[] construction_materials = new string[]
		{
			"RefinedMetal",
			"Transparent"
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.AlwaysOperational = true;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>();
		buildingDef.LogicOutputPorts.Add(LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICLIGHTSENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICLIGHTSENSOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICLIGHTSENSOR.LOGIC_PORT_INACTIVE, true, false));
		SoundEventVolumeCache.instance.AddVolume("switchthermal_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("switchthermal_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicLightSensorConfig.ID);
		return buildingDef;
	}

	// Token: 0x06000B0F RID: 2831 RVA: 0x0003E835 File Offset: 0x0003CA35
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicLightSensor logicLightSensor = go.AddOrGet<LogicLightSensor>();
		logicLightSensor.manuallyControlled = false;
		logicLightSensor.minBrightness = 0f;
		logicLightSensor.maxBrightness = 15000f;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x04000681 RID: 1665
	public static string ID = "LogicLightSensor";
}
