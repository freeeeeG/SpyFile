using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F34 RID: 3892
	[Serializable]
	public class SetSummonCount : IBDCharacterSetting
	{
		// Token: 0x06004BC2 RID: 19394 RVA: 0x000DEC60 File Offset: 0x000DCE60
		public void ApplyTo(Character character)
		{
			SharedInt variable = this._communicator.GetVariable<SharedInt>(this._variableName);
			int value = variable.Value;
			variable.Value = value + 1;
			character.onDie += delegate()
			{
				SharedInt variable2 = this._communicator.GetVariable<SharedInt>(this._variableName);
				int value2 = variable2.Value;
				variable2.Value = value2 - 1;
			};
		}

		// Token: 0x04003AF2 RID: 15090
		[SerializeField]
		private BehaviorDesignerCommunicator _communicator;

		// Token: 0x04003AF3 RID: 15091
		[SerializeField]
		private string _variableName = "SummonCount";
	}
}
