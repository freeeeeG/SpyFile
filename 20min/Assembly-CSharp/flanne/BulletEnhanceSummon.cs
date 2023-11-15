using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000121 RID: 289
	public class BulletEnhanceSummon : Summon
	{
		// Token: 0x060007EC RID: 2028 RVA: 0x00021B54 File Offset: 0x0001FD54
		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.tag == "Bullet")
			{
				Projectile component = other.gameObject.GetComponent<Projectile>();
				if (component == null)
				{
					return;
				}
				this.ModifyProjectile(component);
			}
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x00021B98 File Offset: 0x0001FD98
		private void ModifyProjectile(Projectile projectile)
		{
			projectile.damage += base.summonDamageMod.Modify(this.player.gun.damage * this.damageMultiplier) * this.overallMultiplier;
			projectile.size = projectile.transform.localScale.x * (this.sizeMultiplier * this.overallMultiplier);
			projectile.bounce += Mathf.FloorToInt((float)this.additionalBounces * this.overallMultiplier);
			if (this.addBurn)
			{
				BurnOnCollision burnOnCollision = projectile.gameObject.AddComponent<BurnOnCollision>();
				burnOnCollision.burnDamage = Mathf.FloorToInt(3f * this.overallMultiplier);
				burnOnCollision.hitTag = "Enemy";
			}
		}

		// Token: 0x040005C3 RID: 1475
		[NonSerialized]
		public float overallMultiplier = 1f;

		// Token: 0x040005C4 RID: 1476
		public float damageMultiplier;

		// Token: 0x040005C5 RID: 1477
		public float sizeMultiplier = 1f;

		// Token: 0x040005C6 RID: 1478
		public int additionalBounces;

		// Token: 0x040005C7 RID: 1479
		public bool addBurn;
	}
}
