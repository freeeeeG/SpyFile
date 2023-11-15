using System;
using Characters.Gear;
using Characters.Gear.Items;
using GameResources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInput;

namespace UI.Inventory
{
	// Token: 0x02000433 RID: 1075
	public class ItemOption : MonoBehaviour
	{
		// Token: 0x06001483 RID: 5251 RVA: 0x0003F02C File Offset: 0x0003D22C
		public void Set(Item item)
		{
			this._thumnailIcon.enabled = true;
			this._thumnailIcon.sprite = item.thumbnail;
			this._thumnailIcon.transform.localScale = Vector3.one * 3f;
			this._thumnailIcon.SetNativeSize();
			this._name.text = item.displayName;
			if (item.gearTag.HasFlag(Gear.Tag.Omen))
			{
				this._rarity.text = Localization.GetLocalizedString("synergy/key/Omen/name");
			}
			else
			{
				this._rarity.text = Localization.GetLocalizedString(string.Format("{0}/{1}/{2}", "label", "Rarity", item.rarity));
			}
			string text = item.hasFlavor ? item.flavor : string.Empty;
			this._flavorSimple.text = text;
			this._description.text = item.description;
			this._itemDiscardKey.gameObject.SetActive(true);
			this._itemDiscardText.text = Localization.GetLocalizedString("label/inventory/discardItem");
			if (item.currencyByDiscard > 0)
			{
				this._itemDiscardText.text = string.Format("{0}(<color=#FFDE37>{1}</color>)", this._itemDiscardText.text, item.currencyByDiscard);
			}
			this._keyword1.Set(item.keyword1);
			this._keyword2.Set(item.keyword2);
			this._keyword1Detail.Set(item.keyword1);
			this._keyword2Detail.Set(item.keyword2);
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x0003F1C0 File Offset: 0x0003D3C0
		private void Update()
		{
			if (!this._detailContainer.activeSelf || KeyMapper.Map.UiInteraction3.IsPressed)
			{
				if (!this._detailContainer.activeSelf && KeyMapper.Map.UiInteraction3.IsPressed)
				{
					this._simpleContainer.SetActive(false);
					this._detailContainer.SetActive(true);
				}
				return;
			}
			if (KeyMapper.Map.UiInteraction2.IsPressed)
			{
				return;
			}
			this._simpleContainer.SetActive(true);
			this._detailContainer.SetActive(false);
		}

		// Token: 0x04001175 RID: 4469
		[SerializeField]
		private Image _thumnailIcon;

		// Token: 0x04001176 RID: 4470
		[SerializeField]
		private TMP_Text _name;

		// Token: 0x04001177 RID: 4471
		[SerializeField]
		private TMP_Text _rarity;

		// Token: 0x04001178 RID: 4472
		[SerializeField]
		[Space]
		private GameObject _simpleContainer;

		// Token: 0x04001179 RID: 4473
		[SerializeField]
		private GameObject _detailContainer;

		// Token: 0x0400117A RID: 4474
		[SerializeField]
		[Space]
		private TMP_Text _flavorSimple;

		// Token: 0x0400117B RID: 4475
		[SerializeField]
		private TMP_Text _flavorDetail;

		// Token: 0x0400117C RID: 4476
		[SerializeField]
		private TMP_Text _description;

		// Token: 0x0400117D RID: 4477
		[SerializeField]
		[Space]
		private PressingButton _itemDiscardKey;

		// Token: 0x0400117E RID: 4478
		[SerializeField]
		private TMP_Text _itemDiscardText;

		// Token: 0x0400117F RID: 4479
		[Space]
		[SerializeField]
		private KeywordOption _keyword1;

		// Token: 0x04001180 RID: 4480
		[SerializeField]
		private KeywordOption _keyword2;

		// Token: 0x04001181 RID: 4481
		[SerializeField]
		[Space]
		private KeywordOption _keyword1Detail;

		// Token: 0x04001182 RID: 4482
		[SerializeField]
		private KeywordOption _keyword2Detail;

		// Token: 0x04001183 RID: 4483
		private const string _omenKey = "synergy/key/Omen/name";
	}
}
