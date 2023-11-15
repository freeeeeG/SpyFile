using System;
using Characters.Gear.Items;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D47 RID: 3399
	[Serializable]
	public sealed class EmptyPotion : Ability
	{
		// Token: 0x17000E36 RID: 3638
		// (get) Token: 0x06004480 RID: 17536 RVA: 0x000C7004 File Offset: 0x000C5204
		// (set) Token: 0x06004481 RID: 17537 RVA: 0x000C7011 File Offset: 0x000C5211
		public int stack
		{
			get
			{
				return this._instance.stack;
			}
			set
			{
				this._instance.stack = value;
			}
		}

		// Token: 0x06004482 RID: 17538 RVA: 0x000C7020 File Offset: 0x000C5220
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return this._instance = new EmptyPotion.Instance(owner, this);
		}

		// Token: 0x0400343A RID: 13370
		[SerializeField]
		private AttackTypeBoolArray _attackType;

		// Token: 0x0400343B RID: 13371
		[SerializeField]
		private Item _emptyPotion;

		// Token: 0x0400343C RID: 13372
		[SerializeField]
		private int _maxStack;

		// Token: 0x0400343D RID: 13373
		[SerializeField]
		private CustomFloat _healAmount;

		// Token: 0x0400343E RID: 13374
		private EmptyPotion.Instance _instance;

		// Token: 0x02000D48 RID: 3400
		public class Instance : AbilityInstance<EmptyPotion>
		{
			// Token: 0x06004484 RID: 17540 RVA: 0x000C703D File Offset: 0x000C523D
			public Instance(Character owner, EmptyPotion ability) : base(owner, ability)
			{
			}

			// Token: 0x17000E37 RID: 3639
			// (get) Token: 0x06004485 RID: 17541 RVA: 0x000C7047 File Offset: 0x000C5247
			public override int iconStacks
			{
				get
				{
					return this.stack;
				}
			}

			// Token: 0x06004486 RID: 17542 RVA: 0x000C704F File Offset: 0x000C524F
			protected override void OnAttach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x06004487 RID: 17543 RVA: 0x000C7078 File Offset: 0x000C5278
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x06004488 RID: 17544 RVA: 0x000C70A4 File Offset: 0x000C52A4
			private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				if (target.character == null)
				{
					return;
				}
				if (!target.character.status.wounded)
				{
					return;
				}
				if (!this.ability._attackType[gaveDamage.attackType])
				{
					return;
				}
				this.stack++;
				if (this.stack >= this.ability._maxStack)
				{
					this.Heal();
					this.RemoveItem();
				}
			}

			// Token: 0x06004489 RID: 17545 RVA: 0x000C7119 File Offset: 0x000C5319
			private void Heal()
			{
				this.owner.health.Heal((double)this.ability._healAmount.value, true);
			}

			// Token: 0x0600448A RID: 17546 RVA: 0x000C713E File Offset: 0x000C533E
			private void RemoveItem()
			{
				this.ability._emptyPotion.RemoveOnInventory();
				this.owner.ability.Remove(this);
			}

			// Token: 0x0400343F RID: 13375
			public int stack;
		}
	}
}
