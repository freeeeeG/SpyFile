using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000100 RID: 256
public class LightBugCrystalConfig : IEntityConfig
{
	// Token: 0x060004CC RID: 1228 RVA: 0x000227E4 File Offset: 0x000209E4
	public static GameObject CreateLightBug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugCrystalBaseTrait", LIGHT2D.LIGHTBUG_COLOR_CRYSTAL, DECOR.BONUS.TIER8, is_baby, "cry_");
		EntityTemplates.ExtendEntityToWildCreature(gameObject, LightBugTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("LightBugCrystalBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		gameObject = BaseLightBugConfig.SetupDiet(gameObject, new HashSet<Tag>
		{
			TagManager.Create("CookedMeat"),
			SimHashes.Diamond.CreateTag()
		}, Tag.Invalid, LightBugCrystalConfig.CALORIES_PER_KG_OF_ORE);
		gameObject.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[]
		{
			SimHashes.Diamond.CreateTag(),
			GameTags.Creatures.FlyersLure
		};
		return gameObject;
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x00022956 File Offset: 0x00020B56
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x00022960 File Offset: 0x00020B60
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugCrystalConfig.CreateLightBug("LightBugCrystal", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.DESC, "lightbug_kanim", false);
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, "LightBugCrystalEgg", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.EGG_NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugCrystalBaby", 45f, 15f, LightBugTuning.EGG_CHANCES_CRYSTAL, LightBugCrystalConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
		return gameObject;
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x000229DD File Offset: 0x00020BDD
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x000229DF File Offset: 0x00020BDF
	public void OnSpawn(GameObject inst)
	{
		BaseLightBugConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x04000343 RID: 835
	public const string ID = "LightBugCrystal";

	// Token: 0x04000344 RID: 836
	public const string BASE_TRAIT_ID = "LightBugCrystalBaseTrait";

	// Token: 0x04000345 RID: 837
	public const string EGG_ID = "LightBugCrystalEgg";

	// Token: 0x04000346 RID: 838
	private static float KG_ORE_EATEN_PER_CYCLE = 1f;

	// Token: 0x04000347 RID: 839
	private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugCrystalConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000348 RID: 840
	public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 7;
}
