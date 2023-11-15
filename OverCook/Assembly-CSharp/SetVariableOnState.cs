using System;
using UnityEngine;

// Token: 0x02000110 RID: 272
public class SetVariableOnState : TriggerAfterTimeState
{
	// Token: 0x0600050A RID: 1290 RVA: 0x000290E4 File Offset: 0x000274E4
	protected virtual void Awake()
	{
		if (this.m_boolVariable != null)
		{
			if (string.IsNullOrEmpty(this.m_boolVariable.VariableName))
			{
				this.m_boolVariable = null;
			}
			else
			{
				this.m_boolVariable.MakeHash();
			}
		}
		if (this.m_intVariable != null)
		{
			if (string.IsNullOrEmpty(this.m_intVariable.VariableName))
			{
				this.m_intVariable = null;
			}
			else
			{
				this.m_intVariable.MakeHash();
			}
		}
		if (this.m_floatVariable != null)
		{
			if (string.IsNullOrEmpty(this.m_floatVariable.VariableName))
			{
				this.m_floatVariable = null;
			}
			else
			{
				this.m_floatVariable.MakeHash();
			}
		}
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x00029198 File Offset: 0x00027598
	protected override void PerformAction(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		Animator animator = _animator;
		if (this.m_animatorName != string.Empty)
		{
			animator = GameObject.Find(this.m_animatorName).RequireComponent<Animator>();
		}
		if (this.m_boolVariable != null && this.m_boolVariable.GetNameHash() != 0)
		{
			animator.SetBool(this.m_boolVariable.GetNameHash(), this.m_boolVariable.Value);
		}
		if (this.m_intVariable != null && this.m_intVariable.GetNameHash() != 0)
		{
			animator.SetInteger(this.m_intVariable.GetNameHash(), this.m_intVariable.Value);
		}
		if (this.m_floatVariable != null && this.m_floatVariable.GetNameHash() != 0)
		{
			animator.SetFloat(this.m_floatVariable.GetNameHash(), this.m_floatVariable.Value);
		}
	}

	// Token: 0x0400046E RID: 1134
	[SerializeField]
	private string m_animatorName = string.Empty;

	// Token: 0x0400046F RID: 1135
	[SerializeField]
	private SetVariableOnState.BoolVariableData m_boolVariable;

	// Token: 0x04000470 RID: 1136
	[SerializeField]
	private SetVariableOnState.IntVariableData m_intVariable;

	// Token: 0x04000471 RID: 1137
	[SerializeField]
	private SetVariableOnState.FloatVariableData m_floatVariable;

	// Token: 0x02000111 RID: 273
	[Serializable]
	private class BoolVariableData
	{
		// Token: 0x0600050D RID: 1293 RVA: 0x0002927A File Offset: 0x0002767A
		public int GetNameHash()
		{
			return this.VariableNameHash;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00029282 File Offset: 0x00027682
		public void MakeHash()
		{
			this.VariableNameHash = Animator.StringToHash(this.VariableName);
		}

		// Token: 0x04000472 RID: 1138
		public string VariableName;

		// Token: 0x04000473 RID: 1139
		public bool Value;

		// Token: 0x04000474 RID: 1140
		[HideInInspector]
		private int VariableNameHash;
	}

	// Token: 0x02000112 RID: 274
	[Serializable]
	private class IntVariableData
	{
		// Token: 0x06000510 RID: 1296 RVA: 0x0002929D File Offset: 0x0002769D
		public int GetNameHash()
		{
			return this.VariableNameHash;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x000292A5 File Offset: 0x000276A5
		public void MakeHash()
		{
			this.VariableNameHash = Animator.StringToHash(this.VariableName);
		}

		// Token: 0x04000475 RID: 1141
		public string VariableName;

		// Token: 0x04000476 RID: 1142
		public int Value;

		// Token: 0x04000477 RID: 1143
		[HideInInspector]
		private int VariableNameHash;
	}

	// Token: 0x02000113 RID: 275
	[Serializable]
	private class FloatVariableData
	{
		// Token: 0x06000513 RID: 1299 RVA: 0x000292C0 File Offset: 0x000276C0
		public int GetNameHash()
		{
			return this.VariableNameHash;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x000292C8 File Offset: 0x000276C8
		public void MakeHash()
		{
			this.VariableNameHash = Animator.StringToHash(this.VariableName);
		}

		// Token: 0x04000478 RID: 1144
		public string VariableName;

		// Token: 0x04000479 RID: 1145
		public float Value;

		// Token: 0x0400047A RID: 1146
		[HideInInspector]
		private int VariableNameHash;
	}
}
