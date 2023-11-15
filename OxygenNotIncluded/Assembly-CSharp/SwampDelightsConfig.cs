using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200018C RID: 396
public class SwampDelightsConfig : IEntityConfig
{
	// Token: 0x060007C1 RID: 1985 RVA: 0x0002EBA0 File Offset: 0x0002CDA0
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x0002EBA8 File Offset: 0x0002CDA8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SwampDelights", ITEMS.FOOD.SWAMPDELIGHTS.NAME, ITEMS.FOOD.SWAMPDELIGHTS.DESC, 1f, false, Assets.GetAnim("swamp_delights_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.7f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SWAMP_DELIGHTS);
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x0002EC0C File Offset: 0x0002CE0C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x0002EC0E File Offset: 0x0002CE0E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000520 RID: 1312
	public const string ID = "SwampDelights";
}
