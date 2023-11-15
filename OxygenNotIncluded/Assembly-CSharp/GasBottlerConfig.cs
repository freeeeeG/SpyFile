using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020001A3 RID: 419
public class GasBottlerConfig : IBuildingConfig
{
	// Token: 0x06000847 RID: 2119 RVA: 0x00030640 File Offset: 0x0002E840
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("GasBottler", 3, 2, "gas_bottler_kanim", 100, 120f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.ALL_METALS, 800f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER1, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "GasBottler");
		return buildingDef;
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x000306C4 File Offset: 0x0002E8C4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showDescriptor = true;
		storage.storageFilters = STORAGEFILTERS.GASES;
		storage.capacityKg = 25f;
		storage.allowItemRemoval = false;
		go.AddOrGet<DropAllWorkable>().removeTags = new List<Tag>
		{
			GameTags.GasSource
		};
		GasBottler gasBottler = go.AddOrGet<GasBottler>();
		gasBottler.storage = storage;
		gasBottler.workTime = 9f;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.storage = storage;
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.ignoreMinMassCheck = true;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.alwaysConsume = true;
		conduitConsumer.capacityKG = storage.capacityKg;
		conduitConsumer.keepZeroMassObject = false;
	}

	// Token: 0x06000849 RID: 2121 RVA: 0x0003076A File Offset: 0x0002E96A
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x0400054C RID: 1356
	public const string ID = "GasBottler";

	// Token: 0x0400054D RID: 1357
	private const ConduitType CONDUIT_TYPE = ConduitType.Gas;

	// Token: 0x0400054E RID: 1358
	private const int WIDTH = 3;

	// Token: 0x0400054F RID: 1359
	private const int HEIGHT = 2;
}
