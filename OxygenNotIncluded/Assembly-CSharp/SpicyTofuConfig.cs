using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200018A RID: 394
public class SpicyTofuConfig : IEntityConfig
{
	// Token: 0x060007B7 RID: 1975 RVA: 0x0002EAAE File Offset: 0x0002CCAE
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x0002EAB8 File Offset: 0x0002CCB8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SpicyTofu", ITEMS.FOOD.SPICYTOFU.NAME, ITEMS.FOOD.SPICYTOFU.DESC, 1f, false, Assets.GetAnim("spicey_tofu_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SPICY_TOFU);
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x0002EB1C File Offset: 0x0002CD1C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x0002EB1E File Offset: 0x0002CD1E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400051C RID: 1308
	public const string ID = "SpicyTofu";

	// Token: 0x0400051D RID: 1309
	public static ComplexRecipe recipe;
}
