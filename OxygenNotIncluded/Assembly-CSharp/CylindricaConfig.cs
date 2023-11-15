using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000143 RID: 323
public class CylindricaConfig : IEntityConfig
{
	// Token: 0x06000645 RID: 1605 RVA: 0x00028FE7 File Offset: 0x000271E7
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x00028FF0 File Offset: 0x000271F0
	public GameObject CreatePrefab()
	{
		string id = "Cylindrica";
		string name = STRINGS.CREATURES.SPECIES.CYLINDRICA.NAME;
		string desc = STRINGS.CREATURES.SPECIES.CYLINDRICA.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = CylindricaConfig.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("potted_cylindricafan_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, 298.15f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 288.15f, 293.15f, 323.15f, 373.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 2200f, "CylindricaOriginal", STRINGS.CREATURES.SPECIES.CYLINDRICA.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		prickleGrass.positive_decor_effect = CylindricaConfig.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = CylindricaConfig.NEGATIVE_DECOR_EFFECT;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Hidden, "CylindricaSeed", STRINGS.CREATURES.SPECIES.SEEDS.CYLINDRICA.NAME, STRINGS.CREATURES.SPECIES.SEEDS.CYLINDRICA.DESC, Assets.GetAnim("seed_potted_cylindricafan_kanim"), "object", 1, new List<Tag>
		{
			GameTags.DecorSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 12, STRINGS.CREATURES.SPECIES.CYLINDRICA.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false), "Cylindrica_preview", Assets.GetAnim("potted_cylindricafan_kanim"), "place", 1, 1);
		return gameObject;
	}

	// Token: 0x06000647 RID: 1607 RVA: 0x00029158 File Offset: 0x00027358
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x0002915A File Offset: 0x0002735A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000458 RID: 1112
	public const string ID = "Cylindrica";

	// Token: 0x04000459 RID: 1113
	public const string SEED_ID = "CylindricaSeed";

	// Token: 0x0400045A RID: 1114
	public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;

	// Token: 0x0400045B RID: 1115
	public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}
