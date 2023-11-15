using System;
using System.Collections;
using UnityEngine;

namespace flanne.Player.Buffs
{
	// Token: 0x0200017A RID: 378
	public class RegenerateAmmoBuff : Buff
	{
		// Token: 0x06000954 RID: 2388 RVA: 0x0002648B File Offset: 0x0002468B
		public override void OnAttach()
		{
			this.gun = PlayerController.Instance.gun;
			this.ammo = PlayerController.Instance.ammo;
			this._regenCoroutine = this.RegenAmmoCR();
			this.ammo.StartCoroutine(this._regenCoroutine);
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x000264CB File Offset: 0x000246CB
		public override void OnUnattach()
		{
			this.ammo.StopCoroutine(this._regenCoroutine);
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x000264DE File Offset: 0x000246DE
		private IEnumerator RegenAmmoCR()
		{
			float timer = 0f;
			for (;;)
			{
				yield return null;
				if (!this.gun.isShooting && this.ammo.amount != 0)
				{
					timer += Time.deltaTime;
				}
				if (timer > this.timePerRegen)
				{
					timer -= this.timePerRegen;
					this.ammo.GainAmmo(this.ammoPerRegen);
				}
			}
			yield break;
		}

		// Token: 0x040006C4 RID: 1732
		[SerializeField]
		private float timePerRegen;

		// Token: 0x040006C5 RID: 1733
		[SerializeField]
		private int ammoPerRegen;

		// Token: 0x040006C6 RID: 1734
		private Gun gun;

		// Token: 0x040006C7 RID: 1735
		private Ammo ammo;

		// Token: 0x040006C8 RID: 1736
		private IEnumerator _regenCoroutine;
	}
}
