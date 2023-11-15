using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200016A RID: 362
public class BasicPlantFoodConfig : IEntityConfig
{
	// Token: 0x06000710 RID: 1808 RVA: 0x0002D7B7 File Offset: 0x0002B9B7
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x0002D7C0 File Offset: 0x0002B9C0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BasicPlantFood", ITEMS.FOOD.BASICPLANTFOOD.NAME, ITEMS.FOOD.BASICPLANTFOOD.DESC, 1f, false, Assets.GetAnim("meallicegrain_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.BASICPLANTFOOD);
		return gameObject;
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x0002D826 File Offset: 0x0002BA26
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x0002D828 File Offset: 0x0002BA28
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004E5 RID: 1253
	public const string ID = "BasicPlantFood";
}
