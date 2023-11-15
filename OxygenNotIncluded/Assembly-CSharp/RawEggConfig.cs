using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000184 RID: 388
public class RawEggConfig : IEntityConfig
{
	// Token: 0x06000797 RID: 1943 RVA: 0x0002E714 File Offset: 0x0002C914
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x0002E71C File Offset: 0x0002C91C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("RawEgg", ITEMS.FOOD.RAWEGG.NAME, ITEMS.FOOD.RAWEGG.DESC, 1f, false, Assets.GetAnim("rawegg_kanim"), "object", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.RAWEGG);
		TemperatureCookable temperatureCookable = gameObject.AddOrGet<TemperatureCookable>();
		temperatureCookable.cookTemperature = 344.15f;
		temperatureCookable.cookedID = "CookedEgg";
		return gameObject;
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x0002E79D File Offset: 0x0002C99D
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x0002E79F File Offset: 0x0002C99F
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000513 RID: 1299
	public const string ID = "RawEgg";
}
