using System;
using Characters.Gear.Synergy.Inscriptions;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GearPopup
{
	// Token: 0x02000458 RID: 1112
	public class KeywordElement : MonoBehaviour
	{
		// Token: 0x0600152D RID: 5421 RVA: 0x00042A6C File Offset: 0x00040C6C
		public void Set(Inscription.Key key, int delta = 0)
		{
			this._key = key;
			this._keyword = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy.inscriptions[key];
			if (this._icon != null)
			{
				Sprite sprite = this._keyword.activeIcon;
				int num = this._keyword.count + delta;
				if (num >= this._keyword.maxStep)
				{
					sprite = this._keyword.fullActiveIcon;
				}
				else if (num < this._keyword.settings.steps[1])
				{
					sprite = this._keyword.deactiveIcon;
				}
				this._icon.sprite = sprite;
			}
			if (this._name != null)
			{
				this._name.text = this._keyword.name;
			}
			this.UpdateLevel(delta);
			if (this._description != null)
			{
				this._description.text = this._keyword.GetDescription();
			}
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x00042B70 File Offset: 0x00040D70
		public void UpdateLevel(int delta = 0)
		{
			if (this._level == null)
			{
				return;
			}
			string format;
			if (delta > 0)
			{
				format = "<color=#5FED64>{0}</color>";
			}
			else if (delta == 0)
			{
				format = "{0}";
			}
			else
			{
				format = "<color=#FF4D4D>{0}</color>";
			}
			this._level.text = string.Format(format, this._keyword.count + delta);
		}

		// Token: 0x0400127D RID: 4733
		[SerializeField]
		private Image _icon;

		// Token: 0x0400127E RID: 4734
		[SerializeField]
		private TMP_Text _name;

		// Token: 0x0400127F RID: 4735
		[SerializeField]
		private TMP_Text _level;

		// Token: 0x04001280 RID: 4736
		[SerializeField]
		private TMP_Text _description;

		// Token: 0x04001281 RID: 4737
		private Inscription.Key _key;

		// Token: 0x04001282 RID: 4738
		private Inscription _keyword;
	}
}
