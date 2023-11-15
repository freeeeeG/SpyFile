using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200011E RID: 286
public class PuftConfig : IEntityConfig
{
	// Token: 0x0600057E RID: 1406 RVA: 0x00024D08 File Offset: 0x00022F08
	public static GameObject CreatePuft(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BasePuftConfig.BasePuft(id, name, STRINGS.CREATURES.SPECIES.PUFT.DESC, "PuftBaseTrait", anim_file, is_baby, null, 288.15f, 328.15f);
		EntityTemplates.ExtendEntityToWildCreature(prefab, PuftTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("PuftBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PuftTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -PuftTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		GameObject gameObject = BasePuftConfig.SetupDiet(prefab, SimHashes.ContaminatedOxygen.CreateTag(), SimHashes.SlimeMold.CreateTag(), PuftConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_2, "SlimeLung", 1000f, PuftConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = "SlimeLung";
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x00024E62 File Offset: 0x00023062
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x00024E6C File Offset: 0x0002306C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(PuftConfig.CreatePuft("Puft", STRINGS.CREATURES.SPECIES.PUFT.NAME, STRINGS.CREATURES.SPECIES.PUFT.DESC, "puft_kanim", false), "PuftEgg", STRINGS.CREATURES.SPECIES.PUFT.EGG_NAME, STRINGS.CREATURES.SPECIES.PUFT.DESC, "egg_puft_kanim", PuftTuning.EGG_MASS, "PuftBaby", 45f, 15f, PuftTuning.EGG_CHANCES_BASE, PuftConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x00024EE7 File Offset: 0x000230E7
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x00024EE9 File Offset: 0x000230E9
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x040003C3 RID: 963
	public const string ID = "Puft";

	// Token: 0x040003C4 RID: 964
	public const string BASE_TRAIT_ID = "PuftBaseTrait";

	// Token: 0x040003C5 RID: 965
	public const string EGG_ID = "PuftEgg";

	// Token: 0x040003C6 RID: 966
	public const SimHashes CONSUME_ELEMENT = SimHashes.ContaminatedOxygen;

	// Token: 0x040003C7 RID: 967
	public const SimHashes EMIT_ELEMENT = SimHashes.SlimeMold;

	// Token: 0x040003C8 RID: 968
	public const string EMIT_DISEASE = "SlimeLung";

	// Token: 0x040003C9 RID: 969
	public const float EMIT_DISEASE_PER_KG = 1000f;

	// Token: 0x040003CA RID: 970
	private static float KG_ORE_EATEN_PER_CYCLE = 50f;

	// Token: 0x040003CB RID: 971
	private static float CALORIES_PER_KG_OF_ORE = PuftTuning.STANDARD_CALORIES_PER_CYCLE / PuftConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040003CC RID: 972
	private static float MIN_POOP_SIZE_IN_KG = 15f;

	// Token: 0x040003CD RID: 973
	public static int EGG_SORT_ORDER = 300;
}
