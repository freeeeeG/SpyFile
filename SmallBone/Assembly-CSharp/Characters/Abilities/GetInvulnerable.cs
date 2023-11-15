using System;

namespace Characters.Abilities
{
	// Token: 0x02000A38 RID: 2616
	[Serializable]
	public class GetInvulnerable : Ability
	{
		// Token: 0x06003713 RID: 14099 RVA: 0x000A2C33 File Offset: 0x000A0E33
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new GetInvulnerable.Instance(owner, this);
		}

		// Token: 0x02000A39 RID: 2617
		public class Instance : AbilityInstance<GetInvulnerable>
		{
			// Token: 0x06003715 RID: 14101 RVA: 0x000A2C3C File Offset: 0x000A0E3C
			public Instance(Character owner, GetInvulnerable ability) : base(owner, ability)
			{
			}

			// Token: 0x06003716 RID: 14102 RVA: 0x000A2C46 File Offset: 0x000A0E46
			protected override void OnAttach()
			{
				this.owner.invulnerable.Attach(this);
			}

			// Token: 0x06003717 RID: 14103 RVA: 0x000A2C59 File Offset: 0x000A0E59
			protected override void OnDetach()
			{
				this.owner.invulnerable.Detach(this);
			}
		}
	}
}
