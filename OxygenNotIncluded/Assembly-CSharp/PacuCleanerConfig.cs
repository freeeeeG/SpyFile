using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000113 RID: 275
[EntityConfigOrder(1)]
public class PacuCleanerConfig : IEntityConfig
{
	// Token: 0x06000541 RID: 1345 RVA: 0x00024234 File Offset: 0x00022434
	public static GameObject CreatePacu(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BasePacuConfig.CreatePrefab(id, "PacuCleanerBaseTrait", name, desc, anim_file, is_baby, "glp_", 243.15f, 278.15f);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, PacuTuning.PEN_SIZE_PER_CREATURE);
		if (!is_baby)
		{
			Storage storage = gameObject.AddComponent<Storage>();
			storage.capacityKg = 10f;
			storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
			PassiveElementConsumer passiveElementConsumer = gameObject.AddOrGet<PassiveElementConsumer>();
			passiveElementConsumer.elementToConsume = SimHashes.DirtyWater;
			passiveElementConsumer.consumptionRate = 0.2f;
			passiveElementConsumer.capacityKG = 10f;
			passiveElementConsumer.consumptionRadius = 3;
			passiveElementConsumer.showInStatusPanel = true;
			passiveElementConsumer.sampleCellOffset = new Vector3(0f, 0f, 0f);
			passiveElementConsumer.isRequired = false;
			passiveElementConsumer.storeOnConsume = true;
			passiveElementConsumer.showDescriptor = false;
			gameObject.AddOrGet<UpdateElementConsumerPosition>();
			BubbleSpawner bubbleSpawner = gameObject.AddComponent<BubbleSpawner>();
			bubbleSpawner.element = SimHashes.Water;
			bubbleSpawner.emitMass = 2f;
			bubbleSpawner.emitVariance = 0.5f;
			bubbleSpawner.initialVelocity = new Vector2f(0, 1);
			ElementConverter elementConverter = gameObject.AddOrGet<ElementConverter>();
			elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
			{
				new ElementConverter.ConsumedElement(SimHashes.DirtyWater.CreateTag(), 0.2f, true)
			};
			elementConverter.outputElements = new ElementConverter.OutputElement[]
			{
				new ElementConverter.OutputElement(0.2f, SimHashes.Water, 0f, true, true, 0f, 0.5f, 1f, byte.MaxValue, 0, true)
			};
		}
		return gameObject;
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x000243A0 File Offset: 0x000225A0
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x000243A8 File Offset: 0x000225A8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(EntityTemplates.ExtendEntityToWildCreature(PacuCleanerConfig.CreatePacu("PacuCleaner", STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.NAME, STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.DESC, "pacu_kanim", false), PacuTuning.PEN_SIZE_PER_CREATURE), "PacuCleanerEgg", STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.EGG_NAME, STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.DESC, "egg_pacu_kanim", PacuTuning.EGG_MASS, "PacuCleanerBaby", 15.000001f, 5f, PacuTuning.EGG_CHANCES_CLEANER, 501, false, true, false, 0.75f, false);
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x0002442D File Offset: 0x0002262D
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x00024430 File Offset: 0x00022630
	public void OnSpawn(GameObject inst)
	{
		ElementConsumer component = inst.GetComponent<ElementConsumer>();
		if (component != null)
		{
			component.EnableConsumption(true);
		}
	}

	// Token: 0x04000399 RID: 921
	public const string ID = "PacuCleaner";

	// Token: 0x0400039A RID: 922
	public const string BASE_TRAIT_ID = "PacuCleanerBaseTrait";

	// Token: 0x0400039B RID: 923
	public const string EGG_ID = "PacuCleanerEgg";

	// Token: 0x0400039C RID: 924
	public const float POLLUTED_WATER_CONVERTED_PER_CYCLE = 120f;

	// Token: 0x0400039D RID: 925
	public const SimHashes INPUT_ELEMENT = SimHashes.DirtyWater;

	// Token: 0x0400039E RID: 926
	public const SimHashes OUTPUT_ELEMENT = SimHashes.Water;

	// Token: 0x0400039F RID: 927
	public static readonly EffectorValues DECOR = TUNING.BUILDINGS.DECOR.BONUS.TIER4;

	// Token: 0x040003A0 RID: 928
	public const int EGG_SORT_ORDER = 501;
}
