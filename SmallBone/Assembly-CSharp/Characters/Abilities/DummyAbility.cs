using System;

namespace Characters.Abilities
{
	// Token: 0x02000A1D RID: 2589
	[Serializable]
	public class DummyAbility : Ability
	{
		// Token: 0x060036DA RID: 14042 RVA: 0x000A2659 File Offset: 0x000A0859
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new DummyAbility.Instance(owner, this);
		}

		// Token: 0x02000A1E RID: 2590
		public class Instance : AbilityInstance<DummyAbility>
		{
			// Token: 0x060036DB RID: 14043 RVA: 0x000A2662 File Offset: 0x000A0862
			public Instance(Character owner, DummyAbility ability) : base(owner, ability)
			{
			}

			// Token: 0x060036DC RID: 14044 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnAttach()
			{
			}

			// Token: 0x060036DD RID: 14045 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnDetach()
			{
			}
		}
	}
}
