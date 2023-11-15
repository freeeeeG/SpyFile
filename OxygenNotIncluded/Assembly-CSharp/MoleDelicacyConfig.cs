using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200010A RID: 266
public class MoleDelicacyConfig : IEntityConfig
{
	// Token: 0x06000509 RID: 1289 RVA: 0x00023558 File Offset: 0x00021758
	public static GameObject CreateMole(string id, string name, string desc, string anim_file, bool is_baby = false)
	{
		GameObject gameObject = BaseMoleConfig.BaseMole(id, name, desc, "MoleDelicacyBaseTrait", anim_file, is_baby, "del_", 5);
		gameObject.AddTag(GameTags.Creatures.Digger);
		EntityTemplates.ExtendEntityToWildCreature(gameObject, MoleTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("MoleDelicacyBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, MoleTuning.DELICACY_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -MoleTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		Diet diet = new Diet(BaseMoleConfig.SimpleOreDiet(new List<Tag>
		{
			SimHashes.Regolith.CreateTag(),
			SimHashes.Dirt.CreateTag(),
			SimHashes.IronOre.CreateTag()
		}, MoleDelicacyConfig.CALORIES_PER_KG_OF_DIRT, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL).ToArray());
		CreatureCalorieMonitor.Def def = gameObject.AddOrGetDef<CreatureCalorieMonitor.Def>();
		def.diet = diet;
		def.minPoopSizeInCalories = MoleDelicacyConfig.MIN_POOP_SIZE_IN_CALORIES;
		gameObject.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		gameObject.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = 0;
		gameObject.AddOrGet<LoopingSounds>();
		if (!is_baby)
		{
			ElementGrowthMonitor.Def def2 = gameObject.AddOrGetDef<ElementGrowthMonitor.Def>();
			def2.defaultGrowthRate = 1f / MoleDelicacyConfig.GINGER_GROWTH_TIME_IN_CYCLES / 600f;
			def2.dropMass = MoleDelicacyConfig.GINGER_PER_CYCLE * MoleDelicacyConfig.GINGER_GROWTH_TIME_IN_CYCLES;
			def2.itemDroppedOnShear = MoleDelicacyConfig.SHEAR_DROP_ELEMENT;
			def2.levelCount = 5;
			def2.minTemperature = MoleDelicacyConfig.MIN_GROWTH_TEMPERATURE;
			def2.maxTemperature = MoleDelicacyConfig.MAX_GROWTH_TEMPERATURE;
		}
		else
		{
			gameObject.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.ElementGrowth.Id);
		}
		return gameObject;
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x00023768 File Offset: 0x00021968
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x00023770 File Offset: 0x00021970
	public GameObject CreatePrefab()
	{
		GameObject prefab = MoleDelicacyConfig.CreateMole("MoleDelicacy", STRINGS.CREATURES.SPECIES.MOLE.VARIANT_DELICACY.NAME, STRINGS.CREATURES.SPECIES.MOLE.VARIANT_DELICACY.DESC, "driller_kanim", false);
		string eggId = "MoleDelicacyEgg";
		string eggName = STRINGS.CREATURES.SPECIES.MOLE.VARIANT_DELICACY.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.MOLE.VARIANT_DELICACY.DESC;
		string egg_anim = "egg_driller_kanim";
		float egg_MASS = MoleTuning.EGG_MASS;
		string baby_id = "MoleDelicacyBaby";
		float fertility_cycles = 60.000004f;
		float incubation_cycles = 20f;
		int egg_SORT_ORDER = MoleDelicacyConfig.EGG_SORT_ORDER;
		return EntityTemplates.ExtendEntityToFertileCreature(prefab, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, MoleTuning.EGG_CHANCES_DELICACY, egg_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x000237ED File Offset: 0x000219ED
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x000237EF File Offset: 0x000219EF
	public void OnSpawn(GameObject inst)
	{
		MoleDelicacyConfig.SetSpawnNavType(inst);
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x000237F8 File Offset: 0x000219F8
	public static void SetSpawnNavType(GameObject inst)
	{
		int cell = Grid.PosToCell(inst);
		Navigator component = inst.GetComponent<Navigator>();
		if (component != null)
		{
			if (Grid.IsSolidCell(cell))
			{
				component.SetCurrentNavType(NavType.Solid);
				inst.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.FXFront));
				inst.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.FXFront);
				return;
			}
			inst.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
		}
	}

	// Token: 0x04000366 RID: 870
	public const string ID = "MoleDelicacy";

	// Token: 0x04000367 RID: 871
	public const string BASE_TRAIT_ID = "MoleDelicacyBaseTrait";

	// Token: 0x04000368 RID: 872
	public const string EGG_ID = "MoleDelicacyEgg";

	// Token: 0x04000369 RID: 873
	private static float MIN_POOP_SIZE_IN_CALORIES = 2400000f;

	// Token: 0x0400036A RID: 874
	private static float CALORIES_PER_KG_OF_DIRT = 1000f;

	// Token: 0x0400036B RID: 875
	public static int EGG_SORT_ORDER = 800;

	// Token: 0x0400036C RID: 876
	public static float GINGER_GROWTH_TIME_IN_CYCLES = 8f;

	// Token: 0x0400036D RID: 877
	public static float GINGER_PER_CYCLE = 1f;

	// Token: 0x0400036E RID: 878
	public static Tag SHEAR_DROP_ELEMENT = GingerConfig.ID;

	// Token: 0x0400036F RID: 879
	public static float MIN_GROWTH_TEMPERATURE = 343.15f;

	// Token: 0x04000370 RID: 880
	public static float MAX_GROWTH_TEMPERATURE = 353.15f;

	// Token: 0x04000371 RID: 881
	public static float EGG_CHANCES_TEMPERATURE_MIN = 333.15f;

	// Token: 0x04000372 RID: 882
	public static float EGG_CHANCES_TEMPERATURE_MAX = 373.15f;
}
