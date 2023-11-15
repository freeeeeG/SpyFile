using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200026A RID: 618
public class FullereneCometConfig : IEntityConfig
{
	// Token: 0x06000C76 RID: 3190 RVA: 0x0004649A File Offset: 0x0004469A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000C77 RID: 3191 RVA: 0x000464A4 File Offset: 0x000446A4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseCometConfig.BaseComet(FullereneCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.FULLERENECOMET.NAME, "meteor_fullerene_kanim", SimHashes.Fullerene, new Vector2(3f, 20f), new Vector2(323.15f, 423.15f), "Meteor_Medium_Impact", 1, SimHashes.CarbonDioxide, SpawnFXHashes.MeteorImpactMetal, 0.6f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(2, 4);
		component.entityDamage = 15;
		component.totalTileDamage = 0.5f;
		component.affectedByDifficulty = false;
		return gameObject;
	}

	// Token: 0x06000C78 RID: 3192 RVA: 0x0004652E File Offset: 0x0004472E
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000C79 RID: 3193 RVA: 0x00046530 File Offset: 0x00044730
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000758 RID: 1880
	public static readonly string ID = "FullereneComet";
}
