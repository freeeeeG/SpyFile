﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200022E RID: 558
public class LogicRibbonWriterConfig : IBuildingConfig
{
	// Token: 0x06000B36 RID: 2870 RVA: 0x0003F388 File Offset: 0x0003D588
	public override BuildingDef CreateBuildingDef()
	{
		string id = LogicRibbonWriterConfig.ID;
		int width = 2;
		int height = 1;
		string anim = "logic_ribbon_writer_kanim";
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
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.ObjectLayer = ObjectLayer.LogicGate;
		buildingDef.SceneLayer = Grid.SceneLayer.LogicGates;
		buildingDef.AlwaysOperational = true;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(LogicRibbonWriter.INPUT_PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICRIBBONWRITER.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICRIBBONWRITER.INPUT_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICRIBBONWRITER.INPUT_PORT_INACTIVE, true, false)
		};
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.RibbonOutputPort(LogicRibbonWriter.OUTPUT_PORT_ID, new CellOffset(1, 0), STRINGS.BUILDINGS.PREFABS.LOGICRIBBONWRITER.LOGIC_PORT_OUTPUT, STRINGS.BUILDINGS.PREFABS.LOGICRIBBONWRITER.OUTPUT_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICRIBBONWRITER.OUTPUT_PORT_INACTIVE, true, false)
		};
		SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Open_DoorInternal", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Close_DoorInternal", NOISE_POLLUTION.NOISY.TIER3);
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicRibbonWriterConfig.ID);
		return buildingDef;
	}

	// Token: 0x06000B37 RID: 2871 RVA: 0x0003F4DC File Offset: 0x0003D6DC
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicRibbonWriter>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x0400068B RID: 1675
	public static string ID = "LogicRibbonWriter";
}
