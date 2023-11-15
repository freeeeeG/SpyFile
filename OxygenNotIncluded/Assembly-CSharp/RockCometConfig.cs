using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000266 RID: 614
public class RockCometConfig : IEntityConfig
{
	// Token: 0x06000C5E RID: 3166 RVA: 0x00045EA9 File Offset: 0x000440A9
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C5F RID: 3167 RVA: 0x00045EB0 File Offset: 0x000440B0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(RockCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.ROCKCOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		float mass = ElementLoader.FindElementByHash(SimHashes.Regolith).defaultValues.mass;
		comet.massRange = new Vector2(mass * 0.8f * 6f, mass * 1.2f * 6f);
		comet.temperatureRange = new Vector2(323.15f, 423.15f);
		comet.addTiles = 6;
		comet.addTilesMinHeight = 2;
		comet.addTilesMaxHeight = 8;
		comet.entityDamage = 20;
		comet.totalTileDamage = 0f;
		comet.splashRadius = 1;
		comet.impactSound = "Meteor_Large_Impact";
		comet.flyingSoundID = 2;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactDirt;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.Regolith, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_rock_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "fall_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.AddTag(GameTags.Comet);
		return gameObject;
	}

	// Token: 0x06000C60 RID: 3168 RVA: 0x00046011 File Offset: 0x00044211
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000C61 RID: 3169 RVA: 0x00046013 File Offset: 0x00044213
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000752 RID: 1874
	public static readonly string ID = "RockComet";

	// Token: 0x04000753 RID: 1875
	private const SimHashes element = SimHashes.Regolith;

	// Token: 0x04000754 RID: 1876
	private const int ADDED_CELLS = 6;
}
