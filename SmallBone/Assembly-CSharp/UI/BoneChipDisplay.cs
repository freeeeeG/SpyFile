using System;
using Data;
using TMPro;
using UnityEngine;

namespace UI
{
	// Token: 0x02000388 RID: 904
	public class BoneChipDisplay : MonoBehaviour
	{
		// Token: 0x06001086 RID: 4230 RVA: 0x00030E84 File Offset: 0x0002F084
		private void Update()
		{
			this._amount.text = GameData.Currency.bone.balance.ToString();
		}

		// Token: 0x04000D8E RID: 3470
		[SerializeField]
		private TextMeshProUGUI _amount;
	}
}
