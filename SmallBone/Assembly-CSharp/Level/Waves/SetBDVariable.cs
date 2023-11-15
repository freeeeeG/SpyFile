using System;
using BehaviorDesigner.Runtime;
using Characters;
using UnityEngine;

namespace Level.Waves
{
	// Token: 0x02000566 RID: 1382
	[Serializable]
	public class SetBDVariable : IPinEnemyOption
	{
		// Token: 0x06001B33 RID: 6963 RVA: 0x000548D8 File Offset: 0x00052AD8
		public void ApplyTo(Character character)
		{
			BehaviorDesignerCommunicator component = character.GetComponent<BehaviorDesignerCommunicator>();
			if (component != null)
			{
				if (component.GetVariable(this._variableName) == null)
				{
					return;
				}
				component.SetVariable(this._variableName, this._variable);
			}
		}

		// Token: 0x0400175F RID: 5983
		[SerializeField]
		private string _variableName;

		// Token: 0x04001760 RID: 5984
		[SerializeReference]
		[SubclassSelector]
		private SharedVariable _variable;
	}
}
