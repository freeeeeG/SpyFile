using System;
using UnityEngine;

// Token: 0x02000117 RID: 279
public abstract class TriggerAfterTimeState : StateMachineBehaviour
{
	// Token: 0x0600051F RID: 1311
	protected abstract void PerformAction(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex);

	// Token: 0x06000520 RID: 1312 RVA: 0x0002896A File Offset: 0x00026D6A
	public override void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		this.m_timer = 0f;
		if (this.m_timer == this.m_triggerTime)
		{
			this.PerformAction(_animator, _stateInfo, _layerIndex);
		}
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x00028994 File Offset: 0x00026D94
	public override void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		if (this.m_timer < this.m_triggerTime)
		{
			this.m_timer += TimeManager.GetDeltaTime(_animator.gameObject) * _animator.speed;
			if (this.m_timer >= this.m_triggerTime)
			{
				this.PerformAction(_animator, _stateInfo, _layerIndex);
			}
		}
	}

	// Token: 0x04000483 RID: 1155
	[global::Tooltip("The time from the start of the animation when the action will be performed")]
	[SerializeField]
	private float m_triggerTime;

	// Token: 0x04000484 RID: 1156
	private float m_timer;
}
