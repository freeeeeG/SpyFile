using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200027F RID: 639
public class GasGrassHarvestedConfig : IEntityConfig
{
	// Token: 0x06000CED RID: 3309 RVA: 0x00047BB4 File Offset: 0x00045DB4
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000CEE RID: 3310 RVA: 0x00047BBC File Offset: 0x00045DBC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("GasGrassHarvested", CREATURES.SPECIES.GASGRASS.NAME, CREATURES.SPECIES.GASGRASS.DESC, 1f, false, Assets.GetAnim("harvested_gassygrass_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.Other
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x06000CEF RID: 3311 RVA: 0x00047C2C File Offset: 0x00045E2C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CF0 RID: 3312 RVA: 0x00047C2E File Offset: 0x00045E2E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000775 RID: 1909
	public const string ID = "GasGrassHarvested";
}
