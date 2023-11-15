using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020001D9 RID: 473
public class BabyCrabWoodShellConfig : IEntityConfig
{
	// Token: 0x0600096C RID: 2412 RVA: 0x00037A1A File Offset: 0x00035C1A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x00037A24 File Offset: 0x00035C24
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BabyCrabWoodShell", ITEMS.INDUSTRIAL_PRODUCTS.BABY_CRAB_SHELL.VARIANT_WOOD.NAME, ITEMS.INDUSTRIAL_PRODUCTS.BABY_CRAB_SHELL.VARIANT_WOOD.DESC, 10f, true, Assets.GetAnim("crabshells_small_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.Organics,
			GameTags.MoltShell
		});
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<SimpleMassStatusItem>().symbolPrefix = "wood_";
		SymbolOverrideControllerUtil.AddToPrefab(gameObject).ApplySymbolOverridesByAffix(Assets.GetAnim("crabshells_small_kanim"), "wood_", null, 0);
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x00037AE2 File Offset: 0x00035CE2
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x00037AE4 File Offset: 0x00035CE4
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005ED RID: 1517
	public const string ID = "BabyCrabWoodShell";

	// Token: 0x040005EE RID: 1518
	public static readonly Tag TAG = TagManager.Create("BabyCrabWoodShell");

	// Token: 0x040005EF RID: 1519
	public const float MASS = 10f;

	// Token: 0x040005F0 RID: 1520
	public const string symbolPrefix = "wood_";
}
