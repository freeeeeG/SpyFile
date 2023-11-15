using System;
using System.Collections.ObjectModel;
using Characters.Gear.Synergy.Inscriptions;
using Services;
using Singletons;
using TMPro;
using UI.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GearPopup
{
	// Token: 0x02000450 RID: 1104
	public class GearPopupKeywordDetail : MonoBehaviour
	{
		// Token: 0x0600150B RID: 5387 RVA: 0x00042060 File Offset: 0x00040260
		public void Set(Inscription.Key key)
		{
			Inscription inscription = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy.inscriptions[key];
			this._icon.sprite = inscription.activeIcon;
			this._name.text = inscription.name;
			this._level.text = string.Format("{0}/{1}", inscription.count, inscription.maxStep);
			ReadOnlyCollection<int> steps = inscription.steps;
			if (this._stepElements.Length == 0)
			{
				return;
			}
			InscriptionStepElement[] stepElements = this._stepElements;
			for (int i = 0; i < stepElements.Length; i++)
			{
				stepElements[i].gameObject.SetActive(false);
			}
			Vector2 vector = this._stepElementsHeightByStepLength[steps.Count - 1];
			int num = 0;
			for (int j = 1; j < steps.Count; j++)
			{
				this._stepElements[num].Set(key, steps, j, true);
				this._stepElements[num].ClampHeight(vector.x, vector.y);
				num++;
			}
		}

		// Token: 0x04001258 RID: 4696
		[SerializeField]
		private Image _icon;

		// Token: 0x04001259 RID: 4697
		[SerializeField]
		private TMP_Text _name;

		// Token: 0x0400125A RID: 4698
		[SerializeField]
		private TMP_Text _level;

		// Token: 0x0400125B RID: 4699
		[Space]
		[SerializeField]
		private InscriptionStepElement[] _stepElements;

		// Token: 0x0400125C RID: 4700
		[SerializeField]
		private Vector2[] _stepElementsHeightByStepLength;
	}
}
