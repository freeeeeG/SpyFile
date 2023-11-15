using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200010C RID: 268
public class MooConfig : IEntityConfig
{
	// Token: 0x06000516 RID: 1302 RVA: 0x00023934 File Offset: 0x00021B34
	public static GameObject CreateMoo(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseMooConfig.BaseMoo(id, name, CREATURES.SPECIES.MOO.DESC, "MooBaseTrait", anim_file, is_baby, null);
		EntityTemplates.ExtendEntityToWildCreature(gameObject, MooTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("MooBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, MooTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -MooTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, MooTuning.STANDARD_LIFESPAN, name, false, false, true));
		Diet diet = new Diet(new Diet.Info[]
		{
			new Diet.Info(new HashSet<Tag>
			{
				"GasGrass".ToTag()
			}, MooConfig.POOP_ELEMENT, MooConfig.CALORIES_PER_DAY_OF_PLANT_EATEN, MooConfig.KG_POOP_PER_DAY_OF_PLANT, null, 0f, false, true)
		});
		CreatureCalorieMonitor.Def def = gameObject.AddOrGetDef<CreatureCalorieMonitor.Def>();
		def.diet = diet;
		def.minPoopSizeInCalories = MooConfig.MIN_POOP_SIZE_IN_CALORIES;
		gameObject.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x00023AAA File Offset: 0x00021CAA
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x00023AB1 File Offset: 0x00021CB1
	public GameObject CreatePrefab()
	{
		return MooConfig.CreateMoo("Moo", CREATURES.SPECIES.MOO.NAME, CREATURES.SPECIES.MOO.DESC, "gassy_moo_kanim", false);
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x00023AD7 File Offset: 0x00021CD7
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x00023AD9 File Offset: 0x00021CD9
	public void OnSpawn(GameObject inst)
	{
		BaseMooConfig.OnSpawn(inst);
	}

	// Token: 0x04000374 RID: 884
	public const string ID = "Moo";

	// Token: 0x04000375 RID: 885
	public const string BASE_TRAIT_ID = "MooBaseTrait";

	// Token: 0x04000376 RID: 886
	public const SimHashes CONSUME_ELEMENT = SimHashes.Carbon;

	// Token: 0x04000377 RID: 887
	public static Tag POOP_ELEMENT = SimHashes.Methane.CreateTag();

	// Token: 0x04000378 RID: 888
	public static readonly float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 2f;

	// Token: 0x04000379 RID: 889
	private static float CALORIES_PER_DAY_OF_PLANT_EATEN = MooTuning.STANDARD_CALORIES_PER_CYCLE / MooConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;

	// Token: 0x0400037A RID: 890
	private static float KG_POOP_PER_DAY_OF_PLANT = 5f;

	// Token: 0x0400037B RID: 891
	private static float MIN_POOP_SIZE_IN_KG = 1.5f;

	// Token: 0x0400037C RID: 892
	private static float MIN_POOP_SIZE_IN_CALORIES = MooConfig.CALORIES_PER_DAY_OF_PLANT_EATEN * MooConfig.MIN_POOP_SIZE_IN_KG / MooConfig.KG_POOP_PER_DAY_OF_PLANT;
}
