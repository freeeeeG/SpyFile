using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020000F2 RID: 242
[EntityConfigOrder(1)]
public class HatchConfig : IEntityConfig
{
	// Token: 0x06000477 RID: 1143 RVA: 0x000215FC File Offset: 0x0001F7FC
	public static GameObject CreateHatch(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseHatchConfig.BaseHatch(id, name, desc, anim_file, "HatchBaseTrait", is_baby, null), HatchTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("HatchBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, HatchTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -HatchTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> list = BaseHatchConfig.BasicRockDiet(SimHashes.Carbon.CreateTag(), HatchConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f);
		list.AddRange(BaseHatchConfig.FoodDiet(SimHashes.Carbon.CreateTag(), HatchConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_1, null, 0f));
		GameObject gameObject = BaseHatchConfig.SetupDiet(prefab, list, HatchConfig.CALORIES_PER_KG_OF_ORE, HatchConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x00021754 File Offset: 0x0001F954
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x0002175C File Offset: 0x0001F95C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(HatchConfig.CreateHatch("Hatch", STRINGS.CREATURES.SPECIES.HATCH.NAME, STRINGS.CREATURES.SPECIES.HATCH.DESC, "hatch_kanim", false), "HatchEgg", STRINGS.CREATURES.SPECIES.HATCH.EGG_NAME, STRINGS.CREATURES.SPECIES.HATCH.DESC, "egg_hatch_kanim", HatchTuning.EGG_MASS, "HatchBaby", 60.000004f, 20f, HatchTuning.EGG_CHANCES_BASE, HatchConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x000217D7 File Offset: 0x0001F9D7
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x000217D9 File Offset: 0x0001F9D9
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400030B RID: 779
	public const string ID = "Hatch";

	// Token: 0x0400030C RID: 780
	public const string BASE_TRAIT_ID = "HatchBaseTrait";

	// Token: 0x0400030D RID: 781
	public const string EGG_ID = "HatchEgg";

	// Token: 0x0400030E RID: 782
	private const SimHashes EMIT_ELEMENT = SimHashes.Carbon;

	// Token: 0x0400030F RID: 783
	private static float KG_ORE_EATEN_PER_CYCLE = 140f;

	// Token: 0x04000310 RID: 784
	private static float CALORIES_PER_KG_OF_ORE = HatchTuning.STANDARD_CALORIES_PER_CYCLE / HatchConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000311 RID: 785
	private static float MIN_POOP_SIZE_IN_KG = 25f;

	// Token: 0x04000312 RID: 786
	public static int EGG_SORT_ORDER = 0;
}
