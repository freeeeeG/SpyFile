using System;
using Characters.Gear.Upgrades;
using Characters.Player;
using GameResources;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInput;

namespace UI.Upgrades
{
	// Token: 0x020003ED RID: 1005
	public sealed class Option : MonoBehaviour
	{
		// Token: 0x060012CD RID: 4813 RVA: 0x00038300 File Offset: 0x00036500
		public void Initialize(Panel panel)
		{
			this._panel = panel;
			this._noMoneyColorCode = ColorUtility.ToHtmlStringRGB(Color.red);
			Singleton<UpgradeShop>.Instance.onUpgraded += this.HandleOnLevelUp;
			Singleton<UpgradeShop>.Instance.onLevelUp += this.HandleOnLevelUp;
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x00038350 File Offset: 0x00036550
		private void HandleOnLevelUp(UpgradeResource.Reference reference)
		{
			UpgradeInventory upgrade = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade;
			int num = upgrade.IndexOf(reference);
			if (num < 0)
			{
				return;
			}
			UpgradeObject upgradeObject = upgrade.upgrades[num];
			this._level.LevelUp(upgradeObject.level);
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x000383A7 File Offset: 0x000365A7
		public void Set(UpgradedElement element)
		{
			this._currentUpgradedElement = element;
			this._currentReference = this._currentUpgradedElement.reference;
			this.UpdateData();
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x000383C7 File Offset: 0x000365C7
		public void Set(UpgradeElement element)
		{
			this._currentElement = element;
			this._currentReference = this._currentElement.reference;
			this.UpdateData();
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x000383E8 File Offset: 0x000365E8
		public void UpdateData()
		{
			if (this._currentReference == null)
			{
				this._icon.sprite = null;
				this._name.text = string.Empty;
				this._type.text = string.Empty;
				return;
			}
			this._icon.sprite = this._currentReference.icon;
			this._name.text = this._currentReference.displayName;
			if (this._currentReference.type == UpgradeObject.Type.Cursed)
			{
				this._type.text = UpgradeResource.Reference.curseText;
			}
			else
			{
				this._type.text = string.Empty;
			}
			this.SetFrame();
			int maxLevel = this._currentReference.maxLevel;
			int currentLevel = this._currentReference.GetCurrentLevel();
			this._level.Set(currentLevel, maxLevel, this._currentReference.type == UpgradeObject.Type.Cursed, true);
			this._description.text = this._currentReference.GetNextLevelDescription("F158FF", "3C285F");
			bool flag = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade.IndexOf(this._currentReference) != -1;
			string text = Singleton<UpgradeShop>.Instance.saleCurrency.Has(this._currentReference.price) ? Singleton<UpgradeShop>.Instance.saleCurrency.colorCode : this._noMoneyColorCode;
			this._select.SetActive(currentLevel < maxLevel);
			string localizedString = Localization.GetLocalizedString("label/get");
			this._selectText.text = string.Format("{0} ({1} <color=#{2}>{3}</color>)", new object[]
			{
				localizedString,
				Singleton<UpgradeShop>.Instance.saleCurrency.spriteTMPKey,
				text,
				this._currentReference.price
			});
			this._remove.gameObject.SetActive(flag && this._currentReference.removable);
			this._remove.sprite = ((this._currentReference.type == UpgradeObject.Type.Cursed) ? this._removeRisky : this._removeNormal);
			if (flag && this._currentReference.removable)
			{
				int removeCost = Singleton<UpgradeShop>.Instance.GetRemoveCost(this._currentReference.type);
				string text2 = Singleton<UpgradeShop>.Instance.removeCurrency.Has(removeCost) ? ColorUtility.ToHtmlStringRGB(Color.yellow) : ColorUtility.ToHtmlStringRGB(Color.red);
				string localizedString2 = Localization.GetLocalizedString("label/interaction/destroy");
				this._removeText.text = string.Format("{0} ({1} <color=#{2}>{3}</color>)", new object[]
				{
					localizedString2,
					Singleton<UpgradeShop>.Instance.removeCurrency.spriteTMPKey,
					text2,
					removeCost
				});
			}
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x00038698 File Offset: 0x00036898
		private void SetFrame()
		{
			this._riskFrame.enabled = (this._riskPanel.enabled = (this._currentReference.type == UpgradeObject.Type.Cursed));
			if (this._currentReference.type == UpgradeObject.Type.Cursed)
			{
				Color color;
				ColorUtility.TryParseHtmlString("#D6385E", out color);
				Color color2;
				ColorUtility.TryParseHtmlString("#992555", out color2);
				this._description.color = color2;
				this._name.color = color;
				return;
			}
			Color color3;
			ColorUtility.TryParseHtmlString("#8B36F3", out color3);
			Color color4;
			ColorUtility.TryParseHtmlString("#764CC5", out color4);
			this._description.color = color4;
			this._name.color = color3;
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x00038740 File Offset: 0x00036940
		private void OpenRemoveConfirmText()
		{
			if (!Singleton<UpgradeShop>.Instance.CheckRemovable(this._currentReference))
			{
				if (this._currentUpgradedElement != null)
				{
					this._currentUpgradedElement.PlayFailEffect();
				}
				return;
			}
			UpgradeInventory upgrade = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade;
			int num = upgrade.IndexOf(this._currentReference);
			if (num == -1)
			{
				return;
			}
			UpgradeObject upgradeObject = upgrade.upgrades[num];
			string text = string.Format(Localization.GetLocalizedString("label/upgrade/destroy"), Singleton<UpgradeShop>.Instance.GetRemoveCost(this._currentReference.type), upgradeObject.returnCost);
			this._removeConfirm.Open(text, new Action(this.RemoveUpgrade));
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x00038804 File Offset: 0x00036A04
		private void RemoveUpgrade()
		{
			int focusOnRemoved = 0;
			if (!Singleton<UpgradeShop>.Instance.TryRemoveUpgradeObjet(this._currentReference, ref focusOnRemoved) && this._currentUpgradedElement != null)
			{
				this._currentUpgradedElement.PlayFailEffect();
			}
			UpgradeElement defaultFocusTarget = this._panel.upgradableContainer.GetDefaultFocusTarget();
			this._panel.UpdateUpgradedList();
			this._panel.SetFocusOnRemoved(focusOnRemoved);
			this._currentElement = defaultFocusTarget;
			this._currentReference = this._currentElement.reference;
			this.UpdateData();
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x00038888 File Offset: 0x00036A88
		private void Update()
		{
			if (this._currentReference == null)
			{
				return;
			}
			if (KeyMapper.Map.UiInteraction1.WasPressed && this._currentReference != null && this._currentReference.removable && Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade.Has(this._currentReference))
			{
				this.OpenRemoveConfirmText();
			}
		}

		// Token: 0x04000FC9 RID: 4041
		[SerializeField]
		private Image _riskPanel;

		// Token: 0x04000FCA RID: 4042
		[SerializeField]
		private Image _riskFrame;

		// Token: 0x04000FCB RID: 4043
		[SerializeField]
		private Image _icon;

		// Token: 0x04000FCC RID: 4044
		[SerializeField]
		private TMP_Text _name;

		// Token: 0x04000FCD RID: 4045
		[SerializeField]
		private TMP_Text _type;

		// Token: 0x04000FCE RID: 4046
		[SerializeField]
		private Level _level;

		// Token: 0x04000FCF RID: 4047
		[SerializeField]
		private TMP_Text _description;

		// Token: 0x04000FD0 RID: 4048
		[SerializeField]
		private TMP_Text _cost;

		// Token: 0x04000FD1 RID: 4049
		[SerializeField]
		private Confirm _removeConfirm;

		// Token: 0x04000FD2 RID: 4050
		[Header("Bottom")]
		[SerializeField]
		private GameObject _select;

		// Token: 0x04000FD3 RID: 4051
		[SerializeField]
		private TMP_Text _selectText;

		// Token: 0x04000FD4 RID: 4052
		[SerializeField]
		private Sprite _removeNormal;

		// Token: 0x04000FD5 RID: 4053
		[SerializeField]
		private Sprite _removeRisky;

		// Token: 0x04000FD6 RID: 4054
		[SerializeField]
		private Image _remove;

		// Token: 0x04000FD7 RID: 4055
		[SerializeField]
		private TMP_Text _removeText;

		// Token: 0x04000FD8 RID: 4056
		private UpgradedElement _currentUpgradedElement;

		// Token: 0x04000FD9 RID: 4057
		private UpgradeElement _currentElement;

		// Token: 0x04000FDA RID: 4058
		private UpgradeResource.Reference _currentReference;

		// Token: 0x04000FDB RID: 4059
		private Panel _panel;

		// Token: 0x04000FDC RID: 4060
		private Color _priceColor;

		// Token: 0x04000FDD RID: 4061
		private string _noMoneyColorCode;

		// Token: 0x04000FDE RID: 4062
		private const string originalNameColorCode = "#8B36F3";

		// Token: 0x04000FDF RID: 4063
		private const string originalDescColorCode = "#764CC5";

		// Token: 0x04000FE0 RID: 4064
		private const string riskNameColorCode = "#D6385E";

		// Token: 0x04000FE1 RID: 4065
		private const string riskDescColorCode = "#992555";
	}
}
