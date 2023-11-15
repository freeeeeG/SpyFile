using System;
using UnityEngine;

// Token: 0x02000198 RID: 408
[AddComponentMenu("Scripts/Core/Components/Trigger Timer On Animator")]
public class TriggerTimerOnAnimator : MonoBehaviour
{
	// Token: 0x060006F1 RID: 1777 RVA: 0x0002E00B File Offset: 0x0002C40B
	private void Awake()
	{
		if (this.m_startTimerOnAwake)
		{
			this.m_timer = this.m_time;
		}
		this.m_onCompleteTriggerHash = Animator.StringToHash(this.m_onCompleteTrigger);
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x0002E035 File Offset: 0x0002C435
	private void OnTrigger(string _trigger)
	{
		if (this.m_startTimerTrigger == _trigger)
		{
			this.m_timer = this.m_time;
		}
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x0002E054 File Offset: 0x0002C454
	private void Update()
	{
		if (this.m_sendCompleteOnAwake)
		{
			if (this.m_targetAnimator != null)
			{
				this.m_targetAnimator.SetTrigger(this.m_onCompleteTriggerHash);
			}
			this.m_sendCompleteOnAwake = false;
		}
		if (this.m_timer > 0f)
		{
			this.m_timer -= TimeManager.GetDeltaTime(base.gameObject);
			if (this.m_timer <= 0f && this.m_targetAnimator != null)
			{
				this.m_targetAnimator.SetTrigger(this.m_onCompleteTriggerHash);
			}
		}
	}

	// Token: 0x040005C5 RID: 1477
	[SerializeField]
	private string m_startTimerTrigger;

	// Token: 0x040005C6 RID: 1478
	[SerializeField]
	private string m_onCompleteTrigger;

	// Token: 0x040005C7 RID: 1479
	[SerializeField]
	private Animator m_targetAnimator;

	// Token: 0x040005C8 RID: 1480
	[SerializeField]
	private float m_time;

	// Token: 0x040005C9 RID: 1481
	[SerializeField]
	private bool m_startTimerOnAwake;

	// Token: 0x040005CA RID: 1482
	[SerializeField]
	private bool m_sendCompleteOnAwake;

	// Token: 0x040005CB RID: 1483
	private float m_timer;

	// Token: 0x040005CC RID: 1484
	private int m_onCompleteTriggerHash;
}
