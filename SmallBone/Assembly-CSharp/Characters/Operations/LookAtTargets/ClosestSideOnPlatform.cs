using System;
using UnityEngine;

namespace Characters.Operations.LookAtTargets
{
	// Token: 0x02000EFF RID: 3839
	public class ClosestSideOnPlatform : Target
	{
		// Token: 0x06004B26 RID: 19238 RVA: 0x000DD33C File Offset: 0x000DB53C
		public override Character.LookingDirection GetDirectionFrom(Character character)
		{
			Collider2D lastStandingCollider = character.movement.controller.collisionState.lastStandingCollider;
			if (lastStandingCollider == null)
			{
				return character.lookingDirection;
			}
			if (character.transform.position.x > lastStandingCollider.bounds.center.x)
			{
				if (!this._farthest)
				{
					return Character.LookingDirection.Right;
				}
				return Character.LookingDirection.Left;
			}
			else
			{
				if (!this._farthest)
				{
					return Character.LookingDirection.Left;
				}
				return Character.LookingDirection.Right;
			}
		}

		// Token: 0x04003A57 RID: 14935
		[SerializeField]
		private bool _farthest;
	}
}
