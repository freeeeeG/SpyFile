using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200015C RID: 348
public class SwampForagePlantPlantedConfig : IEntityConfig
{
	// Token: 0x060006C9 RID: 1737 RVA: 0x0002C32C File Offset: 0x0002A52C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060006CA RID: 1738 RVA: 0x0002C334 File Offset: 0x0002A534
	public GameObject CreatePrefab()
	{
		string id = "SwampForagePlantPlanted";
		string name = STRINGS.CREATURES.SPECIES.SWAMPFORAGEPLANTPLANTED.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SWAMPFORAGEPLANTPLANTED.DESC;
		float mass = 100f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("swamptuber_kanim"), "idle", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		gameObject.AddOrGet<SimTemperatureTransfer>();
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddOrGet<Uprootable>();
		gameObject.AddOrGet<UprootedMonitor>();
		gameObject.AddOrGet<Harvestable>();
		gameObject.AddOrGet<HarvestDesignatable>();
		gameObject.AddOrGet<SeedProducer>().Configure("SwampForagePlant", SeedProducer.ProductionType.DigOnly, 1);
		gameObject.AddOrGet<BasicForagePlantPlanted>();
		gameObject.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
		return gameObject;
	}

	// Token: 0x060006CB RID: 1739 RVA: 0x0002C404 File Offset: 0x0002A604
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060006CC RID: 1740 RVA: 0x0002C406 File Offset: 0x0002A606
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004C1 RID: 1217
	public const string ID = "SwampForagePlantPlanted";
}
