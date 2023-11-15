using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000195 RID: 405
public class WormSuperFruitConfig : IEntityConfig
{
	// Token: 0x060007EE RID: 2030 RVA: 0x0002F00C File Offset: 0x0002D20C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x0002F014 File Offset: 0x0002D214
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("WormSuperFruit", ITEMS.FOOD.WORMSUPERFRUIT.NAME, ITEMS.FOOD.WORMSUPERFRUIT.DESC, 1f, false, Assets.GetAnim("wormwood_super_fruits_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.WORMSUPERFRUIT);
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x0002F078 File Offset: 0x0002D278
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x0002F07A File Offset: 0x0002D27A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000530 RID: 1328
	public const string ID = "WormSuperFruit";
}
