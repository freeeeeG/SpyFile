using System;
using UnityEngine;

// Token: 0x02000107 RID: 263
public class RandomizeAnimParam : StateMachineBehaviour
{
	// Token: 0x060004E8 RID: 1256 RVA: 0x00028B11 File Offset: 0x00026F11
	protected virtual void Awake()
	{
		this.m_parameterHash = Animator.StringToHash(this.m_parameter);
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x00028B24 File Offset: 0x00026F24
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);
		animator.SetInteger(this.m_parameterHash, UnityEngine.Random.Range(this.m_minValue, this.m_maxValue + 1));
	}

	// Token: 0x04000447 RID: 1095
	[SerializeField]
	private string m_parameter = string.Empty;

	// Token: 0x04000448 RID: 1096
	[SerializeField]
	private int m_minValue;

	// Token: 0x04000449 RID: 1097
	[SerializeField]
	private int m_maxValue = 1;

	// Token: 0x0400044A RID: 1098
	private int m_parameterHash;
}
