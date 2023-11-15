using System;
using UnityEngine;

// Token: 0x02000638 RID: 1592
[AddComponentMenu("Scripts/Game/Environment/Workstation")]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AttachStation))]
[RequireComponent(typeof(Interactable))]
public class Workstation : MonoBehaviour
{
	// Token: 0x0400175C RID: 5980
	[SerializeField]
	public string m_chopAnimState = "Chop";

	// Token: 0x0400175D RID: 5981
	[SerializeField]
	public string m_chopTrigger = "Impact";

	// Token: 0x0400175E RID: 5982
	[SerializeField]
	public GameObject m_chopPFX;
}
