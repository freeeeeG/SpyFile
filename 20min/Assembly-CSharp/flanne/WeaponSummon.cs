using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000131 RID: 305
	public class WeaponSummon : Summon
	{
		// Token: 0x0600082D RID: 2093 RVA: 0x00022970 File Offset: 0x00020B70
		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.tag.Contains("Enemy"))
			{
				Health component = other.gameObject.GetComponent<Health>();
				if (component == null)
				{
					return;
				}
				int damage = Mathf.FloorToInt(base.summonDamageMod.Modify((float)this.baseDamage));
				damage = base.ApplyDamageMods(damage);
				component.TakeDamage(DamageType.Summon, damage, 1f);
				this.PostNotification(Summon.SummonOnHitNotification, other.gameObject);
			}
		}

		// Token: 0x04000604 RID: 1540
		public int baseDamage = 50;
	}
}
