using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000187 RID: 391
public class ShellfishMeatConfig : IEntityConfig
{
	// Token: 0x060007A7 RID: 1959 RVA: 0x0002E8F8 File Offset: 0x0002CAF8
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060007A8 RID: 1960 RVA: 0x0002E900 File Offset: 0x0002CB00
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("ShellfishMeat", ITEMS.FOOD.SHELLFISHMEAT.NAME, ITEMS.FOOD.SHELLFISHMEAT.DESC, 1f, false, Assets.GetAnim("shellfish_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.SHELLFISH_MEAT);
		return gameObject;
	}

	// Token: 0x060007A9 RID: 1961 RVA: 0x0002E966 File Offset: 0x0002CB66
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x0002E968 File Offset: 0x0002CB68
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000517 RID: 1303
	public const string ID = "ShellfishMeat";
}
