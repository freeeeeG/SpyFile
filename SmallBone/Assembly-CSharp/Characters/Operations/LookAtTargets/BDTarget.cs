using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.LookAtTargets
{
	// Token: 0x02000EFB RID: 3835
	public sealed class BDTarget : Target
	{
		// Token: 0x06004B1E RID: 19230 RVA: 0x000DD19C File Offset: 0x000DB39C
		public override Character.LookingDirection GetDirectionFrom(Character character)
		{
			SharedCharacter sharedCharacter = this._tree.GetVariable(this._targetValueName) as SharedCharacter;
			if (sharedCharacter.Value == null)
			{
				return character.lookingDirection;
			}
			if (character.transform.position.x <= sharedCharacter.Value.transform.position.x)
			{
				return Character.LookingDirection.Right;
			}
			return Character.LookingDirection.Left;
		}

		// Token: 0x04003A52 RID: 14930
		[SerializeField]
		private BehaviorTree _tree;

		// Token: 0x04003A53 RID: 14931
		[SerializeField]
		private string _targetValueName = "Target";
	}
}
