using System;
using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Adventurer.Magician
{
	// Token: 0x02001393 RID: 5011
	public class KeepDistanceMagician : Behaviour
	{
		// Token: 0x060062E4 RID: 25316 RVA: 0x0011FD1D File Offset: 0x0011DF1D
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			Character character = controller.character;
			Character target = controller.target;
			int num = UnityEngine.Random.Range(this._distance.x, this._distance.y);
			float moveDirection = this.GetMoveDirection(character.transform.position);
			this.SetMotion(character.transform.position, moveDirection, target.transform.position, character);
			this.SetDestination(controller, character.transform.position, moveDirection, (float)num);
			this._motion.TryStart();
			character.ForceToLookAt(character.transform.position.x + moveDirection);
			yield return this._moveToDestinationWithFly.CRun(controller);
			if (!controller.stuned && !controller.dead)
			{
				character.CancelAction();
			}
			base.result = Behaviour.Result.Done;
			yield break;
		}

		// Token: 0x060062E5 RID: 25317 RVA: 0x0011FD34 File Offset: 0x0011DF34
		private float GetMoveDirection(Vector3 origin)
		{
			Vector2 vector = MMMaths.RandomBool() ? Vector2.right : Vector2.left;
			if (Physics2D.Raycast(origin, vector, this._minDistanceWithSide, Layers.groundMask))
			{
				vector *= -1f;
			}
			return vector.x;
		}

		// Token: 0x060062E6 RID: 25318 RVA: 0x0011FD8C File Offset: 0x0011DF8C
		private void SetMotion(Vector2 origin, float direction, Vector2 target, Character character)
		{
			if (target.x - origin.x > 0f && direction > 0f)
			{
				this._motion = this._frontMotion;
				return;
			}
			if (target.x - origin.x > 0f && direction < 0f)
			{
				this._motion = this._backMotion;
				return;
			}
			if (target.x - origin.x < 0f && direction > 0f)
			{
				this._motion = this._backMotion;
				return;
			}
			if (target.x - origin.x < 0f && direction < 0f)
			{
				this._motion = this._frontMotion;
				return;
			}
			this._motion = this._frontMotion;
		}

		// Token: 0x060062E7 RID: 25319 RVA: 0x0011FE4C File Offset: 0x0011E04C
		private void SetDestination(AIController controller, Vector2 origin, float direction, float distance)
		{
			Character character = controller.character;
			if (character == null)
			{
				controller.destination = new Vector2(origin.x, origin.y);
				return;
			}
			RaycastHit2D raycastHit2D;
			if (!character.movement.TryBelowRayCast(character.movement.controller.terrainMask, out raycastHit2D, 20f))
			{
				RaycastHit2D hit = Physics2D.Raycast(origin, new Vector2(direction, 0f), distance, Layers.terrainMask);
				float num = (distance > 0f) ? -0.5f : 0.5f;
				controller.destination = new Vector2(origin.x + direction * distance + num, origin.y);
				if (hit)
				{
					float x = controller.target.movement.controller.collisionState.lastStandingCollider.bounds.center.x;
					int num2 = (hit.point.x > x) ? -1 : 1;
					controller.destination = new Vector2(hit.point.x + (float)num2, origin.y);
				}
				return;
			}
			Collider2D collider = raycastHit2D.collider;
			if (direction > 0f)
			{
				if (origin.x + direction * distance > collider.bounds.max.x - 1f)
				{
					controller.destination = new Vector2(collider.bounds.max.x - 1f, origin.y);
					return;
				}
				controller.destination = new Vector2(origin.x + direction * distance, origin.y);
				return;
			}
			else
			{
				if (origin.x + direction * distance < collider.bounds.min.x + 1f)
				{
					controller.destination = new Vector2(collider.bounds.min.x + 1f, origin.y);
					return;
				}
				controller.destination = new Vector2(origin.x + direction * distance, origin.y);
				return;
			}
		}

		// Token: 0x04004FB5 RID: 20405
		[UnityEditor.Subcomponent(typeof(MoveToDestinationWithFly))]
		[SerializeField]
		private MoveToDestinationWithFly _moveToDestinationWithFly;

		// Token: 0x04004FB6 RID: 20406
		[MinMaxSlider(0f, 30f)]
		[SerializeField]
		private Vector2Int _distance;

		// Token: 0x04004FB7 RID: 20407
		[SerializeField]
		private float _minDistanceWithSide;

		// Token: 0x04004FB8 RID: 20408
		[SerializeField]
		private Characters.Actions.Action _backMotion;

		// Token: 0x04004FB9 RID: 20409
		[SerializeField]
		private Characters.Actions.Action _frontMotion;

		// Token: 0x04004FBA RID: 20410
		private Characters.Actions.Action _motion;
	}
}
