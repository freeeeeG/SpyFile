using System;
using Level.BlackMarket;
using UnityEngine;

namespace Characters.Gear.Upgrades
{
	// Token: 0x0200084C RID: 2124
	public sealed class NegotiatorsCoin : UpgradeAbility
	{
		// Token: 0x06002C37 RID: 11319 RVA: 0x000876B0 File Offset: 0x000858B0
		public override void Attach(Character target)
		{
			if (target == null)
			{
				Debug.LogError("Player is null");
				return;
			}
			GlobalSettings marketSettings = Settings.instance.marketSettings;
			marketSettings.collectorItemPriceMultiplier += this._extraCollectorPriceMultiplier;
			marketSettings.collectorFreeRefreshCount += this._extraFreeRefreshCount;
		}

		// Token: 0x06002C38 RID: 11320 RVA: 0x00087700 File Offset: 0x00085900
		public override void Detach()
		{
			GlobalSettings marketSettings = Settings.instance.marketSettings;
			marketSettings.collectorItemPriceMultiplier -= this._extraCollectorPriceMultiplier;
			marketSettings.collectorFreeRefreshCount -= this._extraFreeRefreshCount;
		}

		// Token: 0x04002561 RID: 9569
		[SerializeField]
		private int _extraFreeRefreshCount;

		// Token: 0x04002562 RID: 9570
		[SerializeField]
		private float _extraCollectorPriceMultiplier;
	}
}
