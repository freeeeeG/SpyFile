using System;
using Characters.AI;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DF0 RID: 3568
	public class LookTargetOpposition : CharacterOperation
	{
		// Token: 0x0600476E RID: 18286 RVA: 0x000CF874 File Offset: 0x000CDA74
		public override void Run(Character owner)
		{
			if (this._controller.target == null)
			{
				return;
			}
			Character.LookingDirection lookingDirection = (owner.transform.position.x < this._controller.target.transform.position.x) ? Character.LookingDirection.Left : Character.LookingDirection.Right;
			owner.ForceToLookAt(lookingDirection);
		}

		// Token: 0x04003676 RID: 13942
		[SerializeField]
		private AIController _controller;
	}
}
