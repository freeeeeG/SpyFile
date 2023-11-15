using System;
using Characters.Operations;
using FX.BoundsAttackVisualEffect;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009AD RID: 2477
	[Serializable]
	public class AdditionalHitByTargetStatus : Ability
	{
		// Token: 0x06003506 RID: 13574 RVA: 0x0009CFB6 File Offset: 0x0009B1B6
		public override void Initialize()
		{
			base.Initialize();
			this._targetOperationInfo.Initialize();
		}

		// Token: 0x06003507 RID: 13575 RVA: 0x0009CFC9 File Offset: 0x0009B1C9
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new AdditionalHitByTargetStatus.Instance(owner, this);
		}

		// Token: 0x04002AAC RID: 10924
		[SerializeField]
		private Transform _targetPoint;

		// Token: 0x04002AAD RID: 10925
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x04002AAE RID: 10926
		[SerializeField]
		private int _applyCount;

		// Token: 0x04002AAF RID: 10927
		[SerializeField]
		private AdditionalHitByTargetStatus.DamageAmountType _damageAmountType;

		// Token: 0x04002AB0 RID: 10928
		[SerializeField]
		private int _additionalDamageAmount;

		// Token: 0x04002AB1 RID: 10929
		[SerializeField]
		private HitInfo _additionalHit = new HitInfo(Damage.AttackType.Additional);

		// Token: 0x04002AB2 RID: 10930
		[SerializeField]
		private CharacterStatusKindBoolArray _targetStatusFilter;

		// Token: 0x04002AB3 RID: 10931
		[SerializeField]
		private bool _needCritical;

		// Token: 0x04002AB4 RID: 10932
		[SerializeField]
		private float _minDamage = 1f;

		// Token: 0x04002AB5 RID: 10933
		[SerializeField]
		private MotionTypeBoolArray _attackTypes;

		// Token: 0x04002AB6 RID: 10934
		[SerializeField]
		private AttackTypeBoolArray _damageTypes;

		// Token: 0x04002AB7 RID: 10935
		[SerializeField]
		[BoundsAttackVisualEffect.SubcomponentAttribute]
		private BoundsAttackVisualEffect.Subcomponents _hitEffect;

		// Token: 0x04002AB8 RID: 10936
		[Subcomponent(typeof(TargetedOperationInfo))]
		[SerializeField]
		private TargetedOperationInfo.Subcomponents _targetOperationInfo;

		// Token: 0x020009AE RID: 2478
		public enum DamageAmountType
		{
			// Token: 0x04002ABA RID: 10938
			Constant,
			// Token: 0x04002ABB RID: 10939
			PercentOfOriginalDamage
		}

		// Token: 0x020009AF RID: 2479
		public class Instance : AbilityInstance<AdditionalHitByTargetStatus>
		{
			// Token: 0x06003509 RID: 13577 RVA: 0x0009CFF1 File Offset: 0x0009B1F1
			internal Instance(Character owner, AdditionalHitByTargetStatus ability) : base(owner, ability)
			{
			}

			// Token: 0x0600350A RID: 13578 RVA: 0x0009CFFC File Offset: 0x0009B1FC
			protected override void OnAttach()
			{
				this._remainCount = this.ability._applyCount;
				this.owner.onGiveDamage.Add(int.MaxValue, new GiveDamageDelegate(this.HandleOnGiveDamage));
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
			}

			// Token: 0x0600350B RID: 13579 RVA: 0x0009D064 File Offset: 0x0009B264
			private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
			{
				this._passPrecondition = false;
				if (target.character.status == null || !target.character.status.IsApplying(this.ability._targetStatusFilter))
				{
					return false;
				}
				this._passPrecondition = true;
				return false;
			}

			// Token: 0x0600350C RID: 13580 RVA: 0x0009D0B4 File Offset: 0x0009B2B4
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.HandleOnGiveDamage));
			}

			// Token: 0x0600350D RID: 13581 RVA: 0x0009D108 File Offset: 0x0009B308
			private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				if (!this._passPrecondition)
				{
					return;
				}
				if (this._remainCooldownTime > 0f)
				{
					return;
				}
				if (target.character.health.dead)
				{
					return;
				}
				if (!target.transform.gameObject.activeSelf)
				{
					return;
				}
				if (this.ability._needCritical && !tookDamage.critical)
				{
					return;
				}
				if (!this.ability._attackTypes[tookDamage.motionType])
				{
					return;
				}
				if (!this.ability._damageTypes[tookDamage.attackType])
				{
					return;
				}
				Damage damage = tookDamage;
				if (damage.amount < (double)this.ability._minDamage)
				{
					return;
				}
				if (this.ability._targetPoint != null)
				{
					this.ability._targetPoint.position = target.transform.position;
				}
				double baseDamage;
				if (this.ability._damageAmountType == AdditionalHitByTargetStatus.DamageAmountType.Constant)
				{
					baseDamage = (double)this.ability._additionalDamageAmount;
				}
				else
				{
					double num = (double)this.ability._additionalDamageAmount * 0.01;
					damage = originalDamage;
					baseDamage = num * damage.amount;
				}
				Damage damage2 = this.owner.stat.GetDamage(baseDamage, MMMaths.RandomPointWithinBounds(target.collider.bounds), this.ability._additionalHit);
				this.owner.Attack(target, ref damage2);
				this.ability._hitEffect.Spawn(this.owner, target.collider.bounds, damage2, target);
				this._remainCooldownTime = this.ability._cooldownTime;
				this.owner.StartCoroutine(this.ability._targetOperationInfo.CRun(this.owner, target.character));
				this._remainCount--;
				if (this._remainCount == 0)
				{
					this.owner.ability.Remove(this);
				}
			}

			// Token: 0x04002ABC RID: 10940
			private float _remainCooldownTime;

			// Token: 0x04002ABD RID: 10941
			private int _remainCount;

			// Token: 0x04002ABE RID: 10942
			private bool _passPrecondition;
		}
	}
}
