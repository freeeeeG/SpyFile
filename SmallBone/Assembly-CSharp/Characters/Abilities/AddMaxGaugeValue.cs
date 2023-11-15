using System;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009A8 RID: 2472
	[Serializable]
	public class AddMaxGaugeValue : Ability
	{
		// Token: 0x060034F8 RID: 13560 RVA: 0x0009CC64 File Offset: 0x0009AE64
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new AddMaxGaugeValue.Instance(owner, this);
		}

		// Token: 0x04002A9C RID: 10908
		[SerializeField]
		private ValueGauge _gauge;

		// Token: 0x04002A9D RID: 10909
		[SerializeField]
		private int _amount = 1;

		// Token: 0x020009A9 RID: 2473
		public class Instance : AbilityInstance<AddMaxGaugeValue>
		{
			// Token: 0x060034FA RID: 13562 RVA: 0x0009CC7C File Offset: 0x0009AE7C
			internal Instance(Character owner, AddMaxGaugeValue ability) : base(owner, ability)
			{
			}

			// Token: 0x060034FB RID: 13563 RVA: 0x0009CC86 File Offset: 0x0009AE86
			protected override void OnAttach()
			{
				this.ability._gauge.maxValue += (float)this.ability._amount;
			}

			// Token: 0x060034FC RID: 13564 RVA: 0x0009CCAB File Offset: 0x0009AEAB
			protected override void OnDetach()
			{
				this.ability._gauge.maxValue -= (float)this.ability._amount;
			}
		}
	}
}
