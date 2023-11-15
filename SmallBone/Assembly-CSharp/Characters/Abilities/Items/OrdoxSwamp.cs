using System;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CF3 RID: 3315
	[Serializable]
	public sealed class OrdoxSwamp : Ability
	{
		// Token: 0x06004301 RID: 17153 RVA: 0x000C36DA File Offset: 0x000C18DA
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new OrdoxSwamp.Instance(owner, this);
		}

		// Token: 0x02000CF4 RID: 3316
		public class Instance : AbilityInstance<OrdoxSwamp>
		{
			// Token: 0x06004303 RID: 17155 RVA: 0x000C36E3 File Offset: 0x000C18E3
			public Instance(Character owner, OrdoxSwamp ability) : base(owner, ability)
			{
			}

			// Token: 0x06004304 RID: 17156 RVA: 0x000C36ED File Offset: 0x000C18ED
			protected override void OnAttach()
			{
				this.owner.status.onApplyPoison += this.Status_onApplyPoison;
			}

			// Token: 0x06004305 RID: 17157 RVA: 0x000C370B File Offset: 0x000C190B
			private void Status_onApplyPoison(Character attacker, Character target)
			{
				bool poisoned = target.status.poisoned;
			}

			// Token: 0x06004306 RID: 17158 RVA: 0x000C3719 File Offset: 0x000C1919
			protected override void OnDetach()
			{
				this.owner.status.onApplyPoison -= this.Status_onApplyPoison;
			}
		}
	}
}
