using System;
using UnityEngine;

namespace Characters.Operations.LookAtTargets
{
	// Token: 0x02000F02 RID: 3842
	public class PlatformPoint : Target
	{
		// Token: 0x06004B2C RID: 19244 RVA: 0x000DD458 File Offset: 0x000DB658
		public override Character.LookingDirection GetDirectionFrom(Character character)
		{
			Collider2D lastStandingCollider = character.movement.controller.collisionState.lastStandingCollider;
			if (lastStandingCollider == null)
			{
				return character.lookingDirection;
			}
			float x = character.transform.position.x;
			switch (this._point)
			{
			case PlatformPoint.Point.Left:
			{
				float x2 = lastStandingCollider.bounds.min.x;
				if (x >= x2)
				{
					return Character.LookingDirection.Left;
				}
				return Character.LookingDirection.Right;
			}
			case PlatformPoint.Point.Center:
			{
				float x3 = lastStandingCollider.bounds.center.x;
				if (x >= x3)
				{
					return Character.LookingDirection.Left;
				}
				return Character.LookingDirection.Right;
			}
			case PlatformPoint.Point.Right:
			{
				float x4 = lastStandingCollider.bounds.max.x;
				if (x >= x4)
				{
					return Character.LookingDirection.Left;
				}
				return Character.LookingDirection.Right;
			}
			default:
				return character.lookingDirection;
			}
		}

		// Token: 0x04003A5A RID: 14938
		[SerializeField]
		private PlatformPoint.Point _point;

		// Token: 0x02000F03 RID: 3843
		private enum Point
		{
			// Token: 0x04003A5C RID: 14940
			Left,
			// Token: 0x04003A5D RID: 14941
			Center,
			// Token: 0x04003A5E RID: 14942
			Right
		}
	}
}
