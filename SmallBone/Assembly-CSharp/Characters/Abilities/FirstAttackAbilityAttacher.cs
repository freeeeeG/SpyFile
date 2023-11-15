using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009ED RID: 2541
	public sealed class FirstAttackAbilityAttacher : AbilityAttacher
	{
		// Token: 0x06003610 RID: 13840 RVA: 0x000A05BB File Offset: 0x0009E7BB
		public override void OnIntialize()
		{
			this._abilityComponent.Initialize();
			this._mark = new FirstAttackAbilityAttacher.FirstAttackMark
			{
				duration = 2.1474836E+09f
			};
			this._mark.Initialize();
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x000A05E9 File Offset: 0x0009E7E9
		public override void StartAttach()
		{
			Character owner = base.owner;
			owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x000A0614 File Offset: 0x0009E814
		private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			Character character = target.character;
			if (character == null)
			{
				return;
			}
			if (!this._targetTypeFilter[character.type])
			{
				return;
			}
			if (!this._motionTypeFilter[gaveDamage.motionType])
			{
				return;
			}
			if (!this._attackTypeFilter[gaveDamage.attackType])
			{
				return;
			}
			if (!this._attributeFilter[gaveDamage.attribute])
			{
				return;
			}
			if (character.ability.Contains(this._mark))
			{
				return;
			}
			character.ability.Add(this._mark);
			base.owner.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x06003613 RID: 13843 RVA: 0x000A06C4 File Offset: 0x0009E8C4
		public override void StopAttach()
		{
			if (base.owner == null)
			{
				return;
			}
			Character owner = base.owner;
			owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x06003614 RID: 13844 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04002B69 RID: 11113
		[SerializeField]
		private CharacterTypeBoolArray _targetTypeFilter;

		// Token: 0x04002B6A RID: 11114
		[SerializeField]
		private MotionTypeBoolArray _motionTypeFilter;

		// Token: 0x04002B6B RID: 11115
		[SerializeField]
		private AttackTypeBoolArray _attackTypeFilter;

		// Token: 0x04002B6C RID: 11116
		[SerializeField]
		private DamageAttributeBoolArray _attributeFilter;

		// Token: 0x04002B6D RID: 11117
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		private AbilityComponent _abilityComponent;

		// Token: 0x04002B6E RID: 11118
		private FirstAttackAbilityAttacher.FirstAttackMark _mark;

		// Token: 0x020009EE RID: 2542
		public sealed class FirstAttackMark : Ability
		{
			// Token: 0x06003616 RID: 13846 RVA: 0x000A0723 File Offset: 0x0009E923
			public override IAbilityInstance CreateInstance(Character owner)
			{
				return new FirstAttackAbilityAttacher.FirstAttackMark.Instance(owner, this);
			}

			// Token: 0x020009EF RID: 2543
			public sealed class Instance : AbilityInstance<FirstAttackAbilityAttacher.FirstAttackMark>
			{
				// Token: 0x06003618 RID: 13848 RVA: 0x000A072C File Offset: 0x0009E92C
				public Instance(Character owner, FirstAttackAbilityAttacher.FirstAttackMark ability) : base(owner, ability)
				{
				}

				// Token: 0x06003619 RID: 13849 RVA: 0x00002191 File Offset: 0x00000391
				protected override void OnAttach()
				{
				}

				// Token: 0x0600361A RID: 13850 RVA: 0x00002191 File Offset: 0x00000391
				protected override void OnDetach()
				{
				}
			}
		}
	}
}
