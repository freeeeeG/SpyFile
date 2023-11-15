using System;
using UnityEngine;

// Token: 0x0200010C RID: 268
public class SendTriggerToAnotherAnimator : TriggerAfterTimeState
{
	// Token: 0x060004F8 RID: 1272 RVA: 0x00028DFF File Offset: 0x000271FF
	protected virtual void Awake()
	{
		this.m_triggerNameHash = Animator.StringToHash(this.m_triggerName);
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x00028E12 File Offset: 0x00027212
	protected override void PerformAction(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		SendTriggerToAnotherAnimator.Send(_animator.gameObject, this.m_objectName, this.m_triggerNameHash);
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x00028E2C File Offset: 0x0002722C
	public static void Send(GameObject _sender, string _objectName, string _trigger)
	{
		GameObject obj = GameObject.Find(_objectName);
		Animator animator = obj.RequestComponentRecursive<Animator>();
		animator.SetTrigger(_trigger);
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x00028E50 File Offset: 0x00027250
	public static void Send(GameObject _sender, string _objectName, int _trigger)
	{
		GameObject obj = GameObject.Find(_objectName);
		Animator animator = obj.RequestComponentRecursive<Animator>();
		animator.SetTrigger(_trigger);
	}

	// Token: 0x0400045C RID: 1116
	[global::Tooltip("The name of the object with the animator in question. Must have an animator")]
	[SerializeField]
	private string m_objectName = string.Empty;

	// Token: 0x0400045D RID: 1117
	[global::Tooltip("Name of the trigger to send. Trigger must be a parameter in the Animator")]
	[SerializeField]
	private string m_triggerName = string.Empty;

	// Token: 0x0400045E RID: 1118
	private int m_triggerNameHash;
}
