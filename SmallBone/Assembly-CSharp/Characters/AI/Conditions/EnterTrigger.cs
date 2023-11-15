using System;
using UnityEngine;

namespace Characters.AI.Conditions
{
	// Token: 0x020011D0 RID: 4560
	public class EnterTrigger : Condition
	{
		// Token: 0x06005999 RID: 22937 RVA: 0x0010A8CA File Offset: 0x00108ACA
		protected override bool Check(AIController controller)
		{
			return controller.FindClosestPlayerBody(this._trigger);
		}

		// Token: 0x0400485B RID: 18523
		[SerializeField]
		private Collider2D _trigger;
	}
}
