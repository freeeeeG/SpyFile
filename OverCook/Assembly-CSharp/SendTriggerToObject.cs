using System;
using UnityEngine;

// Token: 0x0200010D RID: 269
public class SendTriggerToObject : StateMachineBehaviour
{
	// Token: 0x060004FD RID: 1277 RVA: 0x00028E7A File Offset: 0x0002727A
	public override void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		this.m_timer = 0f;
		if (!this.m_orTriggerOnExit && this.m_triggerTime == 0f)
		{
			this.SendTrigger(this.GetTarget(_animator), this.m_triggerToSend);
		}
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x00028EB8 File Offset: 0x000272B8
	public override void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		if (this.m_orTriggerOnExit)
		{
			return;
		}
		if (this.m_timer < this.m_triggerTime)
		{
			this.m_timer += TimeManager.GetDeltaTime(_animator.gameObject) * _animator.speed;
			if (this.m_timer >= this.m_triggerTime)
			{
				this.SendTrigger(this.GetTarget(_animator), this.m_triggerToSend);
			}
		}
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x00028F25 File Offset: 0x00027325
	public override void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		if (this.m_orTriggerOnExit)
		{
			this.SendTrigger(this.GetTarget(_animator), this.m_triggerToSend);
		}
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x00028F48 File Offset: 0x00027348
	private GameObject GetTarget(Animator _animator)
	{
		GameObject result = _animator.gameObject;
		if (this.m_objectName != string.Empty)
		{
			result = GameObject.Find(this.m_objectName);
		}
		return result;
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x00028F7E File Offset: 0x0002737E
	private void SendTrigger(GameObject _object, string _trigger)
	{
		if (_object != null)
		{
			_object.SendMessage("OnTrigger", _trigger, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x0400045F RID: 1119
	[SerializeField]
	private string m_objectName;

	// Token: 0x04000460 RID: 1120
	[SerializeField]
	private string m_triggerToSend;

	// Token: 0x04000461 RID: 1121
	[SerializeField]
	private float m_triggerTime;

	// Token: 0x04000462 RID: 1122
	[SerializeField]
	private bool m_orTriggerOnExit;

	// Token: 0x04000463 RID: 1123
	private float m_timer;
}
