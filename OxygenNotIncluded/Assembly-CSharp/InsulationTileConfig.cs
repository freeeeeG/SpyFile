﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x020001E6 RID: 486
public class InsulationTileConfig : IBuildingConfig
{
	// Token: 0x060009B8 RID: 2488 RVA: 0x000384C0 File Offset: 0x000366C0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "InsulationTile";
		int width = 1;
		int height = 1;
		string anim = "floor_insulated_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		BuildingTemplates.CreateFoundationTileDef(buildingDef);
		buildingDef.ThermalConductivity = 0.01f;
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.UseStructureTemperature = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
		buildingDef.isKAnimTile = true;
		buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_insulated");
		buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_insulated_place");
		buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
		buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_solid_tops_info");
		buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_solid_tops_place_info");
		return buildingDef;
	}

	// Token: 0x060009B9 RID: 2489 RVA: 0x000385B4 File Offset: 0x000367B4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.doReplaceElement = true;
		simCellOccupier.notifyOnMelt = true;
		go.AddOrGet<Insulator>();
		go.AddOrGet<TileTemperature>();
		go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = TileConfig.BlockTileConnectorID;
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
	}

	// Token: 0x060009BA RID: 2490 RVA: 0x00038619 File Offset: 0x00036819
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x0003862C File Offset: 0x0003682C
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<KAnimGridTileVisualizer>();
	}

	// Token: 0x04000614 RID: 1556
	public const string ID = "InsulationTile";
}
