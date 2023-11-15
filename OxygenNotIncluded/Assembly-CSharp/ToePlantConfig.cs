using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200015F RID: 351
public class ToePlantConfig : IEntityConfig
{
	// Token: 0x060006D9 RID: 1753 RVA: 0x0002C7C8 File Offset: 0x0002A9C8
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060006DA RID: 1754 RVA: 0x0002C7D0 File Offset: 0x0002A9D0
	public GameObject CreatePrefab()
	{
		string id = "ToePlant";
		string name = STRINGS.CREATURES.SPECIES.TOEPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.TOEPLANT.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = ToePlantConfig.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("potted_toes_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, TUNING.CREATURES.TEMPERATURE.FREEZING_3);
		GameObject template = gameObject;
		SimHashes[] safe_elements = new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		};
		EntityTemplates.ExtendEntityToBasicPlant(template, TUNING.CREATURES.TEMPERATURE.FREEZING_10, TUNING.CREATURES.TEMPERATURE.FREEZING_9, TUNING.CREATURES.TEMPERATURE.FREEZING, TUNING.CREATURES.TEMPERATURE.COOL, safe_elements, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 2200f, "ToePlantOriginal", STRINGS.CREATURES.SPECIES.TOEPLANT.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		prickleGrass.positive_decor_effect = ToePlantConfig.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = ToePlantConfig.NEGATIVE_DECOR_EFFECT;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Hidden, "ToePlantSeed", STRINGS.CREATURES.SPECIES.SEEDS.TOEPLANT.NAME, STRINGS.CREATURES.SPECIES.SEEDS.TOEPLANT.DESC, Assets.GetAnim("seed_potted_toes_kanim"), "object", 1, new List<Tag>
		{
			GameTags.DecorSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 12, STRINGS.CREATURES.SPECIES.TOEPLANT.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false), "ToePlant_preview", Assets.GetAnim("potted_toes_kanim"), "place", 1, 1);
		return gameObject;
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x0002C93B File Offset: 0x0002AB3B
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x0002C93D File Offset: 0x0002AB3D
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004C7 RID: 1223
	public const string ID = "ToePlant";

	// Token: 0x040004C8 RID: 1224
	public const string SEED_ID = "ToePlantSeed";

	// Token: 0x040004C9 RID: 1225
	public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;

	// Token: 0x040004CA RID: 1226
	public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}
