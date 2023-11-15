using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000280 RID: 640
public class GeneShufflerRechargeConfig : IEntityConfig
{
	// Token: 0x06000CF2 RID: 3314 RVA: 0x00047C38 File Offset: 0x00045E38
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000CF3 RID: 3315 RVA: 0x00047C40 File Offset: 0x00045E40
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateLooseEntity("GeneShufflerRecharge", ITEMS.INDUSTRIAL_PRODUCTS.GENE_SHUFFLER_RECHARGE.NAME, ITEMS.INDUSTRIAL_PRODUCTS.GENE_SHUFFLER_RECHARGE.DESC, 5f, true, Assets.GetAnim("vacillator_charge_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient
		});
	}

	// Token: 0x06000CF4 RID: 3316 RVA: 0x00047CA9 File Offset: 0x00045EA9
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CF5 RID: 3317 RVA: 0x00047CAB File Offset: 0x00045EAB
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000776 RID: 1910
	public const string ID = "GeneShufflerRecharge";

	// Token: 0x04000777 RID: 1911
	public static readonly Tag tag = TagManager.Create("GeneShufflerRecharge");

	// Token: 0x04000778 RID: 1912
	public const float MASS = 5f;
}
