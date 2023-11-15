using System;
using System.Reflection;
using UnityEngine;

// Token: 0x02000109 RID: 265
public class RandomizeStateBehaviourOnState : StateMachineBehaviourEx
{
	// Token: 0x060004EE RID: 1262 RVA: 0x00028C04 File Offset: 0x00027004
	public override void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		StateMachineBehaviourEx instance = this.m_otherBehaviour.GetInstance(_animator);
		Type type = instance.GetType();
		FieldInfo field = type.GetField(this.m_propertyName);
		float num = UnityEngine.Random.Range(this.m_minValue, this.m_maxValue);
		field.SetValue(instance, num);
	}

	// Token: 0x0400044F RID: 1103
	[SerializeField]
	private StateMachineBehaviourEx m_otherBehaviour;

	// Token: 0x04000450 RID: 1104
	[SerializeField]
	private string m_propertyName;

	// Token: 0x04000451 RID: 1105
	[SerializeField]
	private float m_minValue;

	// Token: 0x04000452 RID: 1106
	[SerializeField]
	private float m_maxValue = 1f;

	// Token: 0x04000453 RID: 1107
	private StateMachineBehaviourEx m_otherBehaviourInstance;
}
