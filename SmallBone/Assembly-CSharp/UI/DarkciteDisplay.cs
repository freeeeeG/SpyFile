using System;
using Data;
using TMPro;
using UnityEngine;

namespace UI
{
	// Token: 0x02000396 RID: 918
	public class DarkciteDisplay : MonoBehaviour
	{
		// Token: 0x060010D2 RID: 4306 RVA: 0x00031AC0 File Offset: 0x0002FCC0
		private void Update()
		{
			this._amount.text = GameData.Currency.darkQuartz.balance.ToString();
		}

		// Token: 0x04000DC8 RID: 3528
		[SerializeField]
		private TextMeshProUGUI _amount;
	}
}
