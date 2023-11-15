using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000147 RID: 327
public class ForestForagePlantPlantedConfig : IEntityConfig
{
	// Token: 0x0600065A RID: 1626 RVA: 0x00029618 File Offset: 0x00027818
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x00029620 File Offset: 0x00027820
	public GameObject CreatePrefab()
	{
		string id = "ForestForagePlantPlanted";
		string name = STRINGS.CREATURES.SPECIES.FORESTFORAGEPLANTPLANTED.NAME;
		string desc = STRINGS.CREATURES.SPECIES.FORESTFORAGEPLANTPLANTED.DESC;
		float mass = 100f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("podmelon_kanim"), "idle", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		gameObject.AddOrGet<SimTemperatureTransfer>();
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddOrGet<DrowningMonitor>();
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddOrGet<Uprootable>();
		gameObject.AddOrGet<UprootedMonitor>();
		gameObject.AddOrGet<Harvestable>();
		gameObject.AddOrGet<HarvestDesignatable>();
		gameObject.AddOrGet<SeedProducer>().Configure("ForestForagePlant", SeedProducer.ProductionType.DigOnly, 1);
		gameObject.AddOrGet<BasicForagePlantPlanted>();
		gameObject.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
		return gameObject;
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x000296F7 File Offset: 0x000278F7
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x000296F9 File Offset: 0x000278F9
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000466 RID: 1126
	public const string ID = "ForestForagePlantPlanted";
}
