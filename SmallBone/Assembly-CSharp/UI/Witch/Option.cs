using System;
using Characters;
using Data;
using TMPro;
using UnityEngine;

namespace UI.Witch
{
	// Token: 0x020003E6 RID: 998
	public class Option : MonoBehaviour
	{
		// Token: 0x060012A1 RID: 4769 RVA: 0x00037B51 File Offset: 0x00035D51
		public void Set(WitchBonus.Bonus bonus)
		{
			this._bonus = bonus;
			this.UpdateTexts();
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x00037B60 File Offset: 0x00035D60
		public void UpdateTexts()
		{
			string empty = string.Empty;
			this._name.text = this._bonus.displayName;
			this._level.text = string.Format("Lv. {0}/{1}", this._bonus.level, this._bonus.maxLevel);
			this._description.text = this._bonus.GetDescription(this._bonus.level);
			if (this._bonus.level < this._bonus.maxLevel)
			{
				this._nextLevelContainer.SetActive(true);
				this._cost.text = this._bonus.levelUpCost.ToString();
				this._nextLevelDescription.text = this._bonus.GetDescription(this._bonus.level + 1);
				return;
			}
			this._nextLevelContainer.SetActive(false);
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x00037C54 File Offset: 0x00035E54
		private void Update()
		{
			if (this._bonus == null || this._bonus.level == this._bonus.maxLevel)
			{
				return;
			}
			this._cost.color = (GameData.Currency.darkQuartz.Has(this._bonus.levelUpCost) ? Option._darkQuartzColor : Color.red);
		}

		// Token: 0x04000F9D RID: 3997
		private static readonly Color _darkQuartzColor = new Color(0.5686275f, 0.34901962f, 0.85882354f);

		// Token: 0x04000F9E RID: 3998
		[SerializeField]
		private TMP_Text _name;

		// Token: 0x04000F9F RID: 3999
		[SerializeField]
		private TMP_Text _level;

		// Token: 0x04000FA0 RID: 4000
		[SerializeField]
		private TMP_Text _description;

		// Token: 0x04000FA1 RID: 4001
		[SerializeField]
		private TMP_Text _nextLevelDescription;

		// Token: 0x04000FA2 RID: 4002
		[SerializeField]
		private GameObject _nextLevelContainer;

		// Token: 0x04000FA3 RID: 4003
		[SerializeField]
		private TMP_Text _cost;

		// Token: 0x04000FA4 RID: 4004
		private WitchBonus.Bonus _bonus;
	}
}
