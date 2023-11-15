using System;
using Characters.Gear.Weapons.Gauges;
using Characters.Movements;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A5F RID: 2655
	[Serializable]
	public sealed class ModifyDamageByAirTIme : Ability
	{
		// Token: 0x06003778 RID: 14200 RVA: 0x000A38AB File Offset: 0x000A1AAB
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ModifyDamageByAirTIme.Instance(owner, this);
		}

		// Token: 0x04002C25 RID: 11301
		[SerializeField]
		private ValueGauge _gauge;

		// Token: 0x04002C26 RID: 11302
		[SerializeField]
		private MotionTypeBoolArray _motionType;

		// Token: 0x04002C27 RID: 11303
		[SerializeField]
		private AttackTypeBoolArray _attachkType;

		// Token: 0x04002C28 RID: 11304
		[SerializeField]
		private string _attackKey;

		// Token: 0x04002C29 RID: 11305
		[Header("= DamageMultiplier * MaxMultiplier * (AireTime / MaxTime)")]
		[Space(5f)]
		[SerializeField]
		private float _maxDamageMultiplier = 3f;

		// Token: 0x04002C2A RID: 11306
		[SerializeField]
		private float _timeToMaxDamageMultiplier;

		// Token: 0x04002C2B RID: 11307
		[Tooltip("바닥에 착지할 경우 이 시간 후에 버프가 사라짐")]
		[SerializeField]
		private float _remainTimeOnGround = 1f;

		// Token: 0x02000A60 RID: 2656
		public sealed class Instance : AbilityInstance<ModifyDamageByAirTIme>
		{
			// Token: 0x17000BB2 RID: 2994
			// (get) Token: 0x0600377A RID: 14202 RVA: 0x00018C86 File Offset: 0x00016E86
			public override Sprite icon
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17000BB3 RID: 2995
			// (get) Token: 0x0600377B RID: 14203 RVA: 0x000A38D2 File Offset: 0x000A1AD2
			public override float iconFillAmount
			{
				get
				{
					return 1f - this._airTime / this.ability._timeToMaxDamageMultiplier;
				}
			}

			// Token: 0x0600377C RID: 14204 RVA: 0x000A38EC File Offset: 0x000A1AEC
			public Instance(Character owner, ModifyDamageByAirTIme ability) : base(owner, ability)
			{
				if (ability._gauge != null)
				{
					ability._gauge.maxValue = ability._timeToMaxDamageMultiplier;
				}
			}

			// Token: 0x0600377D RID: 14205 RVA: 0x000A3918 File Offset: 0x000A1B18
			protected override void OnAttach()
			{
				this._wasGrounded = this.owner.movement.controller.isGrounded;
				this.owner.movement.onJump += this.OnJump;
				this.owner.movement.onGrounded += this.OnGrounded;
				this.owner.onGiveDamage.Add(0, new GiveDamageDelegate(this.OnGiveDamage));
			}

			// Token: 0x0600377E RID: 14206 RVA: 0x000A3998 File Offset: 0x000A1B98
			private bool OnGiveDamage(ITarget target, ref Damage damage)
			{
				if (!this.ability._motionType[damage.motionType] || !this.ability._attachkType[damage.attackType])
				{
					return false;
				}
				if (!damage.key.Equals(this.ability._attackKey, StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}
				damage.multiplier *= (double)this._cachedMultiplier;
				return false;
			}

			// Token: 0x0600377F RID: 14207 RVA: 0x000A3A04 File Offset: 0x000A1C04
			protected override void OnDetach()
			{
				this.owner.movement.onJump -= this.OnJump;
				this.owner.movement.onGrounded -= this.OnGrounded;
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
				if (this.ability._gauge != null)
				{
					this.ability._gauge.Set(0f);
				}
			}

			// Token: 0x06003780 RID: 14208 RVA: 0x000A3A90 File Offset: 0x000A1C90
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainUpdateTime -= deltaTime;
				if (this._wasGrounded)
				{
					this._remainBuffTime -= deltaTime;
				}
				else
				{
					this._airTime += deltaTime;
				}
				if (this._airTime > this.ability._timeToMaxDamageMultiplier)
				{
					this._airTime = this.ability._timeToMaxDamageMultiplier;
				}
				if (this._remainUpdateTime < 0f)
				{
					this._remainUpdateTime = 0.25f;
					this.UpdateStat();
				}
			}

			// Token: 0x06003781 RID: 14209 RVA: 0x000A3B1C File Offset: 0x000A1D1C
			public void UpdateStat()
			{
				float num = 0f;
				if (this._remainBuffTime > 0f)
				{
					num = this.ability._maxDamageMultiplier * this._airTime / this.ability._timeToMaxDamageMultiplier;
				}
				if (num == this._cachedMultiplier)
				{
					return;
				}
				if (this.ability._gauge != null)
				{
					if (!this._wasGrounded)
					{
						this.ability._gauge.Set(this._airTime);
					}
					else
					{
						this.ability._gauge.Set(0f);
					}
				}
				this._cachedMultiplier = num;
			}

			// Token: 0x06003782 RID: 14210 RVA: 0x000A3BB4 File Offset: 0x000A1DB4
			private void OnJump(Movement.JumpType jumpType, float jumpHeight)
			{
				if (this._wasGrounded)
				{
					this._wasGrounded = false;
					this._airTime = 0f;
				}
				this._remainBuffTime = float.PositiveInfinity;
				this.UpdateStat();
			}

			// Token: 0x06003783 RID: 14211 RVA: 0x000A3BE1 File Offset: 0x000A1DE1
			private void OnGrounded()
			{
				this._wasGrounded = true;
				this._remainBuffTime = this.ability._remainTimeOnGround;
			}

			// Token: 0x04002C2C RID: 11308
			private const float _updateInterval = 0.25f;

			// Token: 0x04002C2D RID: 11309
			private float _remainUpdateTime;

			// Token: 0x04002C2E RID: 11310
			private float _remainBuffTime;

			// Token: 0x04002C2F RID: 11311
			private bool _wasGrounded;

			// Token: 0x04002C30 RID: 11312
			private float _airTime;

			// Token: 0x04002C31 RID: 11313
			private float _cachedMultiplier;
		}
	}
}
