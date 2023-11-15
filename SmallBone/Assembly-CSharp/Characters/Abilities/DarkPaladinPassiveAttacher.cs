using System;
using Characters.Abilities.Customs;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009E1 RID: 2529
	public class DarkPaladinPassiveAttacher : AbilityAttacher
	{
		// Token: 0x060035C2 RID: 13762 RVA: 0x0009FAB2 File Offset: 0x0009DCB2
		public override void OnIntialize()
		{
			this._passive.owner = base.owner;
			this._passive.Initialize();
		}

		// Token: 0x060035C3 RID: 13763 RVA: 0x0009FAD0 File Offset: 0x0009DCD0
		public override void StartAttach()
		{
			this._gauge.onChanged += this.OnGaugeChanged;
		}

		// Token: 0x060035C4 RID: 13764 RVA: 0x0009FAE9 File Offset: 0x0009DCE9
		public override void StopAttach()
		{
			this._gauge.onChanged -= this.OnGaugeChanged;
			if (base.owner == null)
			{
				return;
			}
			base.owner.ability.Remove(this._passive);
		}

		// Token: 0x060035C5 RID: 13765 RVA: 0x0009FB28 File Offset: 0x0009DD28
		private void OnGaugeChanged(float oldValue, float newValue)
		{
			if (newValue == this._gauge.maxValue)
			{
				this._gauge.Clear();
				base.owner.ability.Add(this._passive);
			}
		}

		// Token: 0x060035C6 RID: 13766 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04002B3F RID: 11071
		[SerializeField]
		private DarkPaladinPassive _passive;

		// Token: 0x04002B40 RID: 11072
		[SerializeField]
		private ValueGauge _gauge;
	}
}
