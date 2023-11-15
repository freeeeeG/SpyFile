using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000116 RID: 278
[EntityConfigOrder(1)]
public class PacuConfig : IEntityConfig
{
	// Token: 0x0600054F RID: 1359 RVA: 0x000244D0 File Offset: 0x000226D0
	public static GameObject CreatePacu(string id, string name, string desc, string anim_file, bool is_baby)
	{
		return EntityTemplates.ExtendEntityToWildCreature(BasePacuConfig.CreatePrefab(id, "PacuBaseTrait", name, desc, anim_file, is_baby, null, 273.15f, 333.15f), PacuTuning.PEN_SIZE_PER_CREATURE);
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x00024502 File Offset: 0x00022702
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x0002450C File Offset: 0x0002270C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToFertileCreature(PacuConfig.CreatePacu("Pacu", CREATURES.SPECIES.PACU.NAME, CREATURES.SPECIES.PACU.DESC, "pacu_kanim", false), "PacuEgg", CREATURES.SPECIES.PACU.EGG_NAME, CREATURES.SPECIES.PACU.DESC, "egg_pacu_kanim", PacuTuning.EGG_MASS, "PacuBaby", 15.000001f, 5f, PacuTuning.EGG_CHANCES_BASE, 500, false, true, false, 0.75f, false);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x00024592 File Offset: 0x00022792
	public void OnPrefabInit(GameObject prefab)
	{
		prefab.AddOrGet<LoopingSounds>();
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x0002459B File Offset: 0x0002279B
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003A2 RID: 930
	public const string ID = "Pacu";

	// Token: 0x040003A3 RID: 931
	public const string BASE_TRAIT_ID = "PacuBaseTrait";

	// Token: 0x040003A4 RID: 932
	public const string EGG_ID = "PacuEgg";

	// Token: 0x040003A5 RID: 933
	public const int EGG_SORT_ORDER = 500;
}
