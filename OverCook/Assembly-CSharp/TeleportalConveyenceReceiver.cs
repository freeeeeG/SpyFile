using System;
using UnityEngine;

// Token: 0x0200059E RID: 1438
[RequireComponent(typeof(Teleportal))]
[RequireComponent(typeof(StaticGridLocation))]
public class TeleportalConveyenceReceiver : MonoBehaviour
{
	// Token: 0x04001576 RID: 5494
	[SerializeField]
	[AssignChild("TeleportPoint", Editorbility.NonEditable)]
	public Transform m_attachPoint;
}
