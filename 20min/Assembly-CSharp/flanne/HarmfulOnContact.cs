using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x020000BC RID: 188
	public class HarmfulOnContact : Harmful
	{
		// Token: 0x06000614 RID: 1556 RVA: 0x0001C327 File Offset: 0x0001A527
		private void Start()
		{
			this.player = PlayerController.Instance;
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x0001C334 File Offset: 0x0001A534
		private void OnTriggerEnter2D(Collider2D other)
		{
			this.Harm(other.gameObject);
			if (this.procOnHit)
			{
				this.player.gameObject.PostNotification(Projectile.ImpactEvent, other.gameObject);
			}
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x0001C365 File Offset: 0x0001A565
		private void OnCollisionEnter2D(Collision2D other)
		{
			this.Harm(other.gameObject);
			if (this.procOnHit)
			{
				this.player.gameObject.PostNotification(Projectile.ImpactEvent, other.gameObject);
			}
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x0001C398 File Offset: 0x0001A598
		private void Harm(GameObject gameObject)
		{
			Health component = gameObject.GetComponent<Health>();
			if (component != null)
			{
				component.HPChange(-1 * this.damageAmount);
				this.onHarm.Invoke();
				this.PostNotification(HarmfulOnContact.HitNotification, gameObject);
			}
		}

		// Token: 0x040003E8 RID: 1000
		public static string HitNotification = "HarmfulOnContact.HitNotification";

		// Token: 0x040003E9 RID: 1001
		public bool procOnHit;

		// Token: 0x040003EA RID: 1002
		public UnityEvent onHarm;

		// Token: 0x040003EB RID: 1003
		private PlayerController player;
	}
}
