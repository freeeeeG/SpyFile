using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000106 RID: 262
public class LightBugPurpleConfig : IEntityConfig
{
	// Token: 0x060004F0 RID: 1264 RVA: 0x00022F70 File Offset: 0x00021170
	public static GameObject CreateLightBug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugPurpleBaseTrait", LIGHT2D.LIGHTBUG_COLOR_PURPLE, DECOR.BONUS.TIER6, is_baby, "prp_");
		EntityTemplates.ExtendEntityToWildCreature(prefab, LightBugTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("LightBugPurpleBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 25f, name, false, false, true));
		return BaseLightBugConfig.SetupDiet(prefab, new HashSet<Tag>
		{
			TagManager.Create("FriedMushroom"),
			TagManager.Create("GrilledPrickleFruit"),
			TagManager.Create(SpiceNutConfig.ID),
			TagManager.Create("SpiceBread"),
			SimHashes.Phosphorite.CreateTag()
		}, Tag.Invalid, LightBugPurpleConfig.CALORIES_PER_KG_OF_ORE);
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x000230E3 File Offset: 0x000212E3
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x000230EC File Offset: 0x000212EC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugPurpleConfig.CreateLightBug("LightBugPurple", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.DESC, "lightbug_kanim", false);
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, "LightBugPurpleEgg", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.EGG_NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugPurpleBaby", 15.000001f, 5f, LightBugTuning.EGG_CHANCES_PURPLE, LightBugPurpleConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
		return gameObject;
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x00023169 File Offset: 0x00021369
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x0002316B File Offset: 0x0002136B
	public void OnSpawn(GameObject inst)
	{
		BaseLightBugConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x04000358 RID: 856
	public const string ID = "LightBugPurple";

	// Token: 0x04000359 RID: 857
	public const string BASE_TRAIT_ID = "LightBugPurpleBaseTrait";

	// Token: 0x0400035A RID: 858
	public const string EGG_ID = "LightBugPurpleEgg";

	// Token: 0x0400035B RID: 859
	private static float KG_ORE_EATEN_PER_CYCLE = 1f;

	// Token: 0x0400035C RID: 860
	private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugPurpleConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x0400035D RID: 861
	public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 2;
}
