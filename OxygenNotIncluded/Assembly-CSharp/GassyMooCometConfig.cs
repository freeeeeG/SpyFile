using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200026F RID: 623
public class GassyMooCometConfig : IEntityConfig
{
	// Token: 0x06000C94 RID: 3220 RVA: 0x00046C18 File Offset: 0x00044E18
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C95 RID: 3221 RVA: 0x00046C20 File Offset: 0x00044E20
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(GassyMooCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.GASSYMOOCOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		GassyMooComet gassyMooComet = gameObject.AddOrGet<GassyMooComet>();
		gassyMooComet.massRange = new Vector2(100f, 200f);
		gassyMooComet.EXHAUST_ELEMENT = SimHashes.Methane;
		gassyMooComet.temperatureRange = new Vector2(296.15f, 318.15f);
		gassyMooComet.entityDamage = 0;
		gassyMooComet.explosionOreCount = new Vector2I(0, 0);
		gassyMooComet.totalTileDamage = 0f;
		gassyMooComet.splashRadius = 1;
		gassyMooComet.impactSound = "Meteor_GassyMoo_Impact";
		gassyMooComet.flyingSoundID = 4;
		gassyMooComet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		gassyMooComet.addTiles = 0;
		gassyMooComet.affectedByDifficulty = false;
		gassyMooComet.lootOnDestroyedByMissile = new string[]
		{
			"Meat",
			"Meat",
			"Meat"
		};
		gassyMooComet.destroyOnExplode = false;
		gassyMooComet.craterPrefabs = new string[]
		{
			"Moo"
		};
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.Creature, true);
		primaryElement.Temperature = (gassyMooComet.temperatureRange.x + gassyMooComet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_gassymoo_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "fall_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.AddTag(GameTags.Comet);
		return gameObject;
	}

	// Token: 0x06000C96 RID: 3222 RVA: 0x00046DB2 File Offset: 0x00044FB2
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000C97 RID: 3223 RVA: 0x00046DB4 File Offset: 0x00044FB4
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x0400075E RID: 1886
	public static string ID = "GassyMoo";
}
