using System;
using EndingCredit;
using UnityEngine;

namespace UI.Pause
{
	// Token: 0x02000417 RID: 1047
	public class CreditExit : PauseEvent
	{
		// Token: 0x060013DF RID: 5087 RVA: 0x0003CDB8 File Offset: 0x0003AFB8
		public override void Invoke()
		{
			if (this._activated)
			{
				return;
			}
			this._activated = true;
			base.StartCoroutine(this._endingCredit.CLoadScene());
		}

		// Token: 0x040010E4 RID: 4324
		[SerializeField]
		private CreditRoll _endingCredit;

		// Token: 0x040010E5 RID: 4325
		private bool _activated;
	}
}
