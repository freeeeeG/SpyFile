using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200015B RID: 347
public class SwampForagePlantConfig : IEntityConfig
{
	// Token: 0x060006C4 RID: 1732 RVA: 0x0002C2B4 File Offset: 0x0002A4B4
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x0002C2BC File Offset: 0x0002A4BC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SwampForagePlant", ITEMS.FOOD.SWAMPFORAGEPLANT.NAME, ITEMS.FOOD.SWAMPFORAGEPLANT.DESC, 1f, false, Assets.GetAnim("swamptuber_vegetable_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SWAMPFORAGEPLANT);
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x0002C320 File Offset: 0x0002A520
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060006C7 RID: 1735 RVA: 0x0002C322 File Offset: 0x0002A522
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004C0 RID: 1216
	public const string ID = "SwampForagePlant";
}
