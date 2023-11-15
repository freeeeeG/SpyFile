using System;
using Characters.Operations;
using FX.BoundsAttackVisualEffect;
using UnityEngine;

namespace Characters.Abilities.Essences
{
	// Token: 0x02000BE5 RID: 3045
	[Serializable]
	public class Kiriz : Ability
	{
		// Token: 0x06003E94 RID: 16020 RVA: 0x000B5D79 File Offset: 0x000B3F79
		public void SetAttacker(Character attacker)
		{
			this._attacker = attacker;
		}

		// Token: 0x06003E95 RID: 16021 RVA: 0x000B5D82 File Offset: 0x000B3F82
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Kiriz.Instance(owner, this);
		}

		// Token: 0x04003045 RID: 12357
		private Character _attacker;

		// Token: 0x04003046 RID: 12358
		[Header("Attack")]
		[SerializeField]
		private HitInfo _hitInfo;

		// Token: 0x04003047 RID: 12359
		[SerializeField]
		private ChronoInfo _chrono;

		// Token: 0x04003048 RID: 12360
		[BoundsAttackVisualEffect.SubcomponentAttribute]
		[SerializeField]
		private BoundsAttackVisualEffect.Subcomponents _effect;

		// Token: 0x02000BE6 RID: 3046
		public class Instance : AbilityInstance<Kiriz>
		{
			// Token: 0x06003E97 RID: 16023 RVA: 0x000B5D8B File Offset: 0x000B3F8B
			public Instance(Character owner, Kiriz ability) : base(owner, ability)
			{
			}

			// Token: 0x06003E98 RID: 16024 RVA: 0x000B5D98 File Offset: 0x000B3F98
			protected override void OnAttach()
			{
				this._accumulatedDamage = 0.0;
				this.owner.chronometer.animation.AttachTimeScale(this, 0.03f);
				this.owner.health.onTakeDamage.Add(int.MinValue, new TakeDamageDelegate(this.OnTakeDamage));
			}

			// Token: 0x06003E99 RID: 16025 RVA: 0x000B5DF8 File Offset: 0x000B3FF8
			protected override void OnDetach()
			{
				this.owner.chronometer.animation.DetachTimeScale(this);
				this.owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.OnTakeDamage));
				if (!this.owner.liveAndActive)
				{
					return;
				}
				TargetStruct targetStruct = new TargetStruct(this.owner);
				this.ability._chrono.ApplyTo(this.owner);
				Bounds bounds = this.owner.collider.bounds;
				Vector2 hitPoint = MMMaths.RandomPointWithinBounds(bounds);
				Damage damage = this.owner.stat.GetDamage(this._accumulatedDamage, hitPoint, this.ability._hitInfo);
				if (this.owner.cinematic.value)
				{
					this.ability._effect.Spawn(this.owner, bounds, damage, targetStruct);
					return;
				}
				this.ability._attacker.TryAttackCharacter(targetStruct, ref damage);
				if (damage.amount > 0.0)
				{
					this.ability._effect.Spawn(this.owner, bounds, damage, targetStruct);
				}
			}

			// Token: 0x06003E9A RID: 16026 RVA: 0x000B5F26 File Offset: 0x000B4126
			private bool OnTakeDamage(ref Damage damage)
			{
				this._accumulatedDamage += damage.amount;
				return true;
			}

			// Token: 0x04003049 RID: 12361
			private double _accumulatedDamage;
		}
	}
}
