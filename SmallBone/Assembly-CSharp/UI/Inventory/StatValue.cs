using System;
using TMPro;
using UnityEngine;

namespace UI.Inventory
{
	// Token: 0x02000448 RID: 1096
	public sealed class StatValue : MonoBehaviour
	{
		// Token: 0x060014D9 RID: 5337 RVA: 0x00040ED2 File Offset: 0x0003F0D2
		public void Set(string text, bool positive = true, string unit = "")
		{
			if (!string.IsNullOrEmpty(unit) && positive)
			{
				this._valueText.text = "+" + text;
			}
			else
			{
				this._valueText.text = text;
			}
			this._unit.text = unit;
		}

		// Token: 0x04001202 RID: 4610
		[SerializeField]
		private TMP_Text _valueText;

		// Token: 0x04001203 RID: 4611
		[SerializeField]
		private TMP_Text _unit;
	}
}
