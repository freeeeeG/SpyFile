using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000276 RID: 630
public class PhosphoricCometConfig : IEntityConfig
{
	// Token: 0x06000CBE RID: 3262 RVA: 0x000474BF File Offset: 0x000456BF
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000CBF RID: 3263 RVA: 0x000474C8 File Offset: 0x000456C8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseCometConfig.BaseComet(PhosphoricCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.PHOSPHORICCOMET.NAME, "meteor_phosphoric_kanim", SimHashes.Phosphorite, new Vector2(3f, 20f), new Vector2(310.15f, 323.15f), "Meteor_dust_heavy_Impact", 0, SimHashes.Void, SpawnFXHashes.MeteorImpactPhosphoric, 0.3f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(1, 2);
		component.explosionSpeedRange = new Vector2(4f, 7f);
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		return gameObject;
	}

	// Token: 0x06000CC0 RID: 3264 RVA: 0x0004755F File Offset: 0x0004575F
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000CC1 RID: 3265 RVA: 0x00047561 File Offset: 0x00045761
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x0400076B RID: 1899
	public static string ID = "PhosphoricComet";
}
