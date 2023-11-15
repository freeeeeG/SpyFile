using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000190 RID: 400
public class TableSaltConfig : IEntityConfig
{
	// Token: 0x060007D4 RID: 2004 RVA: 0x0002ED6D File Offset: 0x0002CF6D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x0002ED74 File Offset: 0x0002CF74
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(TableSaltConfig.ID, ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.NAME, ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.DESC, 1f, false, Assets.GetAnim("seed_saltPlant_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.45f, true, SORTORDER.BUILDINGELEMENTS + TableSaltTuning.SORTORDER, SimHashes.Salt, new List<Tag>
		{
			GameTags.Other
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x0002EDEE File Offset: 0x0002CFEE
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x0002EDF0 File Offset: 0x0002CFF0
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000528 RID: 1320
	public static string ID = "TableSalt";
}
