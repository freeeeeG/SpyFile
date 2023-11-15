using System;
using System.Collections;
using UnityEngine;

namespace EpicToonFX
{
	// Token: 0x02000067 RID: 103
	public class ETFXTarget : MonoBehaviour
	{
		// Token: 0x06000176 RID: 374 RVA: 0x00006F04 File Offset: 0x00005104
		private void Start()
		{
			this.targetRenderer = base.GetComponent<Renderer>();
			this.targetCollider = base.GetComponent<Collider>();
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00006F20 File Offset: 0x00005120
		private void SpawnTarget()
		{
			this.targetRenderer.enabled = true;
			this.targetCollider.enabled = true;
			Object.Destroy(Object.Instantiate<GameObject>(this.respawnParticle, base.transform.position, base.transform.rotation), 3.5f);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00006F70 File Offset: 0x00005170
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

		// Token: 0x06000179 RID: 377 RVA: 0x00006FEC File Offset: 0x000051EC
		private IEnumerator Respawn()
		{
			yield return new WaitForSeconds(3f);
			this.SpawnTarget();
			yield break;
		}

		// Token: 0x0400016F RID: 367
		[Header("Effect shown on target hit")]
		public GameObject hitParticle;

		// Token: 0x04000170 RID: 368
		[Header("Effect shown on target respawn")]
		public GameObject respawnParticle;

		// Token: 0x04000171 RID: 369
		private Renderer targetRenderer;

		// Token: 0x04000172 RID: 370
		private Collider targetCollider;
	}
}
