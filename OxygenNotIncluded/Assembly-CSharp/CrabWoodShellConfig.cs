using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020001DB RID: 475
public class CrabWoodShellConfig : IEntityConfig
{
	// Token: 0x06000978 RID: 2424 RVA: 0x00037BAE File Offset: 0x00035DAE
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x00037BB8 File Offset: 0x00035DB8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("CrabWoodShell", ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.VARIANT_WOOD.NAME, ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.VARIANT_WOOD.DESC, 100f, true, Assets.GetAnim("crabshells_large_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.Organics,
			GameTags.MoltShell
		});
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<SimpleMassStatusItem>().symbolPrefix = "wood_";
		SymbolOverrideControllerUtil.AddToPrefab(gameObject).ApplySymbolOverridesByAffix(Assets.GetAnim("crabshells_large_kanim"), "wood_", null, 0);
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x00037C76 File Offset: 0x00035E76
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x00037C78 File Offset: 0x00035E78
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005F4 RID: 1524
	public const string ID = "CrabWoodShell";

	// Token: 0x040005F5 RID: 1525
	public static readonly Tag TAG = TagManager.Create("CrabWoodShell");

	// Token: 0x040005F6 RID: 1526
	public const float MASS = 100f;

	// Token: 0x040005F7 RID: 1527
	public const string symbolPrefix = "wood_";
}
