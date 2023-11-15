using System;
using UnityEngine;

namespace Characters.Abilities.Weapons.Wizard
{
	// Token: 0x02000C05 RID: 3077
	[Serializable]
	public sealed class WizardTranscendence : Ability
	{
		// Token: 0x06003F20 RID: 16160 RVA: 0x000B757C File Offset: 0x000B577C
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new WizardTranscendence.Instance(owner, this);
		}

		// Token: 0x040030AB RID: 12459
		[SerializeField]
		private WizardPassiveComponent _passive;

		// Token: 0x02000C06 RID: 3078
		public class Instance : AbilityInstance<WizardTranscendence>
		{
			// Token: 0x06003F22 RID: 16162 RVA: 0x000B7585 File Offset: 0x000B5785
			public Instance(Character owner, WizardTranscendence ability) : base(owner, ability)
			{
			}

			// Token: 0x06003F23 RID: 16163 RVA: 0x000B758F File Offset: 0x000B578F
			protected override void OnAttach()
			{
				this.ability._passive.transcendence = true;
			}

			// Token: 0x06003F24 RID: 16164 RVA: 0x000B75A2 File Offset: 0x000B57A2
			protected override void OnDetach()
			{
				this.ability._passive.transcendence = false;
			}
		}
	}
}
