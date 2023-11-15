using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000179 RID: 377
public class LettuceConfig : IEntityConfig
{
	// Token: 0x0600075B RID: 1883 RVA: 0x0002DEEC File Offset: 0x0002C0EC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600075C RID: 1884 RVA: 0x0002DEF4 File Offset: 0x0002C0F4
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Lettuce", ITEMS.FOOD.LETTUCE.NAME, ITEMS.FOOD.LETTUCE.DESC, 1f, false, Assets.GetAnim("sea_lettuce_leaves_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.LETTUCE);
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x0002DF58 File Offset: 0x0002C158
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x0002DF5A File Offset: 0x0002C15A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004FF RID: 1279
	public const string ID = "Lettuce";
}
