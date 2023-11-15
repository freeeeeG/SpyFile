using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x02000894 RID: 2196
	public sealed class HiddenBlade : InscriptionInstance
	{
		// Token: 0x06002E6F RID: 11887 RVA: 0x0008BFFD File Offset: 0x0008A1FD
		protected override void Initialize()
		{
			this._attacked = new Nothing
			{
				duration = 2.1474836E+09f
			};
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x00002191 File Offset: 0x00000391
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x0008C018 File Offset: 0x0008A218
		public override void Attach()
		{
			base.character.onGiveDamage.Add(int.MinValue, new GiveDamageDelegate(this.AttachMark));
			base.character.onGiveDamage.Add(int.MinValue, new GiveDamageDelegate(this.EvaluateCritical));
		}

		// Token: 0x06002E72 RID: 11890 RVA: 0x0008C068 File Offset: 0x0008A268
		private bool AttachMark(ITarget target, ref Damage damage)
		{
			if (this.keyword.step < 1)
			{
				return false;
			}
			if (target.character == null)
			{
				return false;
			}
			if (!this._targetTypes[target.character.type])
			{
				return false;
			}
			if (target.character.ability.Contains(this._attacked))
			{
				return false;
			}
			target.character.ability.Add(this._attacked);
			if (this.keyword.isMaxStep)
			{
				target.character.ability.Add(this._enhancedDebuff);
			}
			else
			{
				target.character.ability.Add(this._debuff);
			}
			return false;
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x0008C120 File Offset: 0x0008A320
		private bool EvaluateCritical(ITarget target, ref Damage damage)
		{
			if (!this.keyword.isMaxStep)
			{
				return false;
			}
			if (target.character == null)
			{
				return false;
			}
			if (!this._targetTypes[target.character.type])
			{
				return false;
			}
			if (!target.character.ability.Contains(this._enhancedDebuff))
			{
				return false;
			}
			damage.SetGuaranteedCritical(0, true);
			return false;
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x0008C189 File Offset: 0x0008A389
		public override void Detach()
		{
			base.character.onGiveDamage.Remove(new GiveDamageDelegate(this.AttachMark));
			base.character.onGiveDamage.Remove(new GiveDamageDelegate(this.EvaluateCritical));
		}

		// Token: 0x0400268C RID: 9868
		[SerializeField]
		[Header("2세트 효과")]
		private CharacterTypeBoolArray _targetTypes;

		// Token: 0x0400268D RID: 9869
		[SerializeField]
		private HiddenBlade.HiddenBladeAbility2 _debuff;

		// Token: 0x0400268E RID: 9870
		[Header("4세트 효과")]
		[SerializeField]
		private HiddenBlade.HiddenBladeAbility2 _enhancedDebuff;

		// Token: 0x0400268F RID: 9871
		private Nothing _attacked;

		// Token: 0x02000895 RID: 2197
		[Serializable]
		public sealed class HiddenBladeAbility2 : Ability
		{
			// Token: 0x06002E76 RID: 11894 RVA: 0x0008C1C5 File Offset: 0x0008A3C5
			public override IAbilityInstance CreateInstance(Character owner)
			{
				return new HiddenBlade.HiddenBladeAbility2.Instance(owner, this);
			}

			// Token: 0x04002690 RID: 9872
			[SerializeField]
			private CharacterTypeBoolArray _bossCharacterType;

			// Token: 0x04002691 RID: 9873
			[SerializeField]
			private Stat.Values _debuffStat = new Stat.Values(new Stat.Value[]
			{
				new Stat.Value(Stat.Category.Percent, Stat.Kind.TakingDamage, 1.0)
			});

			// Token: 0x04002692 RID: 9874
			[SerializeField]
			private float _originDuration;

			// Token: 0x04002693 RID: 9875
			[SerializeField]
			private float _durationForBoss;

			// Token: 0x04002694 RID: 9876
			[SerializeField]
			private bool _alwaysCritical;

			// Token: 0x02000896 RID: 2198
			public sealed class Instance : AbilityInstance<HiddenBlade.HiddenBladeAbility2>
			{
				// Token: 0x06002E78 RID: 11896 RVA: 0x0008C202 File Offset: 0x0008A402
				public Instance(Character owner, HiddenBlade.HiddenBladeAbility2 ability) : base(owner, ability)
				{
				}

				// Token: 0x06002E79 RID: 11897 RVA: 0x0008C20C File Offset: 0x0008A40C
				protected override void OnAttach()
				{
					if (this.ability._bossCharacterType[this.owner.type])
					{
						base.remainTime = this.ability._durationForBoss;
					}
					else
					{
						base.remainTime = this.ability._originDuration;
					}
					this.owner.stat.AttachValues(this.ability._debuffStat);
					this.owner.stat.Update();
				}

				// Token: 0x06002E7A RID: 11898 RVA: 0x0008C285 File Offset: 0x0008A485
				protected override void OnDetach()
				{
					this.owner.stat.DetachValues(this.ability._debuffStat);
				}
			}
		}
	}
}
