using System;
using UnityEngine;

// Token: 0x0200043F RID: 1087
public abstract class BaseTeleportalReceiver : MonoBehaviour, IParentable
{
	// Token: 0x06001410 RID: 5136 RVA: 0x0006DD7E File Offset: 0x0006C17E
	public Transform GetAttachPoint(GameObject gameObject)
	{
		return this.m_attachPoint;
	}

	// Token: 0x06001411 RID: 5137 RVA: 0x0006DD86 File Offset: 0x0006C186
	public bool HasClientSidePrediction()
	{
		return false;
	}

	// Token: 0x04000F81 RID: 3969
	[SerializeField]
	[AssignChild("TeleportPoint", Editorbility.NonEditable)]
	public Transform m_attachPoint;
}
