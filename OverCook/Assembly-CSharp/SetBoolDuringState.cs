using System;
using UnityEngine;

// Token: 0x0200010E RID: 270
public class SetBoolDuringState : StateMachineBehaviour
{
	// Token: 0x06000503 RID: 1283 RVA: 0x00028FAC File Offset: 0x000273AC
	protected virtual void Awake()
	{
		this.m_variableNameHash = Animator.StringToHash(this.m_variableName);
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x00028FC0 File Offset: 0x000273C0
	public override void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		this.m_animator = _animator;
		if (this.m_animatorName != string.Empty)
		{
			GameObject obj = GameObject.Find(this.m_animatorName);
			this.m_animator = obj.RequireComponent<Animator>();
		}
		if (this.m_variableNameHash != 0 && this.m_animator != null)
		{
			this.m_animator.SetBool(this.m_variableNameHash, !this.m_invert);
		}
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x00029037 File Offset: 0x00027437
	public override void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		if (this.m_variableNameHash != 0 && this.m_animator != null)
		{
			this.m_animator.SetBool(this.m_variableNameHash, this.m_invert);
		}
	}

	// Token: 0x04000464 RID: 1124
	[SerializeField]
	private string m_variableName;

	// Token: 0x04000465 RID: 1125
	[SerializeField]
	private string m_animatorName = string.Empty;

	// Token: 0x04000466 RID: 1126
	[SerializeField]
	private bool m_invert;

	// Token: 0x04000467 RID: 1127
	private int m_variableNameHash;

	// Token: 0x04000468 RID: 1128
	private Animator m_animator;
}
