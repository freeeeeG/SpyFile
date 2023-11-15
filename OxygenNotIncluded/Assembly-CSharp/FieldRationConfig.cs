using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000172 RID: 370
public class FieldRationConfig : IEntityConfig
{
	// Token: 0x06000738 RID: 1848 RVA: 0x0002DB7C File Offset: 0x0002BD7C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x0002DB84 File Offset: 0x0002BD84
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FieldRation", ITEMS.FOOD.FIELDRATION.NAME, ITEMS.FOOD.FIELDRATION.DESC, 1f, false, Assets.GetAnim("fieldration_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FIELDRATION);
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x0002DBE8 File Offset: 0x0002BDE8
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x0002DBEA File Offset: 0x0002BDEA
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004F3 RID: 1267
	public const string ID = "FieldRation";
}
