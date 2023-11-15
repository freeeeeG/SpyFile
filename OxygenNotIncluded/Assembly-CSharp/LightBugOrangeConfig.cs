using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000102 RID: 258
public class LightBugOrangeConfig : IEntityConfig
{
	// Token: 0x060004D8 RID: 1240 RVA: 0x00022A68 File Offset: 0x00020C68
	public static GameObject CreateLightBug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugOrangeBaseTrait", LIGHT2D.LIGHTBUG_COLOR_ORANGE, DECOR.BONUS.TIER6, is_baby, "org_");
		EntityTemplates.ExtendEntityToWildCreature(prefab, LightBugTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("LightBugOrangeBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 25f, name, false, false, true));
		return BaseLightBugConfig.SetupDiet(prefab, new HashSet<Tag>
		{
			TagManager.Create(MushroomConfig.ID),
			TagManager.Create("FriedMushroom"),
			TagManager.Create("GrilledPrickleFruit"),
			SimHashes.Phosphorite.CreateTag()
		}, Tag.Invalid, LightBugOrangeConfig.CALORIES_PER_KG_OF_ORE);
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x00022BCA File Offset: 0x00020DCA
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x00022BD4 File Offset: 0x00020DD4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugOrangeConfig.CreateLightBug("LightBugOrange", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.DESC, "lightbug_kanim", false);
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, "LightBugOrangeEgg", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.EGG_NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugOrangeBaby", 15.000001f, 5f, LightBugTuning.EGG_CHANCES_ORANGE, LightBugOrangeConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
		return gameObject;
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x00022C51 File Offset: 0x00020E51
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x00022C53 File Offset: 0x00020E53
	public void OnSpawn(GameObject inst)
	{
		BaseLightBugConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x0400034A RID: 842
	public const string ID = "LightBugOrange";

	// Token: 0x0400034B RID: 843
	public const string BASE_TRAIT_ID = "LightBugOrangeBaseTrait";

	// Token: 0x0400034C RID: 844
	public const string EGG_ID = "LightBugOrangeEgg";

	// Token: 0x0400034D RID: 845
	private static float KG_ORE_EATEN_PER_CYCLE = 0.25f;

	// Token: 0x0400034E RID: 846
	private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugOrangeConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x0400034F RID: 847
	public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 1;
}
