using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000270 RID: 624
public class UraniumCometConfig : IEntityConfig
{
	// Token: 0x06000C9A RID: 3226 RVA: 0x00046DCA File Offset: 0x00044FCA
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000C9B RID: 3227 RVA: 0x00046DD4 File Offset: 0x00044FD4
	public GameObject CreatePrefab()
	{
		float mass = ElementLoader.FindElementByHash(SimHashes.UraniumOre).defaultValues.mass;
		GameObject gameObject = BaseCometConfig.BaseComet(UraniumCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.URANIUMORECOMET.NAME, "meteor_uranium_kanim", SimHashes.UraniumOre, new Vector2(mass * 0.8f * 6f, mass * 1.2f * 6f), new Vector2(323.15f, 403.15f), "Meteor_Nuclear_Impact", 3, SimHashes.CarbonDioxide, SpawnFXHashes.MeteorImpactUranium, 0.6f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(1, 2);
		component.entityDamage = 15;
		component.totalTileDamage = 0f;
		component.addTiles = 6;
		component.addTilesMinHeight = 1;
		component.addTilesMaxHeight = 1;
		return gameObject;
	}

	// Token: 0x06000C9C RID: 3228 RVA: 0x00046E91 File Offset: 0x00045091
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000C9D RID: 3229 RVA: 0x00046E93 File Offset: 0x00045093
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x0400075F RID: 1887
	public static readonly string ID = "UraniumComet";

	// Token: 0x04000760 RID: 1888
	private const SimHashes element = SimHashes.UraniumOre;

	// Token: 0x04000761 RID: 1889
	private const int ADDED_CELLS = 6;
}
