using System;
using TMPro;
using UnityEngine;

namespace flanne.UI
{
	// Token: 0x02000232 RID: 562
	public class SetTMPAtRuntime : MonoBehaviour
	{
		// Token: 0x06000C61 RID: 3169 RVA: 0x0002D78B File Offset: 0x0002B98B
		private void Start()
		{
			this.tmp.text = this.text;
		}

		// Token: 0x040008A6 RID: 2214
		[SerializeField]
		private TMP_Text tmp;

		// Token: 0x040008A7 RID: 2215
		[SerializeField]
		private string text;
	}
}
