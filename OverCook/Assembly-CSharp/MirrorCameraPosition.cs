using System;
using UnityEngine;

// Token: 0x0200014C RID: 332
[ExecuteInEditMode]
public class MirrorCameraPosition : MonoBehaviour
{
	// Token: 0x060005E9 RID: 1513 RVA: 0x0002B834 File Offset: 0x00029C34
	private void Awake()
	{
		this.LateUpdate();
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x0002B83C File Offset: 0x00029C3C
	private void LateUpdate()
	{
		if (this.m_mirrorTransform != null)
		{
			Vector3 position = Camera.main.transform.position;
			Vector3 lhs = position - this.m_mirrorTransform.position;
			Vector3 up = this.m_mirrorTransform.up;
			base.transform.position = position - 2f * Vector3.Dot(lhs, up) * up;
		}
	}

	// Token: 0x040004F7 RID: 1271
	[SerializeField]
	private Transform m_mirrorTransform;
}
