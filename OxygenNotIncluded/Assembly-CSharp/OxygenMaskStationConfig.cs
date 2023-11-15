using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020002C2 RID: 706
public class OxygenMaskStationConfig : IBuildingConfig
{
	// Token: 0x06000E5F RID: 3679 RVA: 0x0004F49C File Offset: 0x0004D69C
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000E60 RID: 3680 RVA: 0x0004F4A4 File Offset: 0x0004D6A4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "OxygenMaskStation";
		int width = 2;
		int height = 3;
		string anim = "oxygen_mask_station_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] construction_materials = raw_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.PreventIdleTraversalPastBuilding = true;
		buildingDef.Deprecated = true;
		return buildingDef;
	}

	// Token: 0x06000E61 RID: 3681 RVA: 0x0004F52C File Offset: 0x0004D72C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddComponent<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.showInUI = true;
		storage.storageFilters = new List<Tag>
		{
			GameTags.Metal
		};
		storage.capacityKg = 45f;
		Storage storage2 = go.AddComponent<Storage>();
		storage2.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage2.showInUI = true;
		storage2.storageFilters = new List<Tag>
		{
			GameTags.Breathable
		};
		MaskStation maskStation = go.AddOrGet<MaskStation>();
		maskStation.materialConsumedPerMask = 15f;
		maskStation.oxygenConsumedPerMask = 20f;
		maskStation.maxUses = 3;
		maskStation.materialTag = GameTags.Metal;
		maskStation.oxygenTag = GameTags.Breathable;
		maskStation.choreTypeID = this.fetchChoreType.Id;
		maskStation.PathFlag = PathFinder.PotentialPath.Flags.HasOxygenMask;
		maskStation.materialStorage = storage;
		maskStation.oxygenStorage = storage2;
		ElementConsumer elementConsumer = go.AddOrGet<ElementConsumer>();
		elementConsumer.elementToConsume = SimHashes.Oxygen;
		elementConsumer.configuration = ElementConsumer.Configuration.AllGas;
		elementConsumer.consumptionRate = 0.5f;
		elementConsumer.storeOnConsume = true;
		elementConsumer.showInStatusPanel = false;
		elementConsumer.consumptionRadius = 2;
		elementConsumer.storage = storage2;
		ElementConsumer elementConsumer2 = go.AddComponent<ElementConsumer>();
		elementConsumer2.elementToConsume = SimHashes.ContaminatedOxygen;
		elementConsumer2.configuration = ElementConsumer.Configuration.AllGas;
		elementConsumer2.consumptionRate = 0.5f;
		elementConsumer2.storeOnConsume = true;
		elementConsumer2.showInStatusPanel = false;
		elementConsumer2.consumptionRadius = 2;
		elementConsumer2.storage = storage2;
		Prioritizable.AddRef(go);
		go.AddOrGet<LoopingSounds>();
	}

	// Token: 0x06000E62 RID: 3682 RVA: 0x0004F689 File Offset: 0x0004D889
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400083F RID: 2111
	public const string ID = "OxygenMaskStation";

	// Token: 0x04000840 RID: 2112
	public const float MATERIAL_PER_MASK = 15f;

	// Token: 0x04000841 RID: 2113
	public const float OXYGEN_PER_MASK = 20f;

	// Token: 0x04000842 RID: 2114
	public const int MASKS_PER_REFILL = 3;

	// Token: 0x04000843 RID: 2115
	public const float WORK_TIME = 5f;

	// Token: 0x04000844 RID: 2116
	public ChoreType fetchChoreType = Db.Get().ChoreTypes.Fetch;
}
