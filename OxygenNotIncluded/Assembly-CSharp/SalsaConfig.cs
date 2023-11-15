using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000186 RID: 390
public class SalsaConfig : IEntityConfig
{
	// Token: 0x060007A2 RID: 1954 RVA: 0x0002E880 File Offset: 0x0002CA80
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x0002E888 File Offset: 0x0002CA88
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Salsa", ITEMS.FOOD.SALSA.NAME, ITEMS.FOOD.SALSA.DESC, 1f, false, Assets.GetAnim("zestysalsa_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.5f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SALSA);
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x0002E8EC File Offset: 0x0002CAEC
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x0002E8EE File Offset: 0x0002CAEE
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000515 RID: 1301
	public const string ID = "Salsa";

	// Token: 0x04000516 RID: 1302
	public static ComplexRecipe recipe;
}
