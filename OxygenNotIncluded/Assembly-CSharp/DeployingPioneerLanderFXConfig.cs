using System;
using UnityEngine;

// Token: 0x02000279 RID: 633
public class DeployingPioneerLanderFXConfig : IEntityConfig
{
	// Token: 0x06000CD0 RID: 3280 RVA: 0x00047739 File Offset: 0x00045939
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000CD1 RID: 3281 RVA: 0x00047740 File Offset: 0x00045940
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("DeployingPioneerLanderFX", "DeployingPioneerLanderFX", false);
		ClusterFXEntity clusterFXEntity = gameObject.AddOrGet<ClusterFXEntity>();
		clusterFXEntity.kAnimName = "pioneer01_kanim";
		clusterFXEntity.animName = "landing";
		clusterFXEntity.animPlayMode = KAnim.PlayMode.Loop;
		return gameObject;
	}

	// Token: 0x06000CD2 RID: 3282 RVA: 0x00047774 File Offset: 0x00045974
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CD3 RID: 3283 RVA: 0x00047776 File Offset: 0x00045976
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000770 RID: 1904
	public const string ID = "DeployingPioneerLanderFX";
}
