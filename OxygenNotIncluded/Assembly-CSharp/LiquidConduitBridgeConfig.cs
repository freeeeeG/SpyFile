﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x020001FD RID: 509
public class LiquidConduitBridgeConfig : IBuildingConfig
{
	// Token: 0x06000A33 RID: 2611 RVA: 0x0003B04C File Offset: 0x0003924C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LiquidConduitBridge";
		int width = 3;
		int height = 1;
		string anim = "utilityliquidbridge_kanim";
		int hitpoints = 10;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Conduit;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.ObjectLayer = ObjectLayer.LiquidConduitConnection;
		buildingDef.SceneLayer = Grid.SceneLayer.LiquidConduitBridges;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidConduitBridge");
		return buildingDef;
	}

	// Token: 0x06000A34 RID: 2612 RVA: 0x0003B120 File Offset: 0x00039320
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<ConduitBridge>().type = ConduitType.Liquid;
	}

	// Token: 0x06000A35 RID: 2613 RVA: 0x0003B149 File Offset: 0x00039349
	public override void DoPostConfigureComplete(GameObject go)
	{
		UnityEngine.Object.DestroyImmediate(go.GetComponent<RequireInputs>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
	}

	// Token: 0x04000642 RID: 1602
	public const string ID = "LiquidConduitBridge";

	// Token: 0x04000643 RID: 1603
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;
}
