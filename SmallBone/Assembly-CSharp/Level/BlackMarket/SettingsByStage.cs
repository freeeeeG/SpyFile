using System;
using UnityEngine;

namespace Level.BlackMarket
{
	// Token: 0x02000621 RID: 1569
	[Serializable]
	public class SettingsByStage
	{
		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06001F70 RID: 8048 RVA: 0x0005FB39 File Offset: 0x0005DD39
		public int collectorPossibility
		{
			get
			{
				return this._collectorPossibility;
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06001F71 RID: 8049 RVA: 0x0005FB41 File Offset: 0x0005DD41
		public int masterPossibility
		{
			get
			{
				return this._masterPossibility;
			}
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06001F72 RID: 8050 RVA: 0x0005FB49 File Offset: 0x0005DD49
		public int headlessPossibility
		{
			get
			{
				return this._headlessPossibility;
			}
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06001F73 RID: 8051 RVA: 0x0005FB51 File Offset: 0x0005DD51
		public int quintessencePossibility
		{
			get
			{
				return this._quintessenceMeisterPossibility;
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06001F74 RID: 8052 RVA: 0x0005FB59 File Offset: 0x0005DD59
		public int tombRaiderPossibility
		{
			get
			{
				return this._tombRaiderPossibility;
			}
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06001F75 RID: 8053 RVA: 0x0005FB61 File Offset: 0x0005DD61
		public RarityPossibilities collectorItemPossibilities
		{
			get
			{
				return this._collectorItemPossibilities;
			}
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06001F76 RID: 8054 RVA: 0x0005FB69 File Offset: 0x0005DD69
		public float collectorItemPriceMultiplier
		{
			get
			{
				return this._collectorItemPriceMultiplier;
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06001F77 RID: 8055 RVA: 0x0005FB71 File Offset: 0x0005DD71
		public RarityPossibilities masterDishPossibilities
		{
			get
			{
				return this._masterDishPossibilities;
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06001F78 RID: 8056 RVA: 0x0005FB79 File Offset: 0x0005DD79
		public float masterDishPriceMultiplier
		{
			get
			{
				return this._masterDishPriceMultiplier;
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x06001F79 RID: 8057 RVA: 0x0005FB81 File Offset: 0x0005DD81
		public RarityPossibilities headlessHeadPossibilities
		{
			get
			{
				return this._headlessHeadPossibilities;
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x06001F7A RID: 8058 RVA: 0x0005FB89 File Offset: 0x0005DD89
		public RarityPossibilities quintessenceMeisterPossibilities
		{
			get
			{
				return this._quintessenceMeisterPossibilities;
			}
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x06001F7B RID: 8059 RVA: 0x0005FB91 File Offset: 0x0005DD91
		public float quintessenceMeisterPriceMultiplier
		{
			get
			{
				return this._quintessenceMeisterPriceMultiplier;
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x06001F7C RID: 8060 RVA: 0x0005FB99 File Offset: 0x0005DD99
		public RarityPossibilities tombRaiderGearPossibilities
		{
			get
			{
				return this._tombRaiderGearPossibilities;
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06001F7D RID: 8061 RVA: 0x0005FBA1 File Offset: 0x0005DDA1
		public RarityPrices tombRaiderUnlockPrices
		{
			get
			{
				return this._tombRaiderUnlockPrices;
			}
		}

		// Token: 0x04001A9B RID: 6811
		[SerializeField]
		[Range(0f, 100f)]
		private int _collectorPossibility;

		// Token: 0x04001A9C RID: 6812
		[Range(0f, 100f)]
		[SerializeField]
		private int _masterPossibility;

		// Token: 0x04001A9D RID: 6813
		[Range(0f, 100f)]
		[SerializeField]
		private int _headlessPossibility;

		// Token: 0x04001A9E RID: 6814
		[SerializeField]
		[Range(0f, 100f)]
		private int _quintessenceMeisterPossibility;

		// Token: 0x04001A9F RID: 6815
		[Range(0f, 100f)]
		[SerializeField]
		private int _tombRaiderPossibility;

		// Token: 0x04001AA0 RID: 6816
		[SerializeField]
		[Header("Collector")]
		private RarityPossibilities _collectorItemPossibilities;

		// Token: 0x04001AA1 RID: 6817
		[SerializeField]
		private float _collectorItemPriceMultiplier = 1f;

		// Token: 0x04001AA2 RID: 6818
		[SerializeField]
		[Header("Master (Chef)")]
		private RarityPossibilities _masterDishPossibilities;

		// Token: 0x04001AA3 RID: 6819
		[SerializeField]
		private float _masterDishPriceMultiplier = 1f;

		// Token: 0x04001AA4 RID: 6820
		[Header("Headless")]
		[SerializeField]
		private RarityPossibilities _headlessHeadPossibilities;

		// Token: 0x04001AA5 RID: 6821
		[Header("Essence Meister")]
		[SerializeField]
		private RarityPossibilities _quintessenceMeisterPossibilities;

		// Token: 0x04001AA6 RID: 6822
		[SerializeField]
		private float _quintessenceMeisterPriceMultiplier = 1f;

		// Token: 0x04001AA7 RID: 6823
		[SerializeField]
		[Header("Tomb Raider")]
		private RarityPossibilities _tombRaiderGearPossibilities;

		// Token: 0x04001AA8 RID: 6824
		[SerializeField]
		private RarityPrices _tombRaiderUnlockPrices;
	}
}
