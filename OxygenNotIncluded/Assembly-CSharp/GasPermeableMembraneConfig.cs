using System;
using TUNING;
using UnityEngine;

// Token: 0x020001B0 RID: 432
public class GasPermeableMembraneConfig : IBuildingConfig
{
	// Token: 0x06000889 RID: 2185 RVA: 0x000319E4 File Offset: 0x0002FBE4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GasPermeableMembrane";
		int width = 1;
		int height = 1;
		string anim = "floor_gasperm_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		BuildingTemplates.CreateFoundationTileDef(buildingDef);
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
		buildingDef.isKAnimTile = true;
		buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_gasmembrane");
		buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_gasmembrane_place");
		buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
		buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_mesh_tops_decor_info");
		buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_mesh_tops_decor_place_info");
		buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
		return buildingDef;
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x00031AD0 File Offset: 0x0002FCD0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.setLiquidImpermeable = true;
		simCellOccupier.doReplaceElement = false;
		go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = MeshTileConfig.BlockTileConnectorID;
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		go.AddComponent<SimTemperatureTransfer>();
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x00031B2E File Offset: 0x0002FD2E
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<ZoneTile>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x00031B48 File Offset: 0x0002FD48
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<KAnimGridTileVisualizer>();
	}

	// Token: 0x04000567 RID: 1383
	public const string ID = "GasPermeableMembrane";
}
