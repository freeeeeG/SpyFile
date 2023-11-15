using System;
using Characters.Controllers;

namespace Characters.Abilities.Debuffs
{
	// Token: 0x02000BA9 RID: 2985
	[Serializable]
	public sealed class ReverseHorizontalInput : Ability
	{
		// Token: 0x06003DA8 RID: 15784 RVA: 0x000B3351 File Offset: 0x000B1551
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ReverseHorizontalInput.Instance(owner, this);
		}

		// Token: 0x02000BAA RID: 2986
		public sealed class Instance : AbilityInstance<ReverseHorizontalInput>
		{
			// Token: 0x06003DAA RID: 15786 RVA: 0x000B335A File Offset: 0x000B155A
			public Instance(Character owner, ReverseHorizontalInput ability) : base(owner, ability)
			{
			}

			// Token: 0x06003DAB RID: 15787 RVA: 0x000B3364 File Offset: 0x000B1564
			protected override void OnAttach()
			{
				PlayerInput.reverseHorizontal.Attach(this);
			}

			// Token: 0x06003DAC RID: 15788 RVA: 0x000B3371 File Offset: 0x000B1571
			protected override void OnDetach()
			{
				PlayerInput.reverseHorizontal.Detach(this);
			}
		}
	}
}
