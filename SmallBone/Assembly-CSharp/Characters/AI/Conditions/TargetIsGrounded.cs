using System;
using UnityEngine;

namespace Characters.AI.Conditions
{
	// Token: 0x020011D5 RID: 4565
	public sealed class TargetIsGrounded : Condition
	{
		// Token: 0x0600599F RID: 22943 RVA: 0x0010A97C File Offset: 0x00108B7C
		protected override bool Check(AIController controller)
		{
			return this._controller.target == null || this._controller.target.movement.isGrounded;
		}

		// Token: 0x04004867 RID: 18535
		[SerializeField]
		private AIController _controller;
	}
}
