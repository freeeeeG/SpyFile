using System;
using Characters.Player;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CC5 RID: 3269
	[Serializable]
	public sealed class GoludaluSummonBook : Ability
	{
		// Token: 0x06004247 RID: 16967 RVA: 0x000C10A2 File Offset: 0x000BF2A2
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new GoludaluSummonBook.Instance(owner, this);
		}

		// Token: 0x02000CC6 RID: 3270
		public class Instance : AbilityInstance<GoludaluSummonBook>
		{
			// Token: 0x06004249 RID: 16969 RVA: 0x000C10AB File Offset: 0x000BF2AB
			public Instance(Character owner, GoludaluSummonBook ability) : base(owner, ability)
			{
			}

			// Token: 0x0600424A RID: 16970 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnAttach()
			{
			}

			// Token: 0x0600424B RID: 16971 RVA: 0x000C10B8 File Offset: 0x000BF2B8
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				QuintessenceInventory quintessence = this.owner.playerComponents.inventory.quintessence;
				if (quintessence.items.Count <= 0)
				{
					return;
				}
				if (quintessence.items[0].cooldown.canUse)
				{
					quintessence.items[0].Use();
				}
			}

			// Token: 0x0600424C RID: 16972 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnDetach()
			{
			}
		}
	}
}
