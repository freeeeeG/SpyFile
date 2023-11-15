using System;
using UnityEngine;

// Token: 0x02000116 RID: 278
public class TriggerAfterRandomTime : StateMachineBehaviour
{
	// Token: 0x0600051A RID: 1306 RVA: 0x0002932A File Offset: 0x0002772A
	protected virtual void Awake()
	{
		this.m_TriggerNameHash = Animator.StringToHash(this.TriggerName);
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x0002933D File Offset: 0x0002773D
	public override void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		this.m_timer = UnityEngine.Random.Range(this.MinValue, this.MaxValue);
		if (this.m_timer <= 0f)
		{
			this.GetAnimator(_animator).SetTrigger(this.m_TriggerNameHash);
		}
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x00029378 File Offset: 0x00027778
	public override void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		if (this.m_timer > 0f)
		{
			this.m_timer -= TimeManager.GetDeltaTime(_animator.gameObject) * _animator.speed;
			if (this.m_timer <= 0f)
			{
				this.GetAnimator(_animator).SetTrigger(this.m_TriggerNameHash);
			}
		}
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x000293D6 File Offset: 0x000277D6
	private Animator GetAnimator(Animator _thisAnimator)
	{
		if (this.m_animatorName != string.Empty)
		{
			return GameObject.Find(this.m_animatorName).RequireComponent<Animator>();
		}
		return _thisAnimator;
	}

	// Token: 0x0400047D RID: 1149
	[SerializeField]
	private string m_animatorName = string.Empty;

	// Token: 0x0400047E RID: 1150
	[global::Tooltip("Name of the trigger to send. Trigger must be a parameter in the Animator")]
	public string TriggerName = string.Empty;

	// Token: 0x0400047F RID: 1151
	public float MinValue;

	// Token: 0x04000480 RID: 1152
	public float MaxValue = 1f;

	// Token: 0x04000481 RID: 1153
	private float m_timer;

	// Token: 0x04000482 RID: 1154
	private int m_TriggerNameHash;
}
