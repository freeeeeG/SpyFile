using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001AC RID: 428
	public class DealDamageAction : Action
	{
		// Token: 0x060009F8 RID: 2552 RVA: 0x0002776A File Offset: 0x0002596A
		public override void Activate(GameObject target)
		{
			target.GetComponent<Health>().TakeDamage(this.damageType, this.baseDamage, this.damageMulti);
		}

		// Token: 0x0400070D RID: 1805
		[SerializeField]
		private DamageType damageType;

		// Token: 0x0400070E RID: 1806
		[SerializeField]
		private int baseDamage;

		// Token: 0x0400070F RID: 1807
		[SerializeField]
		private float damageMulti = 1f;
	}
}
