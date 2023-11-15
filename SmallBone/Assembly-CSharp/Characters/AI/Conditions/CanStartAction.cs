using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Conditions
{
	// Token: 0x020011C4 RID: 4548
	public sealed class CanStartAction : Condition
	{
		// Token: 0x0600597B RID: 22907 RVA: 0x0010A528 File Offset: 0x00108728
		protected override bool Check(AIController controller)
		{
			return this._action.canUse;
		}

		// Token: 0x04004842 RID: 18498
		[SerializeField]
		private Characters.Actions.Action _action;
	}
}
