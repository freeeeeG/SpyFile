using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000149 RID: 329
public class ForestTreeConfig : IEntityConfig
{
	// Token: 0x06000664 RID: 1636 RVA: 0x000297E2 File Offset: 0x000279E2
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x000297EC File Offset: 0x000279EC
	public GameObject CreatePrefab()
	{
		string id = "ForestTree";
		string name = STRINGS.CREATURES.SPECIES.WOOD_TREE.NAME;
		string desc = STRINGS.CREATURES.SPECIES.WOOD_TREE.DESC;
		float mass = 2f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("tree_kanim"), "idle_empty", Grid.SceneLayer.Building, 1, 2, tier, default(EffectorValues), SimHashes.Creature, new List<Tag>(), 298.15f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 258.15f, 288.15f, 313.15f, 448.15f, null, true, 0f, 0.15f, "WoodLog", true, true, true, false, 2400f, 0f, 9800f, "ForestTreeOriginal", STRINGS.CREATURES.SPECIES.WOOD_TREE.NAME);
		gameObject.AddOrGet<BuddingTrunk>();
		gameObject.UpdateComponentRequirement(false);
		Tag tag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = tag,
				massConsumptionRate = 0.11666667f
			}
		});
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.Dirt,
				massConsumptionRate = 0.016666668f
			}
		});
		gameObject.AddComponent<StandardCropPlant>();
		gameObject.AddOrGet<BuddingTrunk>().budPrefabID = "ForestTreeBranch";
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Hidden, "ForestTreeSeed", STRINGS.CREATURES.SPECIES.SEEDS.WOOD_TREE.NAME, STRINGS.CREATURES.SPECIES.SEEDS.WOOD_TREE.DESC, Assets.GetAnim("seed_tree_kanim"), "object", 1, new List<Tag>
		{
			GameTags.CropSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 4, STRINGS.CREATURES.SPECIES.WOOD_TREE.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false), "ForestTree_preview", Assets.GetAnim("tree_kanim"), "place", 3, 3);
		return gameObject;
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x000299CF File Offset: 0x00027BCF
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x000299D1 File Offset: 0x00027BD1
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000469 RID: 1129
	public const string ID = "ForestTree";

	// Token: 0x0400046A RID: 1130
	public const string SEED_ID = "ForestTreeSeed";

	// Token: 0x0400046B RID: 1131
	public const float FERTILIZATION_RATE = 0.016666668f;

	// Token: 0x0400046C RID: 1132
	public const float WATER_RATE = 0.11666667f;

	// Token: 0x0400046D RID: 1133
	public const float BRANCH_GROWTH_TIME = 2100f;

	// Token: 0x0400046E RID: 1134
	public const int NUM_BRANCHES = 7;
}
