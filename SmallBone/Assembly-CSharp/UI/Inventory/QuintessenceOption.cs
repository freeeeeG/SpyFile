using System;
using Characters.Gear.Quintessences;
using GameResources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
	// Token: 0x02000440 RID: 1088
	public class QuintessenceOption : MonoBehaviour
	{
		// Token: 0x060014AF RID: 5295 RVA: 0x000404C0 File Offset: 0x0003E6C0
		public void Set(Quintessence essence)
		{
			this._thumnailIcon.enabled = true;
			this._thumnailIcon.sprite = essence.thumbnail;
			this._thumnailIcon.transform.localScale = Vector3.one * 3f;
			this._thumnailIcon.SetNativeSize();
			this._name.text = essence.displayName;
			this._rarity.text = Localization.GetLocalizedString(string.Format("{0}/{1}/{2}", "label", "Rarity", essence.rarity));
			this._cooldown.text = essence.cooldown.time.cooldownTime.ToString();
			this._flavor.text = (essence.hasFlavor ? essence.flavor : string.Empty);
			this._activeName.text = essence.activeName;
			this._activeDescription.text = essence.activeDescription;
		}

		// Token: 0x040011C6 RID: 4550
		[SerializeField]
		private Image _thumnailIcon;

		// Token: 0x040011C7 RID: 4551
		[SerializeField]
		private TMP_Text _name;

		// Token: 0x040011C8 RID: 4552
		[SerializeField]
		private TMP_Text _rarity;

		// Token: 0x040011C9 RID: 4553
		[SerializeField]
		private TMP_Text _cooldown;

		// Token: 0x040011CA RID: 4554
		[Space]
		[SerializeField]
		private TMP_Text _flavor;

		// Token: 0x040011CB RID: 4555
		[Space]
		[SerializeField]
		private TMP_Text _activeName;

		// Token: 0x040011CC RID: 4556
		[SerializeField]
		private TMP_Text _activeDescription;
	}
}
