using System;
using UnityEngine;

// Token: 0x02000291 RID: 657
public class MouthAnimation : IEntityConfig
{
	// Token: 0x06000D62 RID: 3426 RVA: 0x0004B854 File Offset: 0x00049A54
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D63 RID: 3427 RVA: 0x0004B85C File Offset: 0x00049A5C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MouthAnimation.ID, MouthAnimation.ID, false);
		gameObject.AddOrGet<KBatchedAnimController>().AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("anim_mouth_flap_kanim")
		};
		return gameObject;
	}

	// Token: 0x06000D64 RID: 3428 RVA: 0x0004B89E File Offset: 0x00049A9E
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000D65 RID: 3429 RVA: 0x0004B8A0 File Offset: 0x00049AA0
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x040007C5 RID: 1989
	public static string ID = "MouthAnimation";
}
