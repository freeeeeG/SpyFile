﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x0200003C RID: 60
public class ClusterTelescopeEnclosedConfig : IBuildingConfig
{
	// Token: 0x0600011C RID: 284 RVA: 0x00008873 File Offset: 0x00006A73
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600011D RID: 285 RVA: 0x0000887C File Offset: 0x00006A7C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ClusterTelescopeEnclosed";
		int width = 4;
		int height = 6;
		string anim = "telescope_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00008920 File Offset: 0x00006B20
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
		go.AddOrGetDef<PoweredController.Def>();
		ClusterTelescope.Def def = go.AddOrGetDef<ClusterTelescope.Def>();
		def.clearScanCellRadius = 4;
		def.analyzeClusterRadius = 4;
		def.workableOverrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_telescope_kanim")
		};
		def.skyVisibilityInfo = ClusterTelescopeEnclosedConfig.SKY_VISIBILITY_INFO;
		def.providesOxygen = true;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1000f;
		storage.showInUI = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 1f;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.capacityKG = 10f;
		conduitConsumer.forceAlwaysSatisfied = true;
	}

	// Token: 0x0600011F RID: 287 RVA: 0x000089F8 File Offset: 0x00006BF8
	public override void DoPostConfigureComplete(GameObject go)
	{
		ClusterTelescopeEnclosedConfig.AddVisualizer(go);
	}

	// Token: 0x06000120 RID: 288 RVA: 0x00008A00 File Offset: 0x00006C00
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		ClusterTelescopeEnclosedConfig.AddVisualizer(go);
	}

	// Token: 0x06000121 RID: 289 RVA: 0x00008A08 File Offset: 0x00006C08
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		ClusterTelescopeEnclosedConfig.AddVisualizer(go);
	}

	// Token: 0x06000122 RID: 290 RVA: 0x00008A10 File Offset: 0x00006C10
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.OriginOffset.y = 3;
		skyVisibilityVisualizer.TwoWideOrgin = true;
		skyVisibilityVisualizer.RangeMin = -4;
		skyVisibilityVisualizer.RangeMax = 5;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x040000A6 RID: 166
	public const string ID = "ClusterTelescopeEnclosed";

	// Token: 0x040000A7 RID: 167
	public const int SCAN_RADIUS = 4;

	// Token: 0x040000A8 RID: 168
	public const int VERTICAL_SCAN_OFFSET = 3;

	// Token: 0x040000A9 RID: 169
	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 3), 4, new CellOffset(1, 3), 4, 0);
}
