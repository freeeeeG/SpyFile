using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200018D RID: 397
public class SwampFruitConfig : IEntityConfig
{
	// Token: 0x060007C6 RID: 1990 RVA: 0x0002EC18 File Offset: 0x0002CE18
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x0002EC20 File Offset: 0x0002CE20
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity(SwampFruitConfig.ID, ITEMS.FOOD.SWAMPFRUIT.NAME, ITEMS.FOOD.SWAMPFRUIT.DESC, 1f, false, Assets.GetAnim("swampcrop_fruit_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 1f, 0.72f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SWAMPFRUIT);
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x0002EC84 File Offset: 0x0002CE84
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x0002EC86 File Offset: 0x0002CE86
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000521 RID: 1313
	public static string ID = "SwampFruit";
}
