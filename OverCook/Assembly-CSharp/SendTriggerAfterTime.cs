using System;
using UnityEngine;

// Token: 0x0200010B RID: 267
public class SendTriggerAfterTime : StateMachineBehaviourEx
{
	// Token: 0x060004F2 RID: 1266 RVA: 0x00028CCC File Offset: 0x000270CC
	protected virtual void Awake()
	{
		this.m_iTriggerName = Animator.StringToHash(this.TriggerName);
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x00028CDF File Offset: 0x000270DF
	public override void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		this.m_timer = 0f;
		if (!this.OrTriggerOnExit && this.TriggerTime == 0f)
		{
			this.GetAnimator(_animator).SetTrigger(this.m_iTriggerName);
		}
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x00028D1C File Offset: 0x0002711C
	public override void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		if (this.OrTriggerOnExit)
		{
			return;
		}
		if (this.m_timer < this.TriggerTime)
		{
			this.m_timer += TimeManager.GetDeltaTime(_animator.gameObject) * _animator.speed;
			if (this.m_timer >= this.TriggerTime)
			{
				this.GetAnimator(_animator).SetTrigger(this.m_iTriggerName);
			}
		}
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x00028D88 File Offset: 0x00027188
	public override void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		if (this.OrTriggerOnExit && this.m_timer < this.TriggerTime)
		{
			this.GetAnimator(_animator).SetTrigger(this.m_iTriggerName);
		}
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x00028DB8 File Offset: 0x000271B8
	private Animator GetAnimator(Animator _thisAnimator)
	{
		if (this.m_animatorName != string.Empty)
		{
			return GameObject.Find(this.m_animatorName).RequireComponent<Animator>();
		}
		return _thisAnimator;
	}

	// Token: 0x04000456 RID: 1110
	[SerializeField]
	private string m_animatorName = string.Empty;

	// Token: 0x04000457 RID: 1111
	[global::Tooltip("Name of the trigger to send. Trigger must be a parameter in the Animator")]
	public string TriggerName = string.Empty;

	// Token: 0x04000458 RID: 1112
	[global::Tooltip("The time from the start of the aniamtion when the trigger will be sent")]
	public float TriggerTime = 1f;

	// Token: 0x04000459 RID: 1113
	private float m_timer;

	// Token: 0x0400045A RID: 1114
	[SerializeField]
	private bool OrTriggerOnExit;

	// Token: 0x0400045B RID: 1115
	private int m_iTriggerName;
}
