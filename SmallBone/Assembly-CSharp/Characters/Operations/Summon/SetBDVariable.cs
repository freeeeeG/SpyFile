using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F32 RID: 3890
	[Serializable]
	public class SetBDVariable : IBDCharacterSetting
	{
		// Token: 0x06004BBE RID: 19390 RVA: 0x000DEC23 File Offset: 0x000DCE23
		public void ApplyTo(Character character)
		{
			character.GetComponent<BehaviorDesignerCommunicator>().SetVariable(this._variableName, this._variable);
		}

		// Token: 0x04003AF0 RID: 15088
		[SerializeField]
		private string _variableName;

		// Token: 0x04003AF1 RID: 15089
		[SerializeReference]
		[SubclassSelector]
		private SharedVariable _variable;
	}
}
