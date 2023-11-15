using System;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Actions.Constraints
{
	// Token: 0x02000979 RID: 2425
	public class GaugeConstraint : Constraint
	{
		// Token: 0x0600342A RID: 13354 RVA: 0x0009A77C File Offset: 0x0009897C
		public override bool Pass()
		{
			return GaugeConstraint.Pass(this._gauge, this._compare, this._amount);
		}

		// Token: 0x0600342B RID: 13355 RVA: 0x0009A795 File Offset: 0x00098995
		public static bool Pass(ValueGauge gauge, GaugeConstraint.Compare compare, int amount)
		{
			return (compare == GaugeConstraint.Compare.GreaterThanOrEqual && gauge.currentValue >= (float)amount) || (compare == GaugeConstraint.Compare.LessThanOrEqual && gauge.currentValue <= (float)amount);
		}

		// Token: 0x04002A38 RID: 10808
		[SerializeField]
		private ValueGauge _gauge;

		// Token: 0x04002A39 RID: 10809
		[SerializeField]
		private GaugeConstraint.Compare _compare;

		// Token: 0x04002A3A RID: 10810
		[SerializeField]
		private int _amount;

		// Token: 0x0200097A RID: 2426
		public enum Compare
		{
			// Token: 0x04002A3C RID: 10812
			GreaterThanOrEqual,
			// Token: 0x04002A3D RID: 10813
			LessThanOrEqual
		}
	}
}
