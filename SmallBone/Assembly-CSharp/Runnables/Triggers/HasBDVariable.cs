using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x0200035A RID: 858
	public class HasBDVariable : Trigger
	{
		// Token: 0x06001006 RID: 4102 RVA: 0x0002FE2B File Offset: 0x0002E02B
		protected override bool Check()
		{
			return this._communicator.GetVariable(this._variableName).GetValue() != null;
		}

		// Token: 0x04000D1B RID: 3355
		[SerializeField]
		private BehaviorDesignerCommunicator _communicator;

		// Token: 0x04000D1C RID: 3356
		[SerializeField]
		private string _variableName = "Target";
	}
}
