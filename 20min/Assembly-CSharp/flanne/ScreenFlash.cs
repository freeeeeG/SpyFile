using System;
using System.Collections;
using flanne.UI;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200010C RID: 268
	public class ScreenFlash : MonoBehaviour
	{
		// Token: 0x0600078F RID: 1935 RVA: 0x00020D2A File Offset: 0x0001EF2A
		public void Flash(int numTimes)
		{
			if (this.flashhCoroutine != null)
			{
				base.StopCoroutine(this.flashhCoroutine);
				this.flashhCoroutine = null;
				return;
			}
			this.flashhCoroutine = this.FlashCR(numTimes);
			base.StartCoroutine(this.flashhCoroutine);
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x00020D62 File Offset: 0x0001EF62
		private IEnumerator FlashCR(int numTimes)
		{
			int num;
			for (int i = 0; i < numTimes; i = num + 1)
			{
				this.flashPanel.Show();
				yield return new WaitForSecondsRealtime(0.05f);
				this.flashPanel.Hide();
				yield return new WaitForSecondsRealtime(0.05f);
				num = i;
			}
			this.flashhCoroutine = null;
			yield break;
		}

		// Token: 0x0400056F RID: 1391
		[SerializeField]
		private Panel flashPanel;

		// Token: 0x04000570 RID: 1392
		private IEnumerator flashhCoroutine;
	}
}
