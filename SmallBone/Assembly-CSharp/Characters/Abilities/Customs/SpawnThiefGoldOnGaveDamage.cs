using System;
using Data;
using Level;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D91 RID: 3473
	[Serializable]
	public class SpawnThiefGoldOnGaveDamage : Ability
	{
		// Token: 0x060045E9 RID: 17897 RVA: 0x000CA86A File Offset: 0x000C8A6A
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new SpawnThiefGoldOnGaveDamage.Instance(owner, this);
		}

		// Token: 0x04003512 RID: 13586
		[SerializeField]
		private PoolObject _thiefGold;

		// Token: 0x04003513 RID: 13587
		[SerializeField]
		private int _goldAmount;

		// Token: 0x04003514 RID: 13588
		[SerializeField]
		[Header("Filter")]
		private double _minDamage = 1.0;

		// Token: 0x04003515 RID: 13589
		[SerializeField]
		private CharacterTypeBoolArray _characterTypeFilter = new CharacterTypeBoolArray(new bool[]
		{
			true,
			true,
			true,
			true,
			true,
			false,
			false,
			false
		});

		// Token: 0x04003516 RID: 13590
		[SerializeField]
		private MotionTypeBoolArray _motionTypeFilter;

		// Token: 0x04003517 RID: 13591
		[SerializeField]
		private AttackTypeBoolArray _attackTypeFilter;

		// Token: 0x02000D92 RID: 3474
		public class Instance : AbilityInstance<SpawnThiefGoldOnGaveDamage>
		{
			// Token: 0x060045EB RID: 17899 RVA: 0x000CA8A6 File Offset: 0x000C8AA6
			public Instance(Character owner, SpawnThiefGoldOnGaveDamage ability) : base(owner, ability)
			{
			}

			// Token: 0x060045EC RID: 17900 RVA: 0x000CA8B0 File Offset: 0x000C8AB0
			protected override void OnAttach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnCharacterGaveDamage));
			}

			// Token: 0x060045ED RID: 17901 RVA: 0x000CA8D9 File Offset: 0x000C8AD9
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnCharacterGaveDamage));
			}

			// Token: 0x060045EE RID: 17902 RVA: 0x000CA904 File Offset: 0x000C8B04
			private void OnCharacterGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				Damage damage = gaveDamage;
				if (damage.amount < this.ability._minDamage)
				{
					return;
				}
				if (target.character == null)
				{
					return;
				}
				if (!this.ability._characterTypeFilter[target.character.type])
				{
					return;
				}
				if (!this.ability._motionTypeFilter[gaveDamage.motionType])
				{
					return;
				}
				if (!this.ability._attackTypeFilter[gaveDamage.attackType])
				{
					return;
				}
				this.SpawnGold(gaveDamage.hitPoint);
			}

			// Token: 0x060045EF RID: 17903 RVA: 0x000CA99D File Offset: 0x000C8B9D
			private void SpawnGold(Vector3 position)
			{
				CurrencyParticle component = this.ability._thiefGold.Spawn(position, true).GetComponent<CurrencyParticle>();
				component.currencyType = GameData.Currency.Type.Gold;
				component.currencyAmount = this.ability._goldAmount;
			}
		}
	}
}
