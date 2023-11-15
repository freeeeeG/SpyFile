using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200011A RID: 282
public class PuftAlphaConfig : IEntityConfig
{
	// Token: 0x06000566 RID: 1382 RVA: 0x00024738 File Offset: 0x00022938
	public static GameObject CreatePuftAlpha(string id, string name, string desc, string anim_file, bool is_baby)
	{
		string symbol_override_prefix = "alp_";
		GameObject gameObject = BasePuftConfig.BasePuft(id, name, desc, "PuftAlphaBaseTrait", anim_file, is_baby, symbol_override_prefix, 258.15f, 338.15f);
		EntityTemplates.ExtendEntityToWildCreature(gameObject, PuftTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("PuftAlphaBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PuftTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -PuftTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		gameObject = BasePuftConfig.SetupDiet(gameObject, new List<Diet.Info>
		{
			new Diet.Info(new HashSet<Tag>(new Tag[]
			{
				SimHashes.ContaminatedOxygen.CreateTag()
			}), SimHashes.SlimeMold.CreateTag(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, "SlimeLung", 1000f, false, false),
			new Diet.Info(new HashSet<Tag>(new Tag[]
			{
				SimHashes.ChlorineGas.CreateTag()
			}), SimHashes.BleachStone.CreateTag(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, "SlimeLung", 1000f, false, false),
			new Diet.Info(new HashSet<Tag>(new Tag[]
			{
				SimHashes.Oxygen.CreateTag()
			}), SimHashes.OxyRock.CreateTag(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, "SlimeLung", 1000f, false, false)
		}.ToArray(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, PuftAlphaConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = "SlimeLung";
		return gameObject;
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x00024946 File Offset: 0x00022B46
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x00024950 File Offset: 0x00022B50
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(PuftAlphaConfig.CreatePuftAlpha("PuftAlpha", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.DESC, "puft_kanim", false), "PuftAlphaEgg", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.EGG_NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.DESC, "egg_puft_kanim", PuftTuning.EGG_MASS, "PuftAlphaBaby", 45f, 15f, PuftTuning.EGG_CHANCES_ALPHA, PuftAlphaConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x000249CB File Offset: 0x00022BCB
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<KBatchedAnimController>().animScale *= 1.1f;
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x000249E4 File Offset: 0x00022BE4
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x040003AD RID: 941
	public const string ID = "PuftAlpha";

	// Token: 0x040003AE RID: 942
	public const string BASE_TRAIT_ID = "PuftAlphaBaseTrait";

	// Token: 0x040003AF RID: 943
	public const string EGG_ID = "PuftAlphaEgg";

	// Token: 0x040003B0 RID: 944
	public const SimHashes CONSUME_ELEMENT = SimHashes.ContaminatedOxygen;

	// Token: 0x040003B1 RID: 945
	public const SimHashes EMIT_ELEMENT = SimHashes.SlimeMold;

	// Token: 0x040003B2 RID: 946
	public const string EMIT_DISEASE = "SlimeLung";

	// Token: 0x040003B3 RID: 947
	public const float EMIT_DISEASE_PER_KG = 1000f;

	// Token: 0x040003B4 RID: 948
	private static float KG_ORE_EATEN_PER_CYCLE = 30f;

	// Token: 0x040003B5 RID: 949
	private static float CALORIES_PER_KG_OF_ORE = PuftTuning.STANDARD_CALORIES_PER_CYCLE / PuftAlphaConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040003B6 RID: 950
	private static float MIN_POOP_SIZE_IN_KG = 5f;

	// Token: 0x040003B7 RID: 951
	public static int EGG_SORT_ORDER = PuftConfig.EGG_SORT_ORDER + 1;
}
