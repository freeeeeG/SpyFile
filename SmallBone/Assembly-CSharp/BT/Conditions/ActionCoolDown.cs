using System;
using Characters.Actions;
using UnityEngine;

namespace BT.Conditions
{
	// Token: 0x0200142A RID: 5162
	public sealed class ActionCoolDown : Condition
	{
		// Token: 0x06006558 RID: 25944 RVA: 0x001256F8 File Offset: 0x001238F8
		protected override bool Check(Context context)
		{
			return this._action.cooldown.canUse;
		}

		// Token: 0x040051A3 RID: 20899
		[SerializeField]
		private Characters.Actions.Action _action;
	}
}
