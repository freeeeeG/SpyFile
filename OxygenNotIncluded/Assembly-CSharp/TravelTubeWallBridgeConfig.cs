using System;
using TUNING;
using UnityEngine;

// Token: 0x0200036D RID: 877
public class TravelTubeWallBridgeConfig : IBuildingConfig
{
	// Token: 0x060011EE RID: 4590 RVA: 0x00060FB0 File Offset: 0x0005F1B0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "TravelTubeWallBridge";
		int width = 1;
		int height = 1;
		string anim = "tube_tile_bridge_kanim";
		int hitpoints = 100;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, plastics, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		BuildingTemplates.CreateFoundationTileDef(buildingDef);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.AudioCategory = "Plastic";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R90;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 2);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
		return buildingDef;
	}

	// Token: 0x060011EF RID: 4591 RVA: 0x0006106A File Offset: 0x0005F26A
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.doReplaceElement = true;
		simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT.PENALTY_3;
		simCellOccupier.notifyOnMelt = true;
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		go.AddOrGet<TileTemperature>();
		go.AddOrGet<TravelTubeBridge>();
	}

	// Token: 0x060011F0 RID: 4592 RVA: 0x000610AA File Offset: 0x0005F2AA
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AddNetworkLink(go).visualizeOnly = true;
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x060011F1 RID: 4593 RVA: 0x000610C8 File Offset: 0x0005F2C8
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AddNetworkLink(go).visualizeOnly = true;
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x060011F2 RID: 4594 RVA: 0x000610E5 File Offset: 0x0005F2E5
	public override void DoPostConfigureComplete(GameObject go)
	{
		this.AddNetworkLink(go).visualizeOnly = false;
		go.AddOrGet<BuildingCellVisualizer>();
		go.AddOrGet<KPrefabID>().AddTag(GameTags.TravelTubeBridges, false);
	}

	// Token: 0x060011F3 RID: 4595 RVA: 0x0006110C File Offset: 0x0005F30C
	protected virtual TravelTubeUtilityNetworkLink AddNetworkLink(GameObject go)
	{
		TravelTubeUtilityNetworkLink travelTubeUtilityNetworkLink = go.AddOrGet<TravelTubeUtilityNetworkLink>();
		travelTubeUtilityNetworkLink.link1 = new CellOffset(-1, 0);
		travelTubeUtilityNetworkLink.link2 = new CellOffset(1, 0);
		return travelTubeUtilityNetworkLink;
	}

	// Token: 0x040009C0 RID: 2496
	public const string ID = "TravelTubeWallBridge";
}
