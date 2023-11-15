using System;
using Characters;
using Characters.AI;
using UnityEngine;

namespace BT
{
	// Token: 0x02001418 RID: 5144
	public sealed class MoveToLookingDirection : Node
	{
		// Token: 0x06006525 RID: 25893 RVA: 0x00124DD8 File Offset: 0x00122FD8
		protected override NodeState UpdateDeltatime(Context context)
		{
			Character character = context.Get<Character>(Key.OwnerCharacter);
			if (character == null)
			{
				return NodeState.Fail;
			}
			Vector2 normalizedDirection = (character.lookingDirection == Character.LookingDirection.Right) ? Vector2.right : Vector2.left;
			if (this._turnOnEdge)
			{
				character.movement.TurnOnEdge(ref normalizedDirection);
			}
			if (Precondition.CanMove(character))
			{
				character.movement.MoveHorizontal(normalizedDirection);
			}
			return NodeState.Success;
		}

		// Token: 0x04005172 RID: 20850
		[SerializeField]
		private bool _turnOnEdge = true;
	}
}
