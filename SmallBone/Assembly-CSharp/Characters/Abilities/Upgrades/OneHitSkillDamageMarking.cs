using System;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000AFA RID: 2810
	[Serializable]
	public sealed class OneHitSkillDamageMarking : Ability
	{
		// Token: 0x06003950 RID: 14672 RVA: 0x000A9292 File Offset: 0x000A7492
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new OneHitSkillDamageMarking.Instance(owner, this);
		}

		// Token: 0x02000AFB RID: 2811
		public sealed class Instance : AbilityInstance<OneHitSkillDamageMarking>
		{
			// Token: 0x06003952 RID: 14674 RVA: 0x000A929B File Offset: 0x000A749B
			public Instance(Character owner, OneHitSkillDamageMarking ability) : base(owner, ability)
			{
			}

			// Token: 0x06003953 RID: 14675 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnAttach()
			{
			}

			// Token: 0x06003954 RID: 14676 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnDetach()
			{
			}
		}
	}
}
