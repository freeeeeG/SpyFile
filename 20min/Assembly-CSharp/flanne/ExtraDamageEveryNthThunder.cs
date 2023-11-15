using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000064 RID: 100
	public class ExtraDamageEveryNthThunder : MonoBehaviour
	{
		// Token: 0x06000472 RID: 1138 RVA: 0x00016E19 File Offset: 0x00015019
		private void Start()
		{
			this.TG = ThunderGenerator.SharedInstance;
			this.ammo = base.transform.parent.GetComponentInChildren<Ammo>();
			this.AddObserver(new Action<object, object>(this.OnThunderHit), ThunderGenerator.ThunderHitEvent);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00016E53 File Offset: 0x00015053
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnThunderHit), ThunderGenerator.ThunderHitEvent);
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00016E6C File Offset: 0x0001506C
		private void OnThunderHit(object sender, object args)
		{
			this._counter++;
			if (this._counter == this.activationThreshold - 1)
			{
				this.TG.damageMod.AddMultiplierBonus(this.damageBonus);
				return;
			}
			if (this._counter >= this.activationThreshold)
			{
				this._counter = 0;
				this.TG.damageMod.AddMultiplierBonus(-1f * this.damageBonus);
				this.ammo.GainAmmo(1);
			}
		}

		// Token: 0x0400026B RID: 619
		[SerializeField]
		private float damageBonus;

		// Token: 0x0400026C RID: 620
		[SerializeField]
		private int activationThreshold;

		// Token: 0x0400026D RID: 621
		private int _counter;

		// Token: 0x0400026E RID: 622
		private ThunderGenerator TG;

		// Token: 0x0400026F RID: 623
		private Ammo ammo;
	}
}
