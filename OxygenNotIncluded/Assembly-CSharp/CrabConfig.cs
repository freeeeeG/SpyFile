using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020000E3 RID: 227
[EntityConfigOrder(1)]
public class CrabConfig : IEntityConfig
{
	// Token: 0x0600041E RID: 1054 RVA: 0x0001FD9C File Offset: 0x0001DF9C
	public static GameObject CreateCrab(string id, string name, string desc, string anim_file, bool is_baby, string deathDropID)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseCrabConfig.BaseCrab(id, name, desc, anim_file, "CrabBaseTrait", is_baby, null, deathDropID, 1), CrabTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("CrabBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, CrabTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -CrabTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> diet_infos = BaseCrabConfig.BasicDiet(SimHashes.Sand.CreateTag(), CrabConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f);
		GameObject gameObject = BaseCrabConfig.SetupDiet(prefab, diet_infos, CrabConfig.CALORIES_PER_KG_OF_ORE, CrabConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x0001FED2 File Offset: 0x0001E0D2
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x0001FEDC File Offset: 0x0001E0DC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabConfig.CreateCrab("Crab", STRINGS.CREATURES.SPECIES.CRAB.NAME, STRINGS.CREATURES.SPECIES.CRAB.DESC, "pincher_kanim", false, "CrabShell");
		gameObject = EntityTemplates.ExtendEntityToFertileCreature(gameObject, "CrabEgg", STRINGS.CREATURES.SPECIES.CRAB.EGG_NAME, STRINGS.CREATURES.SPECIES.CRAB.DESC, "egg_pincher_kanim", CrabTuning.EGG_MASS, "CrabBaby", 60.000004f, 20f, CrabTuning.EGG_CHANCES_BASE, CrabConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
		gameObject.AddOrGetDef<EggProtectionMonitor.Def>().allyTags = new Tag[]
		{
			GameTags.Creatures.CrabFriend
		};
		return gameObject;
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x0001FF7D File Offset: 0x0001E17D
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x0001FF7F File Offset: 0x0001E17F
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040002B0 RID: 688
	public const string ID = "Crab";

	// Token: 0x040002B1 RID: 689
	public const string BASE_TRAIT_ID = "CrabBaseTrait";

	// Token: 0x040002B2 RID: 690
	public const string EGG_ID = "CrabEgg";

	// Token: 0x040002B3 RID: 691
	private const SimHashes EMIT_ELEMENT = SimHashes.Sand;

	// Token: 0x040002B4 RID: 692
	private static float KG_ORE_EATEN_PER_CYCLE = 70f;

	// Token: 0x040002B5 RID: 693
	private static float CALORIES_PER_KG_OF_ORE = CrabTuning.STANDARD_CALORIES_PER_CYCLE / CrabConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040002B6 RID: 694
	private static float MIN_POOP_SIZE_IN_KG = 25f;

	// Token: 0x040002B7 RID: 695
	public static int EGG_SORT_ORDER = 0;
}
