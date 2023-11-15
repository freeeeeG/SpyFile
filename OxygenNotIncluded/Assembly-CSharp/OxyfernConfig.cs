﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000153 RID: 339
public class OxyfernConfig : IEntityConfig
{
	// Token: 0x06000698 RID: 1688 RVA: 0x0002B1E0 File Offset: 0x000293E0
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x0002B1E8 File Offset: 0x000293E8
	public GameObject CreatePrefab()
	{
		string id = "Oxyfern";
		string name = STRINGS.CREATURES.SPECIES.OXYFERN.NAME;
		string desc = STRINGS.CREATURES.SPECIES.OXYFERN.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.PENALTY.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("oxy_fern_kanim"), "idle_full", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		gameObject = EntityTemplates.ExtendEntityToBasicPlant(gameObject, 253.15f, 273.15f, 313.15f, 373.15f, new SimHashes[]
		{
			SimHashes.CarbonDioxide
		}, true, 0f, 0.025f, null, true, false, true, true, 2400f, 0f, 2200f, "OxyfernOriginal", STRINGS.CREATURES.SPECIES.OXYFERN.NAME);
		Tag tag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = tag,
				massConsumptionRate = 0.031666666f
			}
		});
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.Dirt,
				massConsumptionRate = 0.006666667f
			}
		});
		gameObject.AddOrGet<Oxyfern>();
		gameObject.AddOrGet<LoopingSounds>();
		Storage storage = gameObject.AddOrGet<Storage>();
		storage.showInUI = false;
		storage.capacityKg = 1f;
		ElementConsumer elementConsumer = gameObject.AddOrGet<ElementConsumer>();
		elementConsumer.showInStatusPanel = false;
		elementConsumer.storeOnConsume = true;
		elementConsumer.storage = storage;
		elementConsumer.elementToConsume = SimHashes.CarbonDioxide;
		elementConsumer.configuration = ElementConsumer.Configuration.Element;
		elementConsumer.consumptionRadius = 2;
		elementConsumer.EnableConsumption(true);
		elementConsumer.sampleCellOffset = new Vector3(0f, 0f);
		elementConsumer.consumptionRate = 0.00015625001f;
		ElementConverter elementConverter = gameObject.AddOrGet<ElementConverter>();
		elementConverter.OutputMultiplier = 50f;
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(SimHashes.CarbonDioxide.ToString().ToTag(), 0.00062500004f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.031250004f, SimHashes.Oxygen, 0f, true, false, 0f, 1f, 0.75f, byte.MaxValue, 0, true)
		};
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Hidden, "OxyfernSeed", STRINGS.CREATURES.SPECIES.SEEDS.OXYFERN.NAME, STRINGS.CREATURES.SPECIES.SEEDS.OXYFERN.DESC, Assets.GetAnim("seed_oxyfern_kanim"), "object", 1, new List<Tag>
		{
			GameTags.CropSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 20, STRINGS.CREATURES.SPECIES.OXYFERN.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false), "Oxyfern_preview", Assets.GetAnim("oxy_fern_kanim"), "place", 1, 2);
		SoundEventVolumeCache.instance.AddVolume("oxy_fern_kanim", "MealLice_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("oxy_fern_kanim", "MealLice_LP", NOISE_POLLUTION.CREATURES.TIER4);
		return gameObject;
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x0002B4E9 File Offset: 0x000296E9
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x0002B4EB File Offset: 0x000296EB
	public void OnSpawn(GameObject inst)
	{
		inst.GetComponent<Oxyfern>().SetConsumptionRate();
	}

	// Token: 0x0400049A RID: 1178
	public const string ID = "Oxyfern";

	// Token: 0x0400049B RID: 1179
	public const string SEED_ID = "OxyfernSeed";

	// Token: 0x0400049C RID: 1180
	public const float WATER_CONSUMPTION_RATE = 0.031666666f;

	// Token: 0x0400049D RID: 1181
	public const float FERTILIZATION_RATE = 0.006666667f;

	// Token: 0x0400049E RID: 1182
	public const float CO2_RATE = 0.00062500004f;

	// Token: 0x0400049F RID: 1183
	private const float CONVERSION_RATIO = 50f;

	// Token: 0x040004A0 RID: 1184
	public const float OXYGEN_RATE = 0.031250004f;
}
