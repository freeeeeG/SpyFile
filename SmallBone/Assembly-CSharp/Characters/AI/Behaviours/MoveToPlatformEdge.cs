using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x0200130A RID: 4874
	public class MoveToPlatformEdge : Move
	{
		// Token: 0x06006059 RID: 24665 RVA: 0x00119F82 File Offset: 0x00118182
		public override IEnumerator CRun(AIController controller)
		{
			Character character = controller.character;
			Bounds bounds = character.collider.bounds;
			float rightWidth = bounds.max.x - bounds.center.x;
			float leftWidth = bounds.center.x - bounds.min.x;
			base.result = Behaviour.Result.Doing;
			while (base.result.Equals(Behaviour.Result.Doing))
			{
				if (character.movement.controller.collisionState.lastStandingCollider)
				{
					Bounds bounds2 = character.movement.controller.collisionState.lastStandingCollider.bounds;
					if (this.wander && controller.target != null)
					{
						character.movement.MoveHorizontal(this.direction);
						base.result = Behaviour.Result.Done;
						yield break;
					}
					if (this.checkWithinSight && base.LookAround(controller))
					{
						character.movement.MoveHorizontal(this.direction);
						base.result = Behaviour.Result.Done;
						yield break;
					}
					character.movement.MoveHorizontal(this.direction);
					if ((bounds2.max.x - rightWidth - this._distanceToEdge < character.transform.position.x && this.direction.x > 0f) || (bounds2.min.x + leftWidth + this._distanceToEdge > character.transform.position.x && this.direction.x < 0f))
					{
						yield return this.idle.CRun(controller);
						if (this.direction.x > 0f)
						{
							this.direction = Vector2.left;
						}
						else if (this.direction.x < 0f)
						{
							this.direction = Vector2.right;
						}
					}
					yield return null;
				}
				else
				{
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x04004D98 RID: 19864
		[SerializeField]
		private float _distanceToEdge;
	}
}
