using System;
using UnityEngine;

// Token: 0x02000183 RID: 387
public class TriggerDisableScript : MonoBehaviour
{
	// Token: 0x04000592 RID: 1426
	[SerializeField]
	public Behaviour m_script;

	// Token: 0x04000593 RID: 1427
	[SerializeField]
	public string m_enableTrigger;

	// Token: 0x04000594 RID: 1428
	[SerializeField]
	public string m_disableTrigger;

	// Token: 0x04000595 RID: 1429
	[SerializeField]
	public bool m_startEnabled = true;
}
