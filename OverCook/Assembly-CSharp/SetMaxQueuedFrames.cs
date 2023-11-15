using System;
using UnityEngine;

// Token: 0x020001D6 RID: 470
public class SetMaxQueuedFrames : MonoBehaviour
{
	// Token: 0x06000802 RID: 2050 RVA: 0x000313FC File Offset: 0x0002F7FC
	private void Awake()
	{
		QualitySettings.maxQueuedFrames = this.m_maxQueuedFrames;
	}

	// Token: 0x0400065E RID: 1630
	[SerializeField]
	private int m_maxQueuedFrames = 1;
}
