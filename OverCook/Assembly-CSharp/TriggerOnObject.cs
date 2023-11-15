using System;
using UnityEngine;

// Token: 0x0200018E RID: 398
public class TriggerOnObject : MonoBehaviour
{
	// Token: 0x040005A5 RID: 1445
	[SerializeField]
	public string m_trigger;

	// Token: 0x040005A6 RID: 1446
	[SerializeField]
	public string m_triggerToFire;

	// Token: 0x040005A7 RID: 1447
	[SerializeField]
	public GameObject m_targetObject;

	// Token: 0x040005A8 RID: 1448
	[SerializeField]
	public GameObject[] m_targetObjects;
}
