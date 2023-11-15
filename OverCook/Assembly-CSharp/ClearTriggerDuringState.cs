using System;
using UnityEngine;

// Token: 0x02000102 RID: 258
public class ClearTriggerDuringState : StateMachineBehaviour
{
	// Token: 0x060004D9 RID: 1241 RVA: 0x0002890D File Offset: 0x00026D0D
	protected virtual void Awake()
	{
		this.m_triggerNameHash = Animator.StringToHash(this.m_triggerName);
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x00028920 File Offset: 0x00026D20
	public override void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		_animator.SetBool(this.m_triggerNameHash, false);
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x0002892F File Offset: 0x00026D2F
	public override void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		_animator.SetBool(this.m_triggerNameHash, false);
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x0002893E File Offset: 0x00026D3E
	public override void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		_animator.SetBool(this.m_triggerNameHash, false);
	}

	// Token: 0x04000438 RID: 1080
	[SerializeField]
	private string m_triggerName = string.Empty;

	// Token: 0x04000439 RID: 1081
	private int m_triggerNameHash;
}
