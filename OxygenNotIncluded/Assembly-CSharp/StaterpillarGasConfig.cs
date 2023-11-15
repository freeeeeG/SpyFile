using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200012A RID: 298
public class StaterpillarGasConfig : IEntityConfig
{
	// Token: 0x060005C7 RID: 1479 RVA: 0x00026760 File Offset: 0x00024960
	public static GameObject CreateStaterpillarGas(string id, string name, string desc, string anim_file, bool is_baby)
	{
		InhaleStates.Def inhaleDef = new InhaleStates.Def
		{
			behaviourTag = GameTags.Creatures.WantsToStore,
			inhaleAnimPre = "gas_consume_pre",
			inhaleAnimLoop = "gas_consume_loop",
			inhaleAnimPst = "gas_consume_pst",
			useStorage = true,
			alwaysPlayPstAnim = true,
			inhaleTime = StaterpillarGasConfig.INHALE_TIME,
			storageStatusItem = Db.Get().CreatureStatusItems.LookingForGas
		};
		GameObject gameObject = BaseStaterpillarConfig.BaseStaterpillar(id, name, desc, anim_file, "StaterpillarGasBaseTrait", is_baby, ObjectLayer.GasConduit, StaterpillarGasConnectorConfig.ID, GameTags.Unbreathable, "gas_", StaterpillarGasConfig.WARNING_LOW_TEMPERATURE, StaterpillarGasConfig.WARNING_HIGH_TEMPERATURE, StaterpillarGasConfig.LETHAL_LOW_TEMPERATURE, StaterpillarGasConfig.LETHAL_HIGH_TEMPERATURE, inhaleDef);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, TUNING.CREATURES.SPACE_REQUIREMENTS.TIER3);
		if (!is_baby)
		{
			GasAndLiquidConsumerMonitor.Def def = gameObject.AddOrGetDef<GasAndLiquidConsumerMonitor.Def>();
			def.behaviourTag = GameTags.Creatures.WantsToStore;
			def.consumableElementTag = GameTags.Unbreathable;
			def.transitionTag = new Tag[]
			{
				GameTags.Creature
			};
			def.minCooldown = StaterpillarGasConfig.COOLDOWN_MIN;
			def.maxCooldown = StaterpillarGasConfig.COOLDOWN_MAX;
			def.consumptionRate = StaterpillarGasConfig.CONSUMPTION_RATE;
		}
		Trait trait = Db.Get().CreateTrait("StaterpillarGasBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, StaterpillarTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> list = new List<Diet.Info>();
		list.AddRange(BaseStaterpillarConfig.RawMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarGasConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, null, 0f));
		list.AddRange(BaseStaterpillarConfig.RefinedMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarGasConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, null, 0f));
		gameObject = BaseStaterpillarConfig.SetupDiet(gameObject, list);
		Storage storage = gameObject.AddComponent<Storage>();
		storage.capacityKg = StaterpillarGasConfig.STORAGE_CAPACITY;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		return gameObject;
	}

	// Token: 0x060005C8 RID: 1480 RVA: 0x000269AE File Offset: 0x00024BAE
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x000269B8 File Offset: 0x00024BB8
	public virtual GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(StaterpillarGasConfig.CreateStaterpillarGas("StaterpillarGas", STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.NAME, STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.DESC, "caterpillar_kanim", false), "StaterpillarGasEgg", STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.EGG_NAME, STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.DESC, "egg_caterpillar_kanim", StaterpillarTuning.EGG_MASS, "StaterpillarGasBaby", 60.000004f, 20f, StaterpillarTuning.EGG_CHANCES_GAS, 1, true, false, true, 1f, false);
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x00026A2F File Offset: 0x00024C2F
	public void OnPrefabInit(GameObject prefab)
	{
		KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
		component.SetSymbolVisiblity("electric_bolt_c_bloom", false);
		component.SetSymbolVisiblity("gulp", false);
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x00026A58 File Offset: 0x00024C58
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003FF RID: 1023
	public const string ID = "StaterpillarGas";

	// Token: 0x04000400 RID: 1024
	public const string BASE_TRAIT_ID = "StaterpillarGasBaseTrait";

	// Token: 0x04000401 RID: 1025
	public const string EGG_ID = "StaterpillarGasEgg";

	// Token: 0x04000402 RID: 1026
	public const int EGG_SORT_ORDER = 1;

	// Token: 0x04000403 RID: 1027
	private static float KG_ORE_EATEN_PER_CYCLE = 30f;

	// Token: 0x04000404 RID: 1028
	private static float CALORIES_PER_KG_OF_ORE = StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / StaterpillarGasConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000405 RID: 1029
	private static float STORAGE_CAPACITY = 100f;

	// Token: 0x04000406 RID: 1030
	private static float COOLDOWN_MIN = 20f;

	// Token: 0x04000407 RID: 1031
	private static float COOLDOWN_MAX = 40f;

	// Token: 0x04000408 RID: 1032
	private static float CONSUMPTION_RATE = 0.5f;

	// Token: 0x04000409 RID: 1033
	private static float INHALE_TIME = 6f;

	// Token: 0x0400040A RID: 1034
	private static float LETHAL_LOW_TEMPERATURE = 243.15f;

	// Token: 0x0400040B RID: 1035
	private static float LETHAL_HIGH_TEMPERATURE = 363.15f;

	// Token: 0x0400040C RID: 1036
	private static float WARNING_LOW_TEMPERATURE = StaterpillarGasConfig.LETHAL_LOW_TEMPERATURE + 20f;

	// Token: 0x0400040D RID: 1037
	private static float WARNING_HIGH_TEMPERATURE = StaterpillarGasConfig.LETHAL_HIGH_TEMPERATURE - 20f;
}
