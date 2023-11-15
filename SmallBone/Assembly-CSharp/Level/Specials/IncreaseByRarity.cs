using System;
using Level.BlackMarket;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Specials
{
	// Token: 0x02000628 RID: 1576
	[RequireComponent(typeof(TimeCostEventReward))]
	public class IncreaseByRarity : MonoBehaviour
	{
		// Token: 0x06001F9D RID: 8093 RVA: 0x0006024C File Offset: 0x0005E44C
		private void Start()
		{
			SettingsByStage marketSettings = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.marketSettings;
			GlobalSettings marketSettings2 = Settings.instance.marketSettings;
			float costSpeed = (float)marketSettings2.collectorItemPrices[this._reward.rarity] * marketSettings.collectorItemPriceMultiplier * marketSettings2.collectorItemPriceMultiplier;
			this._costReward.SetCostSpeed(costSpeed);
		}

		// Token: 0x04001AC9 RID: 6857
		[SerializeField]
		private TimeCostEvent _costReward;

		// Token: 0x04001ACA RID: 6858
		[SerializeField]
		private TimeCostEventReward _reward;

		// Token: 0x04001ACB RID: 6859
		[SerializeField]
		private ValueByRarity _multiplierByRarity;
	}
}
