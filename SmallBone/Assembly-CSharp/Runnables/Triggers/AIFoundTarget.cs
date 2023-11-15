using System;
using Characters.AI;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x0200034F RID: 847
	public class AIFoundTarget : Trigger
	{
		// Token: 0x06000FE6 RID: 4070 RVA: 0x0002FACC File Offset: 0x0002DCCC
		protected override bool Check()
		{
			return this._ai.target != null;
		}

		// Token: 0x04000D07 RID: 3335
		[SerializeField]
		private AIController _ai;
	}
}
