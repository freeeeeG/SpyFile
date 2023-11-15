using System;
using System.Collections.ObjectModel;
using Characters.Gear.Synergy.Inscriptions;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
	// Token: 0x02000437 RID: 1079
	public sealed class KeywordOption : MonoBehaviour
	{
		// Token: 0x06001491 RID: 5265 RVA: 0x0003F608 File Offset: 0x0003D808
		public void Set(Inscription.Key key)
		{
			Inscription inscription = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy.inscriptions[key];
			int count = inscription.count;
			this._icon.sprite = inscription.activeIcon;
			this._name.text = inscription.name;
			ReadOnlyCollection<int> steps = inscription.steps;
			int step = inscription.step;
			this._stepLevel.Set(key, false);
			if (this._level != null)
			{
				this._level.text = count.ToString();
			}
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
				this._stepElements[num].Set(key, steps, j, step >= j);
				this._stepElements[num].ClampHeight(vector.x, vector.y);
				num++;
			}
		}

		// Token: 0x04001198 RID: 4504
		[SerializeField]
		private Image _icon;

		// Token: 0x04001199 RID: 4505
		[SerializeField]
		private TMP_Text _name;

		// Token: 0x0400119A RID: 4506
		[SerializeField]
		private TMP_Text _level;

		// Token: 0x0400119B RID: 4507
		[SerializeField]
		[Space]
		private InscriptionStepElement[] _stepElements;

		// Token: 0x0400119C RID: 4508
		[SerializeField]
		private Vector2[] _stepElementsHeightByStepLength;

		// Token: 0x0400119D RID: 4509
		[SerializeField]
		private InscriptionStepLevel _stepLevel;
	}
}
