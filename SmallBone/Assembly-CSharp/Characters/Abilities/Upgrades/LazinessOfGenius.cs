using System;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Player;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000AE6 RID: 2790
	[Serializable]
	public class LazinessOfGenius : Ability
	{
		// Token: 0x06003915 RID: 14613 RVA: 0x000A8417 File Offset: 0x000A6617
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new LazinessOfGenius.Instance(owner, this);
		}

		// Token: 0x02000AE7 RID: 2791
		public class Instance : AbilityInstance<LazinessOfGenius>
		{
			// Token: 0x06003917 RID: 14615 RVA: 0x000A8420 File Offset: 0x000A6620
			public Instance(Character owner, LazinessOfGenius ability) : base(owner, ability)
			{
			}

			// Token: 0x06003918 RID: 14616 RVA: 0x000A842C File Offset: 0x000A662C
			protected override void OnAttach()
			{
				PlayerComponents playerComponents = this.owner.playerComponents;
				if (playerComponents == null)
				{
					return;
				}
				playerComponents.inventory.onUpdatedKeywordCounts += this.HandleOnUpdatedKeywordCounts;
			}

			// Token: 0x06003919 RID: 14617 RVA: 0x000A8460 File Offset: 0x000A6660
			private void HandleOnUpdatedKeywordCounts()
			{
				PlayerComponents playerComponents = this.owner.playerComponents;
				if (playerComponents == null)
				{
					return;
				}
				foreach (Inscription.Key key in Inscription.keys)
				{
					ref int ptr = ref playerComponents.inventory.synergy.inscriptions[key].count;
					if (ptr == 1)
					{
						ptr++;
					}
				}
			}

			// Token: 0x0600391A RID: 14618 RVA: 0x000A84DC File Offset: 0x000A66DC
			protected override void OnDetach()
			{
				PlayerComponents playerComponents = this.owner.playerComponents;
				if (playerComponents == null)
				{
					return;
				}
				playerComponents.inventory.onUpdatedKeywordCounts -= this.HandleOnUpdatedKeywordCounts;
			}
		}
	}
}
