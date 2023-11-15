using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000193 RID: 403
public class WormBasicFruitConfig : IEntityConfig
{
	// Token: 0x060007E4 RID: 2020 RVA: 0x0002EF1C File Offset: 0x0002D11C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x0002EF24 File Offset: 0x0002D124
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("WormBasicFruit", ITEMS.FOOD.WORMBASICFRUIT.NAME, ITEMS.FOOD.WORMBASICFRUIT.DESC, 1f, false, Assets.GetAnim("wormwood_basic_fruit_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.7f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.WORMBASICFRUIT);
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x0002EF88 File Offset: 0x0002D188
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x0002EF8A File Offset: 0x0002D18A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400052D RID: 1325
	public const string ID = "WormBasicFruit";
}
