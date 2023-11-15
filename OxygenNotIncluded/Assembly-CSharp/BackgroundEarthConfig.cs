using System;
using UnityEngine;

// Token: 0x02000260 RID: 608
public class BackgroundEarthConfig : IEntityConfig
{
	// Token: 0x06000C36 RID: 3126 RVA: 0x000458CA File Offset: 0x00043ACA
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C37 RID: 3127 RVA: 0x000458D4 File Offset: 0x00043AD4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(BackgroundEarthConfig.ID, BackgroundEarthConfig.ID, true);
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("earth_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "idle";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
		gameObject.AddOrGet<LoopingSounds>();
		return gameObject;
	}

	// Token: 0x06000C38 RID: 3128 RVA: 0x0004593F File Offset: 0x00043B3F
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000C39 RID: 3129 RVA: 0x00045941 File Offset: 0x00043B41
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000747 RID: 1863
	public static string ID = "BackgroundEarth";
}
