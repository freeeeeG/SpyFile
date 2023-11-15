using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000F4 RID: 244
	public class PersistentHarm : MonoBehaviour
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x0001EFE3 File Offset: 0x0001D1E3
		public float finalSecondsPerTick
		{
			get
			{
				return this.tickRate.ModifyInverse(this.secondsPerTick);
			}
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0001EFF6 File Offset: 0x0001D1F6
		private void Awake()
		{
			this._targets = new HashSet<Health>();
			this.tickRate = new StatMod();
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0001F010 File Offset: 0x0001D210
		private void OnCollisionEnter2D(Collision2D other)
		{
			Health component = other.gameObject.GetComponent<Health>();
			if (component != null)
			{
				this._targets.Add(component);
			}
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0001F040 File Offset: 0x0001D240
		private void OnCollisionExit2D(Collision2D other)
		{
			Health component = other.gameObject.GetComponent<Health>();
			if (component != null)
			{
				this._targets.Remove(component);
			}
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0001F06F File Offset: 0x0001D26F
		private void Update()
		{
			this._timer += Time.deltaTime;
			if (this._timer >= this.finalSecondsPerTick)
			{
				this._timer -= this.finalSecondsPerTick;
				this.DealDamage();
			}
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0001F0AC File Offset: 0x0001D2AC
		private void DealDamage()
		{
			foreach (Health health in new HashSet<Health>(this._targets))
			{
				if (health.gameObject.activeSelf)
				{
					health.TakeDamage(this.damageType, this.damageAmount, 1f);
					if (this.triggerOnHit)
					{
						PlayerController.Instance.gameObject.PostNotification(Projectile.ImpactEvent, health.gameObject);
					}
				}
			}
		}

		// Token: 0x040004CF RID: 1231
		public int damageAmount;

		// Token: 0x040004D0 RID: 1232
		public DamageType damageType;

		// Token: 0x040004D1 RID: 1233
		[SerializeField]
		private float secondsPerTick;

		// Token: 0x040004D2 RID: 1234
		public StatMod tickRate;

		// Token: 0x040004D3 RID: 1235
		public bool triggerOnHit;

		// Token: 0x040004D4 RID: 1236
		private HashSet<Health> _targets;

		// Token: 0x040004D5 RID: 1237
		private float _timer;
	}
}
