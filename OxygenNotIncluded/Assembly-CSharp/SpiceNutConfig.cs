using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000189 RID: 393
public class SpiceNutConfig : IEntityConfig
{
	// Token: 0x060007B1 RID: 1969 RVA: 0x0002E9EC File Offset: 0x0002CBEC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x0002E9F4 File Offset: 0x0002CBF4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(SpiceNutConfig.ID, ITEMS.FOOD.SPICENUT.NAME, ITEMS.FOOD.SPICENUT.DESC, 1f, false, Assets.GetAnim("spicenut_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.SPICENUT);
		SoundEventVolumeCache.instance.AddVolume("vinespicenut_kanim", "VineSpiceNut_grow", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("vinespicenut_kanim", "VineSpiceNut_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		return gameObject;
	}

	// Token: 0x060007B3 RID: 1971 RVA: 0x0002EA8C File Offset: 0x0002CC8C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007B4 RID: 1972 RVA: 0x0002EA8E File Offset: 0x0002CC8E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400051A RID: 1306
	public static float SEEDS_PER_FRUIT = 1f;

	// Token: 0x0400051B RID: 1307
	public static string ID = "SpiceNut";
}
