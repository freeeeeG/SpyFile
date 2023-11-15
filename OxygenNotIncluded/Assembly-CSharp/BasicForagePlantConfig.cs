using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000139 RID: 313
public class BasicForagePlantConfig : IEntityConfig
{
	// Token: 0x06000612 RID: 1554 RVA: 0x00027EEC File Offset: 0x000260EC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x00027EF4 File Offset: 0x000260F4
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("BasicForagePlant", ITEMS.FOOD.BASICFORAGEPLANT.NAME, ITEMS.FOOD.BASICFORAGEPLANT.DESC, 1f, false, Assets.GetAnim("muckrootvegetable_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.BASICFORAGEPLANT);
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x00027F58 File Offset: 0x00026158
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x00027F5A File Offset: 0x0002615A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000435 RID: 1077
	public const string ID = "BasicForagePlant";
}
