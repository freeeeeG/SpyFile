using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001E0 RID: 480
public class OrbitalResearchDatabankConfig : IEntityConfig
{
	// Token: 0x06000996 RID: 2454 RVA: 0x00037EEF File Offset: 0x000360EF
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x00037EF8 File Offset: 0x000360F8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("OrbitalResearchDatabank", ITEMS.INDUSTRIAL_PRODUCTS.ORBITAL_RESEARCH_DATABANK.NAME, ITEMS.INDUSTRIAL_PRODUCTS.ORBITAL_RESEARCH_DATABANK.DESC, 1f, true, Assets.GetAnim("floppy_disc_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.Experimental
		});
		gameObject.AddOrGet<EntitySplitter>().maxStackSize = (float)ROCKETRY.DESTINATION_RESEARCH.BASIC;
		return gameObject;
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x00037F7D File Offset: 0x0003617D
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x00037F7F File Offset: 0x0003617F
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000607 RID: 1543
	public const string ID = "OrbitalResearchDatabank";

	// Token: 0x04000608 RID: 1544
	public static readonly Tag TAG = TagManager.Create("OrbitalResearchDatabank");

	// Token: 0x04000609 RID: 1545
	public const float MASS = 1f;
}
