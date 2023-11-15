using System;
using Characters;
using Characters.AI;
using UnityEngine;

namespace BT
{
	// Token: 0x0200141A RID: 5146
	public sealed class MoveTowards : Node
	{
		// Token: 0x06006529 RID: 25897 RVA: 0x00124F5C File Offset: 0x0012315C
		protected override NodeState UpdateDeltatime(Context context)
		{
			Character character = context.Get<Character>(Key.OwnerCharacter);
			Vector2 normalizedDirection;
			if (MMMaths.Chance(this._rightChance))
			{
				normalizedDirection = Vector2.right;
			}
			else
			{
				normalizedDirection = Vector2.left;
			}
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

		// Token: 0x04005177 RID: 20855
		[SerializeField]
		[Range(0f, 1f)]
		private float _rightChance;

		// Token: 0x04005178 RID: 20856
		[SerializeField]
		private bool _turnOnEdge = true;
	}
}
