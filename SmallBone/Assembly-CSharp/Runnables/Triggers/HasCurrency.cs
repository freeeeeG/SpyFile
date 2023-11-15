using System;
using Data;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x0200035B RID: 859
	public class HasCurrency : Trigger
	{
		// Token: 0x06001008 RID: 4104 RVA: 0x0002FE5C File Offset: 0x0002E05C
		protected override bool Check()
		{
			int amount = this._currencyAmount.GetAmount();
			switch (this._type)
			{
			case GameData.Currency.Type.Gold:
				return GameData.Currency.gold.Has(amount);
			case GameData.Currency.Type.DarkQuartz:
				return GameData.Currency.darkQuartz.Has(amount);
			case GameData.Currency.Type.Bone:
				return GameData.Currency.bone.Has(amount);
			default:
				return GameData.Currency.gold.Has(amount);
			}
		}

		// Token: 0x04000D1D RID: 3357
		[SerializeField]
		private GameData.Currency.Type _type;

		// Token: 0x04000D1E RID: 3358
		[CurrencyAmount.SubcomponentAttribute]
		[SerializeField]
		private CurrencyAmount _currencyAmount;
	}
}
