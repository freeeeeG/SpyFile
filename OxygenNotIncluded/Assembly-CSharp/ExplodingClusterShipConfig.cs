using System;
using UnityEngine;

// Token: 0x0200027C RID: 636
public class ExplodingClusterShipConfig : IEntityConfig
{
	// Token: 0x06000CDC RID: 3292 RVA: 0x00047985 File Offset: 0x00045B85
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000CDD RID: 3293 RVA: 0x0004798C File Offset: 0x00045B8C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("ExplodingClusterShip", "ExplodingClusterShip", false);
		ClusterFXEntity clusterFXEntity = gameObject.AddOrGet<ClusterFXEntity>();
		clusterFXEntity.kAnimName = "rocket_self_destruct_kanim";
		clusterFXEntity.animName = "explode";
		return gameObject;
	}

	// Token: 0x06000CDE RID: 3294 RVA: 0x000479B9 File Offset: 0x00045BB9
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x000479BB File Offset: 0x00045BBB
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000772 RID: 1906
	public const string ID = "ExplodingClusterShip";
}
