using System;
using flanne.Pickups;
using UnityEngine;

namespace flanne.PowerupSystem
{
	// Token: 0x02000243 RID: 579
	public class AmmoOnXP : MonoBehaviour
	{
		// Token: 0x06000CA9 RID: 3241 RVA: 0x0002E31B File Offset: 0x0002C51B
		private void Start()
		{
			this.ammo = base.transform.parent.GetComponentInChildren<Ammo>();
			base.transform.localPosition = Vector3.zero;
			this.AddObserver(new Action<object, object>(this.OnXPPickup), XPPickup.XPPickupEvent);
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x0002E35A File Offset: 0x0002C55A
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnXPPickup), XPPickup.XPPickupEvent);
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x0002E374 File Offset: 0x0002C574
		private void OnXPPickup(object sender, object args)
		{
			if (Random.Range(0f, 1f) < this.chanceToActivate)
			{
				if (this.ammo != null)
				{
					this.ammo.GainAmmo(this.ammoRefillAmount);
					this.sfx.Play(null);
					return;
				}
				Debug.LogWarning("No ammo component found");
			}
		}

		// Token: 0x040008E0 RID: 2272
		[SerializeField]
		private SoundEffectSO sfx;

		// Token: 0x040008E1 RID: 2273
		[Range(0f, 1f)]
		[SerializeField]
		private float chanceToActivate;

		// Token: 0x040008E2 RID: 2274
		[SerializeField]
		private int ammoRefillAmount;

		// Token: 0x040008E3 RID: 2275
		private Ammo ammo;
	}
}
