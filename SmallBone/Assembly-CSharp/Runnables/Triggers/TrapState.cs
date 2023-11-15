using System;
using Level.Traps;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x02000356 RID: 854
	public class TrapState : Trigger
	{
		// Token: 0x06000FF7 RID: 4087 RVA: 0x0002FC76 File Offset: 0x0002DE76
		protected override bool Check()
		{
			return this._trap.activated == this._activated;
		}

		// Token: 0x04000D0F RID: 3343
		[SerializeField]
		private ControlableTrap _trap;

		// Token: 0x04000D10 RID: 3344
		[SerializeField]
		private bool _activated;
	}
}
