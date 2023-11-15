using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000271 RID: 625
public class SlimeCometConfig : IEntityConfig
{
	// Token: 0x06000CA0 RID: 3232 RVA: 0x00046EA9 File Offset: 0x000450A9
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000CA1 RID: 3233 RVA: 0x00046EB0 File Offset: 0x000450B0
	public GameObject CreatePrefab()
	{
		float mass = ElementLoader.FindElementByHash(SimHashes.SlimeMold).defaultValues.mass;
		GameObject gameObject = BaseCometConfig.BaseComet(SlimeCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.SLIMECOMET.NAME, "meteor_slime_kanim", SimHashes.SlimeMold, new Vector2(mass * 0.8f * 2f, mass * 1.2f * 2f), new Vector2(310.15f, 323.15f), "Meteor_slimeball_Impact", 7, SimHashes.ContaminatedOxygen, SpawnFXHashes.MeteorImpactSlime, 0.6f);
		Comet component = gameObject.GetComponent<Comet>();
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		component.explosionOreCount = new Vector2I(1, 2);
		component.explosionSpeedRange = new Vector2(4f, 7f);
		component.addTiles = 2;
		component.addTilesMinHeight = 1;
		component.addTilesMaxHeight = 2;
		component.diseaseIdx = Db.Get().Diseases.GetIndex("SlimeLung");
		component.addDiseaseCount = (int)(component.EXHAUST_RATE * 100000f);
		return gameObject;
	}

	// Token: 0x06000CA2 RID: 3234 RVA: 0x00046FB3 File Offset: 0x000451B3
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000CA3 RID: 3235 RVA: 0x00046FB8 File Offset: 0x000451B8
	public void OnSpawn(GameObject go)
	{
		go.GetComponent<PrimaryElement>().AddDisease(Db.Get().Diseases.GetIndex("SlimeLung"), (int)(UnityEngine.Random.Range(0.9f, 1.2f) * 50f * 100000f), "Meteor");
	}

	// Token: 0x04000762 RID: 1890
	public static string ID = "SlimeComet";

	// Token: 0x04000763 RID: 1891
	public const int ADDED_CELLS = 2;

	// Token: 0x04000764 RID: 1892
	private const SimHashes element = SimHashes.SlimeMold;
}
