using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x02001313 RID: 4883
	public class MoveForDuration : Move
	{
		// Token: 0x0600607A RID: 24698 RVA: 0x0011A74E File Offset: 0x0011894E
		public override IEnumerator CRun(AIController controller)
		{
			Character character = controller.character;
			Bounds platformBounds = character.movement.controller.collisionState.lastStandingCollider.bounds;
			Bounds bounds = character.collider.bounds;
			float rightWidth = bounds.max.x - bounds.center.x;
			float leftWidth = bounds.center.x - bounds.min.x;
			base.result = Behaviour.Result.Doing;
			base.StartCoroutine(base.CExpire(controller, this._duration));
			while (base.result.Equals(Behaviour.Result.Doing))
			{
				if (this.wander && controller.target != null)
				{
					character.movement.move = this.direction;
					base.result = Behaviour.Result.Done;
					yield break;
				}
				if (this.checkWithinSight && base.LookAround(controller))
				{
					character.movement.move = this.direction;
					base.result = Behaviour.Result.Done;
					yield break;
				}
				character.movement.move = this.direction;
				if (character.movement.controller.velocity.x != 0f)
				{
					if (character.lookingDirection == Character.LookingDirection.Right && character.movement.controller.collisionState.right)
					{
						this.direction = Vector2.left;
						yield return null;
						continue;
					}
					if (character.lookingDirection == Character.LookingDirection.Left && character.movement.controller.collisionState.left)
					{
						this.direction = Vector2.right;
						yield return null;
						continue;
					}
				}
				if (this._flipAtPlatformEdge)
				{
					if (platformBounds.max.x - rightWidth < character.transform.position.x && this.direction.x > 0f)
					{
						this.direction = Vector2.left;
					}
					else if (platformBounds.min.x + leftWidth > character.transform.position.x && this.direction.x < 0f)
					{
						this.direction = Vector2.right;
					}
				}
				yield return null;
			}
			if (this._doIdle)
			{
				yield return this.idle.CRun(controller);
			}
			yield break;
		}

		// Token: 0x04004DB5 RID: 19893
		[SerializeField]
		private bool _flipAtPlatformEdge = true;

		// Token: 0x04004DB6 RID: 19894
		[SerializeField]
		[MinMaxSlider(0f, 10f)]
		private Vector2 _duration;

		// Token: 0x04004DB7 RID: 19895
		[SerializeField]
		private bool _doIdle = true;
	}
}
