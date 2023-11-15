using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200026D RID: 621
public class SatelliteCometConfig : IEntityConfig
{
	// Token: 0x06000C88 RID: 3208 RVA: 0x0004688D File Offset: 0x00044A8D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C89 RID: 3209 RVA: 0x00046894 File Offset: 0x00044A94
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(SatelliteCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.SATELLITE.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		comet.massRange = new Vector2(100f, 200f);
		comet.EXHAUST_ELEMENT = SimHashes.AluminumGas;
		comet.temperatureRange = new Vector2(473.15f, 573.15f);
		comet.entityDamage = 2;
		comet.explosionOreCount = new Vector2I(8, 8);
		comet.totalTileDamage = 2f;
		comet.splashRadius = 1;
		comet.impactSound = "Meteor_Large_Impact";
		comet.flyingSoundID = 1;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		comet.addTiles = 0;
		comet.craterPrefabs = new string[]
		{
			"PropSurfaceSatellite1",
			PropSurfaceSatellite2Config.ID,
			PropSurfaceSatellite3Config.ID
		};
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.Aluminum, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_rock_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "fall_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
		gameObject.AddTag(GameTags.Comet);
		gameObject.AddTag(GameTags.DeprecatedContent);
		return gameObject;
	}

	// Token: 0x06000C8A RID: 3210 RVA: 0x00046A2E File Offset: 0x00044C2E
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000C8B RID: 3211 RVA: 0x00046A30 File Offset: 0x00044C30
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x0400075C RID: 1884
	public static string ID = "SatelliteCometComet";
}
