using System;
using System.Collections;
using UnityEngine;

namespace EpicToonFX
{
	// Token: 0x020002BE RID: 702
	public class ETFXTarget : MonoBehaviour
	{
		// Token: 0x06001131 RID: 4401 RVA: 0x00030FBB File Offset: 0x0002F1BB
		private void Start()
		{
			this.targetRenderer = base.GetComponent<Renderer>();
			this.targetCollider = base.GetComponent<Collider>();
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x00030FD8 File Offset: 0x0002F1D8
		private void SpawnTarget()
		{
			this.targetRenderer.enabled = true;
			this.targetCollider.enabled = true;
			Object.Destroy(Object.Instantiate<GameObject>(this.respawnParticle, base.transform.position, base.transform.rotation), 3.5f);
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x00031028 File Offset: 0x0002F228
		private void OnTriggerEnter(Collider col)
		{
			if (col.tag == "Missile" && this.hitParticle)
			{
				Object.Destroy(Object.Instantiate<GameObject>(this.hitParticle, base.transform.position, base.transform.rotation), 2f);
				this.targetRenderer.enabled = false;
				this.targetCollider.enabled = false;
				base.StartCoroutine(this.Respawn());
			}
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x000310A4 File Offset: 0x0002F2A4
		private IEnumerator Respawn()
		{
			yield return new WaitForSeconds(3f);
			this.SpawnTarget();
			yield break;
		}

		// Token: 0x0400097D RID: 2429
		[Header("Effect shown on target hit")]
		public GameObject hitParticle;

		// Token: 0x0400097E RID: 2430
		[Header("Effect shown on target respawn")]
		public GameObject respawnParticle;

		// Token: 0x0400097F RID: 2431
		private Renderer targetRenderer;

		// Token: 0x04000980 RID: 2432
		private Collider targetCollider;
	}
}
