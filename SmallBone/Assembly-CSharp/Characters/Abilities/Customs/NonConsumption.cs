using System;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D74 RID: 3444
	[Serializable]
	public class NonConsumption : Ability
	{
		// Token: 0x06004571 RID: 17777 RVA: 0x000C9574 File Offset: 0x000C7774
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new NonConsumption.Instance(owner, this);
		}

		// Token: 0x040034CA RID: 13514
		[SerializeField]
		private Magazine _magazine;

		// Token: 0x02000D75 RID: 3445
		public class Instance : AbilityInstance<NonConsumption>
		{
			// Token: 0x06004572 RID: 17778 RVA: 0x000C957D File Offset: 0x000C777D
			public Instance(Character owner, NonConsumption ability) : base(owner, ability)
			{
			}

			// Token: 0x06004573 RID: 17779 RVA: 0x000C9587 File Offset: 0x000C7787
			protected override void OnAttach()
			{
				this.ability._magazine.nonConsumptionState = true;
			}

			// Token: 0x06004574 RID: 17780 RVA: 0x000C959A File Offset: 0x000C779A
			protected override void OnDetach()
			{
				this.ability._magazine.nonConsumptionState = false;
			}
		}
	}
}
