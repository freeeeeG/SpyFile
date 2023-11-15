﻿using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020001EC RID: 492
public class KeroseneEngineClusterSmallConfig : IBuildingConfig
{
	// Token: 0x060009D5 RID: 2517 RVA: 0x00038F30 File Offset: 0x00037130
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x00038F38 File Offset: 0x00037138
	public override BuildingDef CreateBuildingDef()
	{
		string id = "KeroseneEngineClusterSmall";
		int width = 3;
		int height = 4;
		string anim = "rocket_petro_engine_small_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] engine_MASS_SMALL = BUILDINGS.ROCKETRY_MASS_KG.ENGINE_MASS_SMALL;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, engine_MASS_SMALL, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.UtilityInputOffset = new CellOffset(0, 2);
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.GeneratorWattageRating = 240f;
		buildingDef.GeneratorBaseCapacity = 4000f;
		buildingDef.RequiresPowerInput = false;
		buildingDef.RequiresPowerOutput = false;
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x00039010 File Offset: 0x00037210
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 4), GameTags.Rocket, null)
		};
	}

	// Token: 0x060009D8 RID: 2520 RVA: 0x00039074 File Offset: 0x00037274
	public override void DoPostConfigureComplete(GameObject go)
	{
		RocketEngineCluster rocketEngineCluster = go.AddOrGet<RocketEngineCluster>();
		rocketEngineCluster.maxModules = 4;
		rocketEngineCluster.maxHeight = ROCKETRY.ROCKET_HEIGHT.MEDIUM;
		rocketEngineCluster.fuelTag = SimHashes.Petroleum.CreateTag();
		rocketEngineCluster.efficiency = ROCKETRY.ENGINE_EFFICIENCY.MEDIUM;
		rocketEngineCluster.requireOxidizer = true;
		rocketEngineCluster.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		rocketEngineCluster.exhaustElement = SimHashes.CarbonDioxide;
		rocketEngineCluster.exhaustTemperature = 1263.15f;
		go.AddOrGet<ModuleGenerator>();
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 450f;
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate
		});
		FuelTank fuelTank = go.AddOrGet<FuelTank>();
		fuelTank.consumeFuelOnLand = false;
		fuelTank.storage = storage;
		fuelTank.FuelType = SimHashes.Petroleum.CreateTag();
		fuelTank.targetFillMass = storage.capacityKg;
		fuelTank.physicalFuelCapacity = storage.capacityKg;
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MODERATE_PLUS, (float)ROCKETRY.ENGINE_POWER.MID_STRONG, ROCKETRY.FUEL_COST_PER_DISTANCE.MEDIUM);
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
		};
	}

	// Token: 0x04000622 RID: 1570
	public const string ID = "KeroseneEngineClusterSmall";

	// Token: 0x04000623 RID: 1571
	public const SimHashes FUEL = SimHashes.Petroleum;

	// Token: 0x04000624 RID: 1572
	public const float FUEL_CAPACITY = 450f;
}
