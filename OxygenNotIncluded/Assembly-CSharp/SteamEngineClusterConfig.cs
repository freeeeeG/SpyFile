﻿using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000352 RID: 850
public class SteamEngineClusterConfig : IBuildingConfig
{
	// Token: 0x0600115F RID: 4447 RVA: 0x0005D659 File Offset: 0x0005B859
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06001160 RID: 4448 RVA: 0x0005D660 File Offset: 0x0005B860
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SteamEngineCluster";
		int width = 7;
		int height = 5;
		string anim = "rocket_cluster_steam_engine_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] dense_TIER = BUILDINGS.ROCKETRY_MASS_KG.DENSE_TIER0;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, dense_TIER, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.UtilityInputOffset = new CellOffset(2, 3);
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.GeneratorWattageRating = 600f;
		buildingDef.GeneratorBaseCapacity = 20000f;
		buildingDef.RequiresPowerInput = false;
		buildingDef.RequiresPowerOutput = false;
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x06001161 RID: 4449 RVA: 0x0005D738 File Offset: 0x0005B938
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

	// Token: 0x06001162 RID: 4450 RVA: 0x0005D79C File Offset: 0x0005B99C
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x06001163 RID: 4451 RVA: 0x0005D79E File Offset: 0x0005B99E
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06001164 RID: 4452 RVA: 0x0005D7A0 File Offset: 0x0005B9A0
	public override void DoPostConfigureComplete(GameObject go)
	{
		RocketEngineCluster rocketEngineCluster = go.AddOrGet<RocketEngineCluster>();
		rocketEngineCluster.maxModules = 6;
		rocketEngineCluster.maxHeight = ROCKETRY.ROCKET_HEIGHT.TALL;
		rocketEngineCluster.fuelTag = ElementLoader.FindElementByHash(SimHashes.Steam).tag;
		rocketEngineCluster.efficiency = ROCKETRY.ENGINE_EFFICIENCY.WEAK;
		rocketEngineCluster.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		rocketEngineCluster.requireOxidizer = false;
		rocketEngineCluster.exhaustElement = SimHashes.Steam;
		rocketEngineCluster.exhaustTemperature = ElementLoader.FindElementByHash(SimHashes.Steam).lowTemp + 50f;
		go.AddOrGet<ModuleGenerator>();
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = BUILDINGS.ROCKETRY_MASS_KG.FUEL_TANK_WET_MASS_GAS_LARGE[0];
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate
		});
		FuelTank fuelTank = go.AddOrGet<FuelTank>();
		fuelTank.consumeFuelOnLand = false;
		fuelTank.storage = storage;
		fuelTank.FuelType = ElementLoader.FindElementByHash(SimHashes.Steam).tag;
		fuelTank.targetFillMass = storage.capacityKg;
		fuelTank.physicalFuelCapacity = storage.capacityKg;
		go.AddOrGet<CopyBuildingSettings>();
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityTag = fuelTank.FuelType;
		conduitConsumer.capacityKG = storage.capacityKg;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MONUMENTAL, (float)ROCKETRY.ENGINE_POWER.MID_WEAK, ROCKETRY.FUEL_COST_PER_DISTANCE.GAS_VERY_LOW);
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
		};
	}

	// Token: 0x0400097C RID: 2428
	public const string ID = "SteamEngineCluster";
}
