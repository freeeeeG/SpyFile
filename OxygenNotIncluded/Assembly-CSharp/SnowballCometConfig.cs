using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000272 RID: 626
public class SnowballCometConfig : IEntityConfig
{
	// Token: 0x06000CA6 RID: 3238 RVA: 0x0004701E File Offset: 0x0004521E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000CA7 RID: 3239 RVA: 0x00047028 File Offset: 0x00045228
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseCometConfig.BaseComet(SnowballCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.SNOWBALLCOMET.NAME, "meteor_snow_kanim", SimHashes.Snow, new Vector2(3f, 20f), new Vector2(253.15f, 263.15f), "Meteor_snowball_Impact", 5, SimHashes.Void, SpawnFXHashes.None, 0.3f);
		Comet component = gameObject.GetComponent<Comet>();
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		component.splashRadius = 1;
		component.addTiles = 3;
		component.addTilesMinHeight = 1;
		component.addTilesMaxHeight = 2;
		return gameObject;
	}

	// Token: 0x06000CA8 RID: 3240 RVA: 0x000470B5 File Offset: 0x000452B5
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000CA9 RID: 3241 RVA: 0x000470B7 File Offset: 0x000452B7
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000765 RID: 1893
	public static string ID = "SnowballComet";
}
