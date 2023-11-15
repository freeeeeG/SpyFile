using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200013E RID: 318
public class CactusPlantConfig : IEntityConfig
{
	// Token: 0x0600062B RID: 1579 RVA: 0x000285F8 File Offset: 0x000267F8
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x00028600 File Offset: 0x00026800
	public GameObject CreatePrefab()
	{
		string id = "CactusPlant";
		string name = STRINGS.CREATURES.SPECIES.CACTUSPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.CACTUSPLANT.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = this.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("potted_cactus_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 200f, 273.15f, 373.15f, 400f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, false, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 2200f, "CactusPlantOriginal", STRINGS.CREATURES.SPECIES.CACTUSPLANT.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		prickleGrass.positive_decor_effect = this.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = this.NEGATIVE_DECOR_EFFECT;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Hidden, "CactusPlantSeed", STRINGS.CREATURES.SPECIES.SEEDS.CACTUSPLANT.NAME, STRINGS.CREATURES.SPECIES.SEEDS.CACTUSPLANT.DESC, Assets.GetAnim("seed_potted_cactus_kanim"), "object", 1, new List<Tag>
		{
			GameTags.DecorSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 13, STRINGS.CREATURES.SPECIES.CACTUSPLANT.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false), "CactusPlant_preview", Assets.GetAnim("potted_cactus_kanim"), "place", 1, 1);
		return gameObject;
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x0002876B File Offset: 0x0002696B
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x0002876D File Offset: 0x0002696D
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000442 RID: 1090
	public const string ID = "CactusPlant";

	// Token: 0x04000443 RID: 1091
	public const string SEED_ID = "CactusPlantSeed";

	// Token: 0x04000444 RID: 1092
	public readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;

	// Token: 0x04000445 RID: 1093
	public readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}
