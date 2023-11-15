using System;
using Characters.Actions;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002D8 RID: 728
	public sealed class RunAction : Runnable
	{
		// Token: 0x06000E93 RID: 3731 RVA: 0x0002D6E3 File Offset: 0x0002B8E3
		public override void Run()
		{
			this._action.TryStart();
		}

		// Token: 0x04000C13 RID: 3091
		[SerializeField]
		private Characters.Actions.Action _action;
	}
}
