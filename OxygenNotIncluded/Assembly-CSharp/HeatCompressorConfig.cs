﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001CD RID: 461
public class HeatCompressorConfig : IBuildingConfig
{
	// Token: 0x06000932 RID: 2354 RVA: 0x000365FC File Offset: 0x000347FC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "HeatCompressor";
		int width = 4;
		int height = 4;
		string anim = "hqbase_kanim";
		int hitpoints = 250;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER7;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER5, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = 400f;
		buildingDef.DefaultAnimState = "idle";
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(2, 0);
		buildingDef.EnergyConsumptionWhenActive = 1600f;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(2, 0);
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(-1, 1), STRINGS.BUILDINGS.PREFABS.TELEPORTALPAD.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.TELEPORTALPAD.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.TELEPORTALPAD.LOGIC_PORT_INACTIVE, false, false)
		};
		SoundEventVolumeCache.instance.AddVolume("hqbase_kanim", "Portal_LP", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("hqbase_kanim", "Portal_open", NOISE_POLLUTION.NOISY.TIER4);
		SoundEventVolumeCache.instance.AddVolume("hqbase_kanim", "Portal_close", NOISE_POLLUTION.NOISY.TIER4);
		buildingDef.Deprecated = true;
		return buildingDef;
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x00036750 File Offset: 0x00034950
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddComponent<Storage>();
		storage.showDescriptor = false;
		storage.showInUI = true;
		storage.storageFilters = STORAGEFILTERS.LIQUIDS;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		Storage storage2 = go.AddComponent<Storage>();
		storage2.showDescriptor = false;
		storage2.showInUI = true;
		storage2.storageFilters = STORAGEFILTERS.LIQUIDS;
		storage2.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		Storage storage3 = go.AddComponent<Storage>();
		storage3.showDescriptor = false;
		storage3.showInUI = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityKG = 100f;
		conduitConsumer.storage = storage;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.storage = storage2;
		conduitDispenser.alwaysDispense = true;
		go.AddOrGet<HeatCompressor>().SetStorage(storage, storage2, storage3);
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x0003680E File Offset: 0x00034A0E
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x040005C5 RID: 1477
	public const string ID = "HeatCompressor";
}
