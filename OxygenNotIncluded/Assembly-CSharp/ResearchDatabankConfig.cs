using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001E2 RID: 482
public class ResearchDatabankConfig : IEntityConfig
{
	// Token: 0x060009A2 RID: 2466 RVA: 0x00038031 File Offset: 0x00036231
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060009A3 RID: 2467 RVA: 0x00038038 File Offset: 0x00036238
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("ResearchDatabank", ITEMS.INDUSTRIAL_PRODUCTS.RESEARCH_DATABANK.NAME, ITEMS.INDUSTRIAL_PRODUCTS.RESEARCH_DATABANK.DESC, 1f, true, Assets.GetAnim("floppy_disc_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.Experimental
		});
		gameObject.AddOrGet<EntitySplitter>().maxStackSize = (float)ROCKETRY.DESTINATION_RESEARCH.BASIC;
		return gameObject;
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x000380BD File Offset: 0x000362BD
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x000380BF File Offset: 0x000362BF
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400060D RID: 1549
	public const string ID = "ResearchDatabank";

	// Token: 0x0400060E RID: 1550
	public static readonly Tag TAG = TagManager.Create("ResearchDatabank");

	// Token: 0x0400060F RID: 1551
	public const float MASS = 1f;
}
