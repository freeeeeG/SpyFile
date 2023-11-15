using System;
using UnityEngine;

// Token: 0x020005A0 RID: 1440
public class FollowPilotMoved : MonoBehaviour
{
	// Token: 0x06001B65 RID: 7013 RVA: 0x00087BB3 File Offset: 0x00085FB3
	private void Awake()
	{
		this.m_localPosition = this.Target.transform.InverseTransformPoint(base.transform.position);
	}

	// Token: 0x06001B66 RID: 7014 RVA: 0x00087BD6 File Offset: 0x00085FD6
	private void LateUpdate()
	{
		base.transform.position = this.Target.transform.TransformPoint(this.m_localPosition);
	}

	// Token: 0x0400157C RID: 5500
	public PilotMovement Target;

	// Token: 0x0400157D RID: 5501
	private Vector3 m_localPosition;
}
