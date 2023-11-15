using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000263 RID: 611
public class CarePackageConfig : IEntityConfig
{
	// Token: 0x06000C52 RID: 3154 RVA: 0x00045C84 File Offset: 0x00043E84
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x00045C8C File Offset: 0x00043E8C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateLooseEntity(CarePackageConfig.ID, ITEMS.CARGO_CAPSULE.NAME, ITEMS.CARGO_CAPSULE.DESC, 1f, true, Assets.GetAnim("portal_carepackage_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f, false, 0, SimHashes.Creature, null);
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x00045CE6 File Offset: 0x00043EE6
	public void OnPrefabInit(GameObject go)
	{
		go.AddOrGet<CarePackage>();
	}

	// Token: 0x06000C55 RID: 3157 RVA: 0x00045CEF File Offset: 0x00043EEF
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000750 RID: 1872
	public static readonly string ID = "CarePackage";
}
