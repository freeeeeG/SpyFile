using System;
using Characters.Gear.Synergy.Inscriptions;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
	// Token: 0x02000436 RID: 1078
	public class KeywordElement : MonoBehaviour
	{
		// Token: 0x0600148F RID: 5263 RVA: 0x0003F540 File Offset: 0x0003D740
		public void Set(Inscription.Key key)
		{
			Inscription inscription = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy.inscriptions[key];
			int count = inscription.count;
			this._icon.sprite = inscription.icon;
			this._name.text = inscription.name;
			this._stepLevel.Set(key, true);
			if (this._level != null)
			{
				string htmlString = "#D7C0AA";
				if (inscription.isMaxStep)
				{
					htmlString = "#fbd53e";
				}
				else if (!inscription.active)
				{
					htmlString = "#9c8160";
				}
				Color color;
				ColorUtility.TryParseHtmlString(htmlString, out color);
				this._level.text = count.ToString();
				this._level.color = color;
			}
		}

		// Token: 0x04001191 RID: 4497
		private const string fullActiveLevel = "#fbd53e";

		// Token: 0x04001192 RID: 4498
		private const string activeLevel = "#D7C0AA";

		// Token: 0x04001193 RID: 4499
		private const string deactiveLevel = "#9c8160";

		// Token: 0x04001194 RID: 4500
		[SerializeField]
		private Image _icon;

		// Token: 0x04001195 RID: 4501
		[SerializeField]
		private TMP_Text _name;

		// Token: 0x04001196 RID: 4502
		[SerializeField]
		private TMP_Text _level;

		// Token: 0x04001197 RID: 4503
		[SerializeField]
		private InscriptionStepLevel _stepLevel;
	}
}
