using System;

namespace Characters.Abilities
{
	// Token: 0x02000A8F RID: 2703
	[Serializable]
	public class Nothing : Ability
	{
		// Token: 0x060037FE RID: 14334 RVA: 0x000A53F0 File Offset: 0x000A35F0
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Nothing.Instance(owner, this);
		}

		// Token: 0x02000A90 RID: 2704
		public class Instance : AbilityInstance<Nothing>
		{
			// Token: 0x06003800 RID: 14336 RVA: 0x000A53F9 File Offset: 0x000A35F9
			public Instance(Character owner, Nothing ability) : base(owner, ability)
			{
			}

			// Token: 0x06003801 RID: 14337 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnAttach()
			{
			}

			// Token: 0x06003802 RID: 14338 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnDetach()
			{
			}
		}
	}
}
