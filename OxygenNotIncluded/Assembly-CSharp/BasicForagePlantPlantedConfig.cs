using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200013A RID: 314
public class BasicForagePlantPlantedConfig : IEntityConfig
{
	// Token: 0x06000617 RID: 1559 RVA: 0x00027F64 File Offset: 0x00026164
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x00027F6C File Offset: 0x0002616C
	public GameObject CreatePrefab()
	{
		string id = "BasicForagePlantPlanted";
		string name = STRINGS.CREATURES.SPECIES.BASICFORAGEPLANTPLANTED.NAME;
		string desc = STRINGS.CREATURES.SPECIES.BASICFORAGEPLANTPLANTED.DESC;
		float mass = 100f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("muckroot_kanim"), "idle", Grid.SceneLayer.BuildingBack, 1, 1, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
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
		gameObject.AddOrGet<SeedProducer>().Configure("BasicForagePlant", SeedProducer.ProductionType.DigOnly, 1);
		gameObject.AddOrGet<BasicForagePlantPlanted>();
		gameObject.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
		return gameObject;
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x00028043 File Offset: 0x00026243
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x00028045 File Offset: 0x00026245
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000436 RID: 1078
	public const string ID = "BasicForagePlantPlanted";
}
