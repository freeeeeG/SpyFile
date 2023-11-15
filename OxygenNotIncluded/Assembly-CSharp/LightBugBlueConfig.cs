using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020000FC RID: 252
public class LightBugBlueConfig : IEntityConfig
{
	// Token: 0x060004B4 RID: 1204 RVA: 0x0002229C File Offset: 0x0002049C
	public static GameObject CreateLightBug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugBlueBaseTrait", LIGHT2D.LIGHTBUG_COLOR_BLUE, DECOR.BONUS.TIER6, is_baby, "blu_");
		EntityTemplates.ExtendEntityToWildCreature(gameObject, LightBugTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("LightBugBlueBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 25f, name, false, false, true));
		gameObject = BaseLightBugConfig.SetupDiet(gameObject, new HashSet<Tag>
		{
			TagManager.Create("SpiceBread"),
			TagManager.Create("Salsa"),
			SimHashes.Phosphorite.CreateTag(),
			SimHashes.Phosphorus.CreateTag()
		}, Tag.Invalid, LightBugBlueConfig.CALORIES_PER_KG_OF_ORE);
		gameObject.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[]
		{
			SimHashes.Phosphorite.CreateTag(),
			SimHashes.Phosphorus.CreateTag(),
			GameTags.Creatures.FlyersLure
		};
		return gameObject;
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x00022441 File Offset: 0x00020641
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x00022448 File Offset: 0x00020648
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugBlueConfig.CreateLightBug("LightBugBlue", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLUE.NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLUE.DESC, "lightbug_kanim", false);
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, "LightBugBlueEgg", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLUE.EGG_NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLUE.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugBlueBaby", 15.000001f, 5f, LightBugTuning.EGG_CHANCES_BLUE, LightBugBlueConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
		return gameObject;
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x000224C5 File Offset: 0x000206C5
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x000224C7 File Offset: 0x000206C7
	public void OnSpawn(GameObject inst)
	{
		BaseLightBugConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x04000335 RID: 821
	public const string ID = "LightBugBlue";

	// Token: 0x04000336 RID: 822
	public const string BASE_TRAIT_ID = "LightBugBlueBaseTrait";

	// Token: 0x04000337 RID: 823
	public const string EGG_ID = "LightBugBlueEgg";

	// Token: 0x04000338 RID: 824
	private static float KG_ORE_EATEN_PER_CYCLE = 1f;

	// Token: 0x04000339 RID: 825
	private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugBlueConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x0400033A RID: 826
	public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 4;
}
