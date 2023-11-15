using System;
using UnityEngine;

namespace Characters.Operations.LookAtTargets
{
	// Token: 0x02000F00 RID: 3840
	public class FlipInDistanceFromPlatform : Target
	{
		// Token: 0x06004B28 RID: 19240 RVA: 0x000DD3AC File Offset: 0x000DB5AC
		public override Character.LookingDirection GetDirectionFrom(Character character)
		{
			Collider2D lastStandingCollider = character.movement.controller.collisionState.lastStandingCollider;
			if (lastStandingCollider == null)
			{
				return character.lookingDirection;
			}
			Vector3 position = character.transform.position;
			if (position.x + this._distance.value > lastStandingCollider.bounds.max.x)
			{
				return Character.LookingDirection.Right;
			}
			if (position.x - this._distance.value < lastStandingCollider.bounds.min.x)
			{
				return Character.LookingDirection.Left;
			}
			return character.lookingDirection;
		}

		// Token: 0x04003A58 RID: 14936
		[SerializeField]
		private CustomFloat _distance;
	}
}
