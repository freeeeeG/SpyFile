using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000126 RID: 294
public class SquirrelHugConfig : IEntityConfig
{
	// Token: 0x060005AF RID: 1455 RVA: 0x00026274 File Offset: 0x00024474
	public static GameObject CreateSquirrelHug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseSquirrelConfig.BaseSquirrel(id, name, desc, anim_file, "SquirrelHugBaseTrait", is_baby, "hug_", true);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, SquirrelTuning.PEN_SIZE_PER_CREATURE_HUG);
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.BONUS.TIER3);
		Trait trait = Db.Get().CreateTrait("SquirrelHugBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SquirrelTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		Diet.Info[] diet_infos = BaseSquirrelConfig.BasicDiet(SimHashes.Dirt.CreateTag(), SquirrelHugConfig.CALORIES_PER_DAY_OF_PLANT_EATEN, SquirrelHugConfig.KG_POOP_PER_DAY_OF_PLANT, null, 0f);
		gameObject = BaseSquirrelConfig.SetupDiet(gameObject, diet_infos, SquirrelHugConfig.MIN_POOP_SIZE_KG);
		if (!is_baby)
		{
			gameObject.AddOrGetDef<HugMonitor.Def>();
		}
		return gameObject;
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x000263BD File Offset: 0x000245BD
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x000263C4 File Offset: 0x000245C4
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(SquirrelHugConfig.CreateSquirrelHug("SquirrelHug", STRINGS.CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.NAME, STRINGS.CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.DESC, "squirrel_kanim", false), "SquirrelHugEgg", STRINGS.CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.EGG_NAME, STRINGS.CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.DESC, "egg_squirrel_kanim", SquirrelTuning.EGG_MASS, "SquirrelHugBaby", 60.000004f, 20f, SquirrelTuning.EGG_CHANCES_HUG, SquirrelHugConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x0002643F File Offset: 0x0002463F
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x00026441 File Offset: 0x00024641
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003EC RID: 1004
	public const string ID = "SquirrelHug";

	// Token: 0x040003ED RID: 1005
	public const string BASE_TRAIT_ID = "SquirrelHugBaseTrait";

	// Token: 0x040003EE RID: 1006
	public const string EGG_ID = "SquirrelHugEgg";

	// Token: 0x040003EF RID: 1007
	public const float OXYGEN_RATE = 0.023437504f;

	// Token: 0x040003F0 RID: 1008
	public const float BABY_OXYGEN_RATE = 0.011718752f;

	// Token: 0x040003F1 RID: 1009
	private const SimHashes EMIT_ELEMENT = SimHashes.Dirt;

	// Token: 0x040003F2 RID: 1010
	public static float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 0.5f;

	// Token: 0x040003F3 RID: 1011
	private static float CALORIES_PER_DAY_OF_PLANT_EATEN = SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / SquirrelHugConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;

	// Token: 0x040003F4 RID: 1012
	private static float KG_POOP_PER_DAY_OF_PLANT = 25f;

	// Token: 0x040003F5 RID: 1013
	private static float MIN_POOP_SIZE_KG = 40f;

	// Token: 0x040003F6 RID: 1014
	public static int EGG_SORT_ORDER = 0;
}
