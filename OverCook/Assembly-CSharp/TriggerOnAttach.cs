using System;
using UnityEngine;

// Token: 0x0200018B RID: 395
[RequireComponent(typeof(AttachStation))]
public class TriggerOnAttach : MonoBehaviour
{
	// Token: 0x040005A1 RID: 1441
	[SerializeField]
	public string m_attachTrigger;

	// Token: 0x040005A2 RID: 1442
	[SerializeField]
	public string m_detachTrigger;

	// Token: 0x040005A3 RID: 1443
	[SerializeField]
	public GameObject m_triggerTarget;
}
