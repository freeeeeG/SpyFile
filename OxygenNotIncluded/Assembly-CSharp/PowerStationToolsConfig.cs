using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020001E1 RID: 481
public class PowerStationToolsConfig : IEntityConfig
{
	// Token: 0x0600099C RID: 2460 RVA: 0x00037F9A File Offset: 0x0003619A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x00037FA4 File Offset: 0x000361A4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("PowerStationTools", ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME, ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.DESC, 5f, true, Assets.GetAnim("kit_electrician_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.MiscPickupable
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x0600099E RID: 2462 RVA: 0x00038014 File Offset: 0x00036214
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600099F RID: 2463 RVA: 0x00038016 File Offset: 0x00036216
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400060A RID: 1546
	public const string ID = "PowerStationTools";

	// Token: 0x0400060B RID: 1547
	public static readonly Tag tag = TagManager.Create("PowerStationTools");

	// Token: 0x0400060C RID: 1548
	public const float MASS = 5f;
}
