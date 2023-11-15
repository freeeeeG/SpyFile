﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000208 RID: 520
public class LiquidLimitValveConfig : IBuildingConfig
{
	// Token: 0x06000A6B RID: 2667 RVA: 0x0003C102 File Offset: 0x0003A302
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000A6C RID: 2668 RVA: 0x0003C10C File Offset: 0x0003A30C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LiquidLimitValve";
		int width = 1;
		int height = 2;
		string anim = "limit_valve_liquid_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] construction_mass = new float[]
		{
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0],
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
		};
		string[] construction_materials = new string[]
		{
			"RefinedMetal",
			"Plastic"
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, tier, 0.2f);
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.Floodable = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 10f;
		buildingDef.PowerInputOffset = new CellOffset(0, 1);
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			new LogicPorts.Port(LimitValve.RESET_PORT_ID, new CellOffset(0, 1), STRINGS.BUILDINGS.PREFABS.LIQUIDLIMITVALVE.LOGIC_PORT_RESET, STRINGS.BUILDINGS.PREFABS.LIQUIDLIMITVALVE.RESET_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LIQUIDLIMITVALVE.RESET_PORT_INACTIVE, false, LogicPortSpriteType.ResetUpdate, true)
		};
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LimitValve.OUTPUT_PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LIQUIDLIMITVALVE.LOGIC_PORT_OUTPUT, STRINGS.BUILDINGS.PREFABS.LIQUIDLIMITVALVE.OUTPUT_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LIQUIDLIMITVALVE.OUTPUT_PORT_INACTIVE, false, false)
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidLimitValve");
		return buildingDef;
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x0003C278 File Offset: 0x0003A478
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGetDef<PoweredActiveTransitionController.Def>();
		UnityEngine.Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<ConduitBridge>().type = ConduitType.Liquid;
		LimitValve limitValve = go.AddOrGet<LimitValve>();
		limitValve.conduitType = ConduitType.Liquid;
		limitValve.maxLimitKg = 500f;
		limitValve.Limit = 0f;
		limitValve.sliderRanges = LimitValveTuning.GetDefaultSlider();
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x0003C2E5 File Offset: 0x0003A4E5
	public override void DoPostConfigureComplete(GameObject go)
	{
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
		go.GetComponent<RequireInputs>().SetRequirements(true, false);
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x04000658 RID: 1624
	public const string ID = "LiquidLimitValve";

	// Token: 0x04000659 RID: 1625
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;
}
