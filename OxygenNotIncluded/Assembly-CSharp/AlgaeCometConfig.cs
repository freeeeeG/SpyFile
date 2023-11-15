using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000275 RID: 629
public class AlgaeCometConfig : IEntityConfig
{
	// Token: 0x06000CB8 RID: 3256 RVA: 0x00047409 File Offset: 0x00045609
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000CB9 RID: 3257 RVA: 0x00047410 File Offset: 0x00045610
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseCometConfig.BaseComet(AlgaeCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.ALGAECOMET.NAME, "meteor_algae_kanim", SimHashes.Algae, new Vector2(3f, 20f), new Vector2(310.15f, 323.15f), "Meteor_algae_Impact", 7, SimHashes.Void, SpawnFXHashes.MeteorImpactAlgae, 0.3f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(2, 4);
		component.explosionSpeedRange = new Vector2(4f, 7f);
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		return gameObject;
	}

	// Token: 0x06000CBA RID: 3258 RVA: 0x000474A7 File Offset: 0x000456A7
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x000474A9 File Offset: 0x000456A9
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x0400076A RID: 1898
	public static string ID = "AlgaeComet";
}
