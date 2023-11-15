using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.BehaviorDesigner
{
	// Token: 0x02000F68 RID: 3944
	public class IncreaseSharedIntVariable : CharacterOperation
	{
		// Token: 0x06004C8F RID: 19599 RVA: 0x000E3318 File Offset: 0x000E1518
		public override void Run(Character owner)
		{
			if (this._communicator == null)
			{
				this._communicator = owner.GetComponent<BehaviorDesignerCommunicator>();
			}
			SharedInt variable = this._communicator.GetVariable<SharedInt>(this._variableName);
			variable.SetValue(variable.Value + this._value);
		}

		// Token: 0x04003C3C RID: 15420
		[SerializeField]
		private BehaviorDesignerCommunicator _communicator;

		// Token: 0x04003C3D RID: 15421
		[SerializeField]
		private string _variableName;

		// Token: 0x04003C3E RID: 15422
		[SerializeField]
		private int _value;
	}
}
