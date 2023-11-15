using System;
using Characters.Abilities.CharacterStat;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A0F RID: 2575
	[Serializable]
	public sealed class HotTag : Ability
	{
		// Token: 0x0600369D RID: 13981 RVA: 0x000A1965 File Offset: 0x0009FB65
		public override void Initialize()
		{
			base.Initialize();
			this._stackablStatBonus.Initialize();
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x000A1978 File Offset: 0x0009FB78
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new HotTag.Instance(owner, this);
		}

		// Token: 0x04002BB6 RID: 11190
		[SerializeField]
		private StackableStatBonus _stackablStatBonus;

		// Token: 0x04002BB7 RID: 11191
		[SerializeField]
		private int _maxStack;

		// Token: 0x04002BB8 RID: 11192
		[SerializeField]
		private AttackTypeBoolArray _attackType;

		// Token: 0x04002BB9 RID: 11193
		[SerializeField]
		private AttackTypeBoolArray _hitType;

		// Token: 0x02000A10 RID: 2576
		public class Instance : AbilityInstance<HotTag>
		{
			// Token: 0x17000BA4 RID: 2980
			// (get) Token: 0x060036A0 RID: 13984 RVA: 0x000A1981 File Offset: 0x0009FB81
			public override int iconStacks
			{
				get
				{
					return this._stack;
				}
			}

			// Token: 0x060036A1 RID: 13985 RVA: 0x000A1989 File Offset: 0x0009FB89
			public Instance(Character owner, HotTag ability) : base(owner, ability)
			{
			}

			// Token: 0x060036A2 RID: 13986 RVA: 0x000A1994 File Offset: 0x0009FB94
			protected override void OnAttach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
				this.owner.health.onTookDamage += new TookDamageDelegate(this.OnTookDamage);
				this.owner.playerComponents.inventory.weapon.onSwap += this.OnSwap;
				this._attached = false;
			}

			// Token: 0x060036A3 RID: 13987 RVA: 0x000A1A14 File Offset: 0x0009FC14
			private void OnSwap()
			{
				this.owner.ability.Add(this.ability._stackablStatBonus);
				this.ability._stackablStatBonus.stack = this._stack;
				this._attached = true;
				this._stack = 0;
			}

			// Token: 0x060036A4 RID: 13988 RVA: 0x000A1A64 File Offset: 0x0009FC64
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
				this.owner.health.onTookDamage -= new TookDamageDelegate(this.OnTookDamage);
				this.owner.playerComponents.inventory.weapon.onSwap -= this.OnSwap;
				this.owner.ability.Remove(this.ability._stackablStatBonus);
				this._stack = 0;
				this._attached = false;
			}

			// Token: 0x060036A5 RID: 13989 RVA: 0x000A1B04 File Offset: 0x0009FD04
			private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				if (!this.ability._attackType[tookDamage.attackType])
				{
					return;
				}
				this.AddStack();
			}

			// Token: 0x060036A6 RID: 13990 RVA: 0x000A1B25 File Offset: 0x0009FD25
			private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				if (!this.ability._attackType[gaveDamage.attackType])
				{
					return;
				}
				this.AddStack();
			}

			// Token: 0x060036A7 RID: 13991 RVA: 0x000A1B46 File Offset: 0x0009FD46
			private void AddStack()
			{
				this._stack = ((this._stack + 1 >= this.ability._maxStack) ? this.ability._maxStack : (this._stack + 1));
			}

			// Token: 0x04002BBA RID: 11194
			private int _stack;

			// Token: 0x04002BBB RID: 11195
			private bool _attached;
		}
	}
}
