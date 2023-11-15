using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200013C RID: 316
public class BeanPlantConfig : IEntityConfig
{
	// Token: 0x06000621 RID: 1569 RVA: 0x0002822D File Offset: 0x0002642D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x00028234 File Offset: 0x00026434
	public GameObject CreatePrefab()
	{
		string id = "BeanPlant";
		string name = STRINGS.CREATURES.SPECIES.BEAN_PLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.BEAN_PLANT.DESC;
		float mass = 2f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("beanplant_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 258.15f);
		GameObject template = gameObject;
		float temperature_lethal_low = 198.15f;
		float temperature_warning_low = 248.15f;
		float temperature_warning_high = 273.15f;
		float temperature_lethal_high = 323.15f;
		string baseTraitName = STRINGS.CREATURES.SPECIES.BEAN_PLANT.NAME;
		EntityTemplates.ExtendEntityToBasicPlant(template, temperature_lethal_low, temperature_warning_low, temperature_warning_high, temperature_lethal_high, new SimHashes[]
		{
			SimHashes.CarbonDioxide
		}, true, 0f, 0.025f, "BeanPlantSeed", true, true, true, true, 2400f, 0f, 9800f, "BeanPlantOriginal", baseTraitName);
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Ethanol.CreateTag(),
				massConsumptionRate = 0.033333335f
			}
		});
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Dirt.CreateTag(),
				massConsumptionRate = 0.008333334f
			}
		});
		gameObject.AddOrGet<StandardCropPlant>();
		GameObject gameObject2 = EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.DigOnly, "BeanPlantSeed", STRINGS.CREATURES.SPECIES.SEEDS.BEAN_PLANT.NAME, STRINGS.CREATURES.SPECIES.SEEDS.BEAN_PLANT.DESC, Assets.GetAnim("seed_beanplant_kanim"), "object", 1, new List<Tag>
		{
			GameTags.CropSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 3, STRINGS.CREATURES.SPECIES.BEAN_PLANT.DOMESTICATEDDESC, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.3f, null, "", true);
		EntityTemplates.ExtendEntityToFood(gameObject2, FOOD.FOOD_TYPES.BEAN);
		EntityTemplates.CreateAndRegisterPreviewForPlant(gameObject2, "BeanPlant_preview", Assets.GetAnim("beanplant_kanim"), "place", 1, 2);
		return gameObject;
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x0002840D File Offset: 0x0002660D
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x0002840F File Offset: 0x0002660F
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400043A RID: 1082
	public const string ID = "BeanPlant";

	// Token: 0x0400043B RID: 1083
	public const string SEED_ID = "BeanPlantSeed";

	// Token: 0x0400043C RID: 1084
	public const float FERTILIZATION_RATE = 0.008333334f;

	// Token: 0x0400043D RID: 1085
	public const float WATER_RATE = 0.033333335f;
}
