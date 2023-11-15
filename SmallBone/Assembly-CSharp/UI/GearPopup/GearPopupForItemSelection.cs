using System;
using Characters.Gear;
using Characters.Gear.Items;
using GameResources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GearPopup
{
	// Token: 0x0200044E RID: 1102
	public class GearPopupForItemSelection : MonoBehaviour
	{
		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06001504 RID: 5380 RVA: 0x00041F79 File Offset: 0x00040179
		public RectTransform rectTransform
		{
			get
			{
				return this._rectTransform;
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06001505 RID: 5381 RVA: 0x000412F6 File Offset: 0x0003F4F6
		private static string _interactionLootLabel
		{
			get
			{
				return Localization.GetLocalizedString("label/interaction/loot");
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06001506 RID: 5382 RVA: 0x00041302 File Offset: 0x0003F502
		private static string _interactionPurcaseLabel
		{
			get
			{
				return Localization.GetLocalizedString("label/interaction/purchase");
			}
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x00041F84 File Offset: 0x00040184
		public void Set(Item item)
		{
			this._gear = item;
			this._name.text = item.displayName;
			if (item.gearTag.HasFlag(Gear.Tag.Omen))
			{
				this._rarity.text = Localization.GetLocalizedString("synergy/key/Omen/name");
			}
			else
			{
				this._rarity.text = Localization.GetLocalizedString(string.Format("{0}/{1}/{2}", "label", "Rarity", item.rarity));
			}
			this._description.text = item.description;
			this._keyword1.Set(item.keyword1);
			this._keyword2.Set(item.keyword2);
		}

		// Token: 0x0400124D RID: 4685
		[SerializeField]
		private Image _image;

		// Token: 0x0400124E RID: 4686
		[Space]
		[SerializeField]
		private RectTransform _rectTransform;

		// Token: 0x0400124F RID: 4687
		[Space]
		[SerializeField]
		private TMP_Text _name;

		// Token: 0x04001250 RID: 4688
		[SerializeField]
		[Space]
		private TMP_Text _rarity;

		// Token: 0x04001251 RID: 4689
		[SerializeField]
		[Space]
		private TMP_Text _description;

		// Token: 0x04001252 RID: 4690
		[Space]
		[SerializeField]
		private GearPopupKeyword _keyword1;

		// Token: 0x04001253 RID: 4691
		[SerializeField]
		private GearPopupKeyword _keyword2;

		// Token: 0x04001254 RID: 4692
		private Gear _gear;

		// Token: 0x04001255 RID: 4693
		private const string _omenKey = "synergy/key/Omen/name";
	}
}
