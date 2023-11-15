using System;
using UnityEngine;

// Token: 0x02000188 RID: 392
public class TriggerOnAnimator : MonoBehaviour
{
	// Token: 0x060006C6 RID: 1734 RVA: 0x0002D92A File Offset: 0x0002BD2A
	protected virtual void Awake()
	{
		this.m_triggerToFireHash = Animator.StringToHash(this.m_triggerToFire);
	}

	// Token: 0x0400059B RID: 1435
	[SerializeField]
	public string m_triggerToReceive;

	// Token: 0x0400059C RID: 1436
	[SerializeField]
	public string m_triggerToFire;

	// Token: 0x0400059D RID: 1437
	[SerializeField]
	public Animator m_targetAnimator;

	// Token: 0x0400059E RID: 1438
	public int m_triggerToFireHash;
}
