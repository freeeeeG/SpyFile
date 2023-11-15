using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.FindOptions
{
	// Token: 0x02000E9D RID: 3741
	[Serializable]
	public class EqualLastStandingColliderOfBDCharacter : ICondition
	{
		// Token: 0x060049D6 RID: 18902 RVA: 0x000D79C0 File Offset: 0x000D5BC0
		public bool Satisfied(Character character)
		{
			Character value = this._communicator.GetVariable<SharedCharacter>(this._targetName).Value;
			if (value == null)
			{
				return false;
			}
			Collider2D lastStandingCollider = character.movement.controller.collisionState.lastStandingCollider;
			Collider2D lastStandingCollider2 = value.movement.controller.collisionState.lastStandingCollider;
			return !(lastStandingCollider == null) && !(lastStandingCollider2 == null) && lastStandingCollider == lastStandingCollider2;
		}

		// Token: 0x0400391A RID: 14618
		[SerializeField]
		private BehaviorDesignerCommunicator _communicator;

		// Token: 0x0400391B RID: 14619
		[SerializeField]
		private string _targetName = "Target";
	}
}
