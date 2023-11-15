using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000155 RID: 341
public class PrickleGrassConfig : IEntityConfig
{
	// Token: 0x060006A2 RID: 1698 RVA: 0x0002B7A1 File Offset: 0x000299A1
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x0002B7A8 File Offset: 0x000299A8
	public GameObject CreatePrefab()
	{
		string id = "PrickleGrass";
		string name = STRINGS.CREATURES.SPECIES.PRICKLEGRASS.NAME;
		string desc = STRINGS.CREATURES.SPECIES.PRICKLEGRASS.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = PrickleGrassConfig.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("bristlebriar_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 218.15f, 283.15f, 303.15f, 398.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 900f, "PrickleGrassOriginal", STRINGS.CREATURES.SPECIES.PRICKLEGRASS.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		prickleGrass.positive_decor_effect = PrickleGrassConfig.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = PrickleGrassConfig.NEGATIVE_DECOR_EFFECT;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Hidden, "PrickleGrassSeed", STRINGS.CREATURES.SPECIES.SEEDS.PRICKLEGRASS.NAME, STRINGS.CREATURES.SPECIES.SEEDS.PRICKLEGRASS.DESC, Assets.GetAnim("seed_bristlebriar_kanim"), "object", 1, new List<Tag>
		{
			GameTags.DecorSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 10, STRINGS.CREATURES.SPECIES.PRICKLEGRASS.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false), "PrickleGrass_preview", Assets.GetAnim("bristlebriar_kanim"), "place", 1, 1);
		return gameObject;
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x0002B910 File Offset: 0x00029B10
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x0002B912 File Offset: 0x00029B12
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004A4 RID: 1188
	public const string ID = "PrickleGrass";

	// Token: 0x040004A5 RID: 1189
	public const string SEED_ID = "PrickleGrassSeed";

	// Token: 0x040004A6 RID: 1190
	public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;

	// Token: 0x040004A7 RID: 1191
	public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}
