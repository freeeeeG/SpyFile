using System;
using UnityEngine;

// Token: 0x02000BFA RID: 3066
[RequireComponent(typeof(Camera))]
public class CameraToWorld : MonoBehaviour
{
	// Token: 0x06003E9E RID: 16030 RVA: 0x0012B91A File Offset: 0x00129D1A
	private void Start()
	{
		this.myCamera = base.GetComponent<Camera>();
	}

	// Token: 0x06003E9F RID: 16031 RVA: 0x0012B928 File Offset: 0x00129D28
	private void OnPreCull()
	{
		Shader.SetGlobalMatrix("_Camera2World", this.myCamera.cameraToWorldMatrix);
	}

	// Token: 0x0400324C RID: 12876
	private Camera myCamera;
}
