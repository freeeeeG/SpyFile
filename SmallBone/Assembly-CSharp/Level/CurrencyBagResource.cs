using System;
using System.Linq;
using Data;
using UnityEngine;

namespace Level
{
	// Token: 0x020004A4 RID: 1188
	[CreateAssetMenu]
	public sealed class CurrencyBagResource : ScriptableObject
	{
		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x060016D3 RID: 5843 RVA: 0x00047F8D File Offset: 0x0004618D
		public static CurrencyBagResource instance
		{
			get
			{
				if (CurrencyBagResource._instance == null)
				{
					CurrencyBagResource._instance = Resources.Load<CurrencyBagResource>("CurrencyBagResource");
				}
				return CurrencyBagResource._instance;
			}
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x00047FB0 File Offset: 0x000461B0
		public CurrencyBag GetCurrencyBag(GameData.Currency.Type type, Rarity rarity)
		{
			switch (type)
			{
			case GameData.Currency.Type.Gold:
				return (from bag in this._goldBags
				where bag.rarity == rarity
				select bag).Random<CurrencyBag>();
			case GameData.Currency.Type.DarkQuartz:
				return (from bag in this._quartzBags
				where bag.rarity == rarity
				select bag).Random<CurrencyBag>();
			case GameData.Currency.Type.Bone:
				return (from bag in this._boneBags
				where bag.rarity == rarity
				select bag).Random<CurrencyBag>();
			default:
				return null;
			}
		}

		// Token: 0x04001403 RID: 5123
		private static CurrencyBagResource _instance;

		// Token: 0x04001404 RID: 5124
		[SerializeField]
		[Header("Gold")]
		private CurrencyBag[] _goldBags;

		// Token: 0x04001405 RID: 5125
		[SerializeField]
		[Header("Bone")]
		private CurrencyBag[] _boneBags;

		// Token: 0x04001406 RID: 5126
		[SerializeField]
		[Header("DarkQuartz")]
		private CurrencyBag[] _quartzBags;
	}
}
