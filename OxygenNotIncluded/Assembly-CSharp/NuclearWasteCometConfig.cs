﻿using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200026C RID: 620
public class NuclearWasteCometConfig : IEntityConfig
{
	// Token: 0x06000C82 RID: 3202 RVA: 0x000466B4 File Offset: 0x000448B4
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x000466BC File Offset: 0x000448BC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(NuclearWasteCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.NUCLEAR_WASTE.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		comet.massRange = new Vector2(NuclearWasteCometConfig.MASS, NuclearWasteCometConfig.MASS);
		comet.EXHAUST_ELEMENT = SimHashes.Fallout;
		comet.EXHAUST_RATE = NuclearWasteCometConfig.MASS * 0.2f;
		comet.temperatureRange = new Vector2(473.15f, 573.15f);
		comet.entityDamage = 2;
		comet.totalTileDamage = 0.45f;
		comet.splashRadius = 0;
		comet.impactSound = "Meteor_Nuclear_Impact";
		comet.flyingSoundID = 3;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		comet.addTiles = 1;
		comet.diseaseIdx = Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.Id);
		comet.addDiseaseCount = 1000000;
		comet.affectedByDifficulty = false;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.Corium, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("nuclear_metldown_comet_fx_kanim")
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

	// Token: 0x06000C84 RID: 3204 RVA: 0x0004686B File Offset: 0x00044A6B
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000C85 RID: 3205 RVA: 0x0004686D File Offset: 0x00044A6D
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x0400075A RID: 1882
	public static string ID = "NuclearWasteComet";

	// Token: 0x0400075B RID: 1883
	public static float MASS = 1f;
}
