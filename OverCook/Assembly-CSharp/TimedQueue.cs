using System;
using UnityEngine;

// Token: 0x02000162 RID: 354
public abstract class TimedQueue : MonoBehaviour
{
	// Token: 0x06000645 RID: 1605
	public abstract float GetQueueLength();

	// Token: 0x06000646 RID: 1606
	public abstract float GetDelay(int _index);

	// Token: 0x04000533 RID: 1331
	[SerializeField]
	public string m_startTrigger;

	// Token: 0x04000534 RID: 1332
	[SerializeField]
	public string m_cancelTrigger;

	// Token: 0x04000535 RID: 1333
	[SerializeField]
	public string m_endTrigger;

	// Token: 0x04000536 RID: 1334
	[SerializeField]
	public GameObject m_endTriggerTarget;

	// Token: 0x04000537 RID: 1335
	[SerializeField]
	public bool m_startOnAwake;

	// Token: 0x04000538 RID: 1336
	[Space]
	[SerializeField]
	public bool m_loopWhenFinished;

	// Token: 0x04000539 RID: 1337
	[SerializeField]
	public float m_loopDelay;
}
