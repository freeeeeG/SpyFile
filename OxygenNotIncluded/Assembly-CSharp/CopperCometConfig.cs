﻿using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000268 RID: 616
public class CopperCometConfig : IEntityConfig
{
	// Token: 0x06000C6A RID: 3178 RVA: 0x000461A2 File Offset: 0x000443A2
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C6B RID: 3179 RVA: 0x000461AC File Offset: 0x000443AC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(CopperCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.COPPERCOMET.NAME, true);
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
		primaryElement.SetElement(SimHashes.Cuprite, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_copper_kanim")
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

	// Token: 0x06000C6C RID: 3180 RVA: 0x00046306 File Offset: 0x00044506
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000C6D RID: 3181 RVA: 0x00046308 File Offset: 0x00044508
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000756 RID: 1878
	public static string ID = "CopperCometConfig";
}
