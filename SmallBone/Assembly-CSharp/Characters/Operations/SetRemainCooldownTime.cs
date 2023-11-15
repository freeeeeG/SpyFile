using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E34 RID: 3636
	public class SetRemainCooldownTime : CharacterOperation
	{
		// Token: 0x0600487F RID: 18559 RVA: 0x000D2C63 File Offset: 0x000D0E63
		public override void Run(Character owner)
		{
			this._action.cooldown.time.remainTime = this._time;
		}

		// Token: 0x0400378F RID: 14223
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x04003790 RID: 14224
		[SerializeField]
		private float _time;
	}
}
