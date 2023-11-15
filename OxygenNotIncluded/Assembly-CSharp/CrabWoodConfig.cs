using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020000E7 RID: 231
[EntityConfigOrder(1)]
public class CrabWoodConfig : IEntityConfig
{
	// Token: 0x06000436 RID: 1078 RVA: 0x000203DC File Offset: 0x0001E5DC
	public static GameObject CreateCrabWood(string id, string name, string desc, string anim_file, bool is_baby, string deathDropID = "CrabWoodShell")
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseCrabConfig.BaseCrab(id, name, desc, anim_file, "CrabWoodBaseTrait", is_baby, CrabWoodConfig.animPrefix, deathDropID, 5), CrabTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("CrabWoodBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, CrabTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -CrabTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> diet_infos = BaseCrabConfig.DietWithSlime(SimHashes.Sand.CreateTag(), CrabWoodConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_1, null, 0f);
		return BaseCrabConfig.SetupDiet(prefab, diet_infos, CrabWoodConfig.CALORIES_PER_KG_OF_ORE, CrabWoodConfig.MIN_POOP_SIZE_IN_KG);
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x0002050B File Offset: 0x0001E70B
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x00020514 File Offset: 0x0001E714
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabWoodConfig.CreateCrabWood("CrabWood", STRINGS.CREATURES.SPECIES.CRAB.VARIANT_WOOD.NAME, STRINGS.CREATURES.SPECIES.CRAB.VARIANT_WOOD.DESC, "pincher_kanim", false, "CrabWoodShell");
		gameObject = EntityTemplates.ExtendEntityToFertileCreature(gameObject, "CrabWoodEgg", STRINGS.CREATURES.SPECIES.CRAB.VARIANT_WOOD.EGG_NAME, STRINGS.CREATURES.SPECIES.CRAB.VARIANT_WOOD.DESC, "egg_pincher_kanim", CrabTuning.EGG_MASS, "CrabWoodBaby", 60.000004f, 20f, CrabTuning.EGG_CHANCES_WOOD, CrabWoodConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
		EggProtectionMonitor.Def def = gameObject.AddOrGetDef<EggProtectionMonitor.Def>();
		def.allyTags = new Tag[]
		{
			GameTags.Creatures.CrabFriend
		};
		def.animPrefix = CrabWoodConfig.animPrefix;
		MoltDropperMonitor.Def def2 = gameObject.AddOrGetDef<MoltDropperMonitor.Def>();
		def2.onGrowDropID = "CrabWoodShell";
		def2.massToDrop = 100f;
		def2.blockedElement = SimHashes.Ethanol;
		return gameObject;
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x000205E8 File Offset: 0x0001E7E8
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x000205EA File Offset: 0x0001E7EA
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040002C3 RID: 707
	public const string ID = "CrabWood";

	// Token: 0x040002C4 RID: 708
	public const string BASE_TRAIT_ID = "CrabWoodBaseTrait";

	// Token: 0x040002C5 RID: 709
	public const string EGG_ID = "CrabWoodEgg";

	// Token: 0x040002C6 RID: 710
	private const SimHashes EMIT_ELEMENT = SimHashes.Sand;

	// Token: 0x040002C7 RID: 711
	private static float KG_ORE_EATEN_PER_CYCLE = 70f;

	// Token: 0x040002C8 RID: 712
	private static float CALORIES_PER_KG_OF_ORE = CrabTuning.STANDARD_CALORIES_PER_CYCLE / CrabWoodConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040002C9 RID: 713
	private static float MIN_POOP_SIZE_IN_KG = 25f;

	// Token: 0x040002CA RID: 714
	public static int EGG_SORT_ORDER = 0;

	// Token: 0x040002CB RID: 715
	private static string animPrefix = "wood_";
}
