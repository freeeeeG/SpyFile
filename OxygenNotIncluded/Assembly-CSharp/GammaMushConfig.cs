using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000177 RID: 375
public class GammaMushConfig : IEntityConfig
{
	// Token: 0x06000751 RID: 1873 RVA: 0x0002DDFB File Offset: 0x0002BFFB
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x0002DE04 File Offset: 0x0002C004
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("GammaMush", ITEMS.FOOD.GAMMAMUSH.NAME, ITEMS.FOOD.GAMMAMUSH.DESC, 1f, false, Assets.GetAnim("mushbarfried_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.GAMMAMUSH);
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x0002DE68 File Offset: 0x0002C068
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x0002DE6A File Offset: 0x0002C06A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004FB RID: 1275
	public const string ID = "GammaMush";

	// Token: 0x040004FC RID: 1276
	public static ComplexRecipe recipe;
}
