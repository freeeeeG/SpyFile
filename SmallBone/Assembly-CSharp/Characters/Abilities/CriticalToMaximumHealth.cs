using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A07 RID: 2567
	[Serializable]
	public class CriticalToMaximumHealth : Ability
	{
		// Token: 0x06003687 RID: 13959 RVA: 0x000A1645 File Offset: 0x0009F845
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new CriticalToMaximumHealth.Instance(owner, this);
		}

		// Token: 0x04002BAC RID: 11180
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x02000A08 RID: 2568
		public class Instance : AbilityInstance<CriticalToMaximumHealth>
		{
			// Token: 0x06003689 RID: 13961 RVA: 0x000A164E File Offset: 0x0009F84E
			internal Instance(Character owner, CriticalToMaximumHealth ability) : base(owner, ability)
			{
			}

			// Token: 0x0600368A RID: 13962 RVA: 0x000A1658 File Offset: 0x0009F858
			protected override void OnAttach()
			{
				this.owner.onGiveDamage.Add(0, new GiveDamageDelegate(this.OnGiveDamage));
			}

			// Token: 0x0600368B RID: 13963 RVA: 0x000A1677 File Offset: 0x0009F877
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
			}

			// Token: 0x0600368C RID: 13964 RVA: 0x000A1698 File Offset: 0x0009F898
			private bool OnGiveDamage(ITarget target, ref Damage damage)
			{
				Character character = target.character;
				CharacterHealth characterHealth = (character != null) ? character.health : null;
				if (characterHealth == null || characterHealth.percent < 1.0)
				{
					return false;
				}
				damage.criticalChance = 1.0;
				this._remainCooldownTime = this.ability._cooldownTime;
				return false;
			}

			// Token: 0x04002BAD RID: 11181
			private float _remainCooldownTime;
		}
	}
}
