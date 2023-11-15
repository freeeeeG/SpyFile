using System;
using UnityEngine;

// Token: 0x0200027A RID: 634
public class DeployingScoutLanderFXConfig : IEntityConfig
{
	// Token: 0x06000CD5 RID: 3285 RVA: 0x00047780 File Offset: 0x00045980
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000CD6 RID: 3286 RVA: 0x00047787 File Offset: 0x00045987
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("DeployingScoutLanderFXConfig", "DeployingScoutLanderFXConfig", false);
		ClusterFXEntity clusterFXEntity = gameObject.AddOrGet<ClusterFXEntity>();
		clusterFXEntity.kAnimName = "rover01_kanim";
		clusterFXEntity.animName = "landing";
		clusterFXEntity.animPlayMode = KAnim.PlayMode.Loop;
		return gameObject;
	}

	// Token: 0x06000CD7 RID: 3287 RVA: 0x000477BB File Offset: 0x000459BB
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CD8 RID: 3288 RVA: 0x000477BD File Offset: 0x000459BD
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000771 RID: 1905
	public const string ID = "DeployingScoutLanderFXConfig";
}
