using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000175 RID: 373
public class FriedMushroomConfig : IEntityConfig
{
	// Token: 0x06000747 RID: 1863 RVA: 0x0002DCE8 File Offset: 0x0002BEE8
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000748 RID: 1864 RVA: 0x0002DCF0 File Offset: 0x0002BEF0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FriedMushroom", ITEMS.FOOD.FRIEDMUSHROOM.NAME, ITEMS.FOOD.FRIEDMUSHROOM.DESC, 1f, false, Assets.GetAnim("funguscapfried_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FRIED_MUSHROOM);
	}

	// Token: 0x06000749 RID: 1865 RVA: 0x0002DD54 File Offset: 0x0002BF54
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x0002DD56 File Offset: 0x0002BF56
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004F7 RID: 1271
	public const string ID = "FriedMushroom";

	// Token: 0x040004F8 RID: 1272
	public static ComplexRecipe recipe;
}
