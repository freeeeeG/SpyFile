using System;
using UnityEngine;

// Token: 0x02000063 RID: 99
public class LoadGameResource : MonoBehaviour
{
	// Token: 0x060001D2 RID: 466 RVA: 0x00008641 File Offset: 0x00006841
	private void Awake()
	{
		GameResourceLoader.Load();
		if (this._waitForCompletion)
		{
			GameResourceLoader.instance.WaitForCompletion();
		}
	}

	// Token: 0x0400019A RID: 410
	[SerializeField]
	private bool _waitForCompletion;
}
