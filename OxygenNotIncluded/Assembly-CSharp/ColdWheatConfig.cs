using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000141 RID: 321
public class ColdWheatConfig : IEntityConfig
{
	// Token: 0x0600063B RID: 1595 RVA: 0x00028BB2 File Offset: 0x00026DB2
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x00028BBC File Offset: 0x00026DBC
	public GameObject CreatePrefab()
	{
		string id = "ColdWheat";
		string name = STRINGS.CREATURES.SPECIES.COLDWHEAT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.COLDWHEAT.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("coldwheat_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 1, tier, default(EffectorValues), SimHashes.Creature, null, 255f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 118.149994f, 218.15f, 278.15f, 358.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, "ColdWheatSeed", true, true, true, true, 2400f, 0f, 12200f, "ColdWheatOriginal", STRINGS.CREATURES.SPECIES.COLDWHEAT.NAME);
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.Dirt,
				massConsumptionRate = 0.008333334f
			}
		});
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.Water,
				massConsumptionRate = 0.033333335f
			}
		});
		gameObject.AddOrGet<StandardCropPlant>();
		GameObject gameObject2 = EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.DigOnly, "ColdWheatSeed", STRINGS.CREATURES.SPECIES.SEEDS.COLDWHEAT.NAME, STRINGS.CREATURES.SPECIES.SEEDS.COLDWHEAT.DESC, Assets.GetAnim("seed_coldwheat_kanim"), "object", 1, new List<Tag>
		{
			GameTags.CropSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 3, STRINGS.CREATURES.SPECIES.COLDWHEAT.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.2f, 0.2f, null, "", true);
		EntityTemplates.ExtendEntityToFood(gameObject2, FOOD.FOOD_TYPES.COLD_WHEAT_SEED);
		EntityTemplates.CreateAndRegisterPreviewForPlant(gameObject2, "ColdWheat_preview", Assets.GetAnim("coldwheat_kanim"), "place", 1, 1);
		SoundEventVolumeCache.instance.AddVolume("coldwheat_kanim", "ColdWheat_grow", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("coldwheat_kanim", "ColdWheat_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		return gameObject;
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x00028DBC File Offset: 0x00026FBC
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x00028DBE File Offset: 0x00026FBE
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000450 RID: 1104
	public const string ID = "ColdWheat";

	// Token: 0x04000451 RID: 1105
	public const string SEED_ID = "ColdWheatSeed";

	// Token: 0x04000452 RID: 1106
	public const float FERTILIZATION_RATE = 0.008333334f;

	// Token: 0x04000453 RID: 1107
	public const float WATER_RATE = 0.033333335f;
}
