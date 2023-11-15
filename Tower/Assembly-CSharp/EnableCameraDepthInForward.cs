using System;
using UnityEngine;

// Token: 0x02000002 RID: 2
[RequireComponent(typeof(Camera))]
public class EnableCameraDepthInForward : MonoBehaviour
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	private void Start()
	{
		this.Set();
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
	private void Set()
	{
		if (base.GetComponent<Camera>().depthTextureMode == DepthTextureMode.None)
		{
			base.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
		}
	}
}
