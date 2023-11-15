using System;
using UnityEngine;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000ADA RID: 2778
	[Serializable]
	public sealed class HealthArmor : Ability
	{
		// Token: 0x060038EE RID: 14574 RVA: 0x000A7BCC File Offset: 0x000A5DCC
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new HealthArmor.Instance(owner, this);
		}

		// Token: 0x04002D39 RID: 11577
		[SerializeField]
		private float _reduceHealthPercent;

		// Token: 0x04002D3A RID: 11578
		[SerializeField]
		private float _shieldRatio;

		// Token: 0x02000ADB RID: 2779
		public sealed class Instance : AbilityInstance<HealthArmor>
		{
			// Token: 0x060038F0 RID: 14576 RVA: 0x000A7BD5 File Offset: 0x000A5DD5
			public Instance(Character owner, HealthArmor ability) : base(owner, ability)
			{
			}

			// Token: 0x060038F1 RID: 14577 RVA: 0x000A7BE0 File Offset: 0x000A5DE0
			protected override void OnAttach()
			{
				float plus = (float)this.owner.health.percent * this.ability._reduceHealthPercent * this.ability._shieldRatio;
				this.owner.playerComponents.savableAbilityManager.IncreaseStack(SavableAbilityManager.Name.PurchasedMaxHealth, -this.ability._reduceHealthPercent);
				this.owner.playerComponents.savableAbilityManager.IncreaseStack(SavableAbilityManager.Name.PurchasedShield, plus);
			}

			// Token: 0x060038F2 RID: 14578 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnDetach()
			{
			}
		}
	}
}
