using System;
using Characters;
using UnityEngine;

namespace BT
{
	// Token: 0x02001419 RID: 5145
	public class MoveToTarget : Node
	{
		// Token: 0x06006527 RID: 25895 RVA: 0x00124E4C File Offset: 0x0012304C
		protected override NodeState UpdateDeltatime(Context context)
		{
			Character character = context.Get<Character>(Key.Target);
			Character character2 = context.Get<Character>(Key.OwnerCharacter);
			if (character == null)
			{
				return NodeState.Fail;
			}
			if (character.health.dead)
			{
				return NodeState.Fail;
			}
			if (this._stopOnCloseToTarget)
			{
				if (Mathf.Abs(character2.transform.position.x - character.transform.position.x) <= this._stopDistance)
				{
					return NodeState.Success;
				}
				if (TargetFinder.GetRandomTarget(this._stopTrigger, 1024) != null)
				{
					return NodeState.Success;
				}
			}
			Vector2 normalizedDirection;
			if (character.transform.position.x > character2.transform.position.x)
			{
				normalizedDirection = Vector2.right;
			}
			else
			{
				normalizedDirection = Vector2.left;
			}
			if (this._turnOnEdge)
			{
				character2.movement.TurnOnEdge(ref normalizedDirection);
			}
			if (character2.movement.controller.collisionState.lastStandingCollider != null)
			{
				character2.movement.MoveHorizontal(normalizedDirection);
			}
			if (this._stopOnCloseToTarget)
			{
				return NodeState.Running;
			}
			return NodeState.Success;
		}

		// Token: 0x04005173 RID: 20851
		[SerializeField]
		private bool _turnOnEdge;

		// Token: 0x04005174 RID: 20852
		[SerializeField]
		private bool _stopOnCloseToTarget;

		// Token: 0x04005175 RID: 20853
		[SerializeField]
		private Collider2D _stopTrigger;

		// Token: 0x04005176 RID: 20854
		[SerializeField]
		private float _stopDistance;
	}
}
