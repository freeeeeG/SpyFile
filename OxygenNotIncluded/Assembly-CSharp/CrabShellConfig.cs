using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020001DA RID: 474
public class CrabShellConfig : IEntityConfig
{
	// Token: 0x06000972 RID: 2418 RVA: 0x00037AFF File Offset: 0x00035CFF
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000973 RID: 2419 RVA: 0x00037B08 File Offset: 0x00035D08
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("CrabShell", ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.NAME, ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.DESC, 10f, true, Assets.GetAnim("crabshells_large_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.Organics
		});
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<SimpleMassStatusItem>();
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x00037B91 File Offset: 0x00035D91
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000975 RID: 2421 RVA: 0x00037B93 File Offset: 0x00035D93
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005F1 RID: 1521
	public const string ID = "CrabShell";

	// Token: 0x040005F2 RID: 1522
	public static readonly Tag TAG = TagManager.Create("CrabShell");

	// Token: 0x040005F3 RID: 1523
	public const float MASS = 10f;
}
