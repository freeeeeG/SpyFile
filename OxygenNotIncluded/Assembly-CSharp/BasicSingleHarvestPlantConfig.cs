﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200013B RID: 315
public class BasicSingleHarvestPlantConfig : IEntityConfig
{
	// Token: 0x0600061C RID: 1564 RVA: 0x0002804F File Offset: 0x0002624F
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x00028058 File Offset: 0x00026258
	public GameObject CreatePrefab()
	{
		string id = "BasicSingleHarvestPlant";
		string name = STRINGS.CREATURES.SPECIES.BASICSINGLEHARVESTPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.BASICSINGLEHARVESTPLANT.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.PENALTY.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("meallice_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 218.15f, 283.15f, 303.15f, 398.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, "BasicPlantFood", true, false, true, true, 2400f, 0f, 4600f, "BasicSingleHarvestPlantOriginal", STRINGS.CREATURES.SPECIES.BASICSINGLEHARVESTPLANT.NAME);
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<LoopingSounds>();
		GameObject seed = EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Harvest, "BasicSingleHarvestPlantSeed", STRINGS.CREATURES.SPECIES.SEEDS.BASICSINGLEHARVESTPLANT.NAME, STRINGS.CREATURES.SPECIES.SEEDS.BASICSINGLEHARVESTPLANT.DESC, Assets.GetAnim("seed_meallice_kanim"), "object", 0, new List<Tag>
		{
			GameTags.CropSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 1, STRINGS.CREATURES.SPECIES.BASICSINGLEHARVESTPLANT.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false);
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.Dirt,
				massConsumptionRate = 0.016666668f
			}
		});
		EntityTemplates.CreateAndRegisterPreviewForPlant(seed, "BasicSingleHarvestPlant_preview", Assets.GetAnim("meallice_kanim"), "place", 1, 2);
		SoundEventVolumeCache.instance.AddVolume("meallice_kanim", "MealLice_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("meallice_kanim", "MealLice_LP", NOISE_POLLUTION.CREATURES.TIER4);
		return gameObject;
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x00028221 File Offset: 0x00026421
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x00028223 File Offset: 0x00026423
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000437 RID: 1079
	public const string ID = "BasicSingleHarvestPlant";

	// Token: 0x04000438 RID: 1080
	public const string SEED_ID = "BasicSingleHarvestPlantSeed";

	// Token: 0x04000439 RID: 1081
	public const float DIRT_RATE = 0.016666668f;
}
