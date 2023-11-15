using System;
using UnityEngine;

// Token: 0x0200019C RID: 412
public class TriggerToggleOnAnimator : MonoBehaviour
{
	// Token: 0x06000702 RID: 1794 RVA: 0x0002E31C File Offset: 0x0002C71C
	protected virtual void Awake()
	{
		this.m_targetParameterHash = Animator.StringToHash(this.m_targetParameter);
	}

	// Token: 0x040005D2 RID: 1490
	[SerializeField]
	public string m_triggerToReceive = string.Empty;

	// Token: 0x040005D3 RID: 1491
	[SerializeField]
	public Animator m_targetAnimator;

	// Token: 0x040005D4 RID: 1492
	[SerializeField]
	public string m_targetParameter = string.Empty;

	// Token: 0x040005D5 RID: 1493
	[SerializeField]
	public bool m_initialValue;

	// Token: 0x040005D6 RID: 1494
	[NonSerialized]
	public int m_targetParameterHash;
}
