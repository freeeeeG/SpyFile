using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000171 RID: 369
public class CurryConfig : IEntityConfig
{
	// Token: 0x06000733 RID: 1843 RVA: 0x0002DB04 File Offset: 0x0002BD04
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000734 RID: 1844 RVA: 0x0002DB0C File Offset: 0x0002BD0C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Curry", ITEMS.FOOD.CURRY.NAME, ITEMS.FOOD.CURRY.DESC, 1f, false, Assets.GetAnim("curried_beans_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.5f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.CURRY);
	}

	// Token: 0x06000735 RID: 1845 RVA: 0x0002DB70 File Offset: 0x0002BD70
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x0002DB72 File Offset: 0x0002BD72
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004F2 RID: 1266
	public const string ID = "Curry";
}
