using System;
using UnityEngine;

// Token: 0x0200013F RID: 319
[ExecuteInEditMode]
public class LateUpdateScale : MonoBehaviour
{
	// Token: 0x060005BA RID: 1466 RVA: 0x0002ABC4 File Offset: 0x00028FC4
	private void OnValidate()
	{
		this.m_localScale.x = Mathf.Max(this.m_localScale.x, 0.1f);
		this.m_localScale.y = Mathf.Max(this.m_localScale.y, 0.1f);
		this.m_localScale.z = Mathf.Max(this.m_localScale.z, 0.1f);
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x0002AC31 File Offset: 0x00029031
	private void LateUpdate()
	{
		base.transform.localScale = this.m_localScale;
	}

	// Token: 0x040004BC RID: 1212
	[SerializeField]
	private Vector3 m_localScale = Vector3.one;
}
