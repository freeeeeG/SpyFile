using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F30 RID: 3888
	[Serializable]
	public class LookAtOwnerLookingDirection : IBDCharacterSetting
	{
		// Token: 0x06004BBA RID: 19386 RVA: 0x000DEBB5 File Offset: 0x000DCDB5
		public void ApplyTo(Character character)
		{
			character.GetComponent<BehaviorDesignerCommunicator>().SetVariable<SharedVector2>(this._variableName, (this._owenr.lookingDirection == Character.LookingDirection.Left) ? Vector2.left : Vector2.right);
		}

		// Token: 0x04003AED RID: 15085
		[SerializeField]
		private Character _owenr;

		// Token: 0x04003AEE RID: 15086
		[SerializeField]
		private string _variableName;
	}
}
