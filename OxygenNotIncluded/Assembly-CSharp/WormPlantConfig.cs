using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000162 RID: 354
public class WormPlantConfig : IEntityConfig
{
	// Token: 0x060006EA RID: 1770 RVA: 0x0002CD76 File Offset: 0x0002AF76
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x0002CD80 File Offset: 0x0002AF80
	public static GameObject BaseWormPlant(string id, string name, string desc, string animFile, EffectorValues decor, string cropID)
	{
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, 1f, Assets.GetAnim(animFile), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, decor, default(EffectorValues), SimHashes.Creature, null, 307.15f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 273.15f, 288.15f, 323.15f, 373.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, cropID, true, true, true, true, 2400f, 0f, 9800f, id + "Original", name);
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Sulfur.CreateTag(),
				massConsumptionRate = 0.016666668f
			}
		});
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<LoopingSounds>();
		return gameObject;
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x0002CE6C File Offset: 0x0002B06C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = WormPlantConfig.BaseWormPlant("WormPlant", STRINGS.CREATURES.SPECIES.WORMPLANT.NAME, STRINGS.CREATURES.SPECIES.WORMPLANT.DESC, "wormwood_kanim", WormPlantConfig.BASIC_DECOR, "WormBasicFruit");
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Harvest, "WormPlantSeed", STRINGS.CREATURES.SPECIES.SEEDS.WORMPLANT.NAME, STRINGS.CREATURES.SPECIES.SEEDS.WORMPLANT.DESC, Assets.GetAnim("seed_wormwood_kanim"), "object", 0, new List<Tag>
		{
			GameTags.CropSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 3, STRINGS.CREATURES.SPECIES.WORMPLANT.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false), "WormPlant_preview", Assets.GetAnim("wormwood_kanim"), "place", 1, 2);
		return gameObject;
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x0002CF34 File Offset: 0x0002B134
	public void OnPrefabInit(GameObject prefab)
	{
		TransformingPlant transformingPlant = prefab.AddOrGet<TransformingPlant>();
		transformingPlant.transformPlantId = "SuperWormPlant";
		transformingPlant.SubscribeToTransformEvent(GameHashes.CropTended);
		transformingPlant.useGrowthTimeRatio = true;
		transformingPlant.eventDataCondition = delegate(object data)
		{
			CropTendingStates.CropTendingEventData cropTendingEventData = (CropTendingStates.CropTendingEventData)data;
			if (cropTendingEventData != null)
			{
				CreatureBrain component = cropTendingEventData.source.GetComponent<CreatureBrain>();
				if (component != null && component.species == GameTags.Creatures.Species.DivergentSpecies)
				{
					return true;
				}
			}
			return false;
		};
		transformingPlant.fxKAnim = "plant_transform_fx_kanim";
		transformingPlant.fxAnim = "plant_transform";
		prefab.AddOrGet<StandardCropPlant>().anims = WormPlantConfig.animSet;
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x0002CFAE File Offset: 0x0002B1AE
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004D6 RID: 1238
	public const string ID = "WormPlant";

	// Token: 0x040004D7 RID: 1239
	public const string SEED_ID = "WormPlantSeed";

	// Token: 0x040004D8 RID: 1240
	public const float SULFUR_CONSUMPTION_RATE = 0.016666668f;

	// Token: 0x040004D9 RID: 1241
	public static readonly EffectorValues BASIC_DECOR = DECOR.PENALTY.TIER0;

	// Token: 0x040004DA RID: 1242
	public const string BASIC_CROP_ID = "WormBasicFruit";

	// Token: 0x040004DB RID: 1243
	private static StandardCropPlant.AnimSet animSet = new StandardCropPlant.AnimSet
	{
		grow = "basic_grow",
		grow_pst = "basic_grow_pst",
		idle_full = "basic_idle_full",
		wilt_base = "basic_wilt",
		harvest = "basic_harvest"
	};
}
