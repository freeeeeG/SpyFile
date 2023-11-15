using System;
using Data;
using UnityEngine;

namespace Runnables
{
	// Token: 0x0200030B RID: 779
	public class ConsumeCurrency : Runnable
	{
		// Token: 0x06000F39 RID: 3897 RVA: 0x0002E8B8 File Offset: 0x0002CAB8
		public override void Run()
		{
			int amount = this._currencyAmount.GetAmount();
			switch (this._type)
			{
			case GameData.Currency.Type.Gold:
				if (!GameData.Currency.gold.Has(amount))
				{
					return;
				}
				GameData.Currency.gold.Consume(amount);
				return;
			case GameData.Currency.Type.DarkQuartz:
				if (!GameData.Currency.darkQuartz.Has(amount))
				{
					return;
				}
				GameData.Currency.darkQuartz.Consume(amount);
				return;
			case GameData.Currency.Type.Bone:
				if (!GameData.Currency.bone.Has(amount))
				{
					return;
				}
				GameData.Currency.bone.Consume(amount);
				return;
			default:
				if (!GameData.Currency.gold.Has(amount))
				{
					return;
				}
				GameData.Currency.gold.Consume(amount);
				return;
			}
		}

		// Token: 0x04000C8C RID: 3212
		[SerializeField]
		private GameData.Currency.Type _type;

		// Token: 0x04000C8D RID: 3213
		[SerializeField]
		[CurrencyAmount.SubcomponentAttribute]
		private CurrencyAmount _currencyAmount;
	}
}
