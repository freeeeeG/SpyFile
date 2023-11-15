using System;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Operations.Gauge
{
	// Token: 0x02000E94 RID: 3732
	public class AddGaugeValue : Operation
	{
		// Token: 0x060049C8 RID: 18888 RVA: 0x000D7875 File Offset: 0x000D5A75
		public override void Run()
		{
			if (this._gaugeWithValue == null)
			{
				return;
			}
			this._gaugeWithValue.Add(this._amount);
		}

		// Token: 0x04003906 RID: 14598
		[SerializeField]
		private ValueGauge _gaugeWithValue;

		// Token: 0x04003907 RID: 14599
		[SerializeField]
		private float _amount = 1f;
	}
}
