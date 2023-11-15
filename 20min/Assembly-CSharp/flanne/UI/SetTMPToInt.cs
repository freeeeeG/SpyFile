using System;
using TMPro;
using UnityEngine;

namespace flanne.UI
{
	// Token: 0x02000233 RID: 563
	public class SetTMPToInt : MonoBehaviour
	{
		// Token: 0x06000C63 RID: 3171 RVA: 0x0002D7A0 File Offset: 0x0002B9A0
		public void SetToInt(int value)
		{
			string text = "";
			for (int i = 0; i < this.minDigits; i++)
			{
				text += "0";
			}
			this.tmp.text = value.ToString(text);
		}

		// Token: 0x040008A8 RID: 2216
		[SerializeField]
		private TMP_Text tmp;

		// Token: 0x040008A9 RID: 2217
		[SerializeField]
		private int minDigits;
	}
}
