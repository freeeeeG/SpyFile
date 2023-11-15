using System;
using Data;
using TMPro;
using UnityEngine;

namespace UI
{
	// Token: 0x020003A2 RID: 930
	public class GoldDisplay : MonoBehaviour
	{
		// Token: 0x0600111F RID: 4383 RVA: 0x00032C78 File Offset: 0x00030E78
		private void Update()
		{
			this._amount.text = GameData.Currency.gold.balance.ToString();
		}

		// Token: 0x04000E19 RID: 3609
		[SerializeField]
		private TextMeshProUGUI _amount;
	}
}
