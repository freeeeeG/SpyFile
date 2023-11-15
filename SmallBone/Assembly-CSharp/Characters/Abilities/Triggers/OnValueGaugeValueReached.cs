using System;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B64 RID: 2916
	[Serializable]
	public class OnValueGaugeValueReached : Trigger
	{
		// Token: 0x06003A5E RID: 14942 RVA: 0x000AC9B6 File Offset: 0x000AABB6
		public override void Attach(Character character)
		{
			this._gauge.onChanged += this.OnGaugeValueChanged;
		}

		// Token: 0x06003A5F RID: 14943 RVA: 0x000AC9CF File Offset: 0x000AABCF
		public override void Detach()
		{
			this._gauge.onChanged -= this.OnGaugeValueChanged;
		}

		// Token: 0x06003A60 RID: 14944 RVA: 0x000AC9E8 File Offset: 0x000AABE8
		private void OnGaugeValueChanged(float oldValue, float newValue)
		{
			if (oldValue < (float)this._targetValue && newValue < (float)this._targetValue)
			{
				return;
			}
			if (this._clearGauge)
			{
				this._gauge.Clear();
			}
			base.Invoke();
		}

		// Token: 0x04002E6B RID: 11883
		[SerializeField]
		private ValueGauge _gauge;

		// Token: 0x04002E6C RID: 11884
		[SerializeField]
		private int _targetValue;

		// Token: 0x04002E6D RID: 11885
		[SerializeField]
		private bool _clearGauge;

		// Token: 0x04002E6E RID: 11886
		private Character _character;
	}
}
