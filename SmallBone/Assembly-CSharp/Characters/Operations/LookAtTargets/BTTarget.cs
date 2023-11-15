using System;
using BT;
using UnityEngine;

namespace Characters.Operations.LookAtTargets
{
	// Token: 0x02000EFC RID: 3836
	public sealed class BTTarget : Target
	{
		// Token: 0x06004B20 RID: 19232 RVA: 0x000DD214 File Offset: 0x000DB414
		public override Character.LookingDirection GetDirectionFrom(Character character)
		{
			Character character2 = this._bt.context.Get<Character>(Key.Target);
			if (character2 == null)
			{
				return character.lookingDirection;
			}
			if (character2.transform.position.x > character.transform.position.x)
			{
				return Character.LookingDirection.Right;
			}
			return Character.LookingDirection.Left;
		}

		// Token: 0x04003A54 RID: 14932
		[SerializeField]
		private BehaviourTreeRunner _bt;
	}
}
