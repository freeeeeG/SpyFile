using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000277 RID: 631
public class OxyliteCometConfig : IEntityConfig
{
	// Token: 0x06000CC4 RID: 3268 RVA: 0x00047577 File Offset: 0x00045777
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000CC5 RID: 3269 RVA: 0x00047580 File Offset: 0x00045780
	public GameObject CreatePrefab()
	{
		float mass = ElementLoader.FindElementByHash(SimHashes.OxyRock).defaultValues.mass;
		GameObject gameObject = BaseCometConfig.BaseComet(OxyliteCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.OXYLITECOMET.NAME, "meteor_oxylite_kanim", SimHashes.OxyRock, new Vector2(mass * 0.8f * 6f, mass * 1.2f * 6f), new Vector2(310.15f, 323.15f), "Meteor_dust_heavy_Impact", 0, SimHashes.Oxygen, SpawnFXHashes.MeteorImpactIce, 0.6f);
		Comet component = gameObject.GetComponent<Comet>();
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		component.addTiles = 6;
		component.addTilesMinHeight = 2;
		component.addTilesMaxHeight = 8;
		return gameObject;
	}

	// Token: 0x06000CC6 RID: 3270 RVA: 0x0004762F File Offset: 0x0004582F
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000CC7 RID: 3271 RVA: 0x00047631 File Offset: 0x00045831
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x0400076C RID: 1900
	public static string ID = "OxyliteComet";

	// Token: 0x0400076D RID: 1901
	private const int ADDED_CELLS = 6;
}
