using System;
using TMPro;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200013C RID: 316
	public class UnlockAtDarkness : MonoBehaviour
	{
		// Token: 0x06000850 RID: 2128 RVA: 0x00022FFE File Offset: 0x000211FE
		public void CheckUnlock(int diffUnlocked)
		{
			if (diffUnlocked >= this.darknessReq)
			{
				this.unlockable.Unlock();
			}
			else
			{
				this.unlockable.Lock();
			}
			this.unlockConTMP.enabled = (diffUnlocked < this.darknessReq);
		}

		// Token: 0x0400061F RID: 1567
		[SerializeField]
		private Unlockable unlockable;

		// Token: 0x04000620 RID: 1568
		[SerializeField]
		private int darknessReq;

		// Token: 0x04000621 RID: 1569
		[SerializeField]
		private TMP_Text unlockConTMP;
	}
}
