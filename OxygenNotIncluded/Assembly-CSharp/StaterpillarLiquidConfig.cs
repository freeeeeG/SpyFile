using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200012C RID: 300
public class StaterpillarLiquidConfig : IEntityConfig
{
	// Token: 0x060005D3 RID: 1491 RVA: 0x00026B58 File Offset: 0x00024D58
	public static GameObject CreateStaterpillarLiquid(string id, string name, string desc, string anim_file, bool is_baby)
	{
		InhaleStates.Def inhaleDef = new InhaleStates.Def
		{
			behaviourTag = GameTags.Creatures.WantsToStore,
			inhaleAnimPre = "liquid_consume_pre",
			inhaleAnimLoop = "liquid_consume_loop",
			inhaleAnimPst = "liquid_consume_pst",
			useStorage = true,
			alwaysPlayPstAnim = true,
			inhaleTime = StaterpillarLiquidConfig.INHALE_TIME,
			storageStatusItem = Db.Get().CreatureStatusItems.LookingForLiquid
		};
		GameObject gameObject = BaseStaterpillarConfig.BaseStaterpillar(id, name, desc, anim_file, "StaterpillarLiquidBaseTrait", is_baby, ObjectLayer.LiquidConduit, StaterpillarLiquidConnectorConfig.ID, GameTags.Unbreathable, "wtr_", StaterpillarLiquidConfig.WARNING_LOW_TEMPERATURE, StaterpillarLiquidConfig.WARNING_HIGH_TEMPERATURE, StaterpillarLiquidConfig.LETHAL_LOW_TEMPERATURE, StaterpillarLiquidConfig.LETHAL_HIGH_TEMPERATURE, inhaleDef);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, TUNING.CREATURES.SPACE_REQUIREMENTS.TIER3);
		if (!is_baby)
		{
			GasAndLiquidConsumerMonitor.Def def = gameObject.AddOrGetDef<GasAndLiquidConsumerMonitor.Def>();
			def.behaviourTag = GameTags.Creatures.WantsToStore;
			def.consumableElementTag = GameTags.Liquid;
			def.transitionTag = new Tag[]
			{
				GameTags.Creature
			};
			def.minCooldown = StaterpillarLiquidConfig.COOLDOWN_MIN;
			def.maxCooldown = StaterpillarLiquidConfig.COOLDOWN_MAX;
			def.consumptionRate = StaterpillarLiquidConfig.CONSUMPTION_RATE;
		}
		Trait trait = Db.Get().CreateTrait("StaterpillarLiquidBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, StaterpillarTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> list = new List<Diet.Info>();
		list.AddRange(BaseStaterpillarConfig.RawMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarLiquidConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, null, 0f));
		list.AddRange(BaseStaterpillarConfig.RefinedMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarLiquidConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, null, 0f));
		gameObject = BaseStaterpillarConfig.SetupDiet(gameObject, list);
		Storage storage = gameObject.AddComponent<Storage>();
		storage.capacityKg = StaterpillarLiquidConfig.STORAGE_CAPACITY;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		return gameObject;
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x00026DA6 File Offset: 0x00024FA6
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x00026DB0 File Offset: 0x00024FB0
	public virtual GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(StaterpillarLiquidConfig.CreateStaterpillarLiquid("StaterpillarLiquid", STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.NAME, STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.DESC, "caterpillar_kanim", false), "StaterpillarLiquidEgg", STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.EGG_NAME, STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.DESC, "egg_caterpillar_kanim", StaterpillarTuning.EGG_MASS, "StaterpillarLiquidBaby", 60.000004f, 20f, StaterpillarTuning.EGG_CHANCES_LIQUID, 2, true, false, true, 1f, false);
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x00026E27 File Offset: 0x00025027
	public void OnPrefabInit(GameObject prefab)
	{
		KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
		component.SetSymbolVisiblity("electric_bolt_c_bloom", false);
		component.SetSymbolVisiblity("gulp", false);
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x00026E50 File Offset: 0x00025050
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400040F RID: 1039
	public const string ID = "StaterpillarLiquid";

	// Token: 0x04000410 RID: 1040
	public const string BASE_TRAIT_ID = "StaterpillarLiquidBaseTrait";

	// Token: 0x04000411 RID: 1041
	public const string EGG_ID = "StaterpillarLiquidEgg";

	// Token: 0x04000412 RID: 1042
	public const int EGG_SORT_ORDER = 2;

	// Token: 0x04000413 RID: 1043
	private static float KG_ORE_EATEN_PER_CYCLE = 30f;

	// Token: 0x04000414 RID: 1044
	private static float CALORIES_PER_KG_OF_ORE = StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / StaterpillarLiquidConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000415 RID: 1045
	private static float STORAGE_CAPACITY = 1000f;

	// Token: 0x04000416 RID: 1046
	private static float COOLDOWN_MIN = 20f;

	// Token: 0x04000417 RID: 1047
	private static float COOLDOWN_MAX = 40f;

	// Token: 0x04000418 RID: 1048
	private static float CONSUMPTION_RATE = 10f;

	// Token: 0x04000419 RID: 1049
	private static float INHALE_TIME = 6f;

	// Token: 0x0400041A RID: 1050
	private static float LETHAL_LOW_TEMPERATURE = 243.15f;

	// Token: 0x0400041B RID: 1051
	private static float LETHAL_HIGH_TEMPERATURE = 363.15f;

	// Token: 0x0400041C RID: 1052
	private static float WARNING_LOW_TEMPERATURE = StaterpillarLiquidConfig.LETHAL_LOW_TEMPERATURE + 20f;

	// Token: 0x0400041D RID: 1053
	private static float WARNING_HIGH_TEMPERATURE = StaterpillarLiquidConfig.LETHAL_HIGH_TEMPERATURE - 20f;
}
