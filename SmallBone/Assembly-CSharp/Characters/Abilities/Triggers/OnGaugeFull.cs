using System;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B3B RID: 2875
	[Serializable]
	public class OnGaugeFull : Trigger
	{
		// Token: 0x060039FD RID: 14845 RVA: 0x000AB1CF File Offset: 0x000A93CF
		public override void Attach(Character character)
		{
			this._gauge.onChanged += this.OnGaugeValueChanged;
		}

		// Token: 0x060039FE RID: 14846 RVA: 0x000AB1E8 File Offset: 0x000A93E8
		public override void Detach()
		{
			this._gauge.onChanged -= this.OnGaugeValueChanged;
		}

		// Token: 0x060039FF RID: 14847 RVA: 0x000AB201 File Offset: 0x000A9401
		private void OnGaugeValueChanged(float oldValue, float newValue)
		{
			if (newValue < this._gauge.maxValue)
			{
				return;
			}
			if (this._clearGauge)
			{
				this._gauge.Clear();
			}
			base.Invoke();
		}

		// Token: 0x04002DF8 RID: 11768
		[SerializeField]
		private ValueGauge _gauge;

		// Token: 0x04002DF9 RID: 11769
		[SerializeField]
		private bool _clearGauge;

		// Token: 0x04002DFA RID: 11770
		private Character _character;
	}
}
