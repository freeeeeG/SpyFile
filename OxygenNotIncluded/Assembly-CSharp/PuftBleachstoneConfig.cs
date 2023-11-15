using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200011C RID: 284
public class PuftBleachstoneConfig : IEntityConfig
{
	// Token: 0x06000572 RID: 1394 RVA: 0x00024A80 File Offset: 0x00022C80
	public static GameObject CreatePuftBleachstone(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BasePuftConfig.BasePuft(id, name, desc, "PuftBleachstoneBaseTrait", anim_file, is_baby, "anti_", 258.15f, 308.15f);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, PuftTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("PuftBleachstoneBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PuftTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -PuftTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		gameObject = BasePuftConfig.SetupDiet(gameObject, SimHashes.ChlorineGas.CreateTag(), SimHashes.BleachStone.CreateTag(), PuftBleachstoneConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_2, null, 0f, PuftBleachstoneConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[]
		{
			SimHashes.BleachStone.CreateTag(),
			GameTags.Creatures.FlyersLure
		};
		return gameObject;
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x00024BE8 File Offset: 0x00022DE8
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x00024BF0 File Offset: 0x00022DF0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(PuftBleachstoneConfig.CreatePuftBleachstone("PuftBleachstone", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.DESC, "puft_kanim", false), "PuftBleachstoneEgg", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.EGG_NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.DESC, "egg_puft_kanim", PuftTuning.EGG_MASS, "PuftBleachstoneBaby", 45f, 15f, PuftTuning.EGG_CHANCES_BLEACHSTONE, PuftBleachstoneConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x00024C6B File Offset: 0x00022E6B
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x00024C6D File Offset: 0x00022E6D
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x040003B9 RID: 953
	public const string ID = "PuftBleachstone";

	// Token: 0x040003BA RID: 954
	public const string BASE_TRAIT_ID = "PuftBleachstoneBaseTrait";

	// Token: 0x040003BB RID: 955
	public const string EGG_ID = "PuftBleachstoneEgg";

	// Token: 0x040003BC RID: 956
	public const SimHashes CONSUME_ELEMENT = SimHashes.ChlorineGas;

	// Token: 0x040003BD RID: 957
	public const SimHashes EMIT_ELEMENT = SimHashes.BleachStone;

	// Token: 0x040003BE RID: 958
	private static float KG_ORE_EATEN_PER_CYCLE = 30f;

	// Token: 0x040003BF RID: 959
	private static float CALORIES_PER_KG_OF_ORE = PuftTuning.STANDARD_CALORIES_PER_CYCLE / PuftBleachstoneConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040003C0 RID: 960
	private static float MIN_POOP_SIZE_IN_KG = 15f;

	// Token: 0x040003C1 RID: 961
	public static int EGG_SORT_ORDER = PuftConfig.EGG_SORT_ORDER + 3;
}
