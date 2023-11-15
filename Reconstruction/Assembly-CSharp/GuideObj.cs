using System;
using UnityEngine;

// Token: 0x0200025F RID: 607
public class GuideObj : MonoBehaviour
{
	// Token: 0x06000F40 RID: 3904 RVA: 0x00028695 File Offset: 0x00026895
	private void Awake()
	{
		Singleton<GameEvents>.Instance.onGuideObjCollect += this.CollectThisObj;
	}

	// Token: 0x06000F41 RID: 3905 RVA: 0x000286B0 File Offset: 0x000268B0
	private void CollectThisObj()
	{
		for (int i = 0; i < this.guideObjs.Length; i++)
		{
			Singleton<GuideGirlSystem>.Instance.AddGuideObj(this.guideObjs[i]);
		}
	}

	// Token: 0x06000F42 RID: 3906 RVA: 0x000286E2 File Offset: 0x000268E2
	private void OnDestroy()
	{
		Singleton<GameEvents>.Instance.onGuideObjCollect -= this.CollectThisObj;
	}

	// Token: 0x0400079F RID: 1951
	[SerializeField]
	private GameObject[] guideObjs;
}
