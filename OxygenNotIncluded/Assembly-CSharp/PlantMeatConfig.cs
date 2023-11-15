using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000181 RID: 385
public class PlantMeatConfig : IEntityConfig
{
	// Token: 0x06000786 RID: 1926 RVA: 0x0002E491 File Offset: 0x0002C691
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x0002E498 File Offset: 0x0002C698
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("PlantMeat", ITEMS.FOOD.PLANTMEAT.NAME, ITEMS.FOOD.PLANTMEAT.DESC, 1f, false, Assets.GetAnim("critter_trap_fruit_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.PLANTMEAT);
		return gameObject;
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x0002E4FE File Offset: 0x0002C6FE
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x0002E500 File Offset: 0x0002C700
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400050D RID: 1293
	public const string ID = "PlantMeat";
}
