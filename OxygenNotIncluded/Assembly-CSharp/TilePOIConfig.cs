﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x02000369 RID: 873
public class TilePOIConfig : IBuildingConfig
{
	// Token: 0x060011DA RID: 4570 RVA: 0x000609E0 File Offset: 0x0005EBE0
	public override BuildingDef CreateBuildingDef()
	{
		string id = TilePOIConfig.ID;
		int width = 1;
		int height = 1;
		string anim = "floor_mesh_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] all_MINERALS = MATERIALS.ALL_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Overheatable = false;
		buildingDef.Repairable = false;
		buildingDef.Replaceable = false;
		buildingDef.Invincible = true;
		buildingDef.IsFoundation = true;
		buildingDef.UseStructureTemperature = false;
		buildingDef.TileLayer = ObjectLayer.FoundationTile;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
		buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
		buildingDef.isKAnimTile = true;
		buildingDef.DebugOnly = true;
		buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_POI");
		buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_POI");
		buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
		buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_POI_tops_decor_info");
		buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_POI_tops_decor_info");
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x060011DB RID: 4571 RVA: 0x00060AFF File Offset: 0x0005ECFF
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<SimCellOccupier>().doReplaceElement = true;
		go.AddOrGet<TileTemperature>();
		go.AddOrGet<KAnimGridTileVisualizer>();
	}

	// Token: 0x060011DC RID: 4572 RVA: 0x00060B36 File Offset: 0x0005ED36
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.Bunker, false);
		go.AddComponent<SimTemperatureTransfer>();
		go.GetComponent<Deconstructable>().allowDeconstruction = true;
	}

	// Token: 0x060011DD RID: 4573 RVA: 0x00060B5C File Offset: 0x0005ED5C
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<KAnimGridTileVisualizer>();
	}

	// Token: 0x040009B8 RID: 2488
	public static string ID = "TilePOI";
}
