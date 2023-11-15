using System;
using Data;
using Level;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D94 RID: 3476
	[Serializable]
	public class SpawnThiefGoldOnTookDamage : Ability
	{
		// Token: 0x060045F1 RID: 17905 RVA: 0x000CA9D5 File Offset: 0x000C8BD5
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new SpawnThiefGoldOnTookDamage.Instance(owner, this);
		}

		// Token: 0x04003518 RID: 13592
		[SerializeField]
		private PoolObject _thiefGold;

		// Token: 0x04003519 RID: 13593
		[SerializeField]
		private int _goldAmount;

		// Token: 0x0400351A RID: 13594
		[SerializeField]
		[Header("Filter")]
		private double _minDamage = 1.0;

		// Token: 0x0400351B RID: 13595
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

		// Token: 0x0400351C RID: 13596
		[SerializeField]
		private MotionTypeBoolArray _motionTypeFilter;

		// Token: 0x0400351D RID: 13597
		[SerializeField]
		private AttackTypeBoolArray _attackTypeFilter;

		// Token: 0x02000D95 RID: 3477
		public class Instance : AbilityInstance<SpawnThiefGoldOnTookDamage>
		{
			// Token: 0x060045F3 RID: 17907 RVA: 0x000CAA11 File Offset: 0x000C8C11
			public Instance(Character owner, SpawnThiefGoldOnTookDamage ability) : base(owner, ability)
			{
			}

			// Token: 0x060045F4 RID: 17908 RVA: 0x000CAA1B File Offset: 0x000C8C1B
			protected override void OnAttach()
			{
				if (!this.ability._characterTypeFilter[this.owner.type])
				{
					return;
				}
				this.owner.health.onTookDamage += new TookDamageDelegate(this.OnCharacterTookDamage);
			}

			// Token: 0x060045F5 RID: 17909 RVA: 0x000CAA57 File Offset: 0x000C8C57
			protected override void OnDetach()
			{
				if (!this.ability._characterTypeFilter[this.owner.type])
				{
					return;
				}
				this.owner.health.onTookDamage -= new TookDamageDelegate(this.OnCharacterTookDamage);
			}

			// Token: 0x060045F6 RID: 17910 RVA: 0x000CAA94 File Offset: 0x000C8C94
			private void OnCharacterTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				Damage damage = tookDamage;
				if (damage.amount < this.ability._minDamage || !this.ability._motionTypeFilter[tookDamage.motionType] || !this.ability._attackTypeFilter[tookDamage.attackType])
				{
					return;
				}
				this.SpawnGold(tookDamage.hitPoint);
			}

			// Token: 0x060045F7 RID: 17911 RVA: 0x000CAAFE File Offset: 0x000C8CFE
			private void SpawnGold(Vector3 position)
			{
				CurrencyParticle component = this.ability._thiefGold.Spawn(position, true).GetComponent<CurrencyParticle>();
				component.currencyType = GameData.Currency.Type.Gold;
				component.currencyAmount = this.ability._goldAmount;
			}
		}
	}
}
