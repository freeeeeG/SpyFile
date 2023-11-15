using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000150 RID: 336
public class MushroomPlantConfig : IEntityConfig
{
	// Token: 0x06000689 RID: 1673 RVA: 0x0002AE43 File Offset: 0x00029043
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600068A RID: 1674 RVA: 0x0002AE4C File Offset: 0x0002904C
	public GameObject CreatePrefab()
	{
		string id = "MushroomPlant";
		string name = STRINGS.CREATURES.SPECIES.MUSHROOMPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.MUSHROOMPLANT.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("fungusplant_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 228.15f, 278.15f, 308.15f, 398.15f, new SimHashes[]
		{
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, MushroomConfig.ID, true, true, true, true, 2400f, 0f, 4600f, "MushroomPlantOriginal", STRINGS.CREATURES.SPECIES.MUSHROOMPLANT.NAME);
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.SlimeMold,
				massConsumptionRate = 0.006666667f
			}
		});
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<IlluminationVulnerable>().SetPrefersDarkness(true);
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Harvest, "MushroomSeed", STRINGS.CREATURES.SPECIES.SEEDS.MUSHROOMPLANT.NAME, STRINGS.CREATURES.SPECIES.SEEDS.MUSHROOMPLANT.DESC, Assets.GetAnim("seed_fungusplant_kanim"), "object", 0, new List<Tag>
		{
			GameTags.CropSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 3, STRINGS.CREATURES.SPECIES.MUSHROOMPLANT.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.33f, 0.33f, null, "", false), "MushroomPlant_preview", Assets.GetAnim("fungusplant_kanim"), "place", 1, 2);
		SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		return gameObject;
	}

	// Token: 0x0600068B RID: 1675 RVA: 0x0002B014 File Offset: 0x00029214
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600068C RID: 1676 RVA: 0x0002B016 File Offset: 0x00029216
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000495 RID: 1173
	public const float FERTILIZATION_RATE = 0.006666667f;

	// Token: 0x04000496 RID: 1174
	public const string ID = "MushroomPlant";

	// Token: 0x04000497 RID: 1175
	public const string SEED_ID = "MushroomSeed";
}
