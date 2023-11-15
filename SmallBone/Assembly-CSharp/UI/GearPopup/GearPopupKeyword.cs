using System;
using Characters.Gear.Synergy.Inscriptions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GearPopup
{
	// Token: 0x0200044F RID: 1103
	public class GearPopupKeyword : MonoBehaviour
	{
		// Token: 0x06001509 RID: 5385 RVA: 0x0004203A File Offset: 0x0004023A
		public void Set(Inscription.Key key)
		{
			this._icon.sprite = Inscription.GetActiveIcon(key);
			this._name.text = Inscription.GetName(key);
		}

		// Token: 0x04001256 RID: 4694
		[SerializeField]
		private Image _icon;

		// Token: 0x04001257 RID: 4695
		[SerializeField]
		private TMP_Text _name;
	}
}
