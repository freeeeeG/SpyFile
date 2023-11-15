using System;
using UnityEngine;

// Token: 0x02000197 RID: 407
[AddComponentMenu("Scripts/Core/Components/TriggerTimer")]
public class TriggerTimer : MonoBehaviour
{
	// Token: 0x040005C0 RID: 1472
	[SerializeField]
	public string m_startTrigger;

	// Token: 0x040005C1 RID: 1473
	[SerializeField]
	public string m_completeTrigger;

	// Token: 0x040005C2 RID: 1474
	[SerializeField]
	public float m_time;

	// Token: 0x040005C3 RID: 1475
	[SerializeField]
	public bool m_startTiming;

	// Token: 0x040005C4 RID: 1476
	[SerializeField]
	public bool m_triggerAtStart;
}
