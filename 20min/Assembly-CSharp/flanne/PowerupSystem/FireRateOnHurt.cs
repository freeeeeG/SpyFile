using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.PowerupSystem
{
	// Token: 0x0200024C RID: 588
	public class FireRateOnHurt : MonoBehaviour
	{
		// Token: 0x06000CD0 RID: 3280 RVA: 0x0002EA30 File Offset: 0x0002CC30
		private void Start()
		{
			PlayerController componentInParent = base.GetComponentInParent<PlayerController>();
			this.stats = componentInParent.stats;
			this.health = componentInParent.playerHealth;
			this.health.onHurt.AddListener(new UnityAction(this.OnHurt));
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x0002EA78 File Offset: 0x0002CC78
		private void OnDestroy()
		{
			this.health.onHurt.RemoveListener(new UnityAction(this.OnHurt));
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x0002EA96 File Offset: 0x0002CC96
		private void OnHurt()
		{
			if (this.statBoostCoroutine != null)
			{
				base.StopCoroutine(this.statBoostCoroutine);
				this.statBoostCoroutine = null;
				this.RemoveBoost();
			}
			this.statBoostCoroutine = this.StatBoostCR();
			base.StartCoroutine(this.statBoostCoroutine);
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x0002EAD2 File Offset: 0x0002CCD2
		private IEnumerator StatBoostCR()
		{
			this.AddBoost();
			yield return new WaitForSeconds(this.duration);
			this.RemoveBoost();
			this.statBoostCoroutine = null;
			yield break;
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x0002EAE1 File Offset: 0x0002CCE1
		private void AddBoost()
		{
			this.stats[StatType.FireRate].AddMultiplierBonus(this.fireRateBoost);
			this.stats[StatType.ReloadRate].AddMultiplierBonus(this.fireRateBoost);
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0002EB11 File Offset: 0x0002CD11
		private void RemoveBoost()
		{
			this.stats[StatType.FireRate].AddMultiplierBonus(-1f * this.fireRateBoost);
			this.stats[StatType.ReloadRate].AddMultiplierBonus(-1f * this.fireRateBoost);
		}

		// Token: 0x0400090A RID: 2314
		[SerializeField]
		private float duration;

		// Token: 0x0400090B RID: 2315
		[SerializeField]
		private float fireRateBoost;

		// Token: 0x0400090C RID: 2316
		[SerializeField]
		private float reloadRateBoost;

		// Token: 0x0400090D RID: 2317
		private StatsHolder stats;

		// Token: 0x0400090E RID: 2318
		private PlayerHealth health;

		// Token: 0x0400090F RID: 2319
		private IEnumerator statBoostCoroutine;
	}
}
