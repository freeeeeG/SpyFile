using System;
using UnityEngine;

namespace Level.BlackMarket
{
	// Token: 0x02000619 RID: 1561
	[Serializable]
	public class GlobalSettings
	{
		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06001F41 RID: 8001 RVA: 0x0005F27B File Offset: 0x0005D47B
		public RarityPrices collectorItemPrices
		{
			get
			{
				return this._collectorItemPrices;
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06001F42 RID: 8002 RVA: 0x0005F283 File Offset: 0x0005D483
		public RarityPrices masterDishPrices
		{
			get
			{
				return this._masterDishPrices;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06001F43 RID: 8003 RVA: 0x0005F28B File Offset: 0x0005D48B
		public RarityPrices quintessenceMeisterPrices
		{
			get
			{
				return this._quintessenceMeisterPrices;
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06001F44 RID: 8004 RVA: 0x0005F293 File Offset: 0x0005D493
		// (set) Token: 0x06001F45 RID: 8005 RVA: 0x0005F29B File Offset: 0x0005D49B
		public float collectorItemPriceMultiplier { get; set; } = 1f;

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06001F46 RID: 8006 RVA: 0x0005F2A4 File Offset: 0x0005D4A4
		// (set) Token: 0x06001F47 RID: 8007 RVA: 0x0005F2AC File Offset: 0x0005D4AC
		public int collectorFreeRefreshCount { get; set; }

		// Token: 0x04001A70 RID: 6768
		[SerializeField]
		private RarityPrices _collectorItemPrices;

		// Token: 0x04001A71 RID: 6769
		[SerializeField]
		private RarityPrices _masterDishPrices;

		// Token: 0x04001A72 RID: 6770
		[SerializeField]
		private RarityPrices _quintessenceMeisterPrices;
	}
}
