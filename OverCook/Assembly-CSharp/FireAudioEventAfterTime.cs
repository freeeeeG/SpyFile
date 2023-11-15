using System;
using UnityEngine;

// Token: 0x02000104 RID: 260
public class FireAudioEventAfterTime : TriggerAfterTimeState
{
	// Token: 0x060004E0 RID: 1248 RVA: 0x000289F3 File Offset: 0x00026DF3
	protected override void PerformAction(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		_animator.gameObject.SendMessage("AudioTrigger", this.m_audioTag.ToString());
	}

	// Token: 0x0400043A RID: 1082
	[global::Tooltip("Name of the audio trigger to send. Trigger must be a parameter in the Animator")]
	[SerializeField]
	public GameOneShotAudioTag m_audioTag;
}
