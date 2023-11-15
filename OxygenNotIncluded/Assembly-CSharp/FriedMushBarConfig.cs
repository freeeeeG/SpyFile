using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000174 RID: 372
public class FriedMushBarConfig : IEntityConfig
{
	// Token: 0x06000742 RID: 1858 RVA: 0x0002DC6E File Offset: 0x0002BE6E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000743 RID: 1859 RVA: 0x0002DC78 File Offset: 0x0002BE78
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FriedMushBar", ITEMS.FOOD.FRIEDMUSHBAR.NAME, ITEMS.FOOD.FRIEDMUSHBAR.DESC, 1f, false, Assets.GetAnim("mushbarfried_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FRIEDMUSHBAR);
	}

	// Token: 0x06000744 RID: 1860 RVA: 0x0002DCDC File Offset: 0x0002BEDC
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000745 RID: 1861 RVA: 0x0002DCDE File Offset: 0x0002BEDE
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004F5 RID: 1269
	public const string ID = "FriedMushBar";

	// Token: 0x040004F6 RID: 1270
	public static ComplexRecipe recipe;
}
