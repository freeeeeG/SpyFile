using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020000EF RID: 239
public class DreckoPlasticConfig : IEntityConfig
{
	// Token: 0x06000466 RID: 1126 RVA: 0x00020FB4 File Offset: 0x0001F1B4
	public static GameObject CreateDrecko(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseDreckoConfig.BaseDrecko(id, name, desc, anim_file, "DreckoPlasticBaseTrait", is_baby, null, 298.15f, 333.15f);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, DreckoTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("DreckoPlasticBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, DreckoTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -DreckoTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 150f, name, false, false, true));
		Diet diet = new Diet(new Diet.Info[]
		{
			new Diet.Info(new HashSet<Tag>
			{
				"BasicSingleHarvestPlant".ToTag(),
				"PrickleFlower".ToTag()
			}, DreckoPlasticConfig.POOP_ELEMENT, DreckoPlasticConfig.CALORIES_PER_DAY_OF_PLANT_EATEN, DreckoPlasticConfig.KG_POOP_PER_DAY_OF_PLANT, null, 0f, false, true)
		});
		CreatureCalorieMonitor.Def def = gameObject.AddOrGetDef<CreatureCalorieMonitor.Def>();
		def.diet = diet;
		def.minPoopSizeInCalories = DreckoPlasticConfig.MIN_POOP_SIZE_IN_CALORIES;
		ScaleGrowthMonitor.Def def2 = gameObject.AddOrGetDef<ScaleGrowthMonitor.Def>();
		def2.defaultGrowthRate = 1f / DreckoPlasticConfig.SCALE_GROWTH_TIME_IN_CYCLES / 600f;
		def2.dropMass = DreckoPlasticConfig.PLASTIC_PER_CYCLE * DreckoPlasticConfig.SCALE_GROWTH_TIME_IN_CYCLES;
		def2.itemDroppedOnShear = DreckoPlasticConfig.EMIT_ELEMENT;
		def2.levelCount = 6;
		def2.targetAtmosphere = SimHashes.Hydrogen;
		gameObject.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		return gameObject;
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x0002117B File Offset: 0x0001F37B
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x00021184 File Offset: 0x0001F384
	public virtual GameObject CreatePrefab()
	{
		GameObject prefab = DreckoPlasticConfig.CreateDrecko("DreckoPlastic", CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.NAME, CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.DESC, "drecko_kanim", false);
		string eggId = "DreckoPlasticEgg";
		string eggName = CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.EGG_NAME;
		string eggDesc = CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.DESC;
		string egg_anim = "egg_drecko_kanim";
		float egg_MASS = DreckoTuning.EGG_MASS;
		string baby_id = "DreckoPlasticBaby";
		float fertility_cycles = 90f;
		float incubation_cycles = 30f;
		int egg_SORT_ORDER = DreckoPlasticConfig.EGG_SORT_ORDER;
		return EntityTemplates.ExtendEntityToFertileCreature(prefab, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, DreckoTuning.EGG_CHANCES_PLASTIC, egg_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x00021201 File Offset: 0x0001F401
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x00021203 File Offset: 0x0001F403
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040002F4 RID: 756
	public const string ID = "DreckoPlastic";

	// Token: 0x040002F5 RID: 757
	public const string BASE_TRAIT_ID = "DreckoPlasticBaseTrait";

	// Token: 0x040002F6 RID: 758
	public const string EGG_ID = "DreckoPlasticEgg";

	// Token: 0x040002F7 RID: 759
	public static Tag POOP_ELEMENT = SimHashes.Phosphorite.CreateTag();

	// Token: 0x040002F8 RID: 760
	public static Tag EMIT_ELEMENT = SimHashes.Polypropylene.CreateTag();

	// Token: 0x040002F9 RID: 761
	private static float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 1f;

	// Token: 0x040002FA RID: 762
	private static float CALORIES_PER_DAY_OF_PLANT_EATEN = DreckoTuning.STANDARD_CALORIES_PER_CYCLE / DreckoPlasticConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;

	// Token: 0x040002FB RID: 763
	private static float KG_POOP_PER_DAY_OF_PLANT = 9f;

	// Token: 0x040002FC RID: 764
	private static float MIN_POOP_SIZE_IN_KG = 1.5f;

	// Token: 0x040002FD RID: 765
	private static float MIN_POOP_SIZE_IN_CALORIES = DreckoPlasticConfig.CALORIES_PER_DAY_OF_PLANT_EATEN * DreckoPlasticConfig.MIN_POOP_SIZE_IN_KG / DreckoPlasticConfig.KG_POOP_PER_DAY_OF_PLANT;

	// Token: 0x040002FE RID: 766
	public static float SCALE_GROWTH_TIME_IN_CYCLES = 3f;

	// Token: 0x040002FF RID: 767
	public static float PLASTIC_PER_CYCLE = 50f;

	// Token: 0x04000300 RID: 768
	public static int EGG_SORT_ORDER = 800;
}
