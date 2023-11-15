using System;
using Characters.Gear;
using Characters.Gear.Weapons;
using Characters.Operations;
using FX;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x0200099C RID: 2460
	[Serializable]
	public class Abyss : Ability
	{
		// Token: 0x060034DE RID: 13534 RVA: 0x0009C89D File Offset: 0x0009AA9D
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Abyss.Instance(owner, this, this._weapon);
		}

		// Token: 0x04002A8A RID: 10890
		[SerializeField]
		private Weapon _weapon;

		// Token: 0x04002A8B RID: 10891
		[SerializeField]
		private Stat.Values _stat;

		// Token: 0x04002A8C RID: 10892
		[SerializeField]
		private float _hitInterval;

		// Token: 0x04002A8D RID: 10893
		[SerializeField]
		private AttackDamage _attackDamage;

		// Token: 0x04002A8E RID: 10894
		[SerializeField]
		private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Additional);

		// Token: 0x04002A8F RID: 10895
		[SerializeField]
		private EffectInfo _hitEffect;

		// Token: 0x04002A90 RID: 10896
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation.Subcomponents _operationOnHit;

		// Token: 0x0200099D RID: 2461
		public class Instance : AbilityInstance<Abyss>
		{
			// Token: 0x060034E0 RID: 13536 RVA: 0x0009C8C0 File Offset: 0x0009AAC0
			public Instance(Character owner, Abyss ability, Weapon from) : base(owner, ability)
			{
				this._from = from;
			}

			// Token: 0x060034E1 RID: 13537 RVA: 0x0009C8D1 File Offset: 0x0009AAD1
			protected override void OnAttach()
			{
				base.remainTime = this.ability.duration;
				this._remainHitTime = this.ability._hitInterval;
				this.owner.stat.AttachValues(this.ability._stat);
			}

			// Token: 0x060034E2 RID: 13538 RVA: 0x0009C910 File Offset: 0x0009AB10
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this.ability._stat);
			}

			// Token: 0x060034E3 RID: 13539 RVA: 0x0009C92D File Offset: 0x0009AB2D
			public override void Refresh()
			{
				base.remainTime = this.ability.duration;
			}

			// Token: 0x060034E4 RID: 13540 RVA: 0x0009C940 File Offset: 0x0009AB40
			public override void UpdateTime(float deltaTime)
			{
				if (this._from == null || this._from.state != Gear.State.Equipped)
				{
					base.remainTime = 0f;
					return;
				}
				base.remainTime -= deltaTime;
				this._remainHitTime -= deltaTime;
				if (this._remainHitTime < 0f)
				{
					this._remainHitTime += this.ability._hitInterval;
					this.Hit();
				}
			}

			// Token: 0x060034E5 RID: 13541 RVA: 0x0009C9BC File Offset: 0x0009ABBC
			private void Hit()
			{
				Vector2 vector = MMMaths.RandomPointWithinBounds(this.owner.collider.bounds);
				this.ability._hitEffect.Spawn(vector, 0f, 1f);
				if (this.owner.cinematic.value)
				{
					return;
				}
				Damage damage = this.owner.stat.GetDamage((double)this.ability._attackDamage.amount, vector, this.ability._hitInfo);
				this._from.owner.Attack(this.owner, ref damage);
				this.ability._operationOnHit.Run(this._from.owner, this.owner);
			}

			// Token: 0x04002A91 RID: 10897
			private Weapon _from;

			// Token: 0x04002A92 RID: 10898
			private float _remainHitTime;
		}
	}
}
