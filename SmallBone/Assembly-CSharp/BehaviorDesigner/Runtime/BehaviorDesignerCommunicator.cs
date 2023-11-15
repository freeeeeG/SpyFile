using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x0200143E RID: 5182
	[RequireComponent(typeof(BehaviorTree))]
	public class BehaviorDesignerCommunicator : MonoBehaviour
	{
		// Token: 0x060065A0 RID: 26016 RVA: 0x001261B5 File Offset: 0x001243B5
		private void Start()
		{
			if (this._behaviorTree == null)
			{
				this._behaviorTree = base.GetComponent<BehaviorTree>();
			}
		}

		// Token: 0x060065A1 RID: 26017 RVA: 0x001261D1 File Offset: 0x001243D1
		public T GetVariable<T>(string variableName) where T : SharedVariable
		{
			return (T)((object)this._behaviorTree.GetVariable(variableName));
		}

		// Token: 0x060065A2 RID: 26018 RVA: 0x001261E4 File Offset: 0x001243E4
		public SharedVariable GetVariable(string variableName)
		{
			return this._behaviorTree.GetVariable(variableName);
		}

		// Token: 0x060065A3 RID: 26019 RVA: 0x001261F2 File Offset: 0x001243F2
		public void SetVariable(string variableName, SharedVariable variableValue)
		{
			this._behaviorTree.SetVariable(variableName, variableValue);
		}

		// Token: 0x060065A4 RID: 26020 RVA: 0x00126201 File Offset: 0x00124401
		public void SetVariable<T>(string variableName, object variableValue) where T : SharedVariable
		{
			this.GetVariable<T>(variableName).SetValue(variableValue);
		}

		// Token: 0x040051CD RID: 20941
		[GetComponent]
		[SerializeField]
		private BehaviorTree _behaviorTree;
	}
}
