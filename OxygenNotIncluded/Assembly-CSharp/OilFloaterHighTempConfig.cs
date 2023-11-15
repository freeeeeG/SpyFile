using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000111 RID: 273
public class OilFloaterHighTempConfig : IEntityConfig
{
	// Token: 0x06000535 RID: 1333 RVA: 0x00023FE8 File Offset: 0x000221E8
	public static GameObject CreateOilFloater(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BaseOilFloaterConfig.BaseOilFloater(id, name, desc, anim_file, "OilfloaterHighTempBaseTrait", 363.15f, 523.15f, is_baby, "hot_");
		EntityTemplates.ExtendEntityToWildCreature(prefab, OilFloaterTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("OilfloaterHighTempBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, OilFloaterTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		return BaseOilFloaterConfig.SetupDiet(prefab, SimHashes.CarbonDioxide.CreateTag(), SimHashes.Petroleum.CreateTag(), OilFloaterHighTempConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f, OilFloaterHighTempConfig.MIN_POOP_SIZE_IN_KG);
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x0002411E File Offset: 0x0002231E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x00024128 File Offset: 0x00022328
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterHighTempConfig.CreateOilFloater("OilfloaterHighTemp", STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.NAME, STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.DESC, "oilfloater_kanim", false);
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, "OilfloaterHighTempEgg", STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.EGG_NAME, STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.DESC, "egg_oilfloater_kanim", OilFloaterTuning.EGG_MASS, "OilfloaterHighTempBaby", 60.000004f, 20f, OilFloaterTuning.EGG_CHANCES_HIGHTEMP, OilFloaterHighTempConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
		return gameObject;
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x000241A5 File Offset: 0x000223A5
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x000241A7 File Offset: 0x000223A7
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400038F RID: 911
	public const string ID = "OilfloaterHighTemp";

	// Token: 0x04000390 RID: 912
	public const string BASE_TRAIT_ID = "OilfloaterHighTempBaseTrait";

	// Token: 0x04000391 RID: 913
	public const string EGG_ID = "OilfloaterHighTempEgg";

	// Token: 0x04000392 RID: 914
	public const SimHashes CONSUME_ELEMENT = SimHashes.CarbonDioxide;

	// Token: 0x04000393 RID: 915
	public const SimHashes EMIT_ELEMENT = SimHashes.Petroleum;

	// Token: 0x04000394 RID: 916
	private static float KG_ORE_EATEN_PER_CYCLE = 20f;

	// Token: 0x04000395 RID: 917
	private static float CALORIES_PER_KG_OF_ORE = OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / OilFloaterHighTempConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000396 RID: 918
	private static float MIN_POOP_SIZE_IN_KG = 0.5f;

	// Token: 0x04000397 RID: 919
	public static int EGG_SORT_ORDER = OilFloaterConfig.EGG_SORT_ORDER + 1;
}
