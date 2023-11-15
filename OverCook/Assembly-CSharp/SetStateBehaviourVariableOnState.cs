using System;
using System.Reflection;
using UnityEngine;

// Token: 0x0200010F RID: 271
public class SetStateBehaviourVariableOnState : StateMachineBehaviourEx
{
	// Token: 0x06000507 RID: 1287 RVA: 0x00029074 File Offset: 0x00027474
	protected virtual void Awake()
	{
		this.m_animatorVariableHash = Animator.StringToHash(this.m_animatorVariable);
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x00029088 File Offset: 0x00027488
	public override void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		StateMachineBehaviourEx instance = this.m_otherBehaviour.GetInstance(_animator);
		Type type = instance.GetType();
		FieldInfo field = type.GetField(this.m_behaviourVariable);
		object value = AnimatorUtils.GetValue(_animator, this.m_animatorVariableHash, this.m_valueType);
		field.SetValue(instance, value);
	}

	// Token: 0x04000469 RID: 1129
	[SerializeField]
	private StateMachineBehaviourEx m_otherBehaviour;

	// Token: 0x0400046A RID: 1130
	[SerializeField]
	private string m_behaviourVariable;

	// Token: 0x0400046B RID: 1131
	[SerializeField]
	private string m_animatorVariable;

	// Token: 0x0400046C RID: 1132
	[SerializeField]
	private AnimatorVariableType m_valueType;

	// Token: 0x0400046D RID: 1133
	private int m_animatorVariableHash;
}
