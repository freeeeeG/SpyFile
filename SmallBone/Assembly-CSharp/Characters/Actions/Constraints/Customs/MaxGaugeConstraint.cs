using System;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Actions.Constraints.Customs
{
	// Token: 0x0200098E RID: 2446
	public sealed class MaxGaugeConstraint : Constraint
	{
		// Token: 0x0600345B RID: 13403 RVA: 0x0009AD90 File Offset: 0x00098F90
		public override bool Pass()
		{
			return this._gauge.maxValue <= this._gauge.currentValue;
		}

		// Token: 0x04002A59 RID: 10841
		[SerializeField]
		private ValueGauge _gauge;
	}
}
