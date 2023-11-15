using System;
using UnityEngine;

// Token: 0x02000442 RID: 1090
public abstract class BaseTeleportalSender : MonoBehaviour, IParentable
{
	// Token: 0x0600141F RID: 5151 RVA: 0x0006DFD2 File Offset: 0x0006C3D2
	public Transform GetAttachPoint(GameObject gameObject)
	{
		return this.m_attachPoint;
	}

	// Token: 0x06001420 RID: 5152 RVA: 0x0006DFDA File Offset: 0x0006C3DA
	public bool HasClientSidePrediction()
	{
		return false;
	}

	// Token: 0x04000F86 RID: 3974
	[SerializeField]
	[AssignChild("TeleportPoint", Editorbility.NonEditable)]
	public Transform m_attachPoint;
}
