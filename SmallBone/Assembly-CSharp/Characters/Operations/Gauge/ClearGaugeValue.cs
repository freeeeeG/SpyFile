using System;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Operations.Gauge
{
	// Token: 0x02000E95 RID: 3733
	public class ClearGaugeValue : Operation
	{
		// Token: 0x060049CA RID: 18890 RVA: 0x000D78AA File Offset: 0x000D5AAA
		public override void Run()
		{
			this._gaugeWithValue.Clear();
		}

		// Token: 0x04003908 RID: 14600
		[SerializeField]
		private ValueGauge _gaugeWithValue;
	}
}
