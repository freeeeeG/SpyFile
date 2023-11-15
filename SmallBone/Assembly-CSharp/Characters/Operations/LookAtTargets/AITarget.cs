using System;
using Characters.AI;
using UnityEngine;

namespace Characters.Operations.LookAtTargets
{
	// Token: 0x02000EFA RID: 3834
	public sealed class AITarget : Target
	{
		// Token: 0x06004B1C RID: 19228 RVA: 0x000DD144 File Offset: 0x000DB344
		public override Character.LookingDirection GetDirectionFrom(Character character)
		{
			Character target = this._aIController.target;
			if (target == null)
			{
				return character.lookingDirection;
			}
			if (target.transform.position.x > character.transform.position.x)
			{
				return Character.LookingDirection.Right;
			}
			return Character.LookingDirection.Left;
		}

		// Token: 0x04003A51 RID: 14929
		[SerializeField]
		private AIController _aIController;
	}
}
