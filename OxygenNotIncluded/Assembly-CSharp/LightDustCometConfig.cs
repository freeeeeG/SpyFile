using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000274 RID: 628
public class LightDustCometConfig : IEntityConfig
{
	// Token: 0x06000CB2 RID: 3250 RVA: 0x00047263 File Offset: 0x00045463
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000CB3 RID: 3251 RVA: 0x0004726C File Offset: 0x0004546C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(LightDustCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.LIGHTDUSTCOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		comet.massRange = new Vector2(10f, 14f);
		comet.temperatureRange = new Vector2(223.15f, 253.15f);
		comet.explosionTemperatureRange = comet.temperatureRange;
		comet.explosionOreCount = new Vector2I(1, 2);
		comet.explosionSpeedRange = new Vector2(4f, 7f);
		comet.entityDamage = 0;
		comet.totalTileDamage = 0f;
		comet.splashRadius = 0;
		comet.impactSound = "Meteor_dust_light_Impact";
		comet.flyingSoundID = 0;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactLightDust;
		comet.EXHAUST_ELEMENT = SimHashes.Void;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.Regolith, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_dust_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "fall_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
		gameObject.AddTag(GameTags.Comet);
		return gameObject;
	}

	// Token: 0x06000CB4 RID: 3252 RVA: 0x000473F1 File Offset: 0x000455F1
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000CB5 RID: 3253 RVA: 0x000473F3 File Offset: 0x000455F3
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000769 RID: 1897
	public static string ID = "LightDustComet";
}
