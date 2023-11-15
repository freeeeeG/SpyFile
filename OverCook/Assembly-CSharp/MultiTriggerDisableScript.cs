using System;
using UnityEngine;

// Token: 0x02000154 RID: 340
public class MultiTriggerDisableScript : MonoBehaviour
{
	// Token: 0x04000502 RID: 1282
	[SerializeField]
	public Behaviour m_script;

	// Token: 0x04000503 RID: 1283
	[SerializeField]
	public TriggerPair[] m_triggers = new TriggerPair[0];

	// Token: 0x04000504 RID: 1284
	[SerializeField]
	public bool m_startEnabled = true;
}
