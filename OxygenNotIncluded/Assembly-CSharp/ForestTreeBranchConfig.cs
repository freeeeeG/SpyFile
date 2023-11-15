using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000148 RID: 328
public class ForestTreeBranchConfig : IEntityConfig
{
	// Token: 0x0600065F RID: 1631 RVA: 0x00029703 File Offset: 0x00027903
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x0002970C File Offset: 0x0002790C
	public GameObject CreatePrefab()
	{
		string id = "ForestTreeBranch";
		string name = STRINGS.CREATURES.SPECIES.WOOD_TREE.NAME;
		string desc = STRINGS.CREATURES.SPECIES.WOOD_TREE.DESC;
		float mass = 8f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("tree_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 1, tier, default(EffectorValues), SimHashes.Creature, new List<Tag>(), 298.15f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 258.15f, 288.15f, 313.15f, 448.15f, null, true, 0f, 0.15f, "WoodLog", true, true, false, true, 12000f, 0f, 9800f, "ForestTreeBranchOriginal", STRINGS.CREATURES.SPECIES.WOOD_TREE.NAME);
		gameObject.AddOrGet<TreeBud>();
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<BudUprootedMonitor>();
		return gameObject;
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x000297D6 File Offset: 0x000279D6
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x000297D8 File Offset: 0x000279D8
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000467 RID: 1127
	public const string ID = "ForestTreeBranch";

	// Token: 0x04000468 RID: 1128
	public const float WOOD_AMOUNT = 300f;
}
