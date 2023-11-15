using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200010D RID: 269
public class OilFloaterConfig : IEntityConfig
{
	// Token: 0x0600051D RID: 1309 RVA: 0x00023B4C File Offset: 0x00021D4C
	public static GameObject CreateOilFloater(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BaseOilFloaterConfig.BaseOilFloater(id, name, desc, anim_file, "OilfloaterBaseTrait", 323.15f, 413.15f, is_baby, null);
		EntityTemplates.ExtendEntityToWildCreature(prefab, OilFloaterTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("OilfloaterBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, OilFloaterTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		GameObject gameObject = BaseOilFloaterConfig.SetupDiet(prefab, SimHashes.CarbonDioxide.CreateTag(), SimHashes.CrudeOil.CreateTag(), OilFloaterConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f, OilFloaterConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x00023C89 File Offset: 0x00021E89
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x00023C90 File Offset: 0x00021E90
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterConfig.CreateOilFloater("Oilfloater", STRINGS.CREATURES.SPECIES.OILFLOATER.NAME, STRINGS.CREATURES.SPECIES.OILFLOATER.DESC, "oilfloater_kanim", false);
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, "OilfloaterEgg", STRINGS.CREATURES.SPECIES.OILFLOATER.EGG_NAME, STRINGS.CREATURES.SPECIES.OILFLOATER.DESC, "egg_oilfloater_kanim", OilFloaterTuning.EGG_MASS, "OilfloaterBaby", 60.000004f, 20f, OilFloaterTuning.EGG_CHANCES_BASE, OilFloaterConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
		return gameObject;
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x00023D0D File Offset: 0x00021F0D
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x00023D0F File Offset: 0x00021F0F
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400037D RID: 893
	public const string ID = "Oilfloater";

	// Token: 0x0400037E RID: 894
	public const string BASE_TRAIT_ID = "OilfloaterBaseTrait";

	// Token: 0x0400037F RID: 895
	public const string EGG_ID = "OilfloaterEgg";

	// Token: 0x04000380 RID: 896
	public const SimHashes CONSUME_ELEMENT = SimHashes.CarbonDioxide;

	// Token: 0x04000381 RID: 897
	public const SimHashes EMIT_ELEMENT = SimHashes.CrudeOil;

	// Token: 0x04000382 RID: 898
	private static float KG_ORE_EATEN_PER_CYCLE = 20f;

	// Token: 0x04000383 RID: 899
	private static float CALORIES_PER_KG_OF_ORE = OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / OilFloaterConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000384 RID: 900
	private static float MIN_POOP_SIZE_IN_KG = 0.5f;

	// Token: 0x04000385 RID: 901
	public static int EGG_SORT_ORDER = 400;
}
