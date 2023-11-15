using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.PowerupSystem
{
	// Token: 0x0200024B RID: 587
	public class FireRateBuffOnGainAmmo : MonoBehaviour
	{
		// Token: 0x06000CCB RID: 3275 RVA: 0x0002E988 File Offset: 0x0002CB88
		private void OnAmmoGained()
		{
			if (this._buffCoroutine == null)
			{
				this._buffCoroutine = this.BuffCR();
				base.StartCoroutine(this._buffCoroutine);
				return;
			}
			this._timer = 0f;
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x0002E9B8 File Offset: 0x0002CBB8
		private void Start()
		{
			PlayerController componentInParent = base.GetComponentInParent<PlayerController>();
			this.playerStats = componentInParent.stats;
			this.ammo = componentInParent.ammo;
			this.ammo.OnAmmoGained.AddListener(new UnityAction(this.OnAmmoGained));
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x0002EA00 File Offset: 0x0002CC00
		private void OnDestroy()
		{
			this.ammo.OnAmmoGained.RemoveListener(new UnityAction(this.OnAmmoGained));
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x0002EA1E File Offset: 0x0002CC1E
		private IEnumerator BuffCR()
		{
			this.playerStats[StatType.FireRate].AddMultiplierBonus(this.buffAmount);
			while (this._timer < this.duration)
			{
				yield return null;
				this._timer += Time.deltaTime;
			}
			this.playerStats[StatType.FireRate].AddMultiplierBonus(-1f * this.buffAmount);
			this._buffCoroutine = null;
			yield break;
		}

		// Token: 0x04000904 RID: 2308
		[SerializeField]
		private float duration;

		// Token: 0x04000905 RID: 2309
		[SerializeField]
		private float buffAmount;

		// Token: 0x04000906 RID: 2310
		private StatsHolder playerStats;

		// Token: 0x04000907 RID: 2311
		private Ammo ammo;

		// Token: 0x04000908 RID: 2312
		private IEnumerator _buffCoroutine;

		// Token: 0x04000909 RID: 2313
		private float _timer;
	}
}
