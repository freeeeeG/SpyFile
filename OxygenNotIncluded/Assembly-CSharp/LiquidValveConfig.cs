﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x0200020E RID: 526
public class LiquidValveConfig : IBuildingConfig
{
	// Token: 0x06000A83 RID: 2691 RVA: 0x0003CB8C File Offset: 0x0003AD8C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LiquidValve";
		int width = 1;
		int height = 2;
		string anim = "valveliquid_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, tier2, 0.2f);
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidValve");
		return buildingDef;
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x0003CC30 File Offset: 0x0003AE30
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		ValveBase valveBase = go.AddOrGet<ValveBase>();
		valveBase.conduitType = ConduitType.Liquid;
		valveBase.maxFlow = 10f;
		valveBase.animFlowRanges = new ValveBase.AnimRangeInfo[]
		{
			new ValveBase.AnimRangeInfo(3f, "lo"),
			new ValveBase.AnimRangeInfo(7f, "med"),
			new ValveBase.AnimRangeInfo(10f, "hi")
		};
		go.AddOrGet<Valve>();
		go.AddOrGet<Workable>().workTime = 5f;
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x0003CCD4 File Offset: 0x0003AED4
	public override void DoPostConfigureComplete(GameObject go)
	{
		UnityEngine.Object.DestroyImmediate(go.GetComponent<RequireInputs>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x04000664 RID: 1636
	public const string ID = "LiquidValve";

	// Token: 0x04000665 RID: 1637
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;
}
