using System;
using System.Collections;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000F8 RID: 248
	public class PlayerFlasher : MonoBehaviour
	{
		// Token: 0x06000727 RID: 1831 RVA: 0x0001F7E0 File Offset: 0x0001D9E0
		private void Start()
		{
			this.isFlashing = new BoolToggle(false);
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0001F7EE File Offset: 0x0001D9EE
		public void Flash()
		{
			this.isFlashing.Flip();
			if (this.isFlashing.value && this.flashCoroutine == null)
			{
				this.flashCoroutine = this.FlashingCR();
				base.StartCoroutine(this.flashCoroutine);
			}
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0001F829 File Offset: 0x0001DA29
		public void StopFlash()
		{
			this.isFlashing.UnFlip();
			if (!this.isFlashing.value && this.flashCoroutine != null)
			{
				base.StopCoroutine(this.flashCoroutine);
				this.flashCoroutine = null;
			}
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0001F85E File Offset: 0x0001DA5E
		private IEnumerator FlashingCR()
		{
			for (;;)
			{
				this.flasher.Flash();
				yield return new WaitForSeconds(0.5f);
			}
			yield break;
		}

		// Token: 0x040004FE RID: 1278
		[SerializeField]
		private FlashSprite flasher;

		// Token: 0x040004FF RID: 1279
		private BoolToggle isFlashing;

		// Token: 0x04000500 RID: 1280
		private IEnumerator flashCoroutine;
	}
}
