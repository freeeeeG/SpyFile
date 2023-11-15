using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020000E9 RID: 233
[EntityConfigOrder(1)]
public class DivergentBeetleConfig : IEntityConfig
{
	// Token: 0x06000442 RID: 1090 RVA: 0x00020694 File Offset: 0x0001E894
	public static GameObject CreateDivergentBeetle(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseDivergentConfig.BaseDivergent(id, name, desc, 50f, anim_file, "DivergentBeetleBaseTrait", is_baby, 8f, null, "DivergentCropTended", 1, true), DivergentTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("DivergentBeetleBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, DivergentTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -DivergentTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		List<Diet.Info> diet_infos = BaseDivergentConfig.BasicSulfurDiet(SimHashes.Sucrose.CreateTag(), DivergentBeetleConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f);
		GameObject gameObject = BaseDivergentConfig.SetupDiet(prefab, diet_infos, DivergentBeetleConfig.CALORIES_PER_KG_OF_ORE, DivergentBeetleConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000443 RID: 1091 RVA: 0x000207D8 File Offset: 0x0001E9D8
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000444 RID: 1092 RVA: 0x000207E0 File Offset: 0x0001E9E0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(DivergentBeetleConfig.CreateDivergentBeetle("DivergentBeetle", STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.NAME, STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.DESC, "critter_kanim", false), "DivergentBeetleEgg", STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.EGG_NAME, STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.DESC, "egg_critter_kanim", DivergentTuning.EGG_MASS, "DivergentBeetleBaby", 45f, 15f, DivergentTuning.EGG_CHANCES_BEETLE, DivergentBeetleConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x06000445 RID: 1093 RVA: 0x0002085B File Offset: 0x0001EA5B
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x0002085D File Offset: 0x0001EA5D
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040002CD RID: 717
	public const string ID = "DivergentBeetle";

	// Token: 0x040002CE RID: 718
	public const string BASE_TRAIT_ID = "DivergentBeetleBaseTrait";

	// Token: 0x040002CF RID: 719
	public const string EGG_ID = "DivergentBeetleEgg";

	// Token: 0x040002D0 RID: 720
	private const float LIFESPAN = 75f;

	// Token: 0x040002D1 RID: 721
	private const SimHashes EMIT_ELEMENT = SimHashes.Sucrose;

	// Token: 0x040002D2 RID: 722
	private static float KG_ORE_EATEN_PER_CYCLE = 20f;

	// Token: 0x040002D3 RID: 723
	private static float CALORIES_PER_KG_OF_ORE = DivergentTuning.STANDARD_CALORIES_PER_CYCLE / DivergentBeetleConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040002D4 RID: 724
	private static float MIN_POOP_SIZE_IN_KG = 4f;

	// Token: 0x040002D5 RID: 725
	public static int EGG_SORT_ORDER = 0;
}
