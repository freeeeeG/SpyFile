using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000125 RID: 293
	public class SeekerWeapon : WeaponSummon
	{
		// Token: 0x060007FE RID: 2046 RVA: 0x00021ED6 File Offset: 0x000200D6
		protected override void Init()
		{
			base.summonAtkSpdMod.ChangedEvent += this.OnSummonAtkSpdChange;
			this.seeker.acceleration = base.summonAtkSpdMod.Modify(this.baseAcceleration);
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00021F0B File Offset: 0x0002010B
		private void OnDestroy()
		{
			base.summonAtkSpdMod.ChangedEvent -= this.OnSummonAtkSpdChange;
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00021F24 File Offset: 0x00020124
		private void OnSummonAtkSpdChange(object sender, EventArgs e)
		{
			if (this.seeker != null)
			{
				this.seeker.acceleration = base.summonAtkSpdMod.Modify(this.baseAcceleration);
			}
		}

		// Token: 0x040005D1 RID: 1489
		[SerializeField]
		private SeekEnemy seeker;

		// Token: 0x040005D2 RID: 1490
		[SerializeField]
		private float baseAcceleration;
	}
}
