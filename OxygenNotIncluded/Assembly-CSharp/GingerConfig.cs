using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200014D RID: 333
public class GingerConfig : IEntityConfig
{
	// Token: 0x06000679 RID: 1657 RVA: 0x0002AADD File Offset: 0x00028CDD
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x0002AAE4 File Offset: 0x00028CE4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(GingerConfig.ID, ITEMS.INGREDIENTS.GINGER.NAME, ITEMS.INGREDIENTS.GINGER.DESC, 1f, true, Assets.GetAnim("ginger_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.45f, 0.4f, true, TUNING.SORTORDER.BUILDINGELEMENTS + GingerConfig.SORTORDER, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x0002AB5E File Offset: 0x00028D5E
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x0002AB60 File Offset: 0x00028D60
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400048E RID: 1166
	public static string ID = "GingerConfig";

	// Token: 0x0400048F RID: 1167
	public static int SORTORDER = 1;
}
