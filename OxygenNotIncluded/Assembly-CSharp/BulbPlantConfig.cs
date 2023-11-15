using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200013D RID: 317
public class BulbPlantConfig : IEntityConfig
{
	// Token: 0x06000626 RID: 1574 RVA: 0x00028419 File Offset: 0x00026619
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000627 RID: 1575 RVA: 0x00028420 File Offset: 0x00026620
	public GameObject CreatePrefab()
	{
		string id = "BulbPlant";
		string name = STRINGS.CREATURES.SPECIES.BULBPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.BULBPLANT.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = this.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("potted_bulb_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 288f, 293.15f, 313.15f, 333.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 2200f, "BulbPlantOriginal", STRINGS.CREATURES.SPECIES.BULBPLANT.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		prickleGrass.positive_decor_effect = this.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = this.NEGATIVE_DECOR_EFFECT;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Hidden, "BulbPlantSeed", STRINGS.CREATURES.SPECIES.SEEDS.BULBPLANT.NAME, STRINGS.CREATURES.SPECIES.SEEDS.BULBPLANT.DESC, Assets.GetAnim("seed_potted_bulb_kanim"), "object", 1, new List<Tag>
		{
			GameTags.DecorSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 12, STRINGS.CREATURES.SPECIES.BULBPLANT.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.4f, 0.4f, null, "", false), "BulbPlant_preview", Assets.GetAnim("potted_bulb_kanim"), "place", 1, 1);
		DiseaseDropper.Def def = gameObject.AddOrGetDef<DiseaseDropper.Def>();
		def.diseaseIdx = Db.Get().Diseases.GetIndex(Db.Get().Diseases.PollenGerms.id);
		def.singleEmitQuantity = 0;
		def.averageEmitPerSecond = 5000;
		def.emitFrequency = 5f;
		return gameObject;
	}

	// Token: 0x06000628 RID: 1576 RVA: 0x000285D6 File Offset: 0x000267D6
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x000285D8 File Offset: 0x000267D8
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400043E RID: 1086
	public const string ID = "BulbPlant";

	// Token: 0x0400043F RID: 1087
	public const string SEED_ID = "BulbPlantSeed";

	// Token: 0x04000440 RID: 1088
	public readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER1;

	// Token: 0x04000441 RID: 1089
	public readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}
