using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000173 RID: 371
public class FishMeatConfig : IEntityConfig
{
	// Token: 0x0600073D RID: 1853 RVA: 0x0002DBF4 File Offset: 0x0002BDF4
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x0002DBFC File Offset: 0x0002BDFC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("FishMeat", ITEMS.FOOD.FISHMEAT.NAME, ITEMS.FOOD.FISHMEAT.DESC, 1f, false, Assets.GetAnim("pacufillet_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.FISH_MEAT);
		return gameObject;
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x0002DC62 File Offset: 0x0002BE62
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x0002DC64 File Offset: 0x0002BE64
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004F4 RID: 1268
	public const string ID = "FishMeat";
}
