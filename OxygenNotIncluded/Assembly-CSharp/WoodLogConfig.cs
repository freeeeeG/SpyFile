using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020001E3 RID: 483
public class WoodLogConfig : IEntityConfig
{
	// Token: 0x060009A8 RID: 2472 RVA: 0x000380DA File Offset: 0x000362DA
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x000380E4 File Offset: 0x000362E4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("WoodLog", ITEMS.INDUSTRIAL_PRODUCTS.WOOD.NAME, ITEMS.INDUSTRIAL_PRODUCTS.WOOD.DESC, 1f, false, Assets.GetAnim("wood_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.Organics,
			GameTags.BuildingWood
		});
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<SimpleMassStatusItem>();
		return gameObject;
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x00038171 File Offset: 0x00036371
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x00038173 File Offset: 0x00036373
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000610 RID: 1552
	public const string ID = "WoodLog";

	// Token: 0x04000611 RID: 1553
	public static readonly Tag TAG = TagManager.Create("WoodLog");
}
