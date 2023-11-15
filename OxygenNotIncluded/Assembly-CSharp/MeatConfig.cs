using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200017A RID: 378
public class MeatConfig : IEntityConfig
{
	// Token: 0x06000760 RID: 1888 RVA: 0x0002DF64 File Offset: 0x0002C164
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000761 RID: 1889 RVA: 0x0002DF6C File Offset: 0x0002C16C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("Meat", ITEMS.FOOD.MEAT.NAME, ITEMS.FOOD.MEAT.DESC, 1f, false, Assets.GetAnim("creaturemeat_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.MEAT);
		return gameObject;
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x0002DFD2 File Offset: 0x0002C1D2
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x0002DFD4 File Offset: 0x0002C1D4
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000500 RID: 1280
	public const string ID = "Meat";
}
