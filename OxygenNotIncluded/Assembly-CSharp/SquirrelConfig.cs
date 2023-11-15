using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000124 RID: 292
public class SquirrelConfig : IEntityConfig
{
	// Token: 0x060005A3 RID: 1443 RVA: 0x0002602C File Offset: 0x0002422C
	public static GameObject CreateSquirrel(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseSquirrelConfig.BaseSquirrel(id, name, desc, anim_file, "SquirrelBaseTrait", is_baby, null, false), SquirrelTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("SquirrelBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SquirrelTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		Diet.Info[] diet_infos = BaseSquirrelConfig.BasicDiet(SimHashes.Dirt.CreateTag(), SquirrelConfig.CALORIES_PER_DAY_OF_PLANT_EATEN, SquirrelConfig.KG_POOP_PER_DAY_OF_PLANT, null, 0f);
		GameObject gameObject = BaseSquirrelConfig.SetupDiet(prefab, diet_infos, SquirrelConfig.MIN_POOP_SIZE_KG);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x060005A4 RID: 1444 RVA: 0x0002615B File Offset: 0x0002435B
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x00026164 File Offset: 0x00024364
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(SquirrelConfig.CreateSquirrel("Squirrel", CREATURES.SPECIES.SQUIRREL.NAME, CREATURES.SPECIES.SQUIRREL.DESC, "squirrel_kanim", false), "SquirrelEgg", CREATURES.SPECIES.SQUIRREL.EGG_NAME, CREATURES.SPECIES.SQUIRREL.DESC, "egg_squirrel_kanim", SquirrelTuning.EGG_MASS, "SquirrelBaby", 60.000004f, 20f, SquirrelTuning.EGG_CHANCES_BASE, SquirrelConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x000261DF File Offset: 0x000243DF
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x000261E1 File Offset: 0x000243E1
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003E0 RID: 992
	public const string ID = "Squirrel";

	// Token: 0x040003E1 RID: 993
	public const string BASE_TRAIT_ID = "SquirrelBaseTrait";

	// Token: 0x040003E2 RID: 994
	public const string EGG_ID = "SquirrelEgg";

	// Token: 0x040003E3 RID: 995
	public const float OXYGEN_RATE = 0.023437504f;

	// Token: 0x040003E4 RID: 996
	public const float BABY_OXYGEN_RATE = 0.011718752f;

	// Token: 0x040003E5 RID: 997
	private const SimHashes EMIT_ELEMENT = SimHashes.Dirt;

	// Token: 0x040003E6 RID: 998
	public static float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 0.4f;

	// Token: 0x040003E7 RID: 999
	private static float CALORIES_PER_DAY_OF_PLANT_EATEN = SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / SquirrelConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;

	// Token: 0x040003E8 RID: 1000
	private static float KG_POOP_PER_DAY_OF_PLANT = 50f;

	// Token: 0x040003E9 RID: 1001
	private static float MIN_POOP_SIZE_KG = 40f;

	// Token: 0x040003EA RID: 1002
	public static int EGG_SORT_ORDER = 0;
}
