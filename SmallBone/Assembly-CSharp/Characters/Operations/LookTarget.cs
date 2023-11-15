using System;
using Characters.AI;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DEF RID: 3567
	public class LookTarget : CharacterOperation
	{
		// Token: 0x0600476C RID: 18284 RVA: 0x000CF83B File Offset: 0x000CDA3B
		public override void Run(Character owner)
		{
			if (this._controller.target == null)
			{
				return;
			}
			owner.ForceToLookAt(this._controller.target.transform.position.x);
		}

		// Token: 0x04003675 RID: 13941
		[SerializeField]
		private AIController _controller;
	}
}
