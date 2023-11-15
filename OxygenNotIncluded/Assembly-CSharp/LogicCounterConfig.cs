﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000212 RID: 530
public class LogicCounterConfig : IBuildingConfig
{
	// Token: 0x06000A95 RID: 2709 RVA: 0x0003D0F4 File Offset: 0x0003B2F4
	public override BuildingDef CreateBuildingDef()
	{
		string id = LogicCounterConfig.ID;
		int width = 1;
		int height = 3;
		string anim = "logic_counter_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.PermittedRotations = PermittedRotations.FlipV;
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.ObjectLayer = ObjectLayer.LogicGate;
		buildingDef.SceneLayer = Grid.SceneLayer.LogicGates;
		buildingDef.AlwaysOperational = true;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(LogicCounter.INPUT_PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICCOUNTER.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICCOUNTER.INPUT_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICCOUNTER.INPUT_PORT_INACTIVE, true, false),
			new LogicPorts.Port(LogicCounter.RESET_PORT_ID, new CellOffset(0, 1), STRINGS.BUILDINGS.PREFABS.LOGICCOUNTER.LOGIC_PORT_RESET, STRINGS.BUILDINGS.PREFABS.LOGICCOUNTER.RESET_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICCOUNTER.RESET_PORT_INACTIVE, false, LogicPortSpriteType.ResetUpdate, true)
		};
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicCounter.OUTPUT_PORT_ID, new CellOffset(0, 2), STRINGS.BUILDINGS.PREFABS.LOGICCOUNTER.LOGIC_PORT_OUTPUT, STRINGS.BUILDINGS.PREFABS.LOGICCOUNTER.OUTPUT_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICCOUNTER.OUTPUT_PORT_INACTIVE, false, false)
		};
		SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Open_DoorInternal", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Close_DoorInternal", NOISE_POLLUTION.NOISY.TIER3);
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicCounterConfig.ID);
		return buildingDef;
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x0003D280 File Offset: 0x0003B480
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicCounter>().manuallyControlled = false;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
		go.GetComponent<Switch>().defaultState = false;
	}

	// Token: 0x0400066B RID: 1643
	public static string ID = "LogicCounter";
}
