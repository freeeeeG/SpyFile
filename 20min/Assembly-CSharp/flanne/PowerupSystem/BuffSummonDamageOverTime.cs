using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne.PowerupSystem
{
	// Token: 0x02000248 RID: 584
	public class BuffSummonDamageOverTime : MonoBehaviour
	{
		// Token: 0x06000CBF RID: 3263 RVA: 0x0002E69C File Offset: 0x0002C89C
		private void Start()
		{
			ShootingSummon[] componentsInChildren = base.GetComponentInParent<PlayerController>().GetComponentsInChildren<ShootingSummon>(true);
			this._summons = new List<ShootingSummon>();
			foreach (ShootingSummon shootingSummon in componentsInChildren)
			{
				if (shootingSummon.SummonTypeID == this.SummonTypeID)
				{
					this._summons.Add(shootingSummon);
				}
			}
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0002E6F4 File Offset: 0x0002C8F4
		private void Update()
		{
			if (this._timer > this.secondsPerBuff)
			{
				foreach (ShootingSummon shootingSummon in this._summons)
				{
					if (shootingSummon != null)
					{
						shootingSummon.baseDamage += this.damageBuff;
					}
				}
				this._timer -= this.secondsPerBuff;
			}
			this._timer += Time.deltaTime;
		}

		// Token: 0x040008F3 RID: 2291
		[SerializeField]
		private string SummonTypeID;

		// Token: 0x040008F4 RID: 2292
		[SerializeField]
		private int damageBuff;

		// Token: 0x040008F5 RID: 2293
		[SerializeField]
		private float secondsPerBuff;

		// Token: 0x040008F6 RID: 2294
		private List<ShootingSummon> _summons;

		// Token: 0x040008F7 RID: 2295
		private float _timer;
	}
}
