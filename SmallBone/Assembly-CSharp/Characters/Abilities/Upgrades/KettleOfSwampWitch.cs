using System;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Player;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000ADD RID: 2781
	[Serializable]
	public class KettleOfSwampWitch : Ability
	{
		// Token: 0x060038F4 RID: 14580 RVA: 0x000A7C58 File Offset: 0x000A5E58
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new KettleOfSwampWitch.Instance(owner, this);
		}

		// Token: 0x02000ADE RID: 2782
		public class Instance : AbilityInstance<KettleOfSwampWitch>
		{
			// Token: 0x060038F6 RID: 14582 RVA: 0x000A7C61 File Offset: 0x000A5E61
			public Instance(Character owner, KettleOfSwampWitch ability) : base(owner, ability)
			{
			}

			// Token: 0x060038F7 RID: 14583 RVA: 0x000A7C84 File Offset: 0x000A5E84
			protected override void OnAttach()
			{
				PlayerComponents playerComponents = this.owner.playerComponents;
				if (playerComponents == null)
				{
					return;
				}
				playerComponents.inventory.onUpdatedKeywordCounts += this.HandleOnUpdatedKeywordCounts;
			}

			// Token: 0x060038F8 RID: 14584 RVA: 0x000A7CB8 File Offset: 0x000A5EB8
			private void HandleOnUpdatedKeywordCounts()
			{
				PlayerComponents playerComponents = this.owner.playerComponents;
				if (playerComponents == null)
				{
					return;
				}
				foreach (Inscription.Key key in Inscription.keys)
				{
					bool flag = false;
					foreach (Inscription.Key key2 in this._statusKeys)
					{
						if (key == key2)
						{
							flag = true;
							break;
						}
					}
					ref int ptr = ref playerComponents.inventory.synergy.inscriptions[key].count;
					if (ptr >= 1 && flag)
					{
						ptr++;
					}
				}
			}

			// Token: 0x060038F9 RID: 14585 RVA: 0x000A7D68 File Offset: 0x000A5F68
			protected override void OnDetach()
			{
				PlayerComponents playerComponents = this.owner.playerComponents;
				if (playerComponents == null)
				{
					return;
				}
				playerComponents.inventory.onUpdatedKeywordCounts -= this.HandleOnUpdatedKeywordCounts;
			}

			// Token: 0x04002D3B RID: 11579
			private readonly Inscription.Key[] _statusKeys = new Inscription.Key[]
			{
				Inscription.Key.AbsoluteZero,
				Inscription.Key.Arson,
				Inscription.Key.Dizziness,
				Inscription.Key.ExcessiveBleeding,
				Inscription.Key.Poisoning
			};
		}
	}
}
