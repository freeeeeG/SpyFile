using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020001D8 RID: 472
public class BabyCrabShellConfig : IEntityConfig
{
	// Token: 0x06000966 RID: 2406 RVA: 0x0003796A File Offset: 0x00035B6A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x00037974 File Offset: 0x00035B74
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BabyCrabShell", ITEMS.INDUSTRIAL_PRODUCTS.BABY_CRAB_SHELL.NAME, ITEMS.INDUSTRIAL_PRODUCTS.BABY_CRAB_SHELL.DESC, 5f, true, Assets.GetAnim("crabshells_small_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.Organics
		});
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<SimpleMassStatusItem>();
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x000379FD File Offset: 0x00035BFD
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x000379FF File Offset: 0x00035BFF
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005EA RID: 1514
	public const string ID = "BabyCrabShell";

	// Token: 0x040005EB RID: 1515
	public static readonly Tag TAG = TagManager.Create("BabyCrabShell");

	// Token: 0x040005EC RID: 1516
	public const float MASS = 5f;
}
