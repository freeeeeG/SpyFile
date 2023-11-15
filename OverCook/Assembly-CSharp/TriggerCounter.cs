using System;
using UnityEngine;

// Token: 0x0200017B RID: 379
public class TriggerCounter : MonoBehaviour
{
	// Token: 0x04000583 RID: 1411
	[SerializeField]
	public string m_inputTrigger;

	// Token: 0x04000584 RID: 1412
	[SerializeField]
	public string m_outputTrigger;

	// Token: 0x04000585 RID: 1413
	[SerializeField]
	public int m_count = 1;

	// Token: 0x04000586 RID: 1414
	[SerializeField]
	public bool m_ResetOnCountReached = true;
}
