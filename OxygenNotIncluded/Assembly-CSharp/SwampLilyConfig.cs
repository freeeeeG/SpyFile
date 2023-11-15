using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200015E RID: 350
public class SwampLilyConfig : IEntityConfig
{
	// Token: 0x060006D3 RID: 1747 RVA: 0x0002C5CC File Offset: 0x0002A7CC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060006D4 RID: 1748 RVA: 0x0002C5D4 File Offset: 0x0002A7D4
	public GameObject CreatePrefab()
	{
		string id = "SwampLily";
		string name = STRINGS.CREATURES.SPECIES.SWAMPLILY.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SWAMPLILY.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("swamplily_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 328.15f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 258.15f, 308.15f, 358.15f, 448.15f, new SimHashes[]
		{
			SimHashes.ChlorineGas
		}, true, 0f, 0.15f, SwampLilyFlowerConfig.ID, true, true, true, true, 2400f, 0f, 4600f, SwampLilyConfig.ID + "Original", STRINGS.CREATURES.SPECIES.SWAMPLILY.NAME);
		gameObject.AddOrGet<StandardCropPlant>();
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Harvest, "SwampLilySeed", STRINGS.CREATURES.SPECIES.SEEDS.SWAMPLILY.NAME, STRINGS.CREATURES.SPECIES.SEEDS.SWAMPLILY.DESC, Assets.GetAnim("seed_swampLily_kanim"), "object", 1, new List<Tag>
		{
			GameTags.CropSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 21, STRINGS.CREATURES.SPECIES.SWAMPLILY.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false), SwampLilyConfig.ID + "_preview", Assets.GetAnim("swamplily_kanim"), "place", 1, 2);
		SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_grow", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_death", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_death_bloom", NOISE_POLLUTION.CREATURES.TIER3);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.HarvestableIDs, SwampLilyConfig.ID);
		return gameObject;
	}

	// Token: 0x060006D5 RID: 1749 RVA: 0x0002C7B0 File Offset: 0x0002A9B0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060006D6 RID: 1750 RVA: 0x0002C7B2 File Offset: 0x0002A9B2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004C5 RID: 1221
	public static string ID = "SwampLily";

	// Token: 0x040004C6 RID: 1222
	public const string SEED_ID = "SwampLilySeed";
}
