﻿using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020002AF RID: 687
public class OrbitalCargoModuleConfig : IBuildingConfig
{
	// Token: 0x06000E06 RID: 3590 RVA: 0x0004DCA0 File Offset: 0x0004BEA0
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000E07 RID: 3591 RVA: 0x0004DCA8 File Offset: 0x0004BEA8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "OrbitalCargoModule";
		int width = 3;
		int height = 2;
		string anim = "rocket_orbital_deploy_cargo_module_kanim";
		int hitpoints = 1000;
		float construction_time = 30f;
		float[] hollow_TIER = BUILDINGS.ROCKETRY_MASS_KG.HOLLOW_TIER2;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, hollow_TIER, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.DefaultAnimState = "deployed";
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.ForegroundLayer = Grid.SceneLayer.Front;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		return buildingDef;
	}

	// Token: 0x06000E08 RID: 3592 RVA: 0x0004DD4C File Offset: 0x0004BF4C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		List<Tag> list = new List<Tag>();
		list.AddRange(STORAGEFILTERS.NOT_EDIBLE_SOLIDS);
		list.AddRange(STORAGEFILTERS.FOOD);
		Storage storage = go.AddComponent<Storage>();
		storage.showInUI = true;
		storage.capacityKg = OrbitalCargoModuleConfig.TOTAL_STORAGE_MASS;
		storage.showCapacityStatusItem = true;
		storage.showDescriptor = true;
		storage.storageFilters = list;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		go.AddOrGet<StorageLocker>();
		go.AddOrGetDef<OrbitalDeployCargoModule.Def>().numCapsules = (float)OrbitalCargoModuleConfig.NUM_CAPSULES;
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 2), GameTags.Rocket, null)
		};
	}

	// Token: 0x06000E09 RID: 3593 RVA: 0x0004DE1C File Offset: 0x0004C01C
	public override void DoPostConfigureComplete(GameObject go)
	{
		Prioritizable.AddRef(go);
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MODERATE, 0f, 0f);
		FakeFloorAdder fakeFloorAdder = go.AddOrGet<FakeFloorAdder>();
		fakeFloorAdder.floorOffsets = new CellOffset[]
		{
			new CellOffset(-1, -1),
			new CellOffset(0, -1),
			new CellOffset(1, -1)
		};
		fakeFloorAdder.initiallyActive = false;
	}

	// Token: 0x04000825 RID: 2085
	public const string ID = "OrbitalCargoModule";

	// Token: 0x04000826 RID: 2086
	public static int NUM_CAPSULES = 3 * Mathf.RoundToInt(ROCKETRY.CARGO_CAPACITY_SCALE);

	// Token: 0x04000827 RID: 2087
	public static float TOTAL_STORAGE_MASS = 200f * (float)OrbitalCargoModuleConfig.NUM_CAPSULES;
}
