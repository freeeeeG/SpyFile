using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200018B RID: 395
public class SurfAndTurfConfig : IEntityConfig
{
	// Token: 0x060007BC RID: 1980 RVA: 0x0002EB28 File Offset: 0x0002CD28
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060007BD RID: 1981 RVA: 0x0002EB30 File Offset: 0x0002CD30
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SurfAndTurf", ITEMS.FOOD.SURFANDTURF.NAME, ITEMS.FOOD.SURFANDTURF.DESC, 1f, false, Assets.GetAnim("surfnturf_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SURF_AND_TURF);
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x0002EB94 File Offset: 0x0002CD94
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x0002EB96 File Offset: 0x0002CD96
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400051E RID: 1310
	public const string ID = "SurfAndTurf";

	// Token: 0x0400051F RID: 1311
	public static ComplexRecipe recipe;
}
