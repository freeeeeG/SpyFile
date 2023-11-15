using System;
using UnityEngine;

// Token: 0x020001CA RID: 458
[RequireComponent(typeof(Camera))]
public class ManualCameraRender : MonoBehaviour
{
	// Token: 0x060007DE RID: 2014 RVA: 0x00030DC4 File Offset: 0x0002F1C4
	private void Awake()
	{
		this.m_camera.enabled = false;
		this.m_camera.Render();
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x00030DDD File Offset: 0x0002F1DD
	private void CanvasRender()
	{
		this.m_camera.Render();
	}

	// Token: 0x0400063F RID: 1599
	[SerializeField]
	[AssignComponent(Editorbility.NonEditable)]
	private Camera m_camera;
}
