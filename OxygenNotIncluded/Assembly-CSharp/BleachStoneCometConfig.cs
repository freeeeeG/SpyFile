using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000278 RID: 632
public class BleachStoneCometConfig : IEntityConfig
{
	// Token: 0x06000CCA RID: 3274 RVA: 0x00047647 File Offset: 0x00045847
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000CCB RID: 3275 RVA: 0x00047650 File Offset: 0x00045850
	public GameObject CreatePrefab()
	{
		float mass = ElementLoader.FindElementByHash(SimHashes.OxyRock).defaultValues.mass;
		GameObject gameObject = BaseCometConfig.BaseComet(BleachStoneCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.BLEACHSTONECOMET.NAME, "meteor_bleachstone_kanim", SimHashes.BleachStone, new Vector2(mass * 0.8f * 1f, mass * 1.2f * 1f), new Vector2(310.15f, 323.15f), "Meteor_dust_heavy_Impact", 1, SimHashes.ChlorineGas, SpawnFXHashes.MeteorImpactIce, 0.6f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(2, 4);
		component.explosionSpeedRange = new Vector2(4f, 7f);
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		component.addTiles = 1;
		component.addTilesMinHeight = 1;
		component.addTilesMaxHeight = 1;
		return gameObject;
	}

	// Token: 0x06000CCC RID: 3276 RVA: 0x00047721 File Offset: 0x00045921
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000CCD RID: 3277 RVA: 0x00047723 File Offset: 0x00045923
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x0400076E RID: 1902
	public static string ID = "BleachStoneComet";

	// Token: 0x0400076F RID: 1903
	private const int ADDED_CELLS = 1;
}
