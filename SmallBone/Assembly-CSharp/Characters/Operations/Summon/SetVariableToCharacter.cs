using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F35 RID: 3893
	[Serializable]
	public class SetVariableToCharacter : IBDCharacterSetting
	{
		// Token: 0x06004BC5 RID: 19397 RVA: 0x000DECE1 File Offset: 0x000DCEE1
		public void ApplyTo(Character character)
		{
			this._communicator.SetVariable<SharedCharacter>(this._variableName, character);
		}

		// Token: 0x04003AF4 RID: 15092
		[SerializeField]
		private BehaviorDesignerCommunicator _communicator;

		// Token: 0x04003AF5 RID: 15093
		[SerializeField]
		private string _variableName;
	}
}
