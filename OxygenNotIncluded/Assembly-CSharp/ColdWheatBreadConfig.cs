using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200016D RID: 365
public class ColdWheatBreadConfig : IEntityConfig
{
	// Token: 0x0600071F RID: 1823 RVA: 0x0002D924 File Offset: 0x0002BB24
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x0002D92C File Offset: 0x0002BB2C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("ColdWheatBread", ITEMS.FOOD.COLDWHEATBREAD.NAME, ITEMS.FOOD.COLDWHEATBREAD.DESC, 1f, false, Assets.GetAnim("frostbread_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COLD_WHEAT_BREAD);
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x0002D990 File Offset: 0x0002BB90
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x0002D992 File Offset: 0x0002BB92
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004EA RID: 1258
	public const string ID = "ColdWheatBread";

	// Token: 0x040004EB RID: 1259
	public static ComplexRecipe recipe;
}
