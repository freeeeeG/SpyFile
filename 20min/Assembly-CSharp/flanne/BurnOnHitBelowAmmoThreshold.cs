using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x020000D6 RID: 214
	public class BurnOnHitBelowAmmoThreshold : MonoBehaviour
	{
		// Token: 0x06000688 RID: 1672 RVA: 0x0001D8FC File Offset: 0x0001BAFC
		private void Start()
		{
			this.BurnSys = BurnSystem.SharedInstance;
			this.ammo = PlayerController.Instance.ammo;
			this.AddObserver(new Action<object, object>(this.OnImpact), Projectile.ImpactEvent, PlayerController.Instance.gameObject);
			this.ammo.OnAmmoChanged.AddListener(new UnityAction<int>(this.OnAmmoChanged));
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0001D961 File Offset: 0x0001BB61
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnImpact), Projectile.ImpactEvent, PlayerController.Instance.gameObject);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0001D984 File Offset: 0x0001BB84
		private void OnImpact(object sender, object args)
		{
			GameObject gameObject = args as GameObject;
			if (gameObject.tag.Contains("Enemy"))
			{
				if (this._active)
				{
					this.BurnSys.Burn(gameObject, this.burnDamageBelowThreshold);
					return;
				}
				this.BurnSys.Burn(gameObject, this.burnDamge);
			}
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0001D9D8 File Offset: 0x0001BBD8
		private void OnAmmoChanged(int amount)
		{
			float num = (float)this.ammo.amount / (float)this.ammo.max;
			if (this._active != num <= this.percentThreshold)
			{
				this._active = (num <= this.percentThreshold);
				if (this._active)
				{
					this.onActivate.Invoke();
					return;
				}
				this.onDeactivate.Invoke();
			}
		}

		// Token: 0x04000453 RID: 1107
		public int burnDamge;

		// Token: 0x04000454 RID: 1108
		[Range(0f, 1f)]
		public float percentThreshold;

		// Token: 0x04000455 RID: 1109
		public int burnDamageBelowThreshold;

		// Token: 0x04000456 RID: 1110
		private BurnSystem BurnSys;

		// Token: 0x04000457 RID: 1111
		private Ammo ammo;

		// Token: 0x04000458 RID: 1112
		public UnityEvent onActivate;

		// Token: 0x04000459 RID: 1113
		public UnityEvent onDeactivate;

		// Token: 0x0400045A RID: 1114
		private bool _active;
	}
}
