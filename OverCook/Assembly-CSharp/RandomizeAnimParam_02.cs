using System;
using UnityEngine;

// Token: 0x02000108 RID: 264
public class RandomizeAnimParam_02 : StateMachineBehaviour
{
	// Token: 0x060004EB RID: 1259 RVA: 0x00028B6C File Offset: 0x00026F6C
	protected virtual void Awake()
	{
		this.m_parameterHash = Animator.StringToHash(this.m_parameter);
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x00028B7F File Offset: 0x00026F7F
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);
		animator.SetFloat(this.m_parameterHash, UnityEngine.Random.Range(this.m_minValue, this.m_maxValue));
	}

	// Token: 0x0400044B RID: 1099
	[SerializeField]
	private string m_parameter = string.Empty;

	// Token: 0x0400044C RID: 1100
	[SerializeField]
	private float m_minValue;

	// Token: 0x0400044D RID: 1101
	[SerializeField]
	private float m_maxValue = 1f;

	// Token: 0x0400044E RID: 1102
	private int m_parameterHash;
}
