using System;
using Characters.Operations;
using FX;
using UnityEngine;

namespace Characters.Abilities.Weapons.Skeleton_Sword
{
	// Token: 0x02000BFC RID: 3068
	[Serializable]
	public class Skeleton_SwordTatanusDamage : Ability
	{
		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x06003EFD RID: 16125 RVA: 0x000B70A4 File Offset: 0x000B52A4
		// (set) Token: 0x06003EFE RID: 16126 RVA: 0x000B70AC File Offset: 0x000B52AC
		public Character attacker { get; set; }

		// Token: 0x06003EFF RID: 16127 RVA: 0x000B70B5 File Offset: 0x000B52B5
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Skeleton_SwordTatanusDamage.Instance(owner, this);
		}

		// Token: 0x04003096 RID: 12438
		[SerializeField]
		private float _delay;

		// Token: 0x04003097 RID: 12439
		[SerializeField]
		private int _damage;

		// Token: 0x04003098 RID: 12440
		[SerializeField]
		private float _damageCycleTime;

		// Token: 0x04003099 RID: 12441
		[SerializeField]
		private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Additional);

		// Token: 0x0400309A RID: 12442
		[SerializeField]
		private EffectInfo _hitEffect;

		// Token: 0x0400309B RID: 12443
		[Space]
		[SerializeField]
		private Stat.Values _stat;

		// Token: 0x02000BFD RID: 3069
		public class Instance : AbilityInstance<Skeleton_SwordTatanusDamage>
		{
			// Token: 0x06003F01 RID: 16129 RVA: 0x000B70D2 File Offset: 0x000B52D2
			public Instance(Character owner, Skeleton_SwordTatanusDamage ability) : base(owner, ability)
			{
			}

			// Token: 0x06003F02 RID: 16130 RVA: 0x000B70DC File Offset: 0x000B52DC
			protected override void OnAttach()
			{
				this._remainTimeToNextTick = (double)this.ability._delay;
				base.remainTime += this.ability._delay;
				if (this.ability._stat.values.Length != 0)
				{
					this.owner.stat.AttachValues(this.ability._stat);
				}
			}

			// Token: 0x06003F03 RID: 16131 RVA: 0x000B7141 File Offset: 0x000B5341
			protected override void OnDetach()
			{
				if (this.ability._stat.values.Length != 0)
				{
					this.owner.stat.DetachValues(this.ability._stat);
				}
			}

			// Token: 0x06003F04 RID: 16132 RVA: 0x000B7174 File Offset: 0x000B5374
			public override void UpdateTime(float deltaTime)
			{
				base.remainTime -= deltaTime;
				this._remainTimeToNextTick -= (double)deltaTime;
				if (this._remainTimeToNextTick < 0.0)
				{
					this._remainTimeToNextTick += (double)this.ability._damageCycleTime;
					this.GiveDamage();
				}
			}

			// Token: 0x06003F05 RID: 16133 RVA: 0x000B71D0 File Offset: 0x000B53D0
			private void GiveDamage()
			{
				if (this.ability.attacker == null)
				{
					return;
				}
				if (this.owner == null)
				{
					return;
				}
				Vector2 vector = MMMaths.RandomPointWithinBounds(this.owner.collider.bounds);
				Damage damage = this.ability.attacker.stat.GetDamage((double)this.ability._damage, vector, this.ability._hitInfo);
				this.ability.attacker.Attack(this.owner, ref damage);
				this.ability._hitEffect.Spawn(vector, (float)UnityEngine.Random.Range(0, 360), 1f);
			}

			// Token: 0x0400309D RID: 12445
			private double _remainTimeToNextTick;
		}
	}
}
