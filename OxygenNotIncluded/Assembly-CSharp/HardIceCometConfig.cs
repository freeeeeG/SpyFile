using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000273 RID: 627
public class HardIceCometConfig : IEntityConfig
{
	// Token: 0x06000CAC RID: 3244 RVA: 0x000470CD File Offset: 0x000452CD
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000CAD RID: 3245 RVA: 0x000470D4 File Offset: 0x000452D4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(HardIceCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.HARDICECOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		float mass = ElementLoader.FindElementByHash(SimHashes.CrushedIce).defaultValues.mass;
		comet.massRange = new Vector2(mass * 0.8f * 6f, mass * 1.2f * 6f);
		comet.temperatureRange = new Vector2(173.15f, 248.15f);
		comet.explosionTemperatureRange = comet.temperatureRange;
		comet.addTiles = 6;
		comet.addTilesMinHeight = 2;
		comet.addTilesMaxHeight = 8;
		comet.entityDamage = 0;
		comet.totalTileDamage = 0f;
		comet.splashRadius = 1;
		comet.impactSound = "Meteor_ice_Impact";
		comet.flyingSoundID = 6;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactIce;
		comet.EXHAUST_ELEMENT = SimHashes.Oxygen;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.CrushedIce, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_ice_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "fall_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.AddTag(GameTags.Comet);
		return gameObject;
	}

	// Token: 0x06000CAE RID: 3246 RVA: 0x0004724B File Offset: 0x0004544B
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000CAF RID: 3247 RVA: 0x0004724D File Offset: 0x0004544D
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000766 RID: 1894
	public static readonly string ID = "HardIceComet";

	// Token: 0x04000767 RID: 1895
	private const SimHashes element = SimHashes.CrushedIce;

	// Token: 0x04000768 RID: 1896
	private const int ADDED_CELLS = 6;
}
