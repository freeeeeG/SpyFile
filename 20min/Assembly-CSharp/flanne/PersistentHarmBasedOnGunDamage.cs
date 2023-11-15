using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000F5 RID: 245
	public class PersistentHarmBasedOnGunDamage : MonoBehaviour
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x0001F144 File Offset: 0x0001D344
		public float finalSecondsPerTick
		{
			get
			{
				return this.tickRate.ModifyInverse(this.secondsPerTick);
			}
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0001F157 File Offset: 0x0001D357
		private void Awake()
		{
			this._targets = new HashSet<Health>();
			this.tickRate = new StatMod();
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0001F16F File Offset: 0x0001D36F
		private void Start()
		{
			this.playerGun = PlayerController.Instance.gun;
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0001F184 File Offset: 0x0001D384
		private void OnCollisionEnter2D(Collision2D other)
		{
			Health component = other.gameObject.GetComponent<Health>();
			if (component != null)
			{
				this._targets.Add(component);
			}
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0001F1B4 File Offset: 0x0001D3B4
		private void OnCollisionExit2D(Collision2D other)
		{
			Health component = other.gameObject.GetComponent<Health>();
			if (component != null)
			{
				this._targets.Remove(component);
			}
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0001F1E3 File Offset: 0x0001D3E3
		private void Update()
		{
			this._timer += Time.deltaTime;
			if (this._timer >= this.finalSecondsPerTick)
			{
				this._timer -= this.finalSecondsPerTick;
				this.DealDamage();
			}
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0001F220 File Offset: 0x0001D420
		private void DealDamage()
		{
			foreach (Health health in new HashSet<Health>(this._targets))
			{
				if (health.gameObject.activeSelf)
				{
					health.TakeDamage(this.damageType, Mathf.FloorToInt(this.multiplier * this.playerGun.damage), 1f);
					if (this.triggerOnHit)
					{
						PlayerController.Instance.gameObject.PostNotification(Projectile.ImpactEvent, health.gameObject);
					}
				}
			}
		}

		// Token: 0x040004D6 RID: 1238
		public float multiplier = 1f;

		// Token: 0x040004D7 RID: 1239
		public DamageType damageType;

		// Token: 0x040004D8 RID: 1240
		[SerializeField]
		private float secondsPerTick;

		// Token: 0x040004D9 RID: 1241
		public StatMod tickRate;

		// Token: 0x040004DA RID: 1242
		public bool triggerOnHit;

		// Token: 0x040004DB RID: 1243
		private Gun playerGun;

		// Token: 0x040004DC RID: 1244
		private HashSet<Health> _targets;

		// Token: 0x040004DD RID: 1245
		private float _timer;
	}
}
