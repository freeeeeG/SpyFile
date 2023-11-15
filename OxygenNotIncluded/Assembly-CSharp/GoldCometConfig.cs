using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000269 RID: 617
public class GoldCometConfig : IEntityConfig
{
	// Token: 0x06000C70 RID: 3184 RVA: 0x0004631E File Offset: 0x0004451E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C71 RID: 3185 RVA: 0x00046328 File Offset: 0x00044528
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(GoldCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.GOLDCOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		comet.massRange = new Vector2(3f, 20f);
		comet.temperatureRange = new Vector2(323.15f, 423.15f);
		comet.explosionOreCount = new Vector2I(2, 4);
		comet.entityDamage = 15;
		comet.totalTileDamage = 0.5f;
		comet.splashRadius = 1;
		comet.impactSound = "Meteor_Medium_Impact";
		comet.flyingSoundID = 1;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactMetal;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.GoldAmalgam, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_gold_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "fall_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
		gameObject.AddTag(GameTags.Comet);
		return gameObject;
	}

	// Token: 0x06000C72 RID: 3186 RVA: 0x00046482 File Offset: 0x00044682
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000C73 RID: 3187 RVA: 0x00046484 File Offset: 0x00044684
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000757 RID: 1879
	public static string ID = "GoldComet";
}
