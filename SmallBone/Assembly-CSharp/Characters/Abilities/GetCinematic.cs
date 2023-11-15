using System;

namespace Characters.Abilities
{
	// Token: 0x02000A29 RID: 2601
	[Serializable]
	public class GetCinematic : Ability
	{
		// Token: 0x060036F2 RID: 14066 RVA: 0x000A28FB File Offset: 0x000A0AFB
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new GetCinematic.Instance(owner, this);
		}

		// Token: 0x02000A2A RID: 2602
		public class Instance : AbilityInstance<GetCinematic>
		{
			// Token: 0x060036F4 RID: 14068 RVA: 0x000A2904 File Offset: 0x000A0B04
			public Instance(Character owner, GetCinematic ability) : base(owner, ability)
			{
			}

			// Token: 0x060036F5 RID: 14069 RVA: 0x000A290E File Offset: 0x000A0B0E
			protected override void OnAttach()
			{
				this.owner.cinematic.Attach(this);
			}

			// Token: 0x060036F6 RID: 14070 RVA: 0x000A2921 File Offset: 0x000A0B21
			protected override void OnDetach()
			{
				this.owner.cinematic.Detach(this);
			}
		}
	}
}
