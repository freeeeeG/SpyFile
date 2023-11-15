using System;

namespace Characters.Abilities
{
	// Token: 0x02000A2F RID: 2607
	[Serializable]
	public sealed class GetEvasion : Ability
	{
		// Token: 0x060036FF RID: 14079 RVA: 0x000A2A7D File Offset: 0x000A0C7D
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new GetEvasion.Instance(owner, this);
		}

		// Token: 0x02000A30 RID: 2608
		public sealed class Instance : AbilityInstance<GetEvasion>
		{
			// Token: 0x06003701 RID: 14081 RVA: 0x000A2A86 File Offset: 0x000A0C86
			public Instance(Character owner, GetEvasion ability) : base(owner, ability)
			{
			}

			// Token: 0x06003702 RID: 14082 RVA: 0x00089D62 File Offset: 0x00087F62
			protected override void OnAttach()
			{
				this.owner.evasion.Attach(this);
			}

			// Token: 0x06003703 RID: 14083 RVA: 0x00089D75 File Offset: 0x00087F75
			protected override void OnDetach()
			{
				this.owner.evasion.Detach(this);
			}
		}
	}
}
