using System;
using UnityEngine;

namespace Runnables.Cost
{
	// Token: 0x02000342 RID: 834
	public class Constant : CurrencyAmount
	{
		// Token: 0x06000FCC RID: 4044 RVA: 0x0002F954 File Offset: 0x0002DB54
		public override int GetAmount()
		{
			return this._amount;
		}

		// Token: 0x04000CF6 RID: 3318
		[SerializeField]
		private int _amount;
	}
}
