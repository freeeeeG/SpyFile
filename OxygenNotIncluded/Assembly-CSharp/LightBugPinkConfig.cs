using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000104 RID: 260
public class LightBugPinkConfig : IEntityConfig
{
	// Token: 0x060004E4 RID: 1252 RVA: 0x00022CDC File Offset: 0x00020EDC
	public static GameObject CreateLightBug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugPinkBaseTrait", LIGHT2D.LIGHTBUG_COLOR_PINK, DECOR.BONUS.TIER6, is_baby, "pnk_");
		EntityTemplates.ExtendEntityToWildCreature(prefab, LightBugTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("LightBugPinkBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 25f, name, false, false, true));
		return BaseLightBugConfig.SetupDiet(prefab, new HashSet<Tag>
		{
			TagManager.Create("FriedMushroom"),
			TagManager.Create("SpiceBread"),
			TagManager.Create(PrickleFruitConfig.ID),
			TagManager.Create("GrilledPrickleFruit"),
			TagManager.Create("Salsa"),
			SimHashes.Phosphorite.CreateTag()
		}, Tag.Invalid, LightBugPinkConfig.CALORIES_PER_KG_OF_ORE);
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x00022E60 File Offset: 0x00021060
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x00022E68 File Offset: 0x00021068
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugPinkConfig.CreateLightBug("LightBugPink", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.DESC, "lightbug_kanim", false);
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, "LightBugPinkEgg", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.EGG_NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugPinkBaby", 15.000001f, 5f, LightBugTuning.EGG_CHANCES_PINK, LightBugPinkConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
		return gameObject;
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x00022EE5 File Offset: 0x000210E5
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x00022EE7 File Offset: 0x000210E7
	public void OnSpawn(GameObject inst)
	{
		BaseLightBugConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x04000351 RID: 849
	public const string ID = "LightBugPink";

	// Token: 0x04000352 RID: 850
	public const string BASE_TRAIT_ID = "LightBugPinkBaseTrait";

	// Token: 0x04000353 RID: 851
	public const string EGG_ID = "LightBugPinkEgg";

	// Token: 0x04000354 RID: 852
	private static float KG_ORE_EATEN_PER_CYCLE = 1f;

	// Token: 0x04000355 RID: 853
	private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugPinkConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000356 RID: 854
	public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 3;
}
