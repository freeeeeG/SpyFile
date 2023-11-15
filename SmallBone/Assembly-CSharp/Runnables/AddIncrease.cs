using System;
using Level.Specials;
using UnityEngine;

namespace Runnables
{
	// Token: 0x02000333 RID: 819
	public abstract class AddIncrease : Runnable
	{
		// Token: 0x06000F9A RID: 3994 RVA: 0x0002F4A2 File Offset: 0x0002D6A2
		public override void Run()
		{
			this._costReward.AddSpeed((double)this.GetIncrease());
		}

		// Token: 0x06000F9B RID: 3995
		protected abstract int GetIncrease();

		// Token: 0x04000CD6 RID: 3286
		[SerializeField]
		private TimeCostEvent _costReward;
	}
}
