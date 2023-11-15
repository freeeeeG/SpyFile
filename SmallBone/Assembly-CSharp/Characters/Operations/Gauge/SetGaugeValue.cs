using System;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Operations.Gauge
{
	// Token: 0x02000E96 RID: 3734
	public class SetGaugeValue : Operation
	{
		// Token: 0x060049CC RID: 18892 RVA: 0x000D78B7 File Offset: 0x000D5AB7
		public override void Run()
		{
			this._gaugeWithValue.Set(this._value);
		}

		// Token: 0x04003909 RID: 14601
		[SerializeField]
		private ValueGauge _gaugeWithValue;

		// Token: 0x0400390A RID: 14602
		[SerializeField]
		private float _value;
	}
}
