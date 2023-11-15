using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000188 RID: 392
public class SpiceBreadConfig : IEntityConfig
{
	// Token: 0x060007AC RID: 1964 RVA: 0x0002E972 File Offset: 0x0002CB72
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060007AD RID: 1965 RVA: 0x0002E97C File Offset: 0x0002CB7C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SpiceBread", ITEMS.FOOD.SPICEBREAD.NAME, ITEMS.FOOD.SPICEBREAD.DESC, 1f, false, Assets.GetAnim("pepperbread_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SPICEBREAD);
	}

	// Token: 0x060007AE RID: 1966 RVA: 0x0002E9E0 File Offset: 0x0002CBE0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x0002E9E2 File Offset: 0x0002CBE2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000518 RID: 1304
	public const string ID = "SpiceBread";

	// Token: 0x04000519 RID: 1305
	public static ComplexRecipe recipe;
}
