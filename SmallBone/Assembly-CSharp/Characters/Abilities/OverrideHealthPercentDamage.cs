using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A1F RID: 2591
	[Serializable]
	public class OverrideHealthPercentDamage : Ability
	{
		// Token: 0x060036DE RID: 14046 RVA: 0x000A266C File Offset: 0x000A086C
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new OverrideHealthPercentDamage.Instance(owner, this);
		}

		// Token: 0x04002BE1 RID: 11233
		[SerializeField]
		private OverrideHealthPercentDamage.HealthPercentByKey[] _multiplierByKey;

		// Token: 0x02000A20 RID: 2592
		[Serializable]
		public struct HealthPercentByKey
		{
			// Token: 0x04002BE2 RID: 11234
			[Range(0f, 1f)]
			[SerializeField]
			public float percent;

			// Token: 0x04002BE3 RID: 11235
			public string key;
		}

		// Token: 0x02000A21 RID: 2593
		public class Instance : AbilityInstance<OverrideHealthPercentDamage>
		{
			// Token: 0x060036E0 RID: 14048 RVA: 0x000A2675 File Offset: 0x000A0875
			internal Instance(Character owner, OverrideHealthPercentDamage ability) : base(owner, ability)
			{
			}

			// Token: 0x060036E1 RID: 14049 RVA: 0x000A267F File Offset: 0x000A087F
			protected override void OnAttach()
			{
				this.owner.onGiveDamage.Add(int.MaxValue, new GiveDamageDelegate(this.HandleOnGiveDamage));
			}

			// Token: 0x060036E2 RID: 14050 RVA: 0x000A26A2 File Offset: 0x000A08A2
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.HandleOnGiveDamage));
			}

			// Token: 0x060036E3 RID: 14051 RVA: 0x000A26C4 File Offset: 0x000A08C4
			private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
			{
				Character character = target.character;
				if (character == null)
				{
					return false;
				}
				if (string.IsNullOrWhiteSpace(damage.key))
				{
					return false;
				}
				foreach (OverrideHealthPercentDamage.HealthPercentByKey healthPercentByKey in this.ability._multiplierByKey)
				{
					if (damage.key.Equals(healthPercentByKey.key, StringComparison.OrdinalIgnoreCase))
					{
						damage.@base = character.health.maximumHealth * (double)healthPercentByKey.percent;
						break;
					}
				}
				return false;
			}
		}
	}
}
