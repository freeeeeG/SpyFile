using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000138 RID: 312
public class BasicFabricMaterialPlantConfig : IEntityConfig
{
	// Token: 0x0600060C RID: 1548 RVA: 0x00027CE4 File Offset: 0x00025EE4
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x00027CEC File Offset: 0x00025EEC
	public GameObject CreatePrefab()
	{
		string id = BasicFabricMaterialPlantConfig.ID;
		string name = STRINGS.CREATURES.SPECIES.BASICFABRICMATERIALPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.BASICFABRICMATERIALPLANT.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.BONUS.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("swampreed_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 3, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		GameObject template = gameObject;
		float temperature_lethal_low = 248.15f;
		float temperature_warning_low = 295.15f;
		float temperature_warning_high = 310.15f;
		float temperature_lethal_high = 398.15f;
		string id2 = BasicFabricConfig.ID;
		EntityTemplates.ExtendEntityToBasicPlant(template, temperature_lethal_low, temperature_warning_low, temperature_warning_high, temperature_lethal_high, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide,
			SimHashes.DirtyWater,
			SimHashes.Water
		}, false, 0f, 0.15f, id2, false, true, true, true, 2400f, 0f, 4600f, BasicFabricMaterialPlantConfig.ID + "Original", STRINGS.CREATURES.SPECIES.BASICFABRICMATERIALPLANT.NAME);
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.DirtyWater,
				massConsumptionRate = 0.26666668f
			}
		});
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<LoopingSounds>();
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Harvest, BasicFabricMaterialPlantConfig.SEED_ID, STRINGS.CREATURES.SPECIES.SEEDS.BASICFABRICMATERIALPLANT.NAME, STRINGS.CREATURES.SPECIES.SEEDS.BASICFABRICMATERIALPLANT.DESC, Assets.GetAnim("seed_swampreed_kanim"), "object", 0, new List<Tag>
		{
			GameTags.WaterSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 20, STRINGS.CREATURES.SPECIES.BASICFABRICMATERIALPLANT.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false), BasicFabricMaterialPlantConfig.ID + "_preview", Assets.GetAnim("swampreed_kanim"), "place", 1, 3);
		SoundEventVolumeCache.instance.AddVolume("swampreed_kanim", "FabricPlant_grow", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("swampreed_kanim", "FabricPlant_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		return gameObject;
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x00027ECA File Offset: 0x000260CA
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x00027ECC File Offset: 0x000260CC
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000432 RID: 1074
	public static string ID = "BasicFabricPlant";

	// Token: 0x04000433 RID: 1075
	public static string SEED_ID = "BasicFabricMaterialPlantSeed";

	// Token: 0x04000434 RID: 1076
	public const float WATER_RATE = 0.26666668f;
}
