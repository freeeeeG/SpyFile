using System;
using UnityEngine;

namespace UI.Boss
{
	// Token: 0x02000459 RID: 1113
	public class BossUIContainer : MonoBehaviour
	{
		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001530 RID: 5424 RVA: 0x00042BCC File Offset: 0x00040DCC
		public BossAppearnaceText appearnaceText
		{
			get
			{
				return this._appearnaceText;
			}
		}

		// Token: 0x04001283 RID: 4739
		[SerializeField]
		private BossAppearnaceText _appearnaceText;

		// Token: 0x04001284 RID: 4740
		[SerializeField]
		private GameObject _container;
	}
}
