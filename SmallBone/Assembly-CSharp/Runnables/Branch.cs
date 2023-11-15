using System;
using Runnables.Chances;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002EA RID: 746
	public sealed class Branch : Runnable
	{
		// Token: 0x06000EBD RID: 3773 RVA: 0x0002DC35 File Offset: 0x0002BE35
		public override void Run()
		{
			if (this._trueChance.IsTrue())
			{
				this._onTrue.Run();
				return;
			}
			this._onFalse.Run();
		}

		// Token: 0x04000C2F RID: 3119
		[Chance.SubcomponentAttribute]
		[SerializeField]
		private Chance _trueChance;

		// Token: 0x04000C30 RID: 3120
		[SerializeField]
		private Runnable _onTrue;

		// Token: 0x04000C31 RID: 3121
		[SerializeField]
		private Runnable _onFalse;
	}
}
