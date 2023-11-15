using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020001DE RID: 478
public class FarmStationToolsConfig : IEntityConfig
{
	// Token: 0x0600098A RID: 2442 RVA: 0x00037DDD File Offset: 0x00035FDD
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x00037DE4 File Offset: 0x00035FE4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("FarmStationTools", ITEMS.INDUSTRIAL_PRODUCTS.FARM_STATION_TOOLS.NAME, ITEMS.INDUSTRIAL_PRODUCTS.FARM_STATION_TOOLS.DESC, 5f, true, Assets.GetAnim("kit_planttender_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.MiscPickupable
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x00037E54 File Offset: 0x00036054
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x00037E56 File Offset: 0x00036056
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000601 RID: 1537
	public const string ID = "FarmStationTools";

	// Token: 0x04000602 RID: 1538
	public static readonly Tag tag = TagManager.Create("FarmStationTools");

	// Token: 0x04000603 RID: 1539
	public const float MASS = 5f;
}
