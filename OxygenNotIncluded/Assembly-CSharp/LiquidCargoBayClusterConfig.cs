﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x020001F9 RID: 505
public class LiquidCargoBayClusterConfig : IBuildingConfig
{
	// Token: 0x06000A1F RID: 2591 RVA: 0x0003AA4F File Offset: 0x00038C4F
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x0003AA58 File Offset: 0x00038C58
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LiquidCargoBayCluster";
		int width = 5;
		int height = 5;
		string anim = "rocket_cluster_storage_liquid_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] dense_TIER = BUILDINGS.ROCKETRY_MASS_KG.DENSE_TIER2;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, dense_TIER, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x06000A21 RID: 2593 RVA: 0x0003AB14 File Offset: 0x00038D14
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, null)
		};
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x0003AB78 File Offset: 0x00038D78
	public override void DoPostConfigureComplete(GameObject go)
	{
		go = BuildingTemplates.ExtendBuildingToClusterCargoBay(go, this.CAPACITY, STORAGEFILTERS.LIQUIDS, CargoBay.CargoType.Liquids);
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MODERATE_PLUS, 0f, 0f);
	}

	// Token: 0x0400063B RID: 1595
	public const string ID = "LiquidCargoBayCluster";

	// Token: 0x0400063C RID: 1596
	public float CAPACITY = ROCKETRY.LIQUID_CARGO_BAY_CLUSTER_CAPACITY * ROCKETRY.CARGO_CAPACITY_SCALE;
}
