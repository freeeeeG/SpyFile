using System;
using UnityEngine;

// Token: 0x02000469 RID: 1129
[ExecuteInEditMode]
public class FaceCamera : MonoBehaviour
{
	// Token: 0x06001501 RID: 5377 RVA: 0x000728AC File Offset: 0x00070CAC
	private void Start()
	{
		this.FaceTheCamera();
	}

	// Token: 0x06001502 RID: 5378 RVA: 0x000728B4 File Offset: 0x00070CB4
	[ContextMenu("Face Camera")]
	private void FaceTheCamera()
	{
		if (this.m_camera == null)
		{
			this.m_camera = Camera.main;
		}
		base.transform.LookAt(base.transform.position + this.m_camera.transform.forward, Vector3.up);
	}

	// Token: 0x0400102C RID: 4140
	private Camera m_camera;
}
