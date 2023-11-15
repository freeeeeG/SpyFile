using System;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CB9 RID: 3257
	[Serializable]
	public sealed class FenrirFang : Ability
	{
		// Token: 0x06004220 RID: 16928 RVA: 0x000C09D4 File Offset: 0x000BEBD4
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new FenrirFang.Instance(owner, this);
		}

		// Token: 0x02000CBA RID: 3258
		public class Instance : AbilityInstance<FenrirFang>
		{
			// Token: 0x06004222 RID: 16930 RVA: 0x000C09DD File Offset: 0x000BEBDD
			public Instance(Character owner, FenrirFang ability) : base(owner, ability)
			{
			}

			// Token: 0x06004223 RID: 16931 RVA: 0x000C09E7 File Offset: 0x000BEBE7
			protected override void OnAttach()
			{
				this.owner.status.giveStoppingPowerOnPoison = true;
			}

			// Token: 0x06004224 RID: 16932 RVA: 0x000C09FA File Offset: 0x000BEBFA
			protected override void OnDetach()
			{
				this.owner.status.giveStoppingPowerOnPoison = false;
			}
		}
	}
}
