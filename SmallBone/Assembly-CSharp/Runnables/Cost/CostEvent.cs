using System;
using Level.Specials;
using UnityEngine;

namespace Runnables.Cost
{
	// Token: 0x02000343 RID: 835
	public class CostEvent : CurrencyAmount
	{
		// Token: 0x06000FCE RID: 4046 RVA: 0x0002F964 File Offset: 0x0002DB64
		public override int GetAmount()
		{
			return (int)this._costEvent.GetValue();
		}

		// Token: 0x04000CF7 RID: 3319
		[SerializeField]
		private CostEvent _costEvent;
	}
}
