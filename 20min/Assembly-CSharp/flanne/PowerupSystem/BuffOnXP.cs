using System;
using System.Collections;
using flanne.Pickups;
using UnityEngine;

namespace flanne.PowerupSystem
{
	// Token: 0x02000246 RID: 582
	public class BuffOnXP : MonoBehaviour
	{
		// Token: 0x06000CB7 RID: 3255 RVA: 0x0002E51D File Offset: 0x0002C71D
		private void OnXPPickup(object sender, object args)
		{
			if (this._timer <= 0f)
			{
				base.StartCoroutine(this.StartBuffCR());
				return;
			}
			this._timer = this.duration;
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x0002E548 File Offset: 0x0002C748
		private void Start()
		{
			PlayerController componentInParent = base.GetComponentInParent<PlayerController>();
			this.stats = componentInParent.stats;
			this.AddObserver(new Action<object, object>(this.OnXPPickup), XPPickup.XPPickupEvent);
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x0002E57F File Offset: 0x0002C77F
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnXPPickup), XPPickup.XPPickupEvent);
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x0002E598 File Offset: 0x0002C798
		private IEnumerator StartBuffCR()
		{
			this._timer = this.duration;
			this.stats[StatType.FireRate].AddMultiplierBonus(this.fireRateBoost);
			while (this._timer > 0f)
			{
				yield return null;
				this._timer -= Time.deltaTime;
			}
			this.stats[StatType.FireRate].AddMultiplierBonus(-1f * this.fireRateBoost);
			this._timer = 0f;
			yield break;
		}

		// Token: 0x040008EA RID: 2282
		[SerializeField]
		private float fireRateBoost;

		// Token: 0x040008EB RID: 2283
		[SerializeField]
		private float duration;

		// Token: 0x040008EC RID: 2284
		private StatsHolder stats;

		// Token: 0x040008ED RID: 2285
		private float _timer;
	}
}
