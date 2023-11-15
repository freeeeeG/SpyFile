using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000183 RID: 387
public class QuicheConfig : IEntityConfig
{
	// Token: 0x06000792 RID: 1938 RVA: 0x0002E69B File Offset: 0x0002C89B
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x0002E6A4 File Offset: 0x0002C8A4
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Quiche", ITEMS.FOOD.QUICHE.NAME, ITEMS.FOOD.QUICHE.DESC, 1f, false, Assets.GetAnim("quiche_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.QUICHE);
	}

	// Token: 0x06000794 RID: 1940 RVA: 0x0002E708 File Offset: 0x0002C908
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000795 RID: 1941 RVA: 0x0002E70A File Offset: 0x0002C90A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000511 RID: 1297
	public const string ID = "Quiche";

	// Token: 0x04000512 RID: 1298
	public static ComplexRecipe recipe;
}
