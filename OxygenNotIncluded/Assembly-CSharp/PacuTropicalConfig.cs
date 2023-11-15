using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000118 RID: 280
[EntityConfigOrder(1)]
public class PacuTropicalConfig : IEntityConfig
{
	// Token: 0x0600055A RID: 1370 RVA: 0x000245F8 File Offset: 0x000227F8
	public static GameObject CreatePacu(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToWildCreature(BasePacuConfig.CreatePrefab(id, "PacuTropicalBaseTrait", name, desc, anim_file, is_baby, "trp_", 303.15f, 353.15f), PacuTuning.PEN_SIZE_PER_CREATURE);
		gameObject.AddOrGet<DecorProvider>().SetValues(PacuTropicalConfig.DECOR);
		return gameObject;
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x0002463E File Offset: 0x0002283E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x00024648 File Offset: 0x00022848
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(EntityTemplates.ExtendEntityToWildCreature(PacuTropicalConfig.CreatePacu("PacuTropical", STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.NAME, STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.DESC, "pacu_kanim", false), PacuTuning.PEN_SIZE_PER_CREATURE), "PacuTropicalEgg", STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.EGG_NAME, STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.DESC, "egg_pacu_kanim", PacuTuning.EGG_MASS, "PacuTropicalBaby", 15.000001f, 5f, PacuTuning.EGG_CHANCES_TROPICAL, 502, false, true, false, 0.75f, false);
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x000246CD File Offset: 0x000228CD
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x000246CF File Offset: 0x000228CF
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003A7 RID: 935
	public const string ID = "PacuTropical";

	// Token: 0x040003A8 RID: 936
	public const string BASE_TRAIT_ID = "PacuTropicalBaseTrait";

	// Token: 0x040003A9 RID: 937
	public const string EGG_ID = "PacuTropicalEgg";

	// Token: 0x040003AA RID: 938
	public static readonly EffectorValues DECOR = TUNING.BUILDINGS.DECOR.BONUS.TIER4;

	// Token: 0x040003AB RID: 939
	public const int EGG_SORT_ORDER = 502;
}
