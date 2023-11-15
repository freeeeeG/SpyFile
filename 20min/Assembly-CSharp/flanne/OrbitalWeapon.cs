using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000123 RID: 291
	public class OrbitalWeapon : WeaponSummon
	{
		// Token: 0x060007F5 RID: 2037 RVA: 0x00021D8D File Offset: 0x0001FF8D
		protected override void Init()
		{
			base.summonAtkSpdMod.ChangedEvent += this.OnSummonAtkSpdChange;
			this.orbital.rotationSpeed = base.summonAtkSpdMod.Modify(this.baseRotationSpeed);
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00021DC2 File Offset: 0x0001FFC2
		private void OnDestroy()
		{
			base.summonAtkSpdMod.ChangedEvent -= this.OnSummonAtkSpdChange;
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00021DDB File Offset: 0x0001FFDB
		private void OnSummonAtkSpdChange(object sender, EventArgs e)
		{
			if (this.orbital != null)
			{
				this.orbital.rotationSpeed = base.summonAtkSpdMod.Modify(this.baseRotationSpeed);
			}
		}

		// Token: 0x040005CE RID: 1486
		[SerializeField]
		private Orbital orbital;

		// Token: 0x040005CF RID: 1487
		[SerializeField]
		private float baseRotationSpeed;
	}
}
