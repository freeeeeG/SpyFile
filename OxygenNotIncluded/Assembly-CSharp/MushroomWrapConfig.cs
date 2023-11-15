using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200017D RID: 381
public class MushroomWrapConfig : IEntityConfig
{
	// Token: 0x06000772 RID: 1906 RVA: 0x0002E29B File Offset: 0x0002C49B
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x0002E2A4 File Offset: 0x0002C4A4
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("MushroomWrap", ITEMS.FOOD.MUSHROOMWRAP.NAME, ITEMS.FOOD.MUSHROOMWRAP.DESC, 1f, false, Assets.GetAnim("mushroom_wrap_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.5f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.MUSHROOM_WRAP);
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x0002E308 File Offset: 0x0002C508
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x0002E30A File Offset: 0x0002C50A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000506 RID: 1286
	public const string ID = "MushroomWrap";

	// Token: 0x04000507 RID: 1287
	public static ComplexRecipe recipe;
}
