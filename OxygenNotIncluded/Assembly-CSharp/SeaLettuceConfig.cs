using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000158 RID: 344
public class SeaLettuceConfig : IEntityConfig
{
	// Token: 0x060006B3 RID: 1715 RVA: 0x0002BD4A File Offset: 0x00029F4A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x0002BD54 File Offset: 0x00029F54
	public GameObject CreatePrefab()
	{
		string id = SeaLettuceConfig.ID;
		string name = STRINGS.CREATURES.SPECIES.SEALETTUCE.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SEALETTUCE.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.BONUS.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("sea_lettuce_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 308.15f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 248.15f, 295.15f, 338.15f, 398.15f, new SimHashes[]
		{
			SimHashes.Water,
			SimHashes.SaltWater,
			SimHashes.Brine
		}, false, 0f, 0.15f, "Lettuce", true, true, true, true, 2400f, 0f, 7400f, SeaLettuceConfig.ID + "Original", STRINGS.CREATURES.SPECIES.SEALETTUCE.NAME);
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.SaltWater.CreateTag(),
				massConsumptionRate = 0.008333334f
			}
		});
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.BleachStone.CreateTag(),
				massConsumptionRate = 0.00083333335f
			}
		});
		gameObject.GetComponent<DrowningMonitor>().canDrownToDeath = false;
		gameObject.GetComponent<DrowningMonitor>().livesUnderWater = true;
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<LoopingSounds>();
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Harvest, "SeaLettuceSeed", STRINGS.CREATURES.SPECIES.SEEDS.SEALETTUCE.NAME, STRINGS.CREATURES.SPECIES.SEEDS.SEALETTUCE.DESC, Assets.GetAnim("seed_sealettuce_kanim"), "object", 0, new List<Tag>
		{
			GameTags.WaterSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 3, STRINGS.CREATURES.SPECIES.SEALETTUCE.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false), SeaLettuceConfig.ID + "_preview", Assets.GetAnim("sea_lettuce_kanim"), "place", 1, 2);
		SoundEventVolumeCache.instance.AddVolume("sea_lettuce_kanim", "SeaLettuce_grow", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("sea_lettuce_kanim", "SeaLettuce_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		return gameObject;
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x0002BF85 File Offset: 0x0002A185
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060006B6 RID: 1718 RVA: 0x0002BF87 File Offset: 0x0002A187
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004B3 RID: 1203
	public static string ID = "SeaLettuce";

	// Token: 0x040004B4 RID: 1204
	public const string SEED_ID = "SeaLettuceSeed";

	// Token: 0x040004B5 RID: 1205
	public const float WATER_RATE = 0.008333334f;

	// Token: 0x040004B6 RID: 1206
	public const float FERTILIZATION_RATE = 0.00083333335f;
}
