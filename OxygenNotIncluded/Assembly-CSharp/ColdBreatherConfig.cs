using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000140 RID: 320
public class ColdBreatherConfig : IEntityConfig
{
	// Token: 0x06000635 RID: 1589 RVA: 0x000288BF File Offset: 0x00026ABF
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x000288C8 File Offset: 0x00026AC8
	public GameObject CreatePrefab()
	{
		string id = "ColdBreather";
		string name = STRINGS.CREATURES.SPECIES.COLDBREATHER.NAME;
		string desc = STRINGS.CREATURES.SPECIES.COLDBREATHER.DESC;
		float mass = 400f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("coldbreather_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 2, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.AddOrGet<ReceptacleMonitor>();
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddOrGet<WiltCondition>();
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddOrGet<Uprootable>();
		gameObject.AddOrGet<UprootedMonitor>();
		gameObject.AddOrGet<DrowningMonitor>();
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Phosphorite.CreateTag(),
				massConsumptionRate = 0.006666667f
			}
		});
		gameObject.AddOrGet<TemperatureVulnerable>().Configure(213.15f, 183.15f, 368.15f, 463.15f);
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		ColdBreather coldBreather = gameObject.AddOrGet<ColdBreather>();
		coldBreather.deltaEmitTemperature = -5f;
		coldBreather.emitOffsetCell = new Vector3(0f, 1f);
		coldBreather.consumptionRate = 1f;
		gameObject.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
		BuildingTemplates.CreateDefaultStorage(gameObject, false).showInUI = false;
		ElementConsumer elementConsumer = gameObject.AddOrGet<ElementConsumer>();
		elementConsumer.storeOnConsume = true;
		elementConsumer.configuration = ElementConsumer.Configuration.AllGas;
		elementConsumer.capacityKG = 2f;
		elementConsumer.consumptionRate = 0.25f;
		elementConsumer.consumptionRadius = 1;
		elementConsumer.sampleCellOffset = new Vector3(0f, 0f);
		SimTemperatureTransfer component = gameObject.GetComponent<SimTemperatureTransfer>();
		component.SurfaceArea = 10f;
		component.Thickness = 0.001f;
		if (DlcManager.FeatureRadiationEnabled())
		{
			RadiationEmitter radiationEmitter = gameObject.AddComponent<RadiationEmitter>();
			radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
			radiationEmitter.radiusProportionalToRads = false;
			radiationEmitter.emitRadiusX = 6;
			radiationEmitter.emitRadiusY = radiationEmitter.emitRadiusX;
			radiationEmitter.emitRads = 480f;
			radiationEmitter.emissionOffset = new Vector3(0f, 0f, 0f);
		}
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Hidden, "ColdBreatherSeed", STRINGS.CREATURES.SPECIES.SEEDS.COLDBREATHER.NAME, STRINGS.CREATURES.SPECIES.SEEDS.COLDBREATHER.DESC, Assets.GetAnim("seed_coldbreather_kanim"), "object", 1, new List<Tag>
		{
			GameTags.CropSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 21, STRINGS.CREATURES.SPECIES.COLDBREATHER.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false), "ColdBreather_preview", Assets.GetAnim("coldbreather_kanim"), "place", 1, 2);
		SoundEventVolumeCache.instance.AddVolume("coldbreather_kanim", "ColdBreather_grow", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("coldbreather_kanim", "ColdBreather_intake", NOISE_POLLUTION.CREATURES.TIER3);
		return gameObject;
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x00028B86 File Offset: 0x00026D86
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000638 RID: 1592 RVA: 0x00028B88 File Offset: 0x00026D88
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000447 RID: 1095
	public const string ID = "ColdBreather";

	// Token: 0x04000448 RID: 1096
	public static readonly Tag TAG = TagManager.Create("ColdBreather");

	// Token: 0x04000449 RID: 1097
	public const float FERTILIZATION_RATE = 0.006666667f;

	// Token: 0x0400044A RID: 1098
	public const SimHashes FERTILIZER = SimHashes.Phosphorite;

	// Token: 0x0400044B RID: 1099
	public const float TEMP_DELTA = -5f;

	// Token: 0x0400044C RID: 1100
	public const float CONSUMPTION_RATE = 1f;

	// Token: 0x0400044D RID: 1101
	public const float RADIATION_STRENGTH = 480f;

	// Token: 0x0400044E RID: 1102
	public const string SEED_ID = "ColdBreatherSeed";

	// Token: 0x0400044F RID: 1103
	public static readonly Tag SEED_TAG = TagManager.Create("ColdBreatherSeed");
}
