using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x0200131E RID: 4894
	public class MoveToTarget : Move
	{
		// Token: 0x060060AA RID: 24746 RVA: 0x0011B31E File Offset: 0x0011951E
		public override IEnumerator CRun(AIController controller)
		{
			Character character = controller.character;
			Character target = controller.target;
			base.result = Behaviour.Result.Doing;
			while (base.result == Behaviour.Result.Doing)
			{
				if (target.movement.controller.collisionState.lastStandingCollider == null)
				{
					yield return null;
				}
				else
				{
					if (controller.target == null || !Precondition.CanChase(character, controller.target))
					{
						base.result = Behaviour.Result.Fail;
						break;
					}
					Bounds bounds = character.movement.controller.collisionState.lastStandingCollider.bounds;
					Bounds bounds2 = target.movement.controller.collisionState.lastStandingCollider.bounds;
					if (bounds.center.y - character.collider.bounds.size.y > bounds2.center.y)
					{
						character.movement.move = ((character.lookingDirection == Character.LookingDirection.Right) ? Vector2.right : Vector2.left);
						yield return null;
					}
					else
					{
						float num = controller.target.transform.position.x - character.transform.position.x;
						if (Mathf.Abs(num) < 0.1f || base.LookAround(controller))
						{
							yield return this.idle.CRun(controller);
							base.result = Behaviour.Result.Success;
							break;
						}
						character.movement.move = ((num > 0f) ? Vector2.right : Vector2.left);
						yield return null;
					}
				}
			}
			yield break;
		}
	}
}
