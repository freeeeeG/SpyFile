using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000161 RID: 353
public class WineCupsConfig : IEntityConfig
{
	// Token: 0x060006E4 RID: 1764 RVA: 0x0002CBE4 File Offset: 0x0002ADE4
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x0002CBEC File Offset: 0x0002ADEC
	public GameObject CreatePrefab()
	{
		string id = "WineCups";
		string name = STRINGS.CREATURES.SPECIES.WINECUPS.NAME;
		string desc = STRINGS.CREATURES.SPECIES.WINECUPS.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = WineCupsConfig.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("potted_cups_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 218.15f, 283.15f, 303.15f, 398.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 900f, "WineCupsOriginal", STRINGS.CREATURES.SPECIES.WINECUPS.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		prickleGrass.positive_decor_effect = WineCupsConfig.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = WineCupsConfig.NEGATIVE_DECOR_EFFECT;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Hidden, "WineCupsSeed", STRINGS.CREATURES.SPECIES.SEEDS.WINECUPS.NAME, STRINGS.CREATURES.SPECIES.SEEDS.WINECUPS.DESC, Assets.GetAnim("seed_potted_cups_kanim"), "object", 1, new List<Tag>
		{
			GameTags.DecorSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 11, STRINGS.CREATURES.SPECIES.WINECUPS.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false), "WineCups_preview", Assets.GetAnim("potted_cups_kanim"), "place", 1, 1);
		return gameObject;
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x0002CD54 File Offset: 0x0002AF54
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x0002CD56 File Offset: 0x0002AF56
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004D2 RID: 1234
	public const string ID = "WineCups";

	// Token: 0x040004D3 RID: 1235
	public const string SEED_ID = "WineCupsSeed";

	// Token: 0x040004D4 RID: 1236
	public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;

	// Token: 0x040004D5 RID: 1237
	public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}
