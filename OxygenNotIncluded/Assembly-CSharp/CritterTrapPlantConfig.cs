using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000142 RID: 322
public class CritterTrapPlantConfig : IEntityConfig
{
	// Token: 0x06000640 RID: 1600 RVA: 0x00028DC8 File Offset: 0x00026FC8
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x00028DD0 File Offset: 0x00026FD0
	public GameObject CreatePrefab()
	{
		string id = "CritterTrapPlant";
		string name = STRINGS.CREATURES.SPECIES.CRITTERTRAPPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.CRITTERTRAPPLANT.DESC;
		float mass = 4f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("venus_critter_trap_kanim"), "idle_open", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, TUNING.CREATURES.TEMPERATURE.FREEZING_3);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, TUNING.CREATURES.TEMPERATURE.FREEZING_10, TUNING.CREATURES.TEMPERATURE.FREEZING_9, TUNING.CREATURES.TEMPERATURE.FREEZING, TUNING.CREATURES.TEMPERATURE.COOL, null, false, 0f, 0.15f, "PlantMeat", true, true, true, false, 2400f, 0f, 2200f, "CritterTrapPlantOriginal", STRINGS.CREATURES.SPECIES.CRITTERTRAPPLANT.NAME);
		UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<MutantPlant>());
		TrapTrigger trapTrigger = gameObject.AddOrGet<TrapTrigger>();
		trapTrigger.trappableCreatures = new Tag[]
		{
			GameTags.Creatures.Walker,
			GameTags.Creatures.Hoverer
		};
		trapTrigger.trappedOffset = new Vector2(0.5f, 0f);
		trapTrigger.enabled = false;
		CritterTrapPlant critterTrapPlant = gameObject.AddOrGet<CritterTrapPlant>();
		critterTrapPlant.gasOutputRate = 0.041666668f;
		critterTrapPlant.outputElement = SimHashes.Hydrogen;
		critterTrapPlant.gasVentThreshold = 33.25f;
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<Storage>();
		Tag tag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = tag,
				massConsumptionRate = 0.016666668f
			}
		});
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Hidden, "CritterTrapPlantSeed", STRINGS.CREATURES.SPECIES.SEEDS.CRITTERTRAPPLANT.NAME, STRINGS.CREATURES.SPECIES.SEEDS.CRITTERTRAPPLANT.DESC, Assets.GetAnim("seed_critter_trap_kanim"), "object", 1, new List<Tag>
		{
			GameTags.CropSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 21, STRINGS.CREATURES.SPECIES.CRITTERTRAPPLANT.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false), "CritterTrapPlant_preview", Assets.GetAnim("venus_critter_trap_kanim"), "place", 1, 2);
		return gameObject;
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x00028FDB File Offset: 0x000271DB
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x00028FDD File Offset: 0x000271DD
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000454 RID: 1108
	public const string ID = "CritterTrapPlant";

	// Token: 0x04000455 RID: 1109
	public const float WATER_RATE = 0.016666668f;

	// Token: 0x04000456 RID: 1110
	public const float GAS_RATE = 0.041666668f;

	// Token: 0x04000457 RID: 1111
	public const float GAS_VENT_THRESHOLD = 33.25f;
}
