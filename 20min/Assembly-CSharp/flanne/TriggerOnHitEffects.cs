using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000136 RID: 310
	public class TriggerOnHitEffects : MonoBehaviour
	{
		// Token: 0x0600083C RID: 2108 RVA: 0x00022CDF File Offset: 0x00020EDF
		private void OnCollisionEnter2D(Collision2D other)
		{
			PlayerController.Instance.gameObject.PostNotification(Projectile.ImpactEvent, other.gameObject);
		}
	}
}
