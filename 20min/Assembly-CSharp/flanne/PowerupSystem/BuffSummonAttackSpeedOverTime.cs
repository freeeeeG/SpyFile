using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne.PowerupSystem
{
	// Token: 0x02000247 RID: 583
	public class BuffSummonAttackSpeedOverTime : MonoBehaviour
	{
		// Token: 0x06000CBC RID: 3260 RVA: 0x0002E5A8 File Offset: 0x0002C7A8
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

		// Token: 0x06000CBD RID: 3261 RVA: 0x0002E600 File Offset: 0x0002C800
		private void Update()
		{
			if (this._timer > this.secondsPerBuff)
			{
				foreach (ShootingSummon shootingSummon in this._summons)
				{
					if (shootingSummon != null)
					{
						shootingSummon.attackSpeedMod.AddMultiplierBonus(this.attackSpeedBuff);
					}
				}
				this._timer -= this.secondsPerBuff;
			}
			this._timer += Time.deltaTime;
		}

		// Token: 0x040008EE RID: 2286
		[SerializeField]
		private string SummonTypeID;

		// Token: 0x040008EF RID: 2287
		[Range(0f, 1f)]
		[SerializeField]
		private float attackSpeedBuff;

		// Token: 0x040008F0 RID: 2288
		[SerializeField]
		private float secondsPerBuff;

		// Token: 0x040008F1 RID: 2289
		private List<ShootingSummon> _summons;

		// Token: 0x040008F2 RID: 2290
		private float _timer;
	}
}
