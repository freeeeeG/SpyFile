using System;
using Data;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables
{
	// Token: 0x02000317 RID: 791
	public class DropCurrency : Runnable
	{
		// Token: 0x06000F4F RID: 3919 RVA: 0x0002EBB0 File Offset: 0x0002CDB0
		public override void Run()
		{
			Rarity rarity = this._rarityPossibilities.Evaluate();
			GameData.Currency.Type? type = this._currencyPossibilities.Evaluate();
			IStageInfo currentStage = Singleton<Service>.Instance.levelManager.currentChapter.currentStage;
			int num = 0;
			if (type != null)
			{
				switch (type.GetValueOrDefault())
				{
				case GameData.Currency.Type.Gold:
					num = currentStage.goldRangeByRarity.Evaluate(rarity);
					break;
				case GameData.Currency.Type.DarkQuartz:
					num = currentStage.darkQuartzRangeByRarity.Evaluate(rarity);
					break;
				case GameData.Currency.Type.Bone:
					num = currentStage.boneRangeByRarity.Evaluate(rarity);
					break;
				}
			}
			if (num == 0)
			{
				throw new InvalidOperationException("드랍되는 재화의 값이 0입니다.");
			}
			Singleton<Service>.Instance.levelManager.DropCurrency(type.Value, num, (int)Mathf.Pow((float)num, 0.5f), (this._dropPoint == null) ? base.transform.position : this._dropPoint.position);
		}

		// Token: 0x04000CA3 RID: 3235
		[SerializeField]
		private CurrencyPossibilities _currencyPossibilities;

		// Token: 0x04000CA4 RID: 3236
		[SerializeField]
		private RarityPossibilities _rarityPossibilities;

		// Token: 0x04000CA5 RID: 3237
		[SerializeField]
		private Transform _dropPoint;
	}
}
