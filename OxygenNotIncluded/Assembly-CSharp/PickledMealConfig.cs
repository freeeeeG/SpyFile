using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000180 RID: 384
public class PickledMealConfig : IEntityConfig
{
	// Token: 0x06000781 RID: 1921 RVA: 0x0002E408 File Offset: 0x0002C608
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x0002E410 File Offset: 0x0002C610
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("PickledMeal", ITEMS.FOOD.PICKLEDMEAL.NAME, ITEMS.FOOD.PICKLEDMEAL.DESC, 1f, false, Assets.GetAnim("pickledmeal_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.PICKLEDMEAL);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.Pickled, false);
		return gameObject;
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x0002E485 File Offset: 0x0002C685
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x0002E487 File Offset: 0x0002C687
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400050B RID: 1291
	public const string ID = "PickledMeal";

	// Token: 0x0400050C RID: 1292
	public static ComplexRecipe recipe;
}
