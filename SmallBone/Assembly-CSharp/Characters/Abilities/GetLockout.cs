using System;

namespace Characters.Abilities
{
	// Token: 0x02000A3B RID: 2619
	[Serializable]
	public class GetLockout : Ability
	{
		// Token: 0x06003719 RID: 14105 RVA: 0x000A2C75 File Offset: 0x000A0E75
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new GetLockout.Instance(owner, this);
		}

		// Token: 0x02000A3C RID: 2620
		public class Instance : AbilityInstance<GetLockout>
		{
			// Token: 0x0600371B RID: 14107 RVA: 0x000A2C7E File Offset: 0x000A0E7E
			public Instance(Character owner, GetLockout ability) : base(owner, ability)
			{
			}

			// Token: 0x0600371C RID: 14108 RVA: 0x000A2C88 File Offset: 0x000A0E88
			protected override void OnAttach()
			{
				this.owner.status.unstoppable.Attach(this);
			}

			// Token: 0x0600371D RID: 14109 RVA: 0x000A2CA0 File Offset: 0x000A0EA0
			protected override void OnDetach()
			{
				this.owner.status.unstoppable.Detach(this);
			}
		}
	}
}
