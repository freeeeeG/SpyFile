using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C68 RID: 3176
	[Serializable]
	public sealed class StatBonusByTargetStatus : Ability
	{
		// Token: 0x060040F2 RID: 16626 RVA: 0x000BCB89 File Offset: 0x000BAD89
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatBonusByTargetStatus.Instance(owner, this);
		}

		// Token: 0x040031DA RID: 12762
		[SerializeField]
		internal CharacterStatus.Kind _kind;

		// Token: 0x040031DB RID: 12763
		[SerializeField]
		private StatBonus _statBonus;

		// Token: 0x02000C69 RID: 3177
		public class Instance : AbilityInstance<StatBonusByTargetStatus>
		{
			// Token: 0x060040F3 RID: 16627 RVA: 0x000BCB92 File Offset: 0x000BAD92
			public Instance(Character owner, StatBonusByTargetStatus ability) : base(owner, ability)
			{
				ability._statBonus.Initialize();
			}

			// Token: 0x060040F4 RID: 16628 RVA: 0x000BCBA7 File Offset: 0x000BADA7
			protected override void OnAttach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x060040F5 RID: 16629 RVA: 0x000BCBD0 File Offset: 0x000BADD0
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
				this.owner.ability.Remove(this.ability._statBonus);
			}

			// Token: 0x060040F6 RID: 16630 RVA: 0x000BCC20 File Offset: 0x000BAE20
			private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				if (target.character == null)
				{
					return;
				}
				switch (this.ability._kind)
				{
				case CharacterStatus.Kind.Stun:
					if (!target.character.status.stuned)
					{
						return;
					}
					this.owner.ability.Add(this.ability._statBonus);
					return;
				case CharacterStatus.Kind.Freeze:
					if (!target.character.status.freezed)
					{
						return;
					}
					this.owner.ability.Add(this.ability._statBonus);
					return;
				case CharacterStatus.Kind.Burn:
					if (!target.character.status.burning)
					{
						return;
					}
					this.owner.ability.Add(this.ability._statBonus);
					return;
				case CharacterStatus.Kind.Wound:
					if (!target.character.status.wounded)
					{
						return;
					}
					this.owner.ability.Add(this.ability._statBonus);
					return;
				case CharacterStatus.Kind.Poison:
					if (!target.character.status.poisoned)
					{
						return;
					}
					this.owner.ability.Add(this.ability._statBonus);
					return;
				case CharacterStatus.Kind.Unmoving:
					if (!target.character.status.unmovable)
					{
						return;
					}
					this.owner.ability.Add(this.ability._statBonus);
					return;
				default:
					return;
				}
			}
		}
	}
}
