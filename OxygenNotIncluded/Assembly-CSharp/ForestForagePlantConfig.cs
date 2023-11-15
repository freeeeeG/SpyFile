using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000146 RID: 326
public class ForestForagePlantConfig : IEntityConfig
{
	// Token: 0x06000655 RID: 1621 RVA: 0x0002959E File Offset: 0x0002779E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000656 RID: 1622 RVA: 0x000295A8 File Offset: 0x000277A8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("ForestForagePlant", ITEMS.FOOD.FORESTFORAGEPLANT.NAME, ITEMS.FOOD.FORESTFORAGEPLANT.DESC, 1f, false, Assets.GetAnim("podmelon_fruit_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FORESTFORAGEPLANT);
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x0002960C File Offset: 0x0002780C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x0002960E File Offset: 0x0002780E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000465 RID: 1125
	public const string ID = "ForestForagePlant";
}
