using System;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Actions.Constraints.Customs
{
	// Token: 0x02000988 RID: 2440
	public class BulletConstraint : Constraint
	{
		// Token: 0x0600344E RID: 13390 RVA: 0x0009ACCA File Offset: 0x00098ECA
		public override bool Pass()
		{
			return BulletConstraint.Pass(this._magazine, this._amount);
		}

		// Token: 0x0600344F RID: 13391 RVA: 0x0009ACDD File Offset: 0x00098EDD
		public static bool Pass(Magazine magazine, int amount)
		{
			return magazine.Has(amount);
		}

		// Token: 0x04002A51 RID: 10833
		[SerializeField]
		private Magazine _magazine;

		// Token: 0x04002A52 RID: 10834
		[SerializeField]
		private int _amount;
	}
}
