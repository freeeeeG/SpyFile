using System;
using Characters.Abilities;
using Characters.Operations;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x020008AD RID: 2221
	public class Revenge : InscriptionInstance
	{
		// Token: 0x06002F34 RID: 12084 RVA: 0x00002191 File Offset: 0x00000391
		protected override void Initialize()
		{
		}

		// Token: 0x06002F35 RID: 12085 RVA: 0x00002191 File Offset: 0x00000391
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
		}

		// Token: 0x06002F36 RID: 12086 RVA: 0x0008DACC File Offset: 0x0008BCCC
		public override void Attach()
		{
			base.character.health.onTakeDamage.Add(int.MaxValue, new TakeDamageDelegate(this.HandleOnTakeDamage));
			base.character.health.onTookDamage += new TookDamageDelegate(this.HandleOnTookDamage);
		}

		// Token: 0x06002F37 RID: 12087 RVA: 0x0008DB1B File Offset: 0x0008BD1B
		public override void Detach()
		{
			base.character.health.onTakeDamage.Remove(new TakeDamageDelegate(this.HandleOnTakeDamage));
			base.character.health.onTookDamage -= new TookDamageDelegate(this.HandleOnTookDamage);
		}

		// Token: 0x06002F38 RID: 12088 RVA: 0x0008DB5B File Offset: 0x0008BD5B
		private bool HandleOnTakeDamage(ref Damage damage)
		{
			this._beforeHealth = base.character.health.currentHealth;
			return false;
		}

		// Token: 0x06002F39 RID: 12089 RVA: 0x0008DB74 File Offset: 0x0008BD74
		private void HandleOnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (this.keyword.step < 1)
			{
				return;
			}
			if (Time.time - this._lastTakeDamageTime < this._cooldownTime)
			{
				return;
			}
			if (this._beforeHealth - base.character.health.currentHealth < 1.0)
			{
				return;
			}
			this._lastTakeDamageTime = Time.time;
			if (this.keyword.isMaxStep)
			{
				base.character.ability.Add(this._revengeAbility4);
				return;
			}
			base.character.ability.Add(this._revengeAbility2);
		}

		// Token: 0x0400270D RID: 9997
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x0400270E RID: 9998
		[SerializeField]
		[Header("2세트")]
		private Revenge.RevengeAbility _revengeAbility2;

		// Token: 0x0400270F RID: 9999
		[Header("4세트")]
		[SerializeField]
		private Revenge.RevengeAbility _revengeAbility4;

		// Token: 0x04002710 RID: 10000
		private float _lastTakeDamageTime;

		// Token: 0x04002711 RID: 10001
		private double _beforeHealth;

		// Token: 0x020008AE RID: 2222
		[Serializable]
		public sealed class RevengeAbility : Ability
		{
			// Token: 0x06002F3B RID: 12091 RVA: 0x0008DC0F File Offset: 0x0008BE0F
			public override IAbilityInstance CreateInstance(Character owner)
			{
				return new Revenge.RevengeAbility.Instance(owner, this);
			}

			// Token: 0x04002712 RID: 10002
			[SerializeField]
			private HitInfo _additionalHitInfo;

			// Token: 0x04002713 RID: 10003
			[SerializeField]
			private AttackDamage _attackDamage;

			// Token: 0x04002714 RID: 10004
			[SerializeField]
			private PositionInfo _targetPositionInfo;

			// Token: 0x04002715 RID: 10005
			[SerializeField]
			private Transform _targetPoint;

			// Token: 0x04002716 RID: 10006
			[Range(0f, 1f)]
			[SerializeField]
			private float _recoveryPercent;

			// Token: 0x04002717 RID: 10007
			[SerializeField]
			[Subcomponent(typeof(OperationInfo))]
			private OperationInfo.Subcomponents _onHitOwner;

			// Token: 0x04002718 RID: 10008
			[SerializeField]
			[Subcomponent(typeof(OperationInfo))]
			private OperationInfo.Subcomponents _onHitTarget;

			// Token: 0x04002719 RID: 10009
			[SerializeField]
			private AbilityComponent _revengeReward;

			// Token: 0x020008AF RID: 2223
			public sealed class Instance : AbilityInstance<Revenge.RevengeAbility>
			{
				// Token: 0x06002F3D RID: 12093 RVA: 0x0008DC18 File Offset: 0x0008BE18
				public Instance(Character owner, Revenge.RevengeAbility ability) : base(owner, ability)
				{
				}

				// Token: 0x06002F3E RID: 12094 RVA: 0x0008DC22 File Offset: 0x0008BE22
				protected override void OnAttach()
				{
					Character owner = this.owner;
					owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
				}

				// Token: 0x06002F3F RID: 12095 RVA: 0x0008DC4B File Offset: 0x0008BE4B
				protected override void OnDetach()
				{
					Character owner = this.owner;
					owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
				}

				// Token: 0x06002F40 RID: 12096 RVA: 0x0008DC74 File Offset: 0x0008BE74
				private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
				{
					this.AttackRevengeTarget(target);
				}

				// Token: 0x06002F41 RID: 12097 RVA: 0x0008DC80 File Offset: 0x0008BE80
				private void AttackRevengeTarget(ITarget target)
				{
					Character owner = this.owner;
					owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
					this.ability._targetPositionInfo.Attach(target.character, this.ability._targetPoint);
					this.ability._additionalHitInfo.ChangeAdaptiveDamageAttribute(this.owner);
					Damage damage = this.owner.stat.GetDamage((double)this.ability._attackDamage.amount, MMMaths.RandomPointWithinBounds(target.collider.bounds), this.ability._additionalHitInfo);
					this.owner.StartCoroutine(this.ability._onHitOwner.CRun(this.owner));
					this.owner.StartCoroutine(this.ability._onHitTarget.CRun(target.character));
					this.owner.TryAttackCharacter(target, ref damage);
					this.Recover();
					if (this.ability._revengeReward != null)
					{
						this.owner.ability.Add(this.ability._revengeReward.ability);
					}
					this.owner.ability.Remove(this);
				}

				// Token: 0x06002F42 RID: 12098 RVA: 0x0008DDC8 File Offset: 0x0008BFC8
				private void Recover()
				{
					double num = this.owner.health.lastTakenDamage * (double)this.ability._recoveryPercent;
					this.owner.health.Heal(num, num > 0.0);
				}
			}
		}
	}
}
