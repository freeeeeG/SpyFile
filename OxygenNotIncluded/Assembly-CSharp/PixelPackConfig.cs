﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002CC RID: 716
public class PixelPackConfig : IBuildingConfig
{
	// Token: 0x06000E8A RID: 3722 RVA: 0x000504D8 File Offset: 0x0004E6D8
	public override BuildingDef CreateBuildingDef()
	{
		string id = PixelPackConfig.ID;
		int width = 4;
		int height = 1;
		string anim = "pixel_pack_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] construction_mass = new float[]
		{
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0],
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0]
		};
		string[] construction_materials = new string[]
		{
			"Glass",
			"RefinedMetal"
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER3, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Replaceable = false;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(1, 0);
		buildingDef.EnergyConsumptionWhenActive = 10f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.RibbonInputPort(PixelPack.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.PIXELPACK.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.PIXELPACK.INPUT_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.PIXELPACK.INPUT_PORT_INACTIVE, false, false)
		};
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.ObjectLayer = ObjectLayer.Backwall;
		buildingDef.SceneLayer = Grid.SceneLayer.InteriorWall;
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, PixelPackConfig.ID);
		return buildingDef;
	}

	// Token: 0x06000E8B RID: 3723 RVA: 0x00050612 File Offset: 0x0004E812
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddComponent<ZoneTile>();
	}

	// Token: 0x06000E8C RID: 3724 RVA: 0x0005063C File Offset: 0x0004E83C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.Backwall, false);
		go.AddOrGet<PixelPack>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x04000862 RID: 2146
	public static string ID = "PixelPack";
}
