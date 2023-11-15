using System;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x0200110F RID: 4367
	public static class Precondition
	{
		// Token: 0x060054F5 RID: 21749 RVA: 0x000FDDD0 File Offset: 0x000FBFD0
		public static bool CanMove(Character character)
		{
			Collider2D lastStandingCollider = character.movement.controller.collisionState.lastStandingCollider;
			return lastStandingCollider == null || lastStandingCollider.bounds.size.x > 3f;
		}

		// Token: 0x060054F6 RID: 21750 RVA: 0x000FDE1C File Offset: 0x000FC01C
		public static bool CanChase(Character character, Character target)
		{
			Collider2D lastStandingCollider = character.movement.controller.collisionState.lastStandingCollider;
			if (target == null || target.movement == null || target.movement.controller == null || target.movement.controller.collisionState == null || target.movement.controller.collisionState.lastStandingCollider == null)
			{
				return false;
			}
			Collider2D lastStandingCollider2 = target.movement.controller.collisionState.lastStandingCollider;
			return !(lastStandingCollider != lastStandingCollider2) && (lastStandingCollider.bounds.center.y - character.collider.bounds.size.y <= lastStandingCollider2.bounds.center.y || lastStandingCollider.bounds.max.x <= target.transform.position.x || lastStandingCollider.bounds.min.x >= target.transform.position.x);
		}

		// Token: 0x060054F7 RID: 21751 RVA: 0x000FDF4C File Offset: 0x000FC14C
		public static bool CanMoveToDirection(Character character, Vector2 direction, float minimumDistance)
		{
			Collider2D lastStandingCollider = character.movement.controller.collisionState.lastStandingCollider;
			if (lastStandingCollider == null)
			{
				return false;
			}
			if (!Precondition.CanMove(character))
			{
				return false;
			}
			float num = character.movement.velocity.x * character.chronometer.master.deltaTime;
			return (direction.x <= 0f || character.collider.bounds.max.x + num + minimumDistance < lastStandingCollider.bounds.max.x) && (direction.x >= 0f || character.collider.bounds.min.x + num - minimumDistance > lastStandingCollider.bounds.min.x);
		}
	}
}
