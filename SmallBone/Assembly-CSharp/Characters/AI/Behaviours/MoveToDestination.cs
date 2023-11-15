using System;
using System.Collections;
using Characters.Movements;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x02001319 RID: 4889
	public class MoveToDestination : Move
	{
		// Token: 0x06006093 RID: 24723 RVA: 0x0011AEC4 File Offset: 0x001190C4
		public override IEnumerator CRun(AIController controller)
		{
			Character character = controller.character;
			base.result = Behaviour.Result.Doing;
			Vector2 move = (controller.destination.x - character.transform.position.x > 0f) ? Vector2.right : Vector2.left;
			character.movement.move = move;
			base.StartCoroutine(this.CanMove(controller));
			while (base.result.Equals(Behaviour.Result.Doing))
			{
				yield return null;
				if (!character.stunedOrFreezed)
				{
					if (this.wander && controller.target != null)
					{
						base.result = Behaviour.Result.Success;
						yield break;
					}
					if (this.checkWithinSight && controller.target != null && Precondition.CanChase(character, controller.target))
					{
						base.result = Behaviour.Result.Success;
						yield break;
					}
					float num = controller.destination.x - character.transform.position.x;
					move = ((num > 0f) ? Vector2.right : Vector2.left);
					if (Mathf.Abs(num) < this._endDistance)
					{
						base.result = Behaviour.Result.Done;
						yield return this.idle.CRun(controller);
						break;
					}
					character.movement.move = move;
				}
			}
			yield break;
		}

		// Token: 0x06006094 RID: 24724 RVA: 0x0011AEDA File Offset: 0x001190DA
		private IEnumerator CanMove(AIController controller)
		{
			Character character = controller.character;
			CharacterController2D characterController = character.movement.controller;
			while (base.result == Behaviour.Result.Doing)
			{
				yield return null;
				if (!character.stunedOrFreezed && characterController.velocity.x == 0f)
				{
					base.result = Behaviour.Result.Fail;
					break;
				}
			}
			yield break;
		}

		// Token: 0x04004DD1 RID: 19921
		[SerializeField]
		private float _endDistance = 1f;
	}
}
