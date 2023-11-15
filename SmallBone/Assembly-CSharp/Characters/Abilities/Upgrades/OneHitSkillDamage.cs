using System;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000AF7 RID: 2807
	[Serializable]
	public sealed class OneHitSkillDamage : Ability
	{
		// Token: 0x06003949 RID: 14665 RVA: 0x000A9177 File Offset: 0x000A7377
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new OneHitSkillDamage.Instance(owner, this);
		}

		// Token: 0x04002D81 RID: 11649
		[SerializeField]
		private SoundInfo _effectAudioClipInfo;

		// Token: 0x04002D82 RID: 11650
		[SerializeField]
		private float _damagePercent;

		// Token: 0x04002D83 RID: 11651
		[SerializeField]
		private OneHitSkillDamageMarkingComponent _marking;

		// Token: 0x02000AF8 RID: 2808
		public sealed class Instance : AbilityInstance<OneHitSkillDamage>
		{
			// Token: 0x0600394B RID: 14667 RVA: 0x000A9180 File Offset: 0x000A7380
			public Instance(Character owner, OneHitSkillDamage ability) : base(owner, ability)
			{
			}

			// Token: 0x0600394C RID: 14668 RVA: 0x000A918A File Offset: 0x000A738A
			protected override void OnAttach()
			{
				this.owner.onGiveDamage.Add(int.MaxValue, new GiveDamageDelegate(this.OnGiveDamageDelegate));
			}

			// Token: 0x0600394D RID: 14669 RVA: 0x000A91B0 File Offset: 0x000A73B0
			private bool OnGiveDamageDelegate(ITarget target, ref Damage damage)
			{
				if (damage.motionType != Damage.MotionType.Skill)
				{
					return false;
				}
				if (damage.amount < 1.0)
				{
					return false;
				}
				Character character = target.character;
				if (character == null)
				{
					return false;
				}
				if (character.ability.Contains(this.ability._marking.ability))
				{
					return false;
				}
				damage.percentMultiplier *= (double)this.ability._damagePercent;
				character.ability.Add(this.ability._marking.ability);
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._effectAudioClipInfo, this.owner.transform.position) == null;
				return false;
			}

			// Token: 0x0600394E RID: 14670 RVA: 0x000A926B File Offset: 0x000A746B
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamageDelegate));
			}
		}
	}
}
