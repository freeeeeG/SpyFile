using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200016B RID: 363
public class BerryPieConfig : IEntityConfig
{
	// Token: 0x06000715 RID: 1813 RVA: 0x0002D832 File Offset: 0x0002BA32
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x0002D83C File Offset: 0x0002BA3C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("BerryPie", ITEMS.FOOD.BERRYPIE.NAME, ITEMS.FOOD.BERRYPIE.DESC, 1f, false, Assets.GetAnim("wormwood_berry_pie_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.55f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.BERRY_PIE);
	}

	// Token: 0x06000717 RID: 1815 RVA: 0x0002D8A0 File Offset: 0x0002BAA0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000718 RID: 1816 RVA: 0x0002D8A2 File Offset: 0x0002BAA2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004E6 RID: 1254
	public const string ID = "BerryPie";

	// Token: 0x040004E7 RID: 1255
	public static ComplexRecipe recipe;
}
