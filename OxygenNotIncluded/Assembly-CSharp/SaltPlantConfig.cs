using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000156 RID: 342
public class SaltPlantConfig : IEntityConfig
{
	// Token: 0x060006A8 RID: 1704 RVA: 0x0002B932 File Offset: 0x00029B32
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x0002B93C File Offset: 0x00029B3C
	public GameObject CreatePrefab()
	{
		string id = "SaltPlant";
		string name = STRINGS.CREATURES.SPECIES.SALTPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SALTPLANT.DESC;
		float mass = 2f;
		EffectorValues tier = DECOR.PENALTY.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("saltplant_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 2, tier, default(EffectorValues), SimHashes.Creature, new List<Tag>
		{
			GameTags.Hanging
		}, 258.15f);
		EntityTemplates.MakeHangingOffsets(gameObject, 1, 2);
		GameObject template = gameObject;
		float temperature_lethal_low = 198.15f;
		float temperature_warning_low = 248.15f;
		float temperature_warning_high = 323.15f;
		float temperature_lethal_high = 393.15f;
		string crop_id = SimHashes.Salt.ToString();
		string baseTraitName = STRINGS.CREATURES.SPECIES.SALTPLANT.NAME;
		EntityTemplates.ExtendEntityToBasicPlant(template, temperature_lethal_low, temperature_warning_low, temperature_warning_high, temperature_lethal_high, new SimHashes[]
		{
			SimHashes.ChlorineGas
		}, true, 0f, 0.025f, crop_id, true, true, true, true, 2400f, 0f, 7400f, "SaltPlantOriginal", baseTraitName);
		gameObject.AddOrGet<SaltPlant>();
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Sand.CreateTag(),
				massConsumptionRate = 0.011666667f
			}
		});
		Storage storage = gameObject.AddOrGet<Storage>();
		storage.showInUI = false;
		storage.capacityKg = 1f;
		ElementConsumer elementConsumer = gameObject.AddOrGet<ElementConsumer>();
		elementConsumer.showInStatusPanel = true;
		elementConsumer.showDescriptor = true;
		elementConsumer.storeOnConsume = false;
		elementConsumer.elementToConsume = SimHashes.ChlorineGas;
		elementConsumer.configuration = ElementConsumer.Configuration.Element;
		elementConsumer.consumptionRadius = 4;
		elementConsumer.sampleCellOffset = new Vector3(0f, -1f);
		elementConsumer.consumptionRate = 0.006f;
		gameObject.GetComponent<UprootedMonitor>().monitorCells = new CellOffset[]
		{
			new CellOffset(0, 1)
		};
		gameObject.AddOrGet<StandardCropPlant>();
		EntityTemplates.MakeHangingOffsets(EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Harvest, "SaltPlantSeed", STRINGS.CREATURES.SPECIES.SEEDS.SALTPLANT.NAME, STRINGS.CREATURES.SPECIES.SEEDS.SALTPLANT.DESC, Assets.GetAnim("seed_saltplant_kanim"), "object", 1, new List<Tag>
		{
			GameTags.CropSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Bottom, default(Tag), 5, STRINGS.CREATURES.SPECIES.SALTPLANT.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, null, "", false), "SaltPlant_preview", Assets.GetAnim("saltplant_kanim"), "place", 1, 2), 1, 2);
		return gameObject;
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x0002BB90 File Offset: 0x00029D90
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x0002BB92 File Offset: 0x00029D92
	public void OnSpawn(GameObject inst)
	{
		inst.GetComponent<ElementConsumer>().EnableConsumption(true);
	}

	// Token: 0x040004A8 RID: 1192
	public const string ID = "SaltPlant";

	// Token: 0x040004A9 RID: 1193
	public const string SEED_ID = "SaltPlantSeed";

	// Token: 0x040004AA RID: 1194
	public const float FERTILIZATION_RATE = 0.011666667f;

	// Token: 0x040004AB RID: 1195
	public const float CHLORINE_CONSUMPTION_RATE = 0.006f;
}
