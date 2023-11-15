﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001C3 RID: 451
public class GravitasDoorConfig : IBuildingConfig
{
	// Token: 0x060008F8 RID: 2296 RVA: 0x00035035 File Offset: 0x00033235
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x0003503C File Offset: 0x0003323C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GravitasDoor";
		int width = 1;
		int height = 3;
		string anim = "gravitas_door_internal_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 1f);
		buildingDef.ShowInBuildMenu = false;
		buildingDef.Entombable = false;
		buildingDef.Floodable = false;
		buildingDef.Invincible = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.R90;
		buildingDef.ForegroundLayer = Grid.SceneLayer.InteriorWall;
		buildingDef.LogicInputPorts = DoorConfig.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.LogicInputPorts = GravitasDoorConfig.CreateSingleInputPortList(new CellOffset(0, 0));
		SoundEventVolumeCache.instance.AddVolume("gravitas_door_internal_kanim", "GravitasDoorInternal_open", NOISE_POLLUTION.NOISY.TIER2);
		SoundEventVolumeCache.instance.AddVolume("gravitas_door_internal_kanim", "GravitasDoorInternal_close", NOISE_POLLUTION.NOISY.TIER2);
		return buildingDef;
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x00035110 File Offset: 0x00033310
	public static List<LogicPorts.Port> CreateSingleInputPortList(CellOffset offset)
	{
		return new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(Door.OPEN_CLOSE_PORT_ID, offset, STRINGS.BUILDINGS.PREFABS.DOOR.LOGIC_OPEN, STRINGS.BUILDINGS.PREFABS.DOOR.LOGIC_OPEN_ACTIVE, STRINGS.BUILDINGS.PREFABS.DOOR.LOGIC_OPEN_INACTIVE, false, false)
		};
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x00035154 File Offset: 0x00033354
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.Gravitas);
		Door door = go.AddOrGet<Door>();
		door.unpoweredAnimSpeed = 1f;
		door.doorType = Door.DoorType.Internal;
		door.doorOpeningSoundEventName = "GravitasDoorInternal_open";
		door.doorClosingSoundEventName = "GravitasDoorInternal_close";
		go.AddOrGet<ZoneTile>();
		go.AddOrGet<AccessControl>().controlEnabled = true;
		go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Door;
		go.AddOrGet<Workable>().workTime = 3f;
		go.AddOrGet<KBoxCollider2D>();
		Prioritizable.AddRef(go);
		UnityEngine.Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x000351E4 File Offset: 0x000333E4
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040005A2 RID: 1442
	public const string ID = "GravitasDoor";
}
