using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200016C RID: 364
public class BurgerConfig : IEntityConfig
{
	// Token: 0x0600071A RID: 1818 RVA: 0x0002D8AC File Offset: 0x0002BAAC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x0002D8B4 File Offset: 0x0002BAB4
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Burger", ITEMS.FOOD.BURGER.NAME, ITEMS.FOOD.BURGER.DESC, 1f, false, Assets.GetAnim("frost_burger_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.BURGER);
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x0002D918 File Offset: 0x0002BB18
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x0002D91A File Offset: 0x0002BB1A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004E8 RID: 1256
	public const string ID = "Burger";

	// Token: 0x040004E9 RID: 1257
	public static ComplexRecipe recipe;
}
