﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000214 RID: 532
public class LogicDiseaseSensorConfig : IBuildingConfig
{
	// Token: 0x06000A9D RID: 2717 RVA: 0x0003D3F8 File Offset: 0x0003B5F8
	public override BuildingDef CreateBuildingDef()
	{
		string id = LogicDiseaseSensorConfig.ID;
		int width = 1;
		int height = 1;
		string anim = "diseasesensor_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] construction_mass = new float[]
		{
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0],
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
		};
		string[] construction_materials = new string[]
		{
			"RefinedMetal",
			"Plastic"
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
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.AlwaysOperational = true;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICDISEASESENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICDISEASESENSOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICDISEASESENSOR.LOGIC_PORT_INACTIVE, true, false)
		};
		SoundEventVolumeCache.instance.AddVolume("diseasesensor_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("diseasesensor_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicDiseaseSensorConfig.ID);
		return buildingDef;
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x0003D521 File Offset: 0x0003B721
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicDiseaseSensor logicDiseaseSensor = go.AddOrGet<LogicDiseaseSensor>();
		logicDiseaseSensor.Threshold = 0f;
		logicDiseaseSensor.ActivateAboveThreshold = true;
		logicDiseaseSensor.manuallyControlled = false;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x0400066D RID: 1645
	public static string ID = "LogicDiseaseSensor";
}
