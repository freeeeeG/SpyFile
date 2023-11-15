using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000267 RID: 615
public class IronCometConfig : IEntityConfig
{
	// Token: 0x06000C64 RID: 3172 RVA: 0x00046029 File Offset: 0x00044229
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C65 RID: 3173 RVA: 0x00046030 File Offset: 0x00044230
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(IronCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.IRONCOMET.NAME, true);
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
		primaryElement.SetElement(SimHashes.Iron, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_metal_kanim")
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

	// Token: 0x06000C66 RID: 3174 RVA: 0x0004618A File Offset: 0x0004438A
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000C67 RID: 3175 RVA: 0x0004618C File Offset: 0x0004438C
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000755 RID: 1877
	public static string ID = "IronComet";
}
