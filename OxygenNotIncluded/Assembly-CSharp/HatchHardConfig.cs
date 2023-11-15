using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020000F4 RID: 244
[EntityConfigOrder(1)]
public class HatchHardConfig : IEntityConfig
{
	// Token: 0x06000483 RID: 1155 RVA: 0x00021860 File Offset: 0x0001FA60
	public static GameObject CreateHatch(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseHatchConfig.BaseHatch(id, name, desc, anim_file, "HatchHardBaseTrait", is_baby, "hvy_"), HatchTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("HatchHardBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, HatchTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -HatchTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 200f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> list = BaseHatchConfig.HardRockDiet(SimHashes.Carbon.CreateTag(), HatchHardConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f);
		list.AddRange(BaseHatchConfig.MetalDiet(SimHashes.Carbon.CreateTag(), HatchHardConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_1, null, 0f));
		return BaseHatchConfig.SetupDiet(prefab, list, HatchHardConfig.CALORIES_PER_KG_OF_ORE, HatchHardConfig.MIN_POOP_SIZE_IN_KG);
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x000219B1 File Offset: 0x0001FBB1
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x000219B8 File Offset: 0x0001FBB8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(HatchHardConfig.CreateHatch("HatchHard", STRINGS.CREATURES.SPECIES.HATCH.VARIANT_HARD.NAME, STRINGS.CREATURES.SPECIES.HATCH.VARIANT_HARD.DESC, "hatch_kanim", false), "HatchHardEgg", STRINGS.CREATURES.SPECIES.HATCH.VARIANT_HARD.EGG_NAME, STRINGS.CREATURES.SPECIES.HATCH.VARIANT_HARD.DESC, "egg_hatch_kanim", HatchTuning.EGG_MASS, "HatchHardBaby", 60.000004f, 20f, HatchTuning.EGG_CHANCES_HARD, HatchHardConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x00021A33 File Offset: 0x0001FC33
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x00021A35 File Offset: 0x0001FC35
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000314 RID: 788
	public const string ID = "HatchHard";

	// Token: 0x04000315 RID: 789
	public const string BASE_TRAIT_ID = "HatchHardBaseTrait";

	// Token: 0x04000316 RID: 790
	public const string EGG_ID = "HatchHardEgg";

	// Token: 0x04000317 RID: 791
	private const SimHashes EMIT_ELEMENT = SimHashes.Carbon;

	// Token: 0x04000318 RID: 792
	private static float KG_ORE_EATEN_PER_CYCLE = 140f;

	// Token: 0x04000319 RID: 793
	private static float CALORIES_PER_KG_OF_ORE = HatchTuning.STANDARD_CALORIES_PER_CYCLE / HatchHardConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x0400031A RID: 794
	private static float MIN_POOP_SIZE_IN_KG = 25f;

	// Token: 0x0400031B RID: 795
	public static int EGG_SORT_ORDER = HatchConfig.EGG_SORT_ORDER + 2;
}
