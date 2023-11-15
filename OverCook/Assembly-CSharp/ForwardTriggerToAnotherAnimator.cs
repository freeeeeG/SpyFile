using System;
using UnityEngine;

// Token: 0x02000105 RID: 261
public class ForwardTriggerToAnotherAnimator : StateMachineBehaviour
{
	// Token: 0x060004E2 RID: 1250 RVA: 0x00028A46 File Offset: 0x00026E46
	protected virtual void Awake()
	{
		this.m_receiveTriggerHash = Animator.StringToHash(this.m_receiveTrigger);
		this.m_sendTriggerNameHash = Animator.StringToHash(this.m_sendTriggerName);
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x00028A6C File Offset: 0x00026E6C
	public override void OnStateUpdate(Animator _animator, AnimatorStateInfo _animatorStateInfo, int _layerIndex)
	{
		if (_animator.GetBool(this.m_receiveTriggerHash))
		{
			if (this.m_consumeTrigger)
			{
				_animator.ResetTrigger(this.m_receiveTriggerHash);
			}
			SendTriggerToAnotherAnimator.Send(_animator.gameObject, this.m_objectName, this.m_sendTriggerNameHash);
		}
	}

	// Token: 0x0400043B RID: 1083
	[global::Tooltip("The name of the trigger on this animator on which to send the trigger to another object")]
	[SerializeField]
	private string m_receiveTrigger = string.Empty;

	// Token: 0x0400043C RID: 1084
	[global::Tooltip("Wehether the received trigger is unset on receiving")]
	[SerializeField]
	private bool m_consumeTrigger = true;

	// Token: 0x0400043D RID: 1085
	[global::Tooltip("The name of the object with the animator in question. Must have an animator")]
	[SerializeField]
	private string m_objectName = string.Empty;

	// Token: 0x0400043E RID: 1086
	[global::Tooltip("Name of the trigger to send. Trigger must be a parameter in the Animator")]
	[SerializeField]
	private string m_sendTriggerName = string.Empty;

	// Token: 0x0400043F RID: 1087
	private int m_receiveTriggerHash;

	// Token: 0x04000440 RID: 1088
	private int m_sendTriggerNameHash;
}
