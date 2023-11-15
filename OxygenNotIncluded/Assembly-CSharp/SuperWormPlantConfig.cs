using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200015A RID: 346
public class SuperWormPlantConfig : IEntityConfig
{
	// Token: 0x060006BE RID: 1726 RVA: 0x0002C1AE File Offset: 0x0002A3AE
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x0002C1B8 File Offset: 0x0002A3B8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = WormPlantConfig.BaseWormPlant("SuperWormPlant", STRINGS.CREATURES.SPECIES.SUPERWORMPLANT.NAME, STRINGS.CREATURES.SPECIES.SUPERWORMPLANT.DESC, "wormwood_kanim", SuperWormPlantConfig.SUPER_DECOR, "WormSuperFruit");
		gameObject.AddOrGet<SeedProducer>().Configure("WormPlantSeed", SeedProducer.ProductionType.Harvest, 1);
		return gameObject;
	}

	// Token: 0x060006C0 RID: 1728 RVA: 0x0002C204 File Offset: 0x0002A404
	public void OnPrefabInit(GameObject prefab)
	{
		TransformingPlant transformingPlant = prefab.AddOrGet<TransformingPlant>();
		transformingPlant.SubscribeToTransformEvent(GameHashes.HarvestComplete);
		transformingPlant.transformPlantId = "WormPlant";
		prefab.GetComponent<KAnimControllerBase>().SetSymbolVisiblity("flower", false);
		prefab.AddOrGet<StandardCropPlant>().anims = SuperWormPlantConfig.animSet;
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x0002C252 File Offset: 0x0002A452
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004BB RID: 1211
	public const string ID = "SuperWormPlant";

	// Token: 0x040004BC RID: 1212
	public static readonly EffectorValues SUPER_DECOR = DECOR.BONUS.TIER1;

	// Token: 0x040004BD RID: 1213
	public const string SUPER_CROP_ID = "WormSuperFruit";

	// Token: 0x040004BE RID: 1214
	public const int CROP_YIELD = 8;

	// Token: 0x040004BF RID: 1215
	private static StandardCropPlant.AnimSet animSet = new StandardCropPlant.AnimSet
	{
		grow = "super_grow",
		grow_pst = "super_grow_pst",
		idle_full = "super_idle_full",
		wilt_base = "super_wilt",
		harvest = "super_harvest"
	};
}
