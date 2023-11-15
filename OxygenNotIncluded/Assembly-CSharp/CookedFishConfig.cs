using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200016F RID: 367
public class CookedFishConfig : IEntityConfig
{
	// Token: 0x06000729 RID: 1833 RVA: 0x0002DA14 File Offset: 0x0002BC14
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x0002DA1C File Offset: 0x0002BC1C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("CookedFish", ITEMS.FOOD.COOKEDFISH.NAME, ITEMS.FOOD.COOKEDFISH.DESC, 1f, false, Assets.GetAnim("grilled_pacu_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COOKED_FISH);
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x0002DA80 File Offset: 0x0002BC80
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x0002DA82 File Offset: 0x0002BC82
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004EE RID: 1262
	public const string ID = "CookedFish";

	// Token: 0x040004EF RID: 1263
	public static ComplexRecipe recipe;
}
