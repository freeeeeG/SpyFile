using System;
using Characters.Actions;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x0200034D RID: 845
	public class CharacterActionRunning : Trigger
	{
		// Token: 0x06000FE2 RID: 4066 RVA: 0x0002FA98 File Offset: 0x0002DC98
		protected override bool Check()
		{
			return this._action.running;
		}

		// Token: 0x04000D05 RID: 3333
		[SerializeField]
		private Characters.Actions.Action _action;
	}
}
