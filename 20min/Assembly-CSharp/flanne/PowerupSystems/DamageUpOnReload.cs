using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.PowerupSystems
{
	// Token: 0x020001DC RID: 476
	public class DamageUpOnReload : MonoBehaviour
	{
		// Token: 0x06000A72 RID: 2674 RVA: 0x00028BB0 File Offset: 0x00026DB0
		private void OnReload()
		{
			if (this._timer <= 0f)
			{
				this.stats[StatType.BulletDamage].AddMultiplierBonus(this.damageBonus);
			}
			this._timer = this.duration;
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x00028BE4 File Offset: 0x00026DE4
		private void Start()
		{
			PlayerController componentInParent = base.transform.GetComponentInParent<PlayerController>();
			this.stats = componentInParent.stats;
			this.ammo = componentInParent.ammo;
			this.ammo.OnReload.AddListener(new UnityAction(this.OnReload));
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x00028C31 File Offset: 0x00026E31
		private void OnDestroy()
		{
			this.ammo.OnReload.RemoveListener(new UnityAction(this.OnReload));
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x00028C50 File Offset: 0x00026E50
		private void Update()
		{
			if (this._timer > 0f)
			{
				this._timer -= Time.deltaTime;
				if (this._timer <= 0f)
				{
					this.stats[StatType.BulletDamage].AddMultiplierBonus(-1f * this.damageBonus);
				}
			}
		}

		// Token: 0x0400077C RID: 1916
		[SerializeField]
		private float damageBonus;

		// Token: 0x0400077D RID: 1917
		[SerializeField]
		private float duration;

		// Token: 0x0400077E RID: 1918
		private StatsHolder stats;

		// Token: 0x0400077F RID: 1919
		private Ammo ammo;

		// Token: 0x04000780 RID: 1920
		private float _timer;
	}
}
