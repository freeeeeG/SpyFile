using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020000ED RID: 237
public class DreckoConfig : IEntityConfig
{
	// Token: 0x0600045A RID: 1114 RVA: 0x00020C58 File Offset: 0x0001EE58
	public static GameObject CreateDrecko(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseDreckoConfig.BaseDrecko(id, name, desc, anim_file, "DreckoBaseTrait", is_baby, "fbr_", 308.15f, 363.15f);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, DreckoTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("DreckoBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, DreckoTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -DreckoTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 150f, name, false, false, true));
		Diet diet = new Diet(new Diet.Info[]
		{
			new Diet.Info(new HashSet<Tag>
			{
				"SpiceVine".ToTag(),
				SwampLilyConfig.ID.ToTag(),
				"BasicSingleHarvestPlant".ToTag()
			}, DreckoConfig.POOP_ELEMENT, DreckoConfig.CALORIES_PER_DAY_OF_PLANT_EATEN, DreckoConfig.KG_POOP_PER_DAY_OF_PLANT, null, 0f, false, true)
		});
		CreatureCalorieMonitor.Def def = gameObject.AddOrGetDef<CreatureCalorieMonitor.Def>();
		def.diet = diet;
		def.minPoopSizeInCalories = DreckoConfig.MIN_POOP_SIZE_IN_CALORIES;
		gameObject.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		ScaleGrowthMonitor.Def def2 = gameObject.AddOrGetDef<ScaleGrowthMonitor.Def>();
		def2.defaultGrowthRate = 1f / DreckoConfig.SCALE_GROWTH_TIME_IN_CYCLES / 600f;
		def2.dropMass = DreckoConfig.FIBER_PER_CYCLE * DreckoConfig.SCALE_GROWTH_TIME_IN_CYCLES;
		def2.itemDroppedOnShear = DreckoConfig.EMIT_ELEMENT;
		def2.levelCount = 6;
		def2.targetAtmosphere = SimHashes.Hydrogen;
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x00020E3F File Offset: 0x0001F03F
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600045C RID: 1116 RVA: 0x00020E48 File Offset: 0x0001F048
	public virtual GameObject CreatePrefab()
	{
		GameObject prefab = DreckoConfig.CreateDrecko("Drecko", CREATURES.SPECIES.DRECKO.NAME, CREATURES.SPECIES.DRECKO.DESC, "drecko_kanim", false);
		string eggId = "DreckoEgg";
		string eggName = CREATURES.SPECIES.DRECKO.EGG_NAME;
		string eggDesc = CREATURES.SPECIES.DRECKO.DESC;
		string egg_anim = "egg_drecko_kanim";
		float egg_MASS = DreckoTuning.EGG_MASS;
		string baby_id = "DreckoBaby";
		float fertility_cycles = 90f;
		float incubation_cycles = 30f;
		int egg_SORT_ORDER = DreckoConfig.EGG_SORT_ORDER;
		return EntityTemplates.ExtendEntityToFertileCreature(prefab, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, DreckoTuning.EGG_CHANCES_BASE, egg_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x00020EC5 File Offset: 0x0001F0C5
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x00020EC7 File Offset: 0x0001F0C7
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040002E6 RID: 742
	public const string ID = "Drecko";

	// Token: 0x040002E7 RID: 743
	public const string BASE_TRAIT_ID = "DreckoBaseTrait";

	// Token: 0x040002E8 RID: 744
	public const string EGG_ID = "DreckoEgg";

	// Token: 0x040002E9 RID: 745
	public static Tag POOP_ELEMENT = SimHashes.Phosphorite.CreateTag();

	// Token: 0x040002EA RID: 746
	public static Tag EMIT_ELEMENT = BasicFabricConfig.ID;

	// Token: 0x040002EB RID: 747
	private static float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 0.75f;

	// Token: 0x040002EC RID: 748
	private static float CALORIES_PER_DAY_OF_PLANT_EATEN = DreckoTuning.STANDARD_CALORIES_PER_CYCLE / DreckoConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;

	// Token: 0x040002ED RID: 749
	private static float KG_POOP_PER_DAY_OF_PLANT = 13.33f;

	// Token: 0x040002EE RID: 750
	private static float MIN_POOP_SIZE_IN_KG = 1.5f;

	// Token: 0x040002EF RID: 751
	private static float MIN_POOP_SIZE_IN_CALORIES = DreckoConfig.CALORIES_PER_DAY_OF_PLANT_EATEN * DreckoConfig.MIN_POOP_SIZE_IN_KG / DreckoConfig.KG_POOP_PER_DAY_OF_PLANT;

	// Token: 0x040002F0 RID: 752
	public static float SCALE_GROWTH_TIME_IN_CYCLES = 8f;

	// Token: 0x040002F1 RID: 753
	public static float FIBER_PER_CYCLE = 0.25f;

	// Token: 0x040002F2 RID: 754
	public static int EGG_SORT_ORDER = 800;
}
