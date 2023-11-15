using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000120 RID: 288
public class PuftOxyliteConfig : IEntityConfig
{
	// Token: 0x0600058A RID: 1418 RVA: 0x00024F7C File Offset: 0x0002317C
	public static GameObject CreatePuftOxylite(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BasePuftConfig.BasePuft(id, name, desc, "PuftOxyliteBaseTrait", anim_file, is_baby, "com_", 303.15f, 338.15f);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, PuftTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("PuftOxyliteBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PuftTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -PuftTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		gameObject = BasePuftConfig.SetupDiet(gameObject, SimHashes.Oxygen.CreateTag(), SimHashes.OxyRock.CreateTag(), PuftOxyliteConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_2, null, 0f, PuftOxyliteConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[]
		{
			SimHashes.OxyRock.CreateTag(),
			GameTags.Creatures.FlyersLure
		};
		return gameObject;
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x000250E4 File Offset: 0x000232E4
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x000250EC File Offset: 0x000232EC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(PuftOxyliteConfig.CreatePuftOxylite("PuftOxylite", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.DESC, "puft_kanim", false), "PuftOxyliteEgg", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.EGG_NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.DESC, "egg_puft_kanim", PuftTuning.EGG_MASS, "PuftOxyliteBaby", 45f, 15f, PuftTuning.EGG_CHANCES_OXYLITE, PuftOxyliteConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x0600058D RID: 1421 RVA: 0x00025167 File Offset: 0x00023367
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x00025169 File Offset: 0x00023369
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x040003CF RID: 975
	public const string ID = "PuftOxylite";

	// Token: 0x040003D0 RID: 976
	public const string BASE_TRAIT_ID = "PuftOxyliteBaseTrait";

	// Token: 0x040003D1 RID: 977
	public const string EGG_ID = "PuftOxyliteEgg";

	// Token: 0x040003D2 RID: 978
	public const SimHashes CONSUME_ELEMENT = SimHashes.Oxygen;

	// Token: 0x040003D3 RID: 979
	public const SimHashes EMIT_ELEMENT = SimHashes.OxyRock;

	// Token: 0x040003D4 RID: 980
	private static float KG_ORE_EATEN_PER_CYCLE = 50f;

	// Token: 0x040003D5 RID: 981
	private static float CALORIES_PER_KG_OF_ORE = PuftTuning.STANDARD_CALORIES_PER_CYCLE / PuftOxyliteConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040003D6 RID: 982
	private static float MIN_POOP_SIZE_IN_KG = 25f;

	// Token: 0x040003D7 RID: 983
	public static int EGG_SORT_ORDER = PuftConfig.EGG_SORT_ORDER + 2;
}
