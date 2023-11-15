using System;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B39 RID: 2873
	[Serializable]
	public class OnGaugeEmpty : Trigger
	{
		// Token: 0x060039F8 RID: 14840 RVA: 0x000AB185 File Offset: 0x000A9385
		public override void Attach(Character character)
		{
			this._gauge.onChanged += this.OnGaugeValueChanged;
		}

		// Token: 0x060039F9 RID: 14841 RVA: 0x000AB19E File Offset: 0x000A939E
		public override void Detach()
		{
			this._gauge.onChanged -= this.OnGaugeValueChanged;
		}

		// Token: 0x060039FA RID: 14842 RVA: 0x000AB1B7 File Offset: 0x000A93B7
		private void OnGaugeValueChanged(float oldValue, float newValue)
		{
			if (newValue == 0f)
			{
				base.Invoke();
			}
		}

		// Token: 0x04002DF7 RID: 11767
		[SerializeField]
		private ValueGauge _gauge;
	}
}
