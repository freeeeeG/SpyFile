using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200026E RID: 622
public class FoodCometConfig : IEntityConfig
{
	// Token: 0x06000C8E RID: 3214 RVA: 0x00046A46 File Offset: 0x00044C46
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C8F RID: 3215 RVA: 0x00046A50 File Offset: 0x00044C50
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(FoodCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.FOODCOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		comet.massRange = new Vector2(0.2f, 0.5f);
		comet.temperatureRange = new Vector2(298.15f, 303.15f);
		comet.entityDamage = 0;
		comet.totalTileDamage = 0f;
		comet.splashRadius = 0;
		comet.impactSound = "Meteor_dust_heavy_Impact";
		comet.flyingSoundID = 0;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		comet.canHitDuplicants = true;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.Creature, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_sand_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "fall_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
		comet.EXHAUST_ELEMENT = SimHashes.Void;
		gameObject.AddTag(GameTags.Comet);
		gameObject.AddTag(GameTags.DeprecatedContent);
		return gameObject;
	}

	// Token: 0x06000C90 RID: 3216 RVA: 0x00046BBC File Offset: 0x00044DBC
	public void OnPrefabInit(GameObject go)
	{
		Comet component = go.GetComponent<Comet>();
		component.OnImpact = (System.Action)Delegate.Combine(component.OnImpact, new System.Action(delegate()
		{
			int i = 10;
			while (i > 0)
			{
				i--;
				Vector3 vector = go.transform.position + new Vector3((float)UnityEngine.Random.Range(-2, 3), (float)UnityEngine.Random.Range(-2, 3), 0f);
				if (!Grid.Solid[Grid.PosToCell(vector)])
				{
					GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("FoodSplat"), vector);
					gameObject.SetActive(true);
					gameObject.transform.Rotate(0f, 0f, (float)UnityEngine.Random.Range(-90, 90));
					i = 0;
				}
			}
		}));
	}

	// Token: 0x06000C91 RID: 3217 RVA: 0x00046C02 File Offset: 0x00044E02
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x0400075D RID: 1885
	public static string ID = "FoodComet";
}
