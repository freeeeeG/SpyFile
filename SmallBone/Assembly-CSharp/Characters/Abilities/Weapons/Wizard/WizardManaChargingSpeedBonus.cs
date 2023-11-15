using System;
using UnityEngine;

namespace Characters.Abilities.Weapons.Wizard
{
	// Token: 0x02000BFF RID: 3071
	[Serializable]
	public sealed class WizardManaChargingSpeedBonus : Ability
	{
		// Token: 0x06003F07 RID: 16135 RVA: 0x000B728C File Offset: 0x000B548C
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new WizardManaChargingSpeedBonus.Instance(owner, this);
		}

		// Token: 0x0400309E RID: 12446
		[SerializeField]
		private WizardPassiveComponent _passive;

		// Token: 0x0400309F RID: 12447
		[SerializeField]
		private float _multiplier;

		// Token: 0x02000C00 RID: 3072
		public class Instance : AbilityInstance<WizardManaChargingSpeedBonus>
		{
			// Token: 0x06003F09 RID: 16137 RVA: 0x000B7295 File Offset: 0x000B5495
			public Instance(Character owner, WizardManaChargingSpeedBonus ability) : base(owner, ability)
			{
			}

			// Token: 0x06003F0A RID: 16138 RVA: 0x000B729F File Offset: 0x000B549F
			protected override void OnAttach()
			{
				this.ability._passive.manaChargingSpeedMultiplier += this.ability._multiplier;
			}

			// Token: 0x06003F0B RID: 16139 RVA: 0x000B72C3 File Offset: 0x000B54C3
			protected override void OnDetach()
			{
				this.ability._passive.manaChargingSpeedMultiplier -= this.ability._multiplier;
			}
		}
	}
}
