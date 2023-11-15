using System;
using UnityEngine;

// Token: 0x020005E1 RID: 1505
public class TriggerKillAttachments : MonoBehaviour
{
	// Token: 0x04001660 RID: 5728
	[SerializeField]
	public string m_trigger;

	// Token: 0x04001661 RID: 5729
	[SerializeField]
	[Mask(typeof(TriggerKillAttachments.KillMode))]
	public int m_killMode = 2;

	// Token: 0x04001662 RID: 5730
	private const int c_DefaultKillMode = 2;

	// Token: 0x020005E2 RID: 1506
	public enum KillMode
	{
		// Token: 0x04001664 RID: 5732
		Loose,
		// Token: 0x04001665 RID: 5733
		Attached
	}
}
