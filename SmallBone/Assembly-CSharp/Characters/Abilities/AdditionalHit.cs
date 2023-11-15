using System;
using Characters.Operations;
using FX;
using FX.BoundsAttackVisualEffect;
using Singletons;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Abilities
{
	// Token: 0x020009AB RID: 2475
	[Serializable]
	public class AdditionalHit : Ability
	{
		// Token: 0x060034FE RID: 13566 RVA: 0x0009CCD8 File Offset: 0x0009AED8
		public override void Initialize()
		{
			base.Initialize();
			this._targetOperationInfo.Initialize();
		}

		// Token: 0x060034FF RID: 13567 RVA: 0x0009CCEB File Offset: 0x0009AEEB
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new AdditionalHit.Instance(owner, this);
		}

		// Token: 0x04002A9E RID: 10910
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x04002A9F RID: 10911
		[SerializeField]
		private int _applyCount;

		// Token: 0x04002AA0 RID: 10912
		[SerializeField]
		private float _additionalDamageAmount;

		// Token: 0x04002AA1 RID: 10913
		[SerializeField]
		private HitInfo _additionalHit = new HitInfo(Damage.AttackType.Additional);

		// Token: 0x04002AA2 RID: 10914
		[SerializeField]
		private bool _needCritical;

		// Token: 0x04002AA3 RID: 10915
		[SerializeField]
		private MotionTypeBoolArray _attackTypes;

		// Token: 0x04002AA4 RID: 10916
		[SerializeField]
		private AttackTypeBoolArray _damageTypes;

		// Token: 0x04002AA5 RID: 10917
		[SerializeField]
		private PositionInfo _positionInfo;

		// Token: 0x04002AA6 RID: 10918
		[SerializeField]
		private Transform _targetPoint;

		// Token: 0x04002AA7 RID: 10919
		[SerializeField]
		[Subcomponent(typeof(TargetedOperationInfo))]
		private TargetedOperationInfo.Subcomponents _targetOperationInfo;

		// Token: 0x04002AA8 RID: 10920
		[SerializeField]
		[BoundsAttackVisualEffect.SubcomponentAttribute]
		private BoundsAttackVisualEffect.Subcomponents _hitEffect;

		// Token: 0x04002AA9 RID: 10921
		[SerializeField]
		private SoundInfo _attackSoundInfo;

		// Token: 0x020009AC RID: 2476
		public class Instance : AbilityInstance<AdditionalHit>
		{
			// Token: 0x06003501 RID: 13569 RVA: 0x0009CD08 File Offset: 0x0009AF08
			internal Instance(Character owner, AdditionalHit ability) : base(owner, ability)
			{
			}

			// Token: 0x06003502 RID: 13570 RVA: 0x0009CD14 File Offset: 0x0009AF14
			protected override void OnAttach()
			{
				this._remainCount = this.ability._applyCount;
				if (this.ability._applyCount == 0)
				{
					this._remainCount = int.MaxValue;
				}
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
			}

			// Token: 0x06003503 RID: 13571 RVA: 0x0009CD71 File Offset: 0x0009AF71
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCooldownTime -= deltaTime;
			}

			// Token: 0x06003504 RID: 13572 RVA: 0x0009CD88 File Offset: 0x0009AF88
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
			}

			// Token: 0x06003505 RID: 13573 RVA: 0x0009CDB4 File Offset: 0x0009AFB4
			private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				if (this._remainCooldownTime > 0f || target.character.health.dead || !target.transform.gameObject.activeSelf || (this.ability._needCritical && !tookDamage.critical) || !this.ability._attackTypes[tookDamage.motionType] || !this.ability._damageTypes[tookDamage.attackType])
				{
					return;
				}
				if (this.ability._targetPoint != null)
				{
					Vector3 center = target.collider.bounds.center;
					Vector3 size = target.collider.bounds.size;
					size.x *= this.ability._positionInfo.pivotValue.x;
					size.y *= this.ability._positionInfo.pivotValue.y;
					Vector3 position = center + size;
					this.ability._targetPoint.position = position;
				}
				Damage damage = this.owner.stat.GetDamage((double)this.ability._additionalDamageAmount, MMMaths.RandomPointWithinBounds(target.collider.bounds), this.ability._additionalHit);
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._attackSoundInfo, target.transform.position);
				this.owner.StartCoroutine(this.ability._targetOperationInfo.CRun(this.owner, target.character));
				this.owner.Attack(target, ref damage);
				this.ability._hitEffect.Spawn(this.owner, target.collider.bounds, damage, target);
				this._remainCooldownTime = this.ability._cooldownTime;
				this._remainCount--;
				if (this._remainCount == 0)
				{
					this.owner.ability.Remove(this);
				}
			}

			// Token: 0x04002AAA RID: 10922
			private float _remainCooldownTime;

			// Token: 0x04002AAB RID: 10923
			private int _remainCount;
		}
	}
}
