using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200015D RID: 349
public class SwampHarvestPlantConfig : IEntityConfig
{
	// Token: 0x060006CE RID: 1742 RVA: 0x0002C410 File Offset: 0x0002A610
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x0002C418 File Offset: 0x0002A618
	public GameObject CreatePrefab()
	{
		string id = "SwampHarvestPlant";
		string name = STRINGS.CREATURES.SPECIES.SWAMPHARVESTPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SWAMPHARVESTPLANT.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.PENALTY.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("swampcrop_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		GameObject template = gameObject;
		float temperature_lethal_low = 218.15f;
		float temperature_warning_low = 283.15f;
		float temperature_warning_high = 303.15f;
		float temperature_lethal_high = 398.15f;
		string id2 = SwampFruitConfig.ID;
		EntityTemplates.ExtendEntityToBasicPlant(template, temperature_lethal_low, temperature_warning_low, temperature_warning_high, temperature_lethal_high, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, id2, true, true, true, true, 2400f, 0f, 4600f, "SwampHarvestPlantOriginal", gameObject.PrefabID().Name);
		gameObject.AddOrGet<IlluminationVulnerable>().SetPrefersDarkness(true);
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.DirtyWater,
				massConsumptionRate = 0.06666667f
			}
		});
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<LoopingSounds>();
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Harvest, "SwampHarvestPlantSeed", STRINGS.CREATURES.SPECIES.SEEDS.SWAMPHARVESTPLANT.NAME, STRINGS.CREATURES.SPECIES.SEEDS.SWAMPHARVESTPLANT.DESC, Assets.GetAnim("seed_swampcrop_kanim"), "object", 0, new List<Tag>
		{
			GameTags.CropSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 2, STRINGS.CREATURES.SPECIES.SWAMPHARVESTPLANT.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false), "SwampHarvestPlant_preview", Assets.GetAnim("swampcrop_kanim"), "place", 1, 2);
		return gameObject;
	}

	// Token: 0x060006D0 RID: 1744 RVA: 0x0002C5C0 File Offset: 0x0002A7C0
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060006D1 RID: 1745 RVA: 0x0002C5C2 File Offset: 0x0002A7C2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004C2 RID: 1218
	public const string ID = "SwampHarvestPlant";

	// Token: 0x040004C3 RID: 1219
	public const string SEED_ID = "SwampHarvestPlantSeed";

	// Token: 0x040004C4 RID: 1220
	public const float WATER_RATE = 0.06666667f;
}
