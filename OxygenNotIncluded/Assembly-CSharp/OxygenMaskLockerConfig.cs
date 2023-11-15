﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x020002C0 RID: 704
public class OxygenMaskLockerConfig : IBuildingConfig
{
	// Token: 0x06000E55 RID: 3669 RVA: 0x0004F270 File Offset: 0x0004D470
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000E56 RID: 3670 RVA: 0x0004F278 File Offset: 0x0004D478
	public override BuildingDef CreateBuildingDef()
	{
		string id = "OxygenMaskLocker";
		int width = 1;
		int height = 2;
		string anim = "oxygen_mask_locker_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] construction_materials = raw_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.PreventIdleTraversalPastBuilding = true;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "OxygenMaskLocker");
		return buildingDef;
	}

	// Token: 0x06000E57 RID: 3671 RVA: 0x0004F2EC File Offset: 0x0004D4EC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<SuitLocker>().OutfitTags = new Tag[]
		{
			GameTags.OxygenMask
		};
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 1f;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.capacityKG = 30f;
		go.AddOrGet<AnimTileable>().tags = new Tag[]
		{
			new Tag("OxygenMaskLocker"),
			new Tag("OxygenMaskMarker")
		};
		go.AddOrGet<Storage>();
		Prioritizable.AddRef(go);
	}

	// Token: 0x06000E58 RID: 3672 RVA: 0x0004F39B File Offset: 0x0004D59B
	public override void DoPostConfigureComplete(GameObject go)
	{
		SymbolOverrideControllerUtil.AddToPrefab(go);
	}

	// Token: 0x0400083D RID: 2109
	public const string ID = "OxygenMaskLocker";
}
