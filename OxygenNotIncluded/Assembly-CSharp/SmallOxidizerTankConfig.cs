﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000332 RID: 818
public class SmallOxidizerTankConfig : IBuildingConfig
{
	// Token: 0x060010A2 RID: 4258 RVA: 0x00059FD3 File Offset: 0x000581D3
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060010A3 RID: 4259 RVA: 0x00059FDC File Offset: 0x000581DC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SmallOxidizerTank";
		int width = 3;
		int height = 2;
		string anim = "rocket_oxidizer_tank_small_kanim";
		int hitpoints = 1000;
		float construction_time = 30f;
		float[] dense_TIER = TUNING.BUILDINGS.ROCKETRY_MASS_KG.DENSE_TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, dense_TIER, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.DefaultAnimState = "grounded";
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x060010A4 RID: 4260 RVA: 0x0005A08C File Offset: 0x0005828C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 2), GameTags.Rocket, null)
		};
		Prioritizable.AddRef(go);
	}

	// Token: 0x060010A5 RID: 4261 RVA: 0x0005A0F8 File Offset: 0x000582F8
	public override void DoPostConfigureComplete(GameObject go)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 450f;
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate
		});
		FlatTagFilterable flatTagFilterable = go.AddOrGet<FlatTagFilterable>();
		flatTagFilterable.tagOptions = new List<Tag>
		{
			SimHashes.OxyRock.CreateTag(),
			SimHashes.Fertilizer.CreateTag()
		};
		flatTagFilterable.headerText = STRINGS.BUILDINGS.PREFABS.OXIDIZERTANK.UI_FILTER_CATEGORY;
		OxidizerTank oxidizerTank = go.AddOrGet<OxidizerTank>();
		oxidizerTank.consumeOnLand = !DlcManager.FeatureClusterSpaceEnabled();
		oxidizerTank.storage = storage;
		oxidizerTank.targetFillMass = 450f;
		oxidizerTank.maxFillMass = 450f;
		oxidizerTank.supportsMultipleOxidizers = true;
		go.AddOrGet<Prioritizable>();
		go.AddOrGet<CopyBuildingSettings>();
		go.AddOrGet<DropToUserCapacity>();
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MINOR, 0f, 0f);
	}

	// Token: 0x0400091C RID: 2332
	public const string ID = "SmallOxidizerTank";

	// Token: 0x0400091D RID: 2333
	public const float FuelCapacity = 450f;
}
