using System;

namespace Characters.Abilities
{
	// Token: 0x02000A3E RID: 2622
	[Serializable]
	public class GetSilence : Ability
	{
		// Token: 0x0600371F RID: 14111 RVA: 0x000A2CC1 File Offset: 0x000A0EC1
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new GetSilence.Instance(owner, this);
		}

		// Token: 0x02000A3F RID: 2623
		public class Instance : AbilityInstance<GetSilence>
		{
			// Token: 0x06003721 RID: 14113 RVA: 0x000A2CCA File Offset: 0x000A0ECA
			public Instance(Character owner, GetSilence ability) : base(owner, ability)
			{
			}

			// Token: 0x06003722 RID: 14114 RVA: 0x000A2CD4 File Offset: 0x000A0ED4
			protected override void OnAttach()
			{
				this.owner.silence.Attach(this);
			}

			// Token: 0x06003723 RID: 14115 RVA: 0x000A2CE7 File Offset: 0x000A0EE7
			protected override void OnDetach()
			{
				this.owner.silence.Detach(this);
			}
		}
	}
}
