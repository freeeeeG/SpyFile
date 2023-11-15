using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.BehaviorDesigner
{
	// Token: 0x02000F6B RID: 3947
	public sealed class SetBehaviorTreeVariable : Operation
	{
		// Token: 0x06004C97 RID: 19607 RVA: 0x000E3518 File Offset: 0x000E1718
		public override void Run()
		{
			this._communicator.SetVariable(this.variableName, this._variable);
		}

		// Token: 0x04003C47 RID: 15431
		[SerializeField]
		private BehaviorDesignerCommunicator _communicator;

		// Token: 0x04003C48 RID: 15432
		[SerializeField]
		private string variableName;

		// Token: 0x04003C49 RID: 15433
		[SerializeReference]
		[SubclassSelector]
		private SharedVariable _variable;
	}
}
