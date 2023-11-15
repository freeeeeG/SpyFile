using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020000F6 RID: 246
[EntityConfigOrder(1)]
public class HatchMetalConfig : IEntityConfig
{
	// Token: 0x17000010 RID: 16
	// (get) Token: 0x0600048F RID: 1167 RVA: 0x00021AC4 File Offset: 0x0001FCC4
	public static HashSet<Tag> METAL_ORE_TAGS
	{
		get
		{
			HashSet<Tag> hashSet = new HashSet<Tag>
			{
				SimHashes.Cuprite.CreateTag(),
				SimHashes.GoldAmalgam.CreateTag(),
				SimHashes.IronOre.CreateTag(),
				SimHashes.Wolframite.CreateTag(),
				SimHashes.AluminumOre.CreateTag()
			};
			if (DlcManager.IsExpansion1Active())
			{
				hashSet.Add(SimHashes.Cobaltite.CreateTag());
			}
			return hashSet;
		}
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x00021B48 File Offset: 0x0001FD48
	public static GameObject CreateHatch(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseHatchConfig.BaseHatch(id, name, desc, anim_file, "HatchMetalBaseTrait", is_baby, "mtl_"), HatchTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("HatchMetalBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, HatchTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -HatchTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 400f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> diet_infos = BaseHatchConfig.MetalDiet(GameTags.Metal, HatchMetalConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_1, null, 0f);
		return BaseHatchConfig.SetupDiet(prefab, diet_infos, HatchMetalConfig.CALORIES_PER_KG_OF_ORE, HatchMetalConfig.MIN_POOP_SIZE_IN_KG);
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x00021C6F File Offset: 0x0001FE6F
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x00021C78 File Offset: 0x0001FE78
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(HatchMetalConfig.CreateHatch("HatchMetal", STRINGS.CREATURES.SPECIES.HATCH.VARIANT_METAL.NAME, STRINGS.CREATURES.SPECIES.HATCH.VARIANT_METAL.DESC, "hatch_kanim", false), "HatchMetalEgg", STRINGS.CREATURES.SPECIES.HATCH.VARIANT_METAL.EGG_NAME, STRINGS.CREATURES.SPECIES.HATCH.VARIANT_METAL.DESC, "egg_hatch_kanim", HatchTuning.EGG_MASS, "HatchMetalBaby", 60.000004f, 20f, HatchTuning.EGG_CHANCES_METAL, HatchMetalConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x00021CF3 File Offset: 0x0001FEF3
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x00021CF5 File Offset: 0x0001FEF5
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400031D RID: 797
	public const string ID = "HatchMetal";

	// Token: 0x0400031E RID: 798
	public const string BASE_TRAIT_ID = "HatchMetalBaseTrait";

	// Token: 0x0400031F RID: 799
	public const string EGG_ID = "HatchMetalEgg";

	// Token: 0x04000320 RID: 800
	private static float KG_ORE_EATEN_PER_CYCLE = 100f;

	// Token: 0x04000321 RID: 801
	private static float CALORIES_PER_KG_OF_ORE = HatchTuning.STANDARD_CALORIES_PER_CYCLE / HatchMetalConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000322 RID: 802
	private static float MIN_POOP_SIZE_IN_KG = 10f;

	// Token: 0x04000323 RID: 803
	public static int EGG_SORT_ORDER = HatchConfig.EGG_SORT_ORDER + 3;
}
