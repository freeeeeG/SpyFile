using System;
using UnityEngine;

// Token: 0x0200027D RID: 637
public class EyeAnimation : IEntityConfig
{
	// Token: 0x06000CE1 RID: 3297 RVA: 0x000479C5 File Offset: 0x00045BC5
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000CE2 RID: 3298 RVA: 0x000479CC File Offset: 0x00045BCC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(EyeAnimation.ID, EyeAnimation.ID, false);
		gameObject.AddOrGet<KBatchedAnimController>().AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("anim_blinks_kanim")
		};
		return gameObject;
	}

	// Token: 0x06000CE3 RID: 3299 RVA: 0x00047A0E File Offset: 0x00045C0E
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000CE4 RID: 3300 RVA: 0x00047A10 File Offset: 0x00045C10
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000773 RID: 1907
	public static string ID = "EyeAnimation";
}
