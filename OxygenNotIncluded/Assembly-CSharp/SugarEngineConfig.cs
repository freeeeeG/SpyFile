using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x0200035A RID: 858
public class SugarEngineConfig : IBuildingConfig
{
	// Token: 0x0600118C RID: 4492 RVA: 0x0005E611 File Offset: 0x0005C811
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600118D RID: 4493 RVA: 0x0005E618 File Offset: 0x0005C818
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SugarEngine";
		int width = 3;
		int height = 3;
		string anim = "rocket_sugar_engine_kanim";
		int hitpoints = 1000;
		float construction_time = 30f;
		float[] dense_TIER = BUILDINGS.ROCKETRY_MASS_KG.DENSE_TIER1;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, dense_TIER, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.InputConduitType = ConduitType.None;
		buildingDef.GeneratorWattageRating = 60f;
		buildingDef.GeneratorBaseCapacity = 2000f;
		buildingDef.RequiresPowerInput = false;
		buildingDef.RequiresPowerOutput = false;
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		return buildingDef;
	}

	// Token: 0x0600118E RID: 4494 RVA: 0x0005E6DC File Offset: 0x0005C8DC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 3), GameTags.Rocket, null)
		};
	}

	// Token: 0x0600118F RID: 4495 RVA: 0x0005E740 File Offset: 0x0005C940
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x06001190 RID: 4496 RVA: 0x0005E742 File Offset: 0x0005C942
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06001191 RID: 4497 RVA: 0x0005E744 File Offset: 0x0005C944
	public override void DoPostConfigureComplete(GameObject go)
	{
		RocketEngineCluster rocketEngineCluster = go.AddOrGet<RocketEngineCluster>();
		rocketEngineCluster.maxModules = 5;
		rocketEngineCluster.maxHeight = ROCKETRY.ROCKET_HEIGHT.SHORT;
		rocketEngineCluster.fuelTag = SimHashes.Sucrose.CreateTag();
		rocketEngineCluster.efficiency = ROCKETRY.ENGINE_EFFICIENCY.STRONG;
		rocketEngineCluster.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		rocketEngineCluster.requireOxidizer = true;
		rocketEngineCluster.exhaustElement = SimHashes.CarbonDioxide;
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
		fuelTank.FuelType = SimHashes.Sucrose.CreateTag();
		fuelTank.targetFillMass = storage.capacityKg;
		fuelTank.physicalFuelCapacity = storage.capacityKg;
		go.AddOrGet<CopyBuildingSettings>();
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = ElementLoader.FindElementByHash(SimHashes.Sucrose).tag;
		manualDeliveryKG.refillMass = storage.capacityKg;
		manualDeliveryKG.capacity = storage.capacityKg;
		manualDeliveryKG.operationalRequirement = Operational.State.None;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.INSIGNIFICANT, (float)ROCKETRY.ENGINE_POWER.EARLY_WEAK, SugarEngineConfig.FUEL_EFFICIENCY);
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
		};
	}

	// Token: 0x0400098F RID: 2447
	public const string ID = "SugarEngine";

	// Token: 0x04000990 RID: 2448
	public const SimHashes FUEL = SimHashes.Sucrose;

	// Token: 0x04000991 RID: 2449
	public const float FUEL_CAPACITY = 450f;

	// Token: 0x04000992 RID: 2450
	public static float FUEL_EFFICIENCY = 0.125f;
}
