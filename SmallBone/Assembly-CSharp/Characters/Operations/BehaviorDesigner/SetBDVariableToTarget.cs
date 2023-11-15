using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.BehaviorDesigner
{
	// Token: 0x02000F6A RID: 3946
	public sealed class SetBDVariableToTarget : TargetedCharacterOperation
	{
		// Token: 0x06004C95 RID: 19605 RVA: 0x000E34EA File Offset: 0x000E16EA
		public override void Run(Character owner, Character target)
		{
			if (this._communicator == null)
			{
				this._communicator = owner.GetComponent<BehaviorDesignerCommunicator>();
			}
			this._communicator.SetVariable<SharedCharacter>(this.variableName, target);
		}

		// Token: 0x04003C45 RID: 15429
		[SerializeField]
		private BehaviorDesignerCommunicator _communicator;

		// Token: 0x04003C46 RID: 15430
		[SerializeField]
		private string variableName;
	}
}
