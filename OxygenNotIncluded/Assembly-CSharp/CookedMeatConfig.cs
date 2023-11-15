using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000170 RID: 368
public class CookedMeatConfig : IEntityConfig
{
	// Token: 0x0600072E RID: 1838 RVA: 0x0002DA8C File Offset: 0x0002BC8C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x0002DA94 File Offset: 0x0002BC94
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("CookedMeat", ITEMS.FOOD.COOKEDMEAT.NAME, ITEMS.FOOD.COOKEDMEAT.DESC, 1f, false, Assets.GetAnim("barbeque_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COOKED_MEAT);
	}

	// Token: 0x06000730 RID: 1840 RVA: 0x0002DAF8 File Offset: 0x0002BCF8
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000731 RID: 1841 RVA: 0x0002DAFA File Offset: 0x0002BCFA
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004F0 RID: 1264
	public const string ID = "CookedMeat";

	// Token: 0x040004F1 RID: 1265
	public static ComplexRecipe recipe;
}
