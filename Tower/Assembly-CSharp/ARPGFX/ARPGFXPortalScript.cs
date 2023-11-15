using System;
using System.Collections;
using UnityEngine;

namespace ARPGFX
{
	// Token: 0x02000070 RID: 112
	public class ARPGFXPortalScript : MonoBehaviour
	{
		// Token: 0x060001A2 RID: 418 RVA: 0x000076CC File Offset: 0x000058CC
		private void Start()
		{
			this.portalOpen = Object.Instantiate<GameObject>(this.portalOpenPrefab, base.transform.position, base.transform.rotation);
			this.portalIdle = Object.Instantiate<GameObject>(this.portalIdlePrefab, base.transform.position, base.transform.rotation);
			this.portalIdle.SetActive(false);
			this.portalClose = Object.Instantiate<GameObject>(this.portalClosePrefab, base.transform.position, base.transform.rotation);
			this.portalClose.SetActive(false);
			base.StartCoroutine("PortalLoop");
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00007772 File Offset: 0x00005972
		private IEnumerator PortalLoop()
		{
			for (;;)
			{
				this.portalOpen.SetActive(true);
				yield return new WaitForSeconds(0.8f);
				this.portalIdle.SetActive(true);
				this.portalOpen.SetActive(false);
				yield return new WaitForSeconds(this.portalLifetime);
				this.portalIdle.SetActive(false);
				this.portalClose.SetActive(true);
				yield return new WaitForSeconds(1f);
				this.portalClose.SetActive(false);
			}
			yield break;
		}

		// Token: 0x0400018D RID: 397
		public GameObject portalOpenPrefab;

		// Token: 0x0400018E RID: 398
		public GameObject portalIdlePrefab;

		// Token: 0x0400018F RID: 399
		public GameObject portalClosePrefab;

		// Token: 0x04000190 RID: 400
		private GameObject portalOpen;

		// Token: 0x04000191 RID: 401
		private GameObject portalIdle;

		// Token: 0x04000192 RID: 402
		private GameObject portalClose;

		// Token: 0x04000193 RID: 403
		public float portalLifetime = 4f;
	}
}
