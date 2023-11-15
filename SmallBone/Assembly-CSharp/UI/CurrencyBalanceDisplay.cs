using System;
using Data;
using GameResources;
using TMPro;
using UnityEngine;

namespace UI
{
	// Token: 0x02000390 RID: 912
	public class CurrencyBalanceDisplay : MonoBehaviour
	{
		// Token: 0x1700035B RID: 859
		// (get) Token: 0x060010B3 RID: 4275 RVA: 0x00031507 File Offset: 0x0002F707
		private EnumArray<GameData.Currency.Type, string> _colorOpenByCurrency
		{
			get
			{
				return new EnumArray<GameData.Currency.Type, string>(new string[]
				{
					Localization.GetLocalizedString(Localization.Key.colorOpenGold),
					Localization.GetLocalizedString(Localization.Key.colorOpenDarkQuartz),
					Localization.GetLocalizedString(Localization.Key.colorOpenBone)
				});
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x060010B4 RID: 4276 RVA: 0x0003153B File Offset: 0x0002F73B
		private string _colorClose
		{
			get
			{
				return Localization.GetLocalizedString("cc");
			}
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x00031547 File Offset: 0x0002F747
		private void Awake()
		{
			this.UpdateText(true);
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x00031550 File Offset: 0x0002F750
		private void Update()
		{
			this.UpdateText(false);
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x0003155C File Offset: 0x0002F75C
		private void UpdateText(bool force)
		{
			int balance = GameData.Currency.currencies[this._type].balance;
			if (!force && this._balanceCache == balance)
			{
				return;
			}
			this._balanceCache = balance;
			if (this._colored)
			{
				this._text.text = string.Format("{0}{1}{2}", this._colorOpenByCurrency[this._type], balance, this._colorClose);
				return;
			}
			this._text.text = balance.ToString();
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x000315E0 File Offset: 0x0002F7E0
		public void SetType(GameData.Currency.Type type)
		{
			this._type = type;
			switch (type)
			{
			case GameData.Currency.Type.Gold:
				this._label.text = Localization.GetLocalizedString("label/balance/goldBalance");
				break;
			case GameData.Currency.Type.DarkQuartz:
				this._label.text = Localization.GetLocalizedString("label/balance/darkQuartzBalance");
				break;
			case GameData.Currency.Type.Bone:
				this._label.text = Localization.GetLocalizedString("label/balance/boneBalance");
				break;
			}
			this.UpdateText(true);
		}

		// Token: 0x04000DAF RID: 3503
		[SerializeField]
		private GameData.Currency.Type _type;

		// Token: 0x04000DB0 RID: 3504
		[SerializeField]
		private TextMeshProUGUI _label;

		// Token: 0x04000DB1 RID: 3505
		[SerializeField]
		private TextMeshProUGUI _text;

		// Token: 0x04000DB2 RID: 3506
		[SerializeField]
		private bool _colored;

		// Token: 0x04000DB3 RID: 3507
		private int _balanceCache;
	}
}
