using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200017E RID: 382
public class PacuFilletConfig : IEntityConfig
{
	// Token: 0x06000777 RID: 1911 RVA: 0x0002E314 File Offset: 0x0002C514
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x0002E31C File Offset: 0x0002C51C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("PacuFillet", ITEMS.FOOD.MEAT.NAME, ITEMS.FOOD.MEAT.DESC, 1f, false, Assets.GetAnim("pacufillet_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.FISH_MEAT);
		return gameObject;
	}

	// Token: 0x06000779 RID: 1913 RVA: 0x0002E382 File Offset: 0x0002C582
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x0002E384 File Offset: 0x0002C584
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000508 RID: 1288
	public const string ID = "PacuFillet";
}
