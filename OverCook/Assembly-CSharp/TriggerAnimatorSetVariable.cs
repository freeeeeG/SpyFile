using System;
using UnityEngine;

// Token: 0x02000171 RID: 369
public class TriggerAnimatorSetVariable : MonoBehaviour
{
	// Token: 0x0600067D RID: 1661 RVA: 0x0002D158 File Offset: 0x0002B558
	protected virtual void Awake()
	{
		this.m_variableNameHash = Animator.StringToHash(this.m_variableName);
	}

	// Token: 0x04000562 RID: 1378
	[SerializeField]
	public bool m_onAwake;

	// Token: 0x04000563 RID: 1379
	[SerializeField]
	[HideInInspectorTest("m_onAwake", false)]
	public string m_triggerToReceive;

	// Token: 0x04000564 RID: 1380
	[SerializeField]
	[AssignComponent(Editorbility.Editable)]
	public Animator m_targetAnimator;

	// Token: 0x04000565 RID: 1381
	[SerializeField]
	public string m_variableName;

	// Token: 0x04000566 RID: 1382
	[SerializeField]
	public AnimatorVariableType m_variableType;

	// Token: 0x04000567 RID: 1383
	[SerializeField]
	public bool m_randomValue;

	// Token: 0x04000568 RID: 1384
	[SerializeField]
	[HideInInspectorTest("m_variableType", AnimatorVariableType.Bool, "m_randomValue", false)]
	public bool m_boolValue;

	// Token: 0x04000569 RID: 1385
	[SerializeField]
	[HideInInspectorTest("m_variableType", AnimatorVariableType.Int, "m_randomValue", false)]
	public int m_intValue;

	// Token: 0x0400056A RID: 1386
	[SerializeField]
	[HideInInspectorTest("m_variableType", AnimatorVariableType.Int, "m_randomValue", true)]
	public int m_minIntValue;

	// Token: 0x0400056B RID: 1387
	[SerializeField]
	[HideInInspectorTest("m_variableType", AnimatorVariableType.Int, "m_randomValue", true)]
	public int m_maxIntValue;

	// Token: 0x0400056C RID: 1388
	[SerializeField]
	[HideInInspectorTest("m_variableType", AnimatorVariableType.Float, "m_randomValue", false)]
	public float m_floatValue;

	// Token: 0x0400056D RID: 1389
	[SerializeField]
	[HideInInspectorTest("m_variableType", AnimatorVariableType.Float, "m_randomValue", true)]
	public float m_minFloatValue;

	// Token: 0x0400056E RID: 1390
	[SerializeField]
	[HideInInspectorTest("m_variableType", AnimatorVariableType.Float, "m_randomValue", true)]
	public float m_maxFloatValue;

	// Token: 0x0400056F RID: 1391
	[NonSerialized]
	public int m_variableNameHash;
}
