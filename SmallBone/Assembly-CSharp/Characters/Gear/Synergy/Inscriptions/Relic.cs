using System;
using Characters.Abilities;
using Characters.Operations;
using Characters.Utils;
using Services;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x020008AA RID: 2218
	public sealed class Relic : InscriptionInstance
	{
		// Token: 0x06002F26 RID: 12070 RVA: 0x00002191 File Offset: 0x00000391
		protected override void Initialize()
		{
		}

		// Token: 0x06002F27 RID: 12071 RVA: 0x00002191 File Offset: 0x00000391
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
		}

		// Token: 0x06002F28 RID: 12072 RVA: 0x0008D7C8 File Offset: 0x0008B9C8
		public override void Attach()
		{
			base.character.health.onHealByGiver += this.HandleOnHealedByPotion;
			base.character.health.onHealed += this.OnHealed;
		}

		// Token: 0x06002F29 RID: 12073 RVA: 0x0008D804 File Offset: 0x0008BA04
		public override void Detach()
		{
			if (Service.quitting)
			{
				return;
			}
			base.character.health.onHealByGiver -= this.HandleOnHealedByPotion;
			base.character.health.onHealed -= this.OnHealed;
			base.character.ability.Remove(this._ability);
		}

		// Token: 0x06002F2A RID: 12074 RVA: 0x0008D868 File Offset: 0x0008BA68
		private void OnHealed(double healed, double overHealed)
		{
			if (healed < 1.0)
			{
				return;
			}
			if (this.keyword.isMaxStep)
			{
				base.character.ability.Add(this._ability);
			}
		}

		// Token: 0x06002F2B RID: 12075 RVA: 0x0008D89B File Offset: 0x0008BA9B
		private void HandleOnHealedByPotion(ref Health.HealInfo info)
		{
			if (info.healthGiver != Health.HealthGiverType.Potion)
			{
				return;
			}
			info.amount += this._extraHealAmount;
		}

		// Token: 0x040026FE RID: 9982
		[SerializeField]
		[Header("2세트 효과")]
		private double _extraHealAmount;

		// Token: 0x040026FF RID: 9983
		[SerializeField]
		[Header("4세트 효과")]
		private Relic.RelicAbility _ability;

		// Token: 0x020008AB RID: 2219
		[Serializable]
		public sealed class RelicAbility : Ability
		{
			// Token: 0x06002F2D RID: 12077 RVA: 0x0008D8B7 File Offset: 0x0008BAB7
			public override void Initialize()
			{
				base.Initialize();
				this._onTarget.Initialize();
			}

			// Token: 0x06002F2E RID: 12078 RVA: 0x0008D8CA File Offset: 0x0008BACA
			public override IAbilityInstance CreateInstance(Character owner)
			{
				return new Relic.RelicAbility.Instance(owner, this);
			}

			// Token: 0x04002700 RID: 9984
			[SerializeField]
			[Header("공격 설정")]
			private HitInfo _hitInfo;

			// Token: 0x04002701 RID: 9985
			[SerializeField]
			private PositionInfo _targetPositionInfo;

			// Token: 0x04002702 RID: 9986
			[SerializeField]
			private Transform _targetPosition;

			// Token: 0x04002703 RID: 9987
			[SerializeField]
			[MinMaxSlider(0f, 9999999f)]
			private Vector2 _damageRange;

			// Token: 0x04002704 RID: 9988
			[SerializeField]
			private int _healthPerStack;

			// Token: 0x04002705 RID: 9989
			[SerializeField]
			private int _maxStack;

			// Token: 0x04002706 RID: 9990
			[Range(0f, 100f)]
			[SerializeField]
			private float _damagePercentPerStack;

			// Token: 0x04002707 RID: 9991
			[SerializeField]
			private float _damageMultiplierPerStackToNormal;

			// Token: 0x04002708 RID: 9992
			[SerializeField]
			private float _damageMultiplierPerStackToBoss;

			// Token: 0x04002709 RID: 9993
			[Subcomponent(typeof(OperationInfo))]
			[SerializeField]
			private OperationInfo.Subcomponents _onTarget;

			// Token: 0x0400270A RID: 9994
			[Header("공격 대상 쿨타임 설정")]
			[SerializeField]
			private int _cooldownTime;

			// Token: 0x0400270B RID: 9995
			[SerializeField]
			private int _maxHitDuringCooldown;

			// Token: 0x020008AC RID: 2220
			private sealed class Instance : AbilityInstance<Relic.RelicAbility>
			{
				// Token: 0x06002F30 RID: 12080 RVA: 0x0008D8D3 File Offset: 0x0008BAD3
				public Instance(Character owner, Relic.RelicAbility ability) : base(owner, ability)
				{
					this._historyManager = new StackHistoryManager<Character>(128);
				}

				// Token: 0x06002F31 RID: 12081 RVA: 0x0008D8ED File Offset: 0x0008BAED
				protected override void OnAttach()
				{
					Character owner = this.owner;
					owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
				}

				// Token: 0x06002F32 RID: 12082 RVA: 0x0008D916 File Offset: 0x0008BB16
				protected override void OnDetach()
				{
					this._historyManager.ClearIfExpired();
					Character owner = this.owner;
					owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
				}

				// Token: 0x06002F33 RID: 12083 RVA: 0x0008D94C File Offset: 0x0008BB4C
				private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
				{
					if (gaveDamage.attackType == Damage.AttackType.Additional)
					{
						return;
					}
					Character character = target.character;
					this._historyManager.ClearIfExpired();
					if (!this._historyManager.TryAddStack(character, 1, this.ability._maxHitDuringCooldown, (float)this.ability._cooldownTime))
					{
						return;
					}
					double num = (double)Mathf.Min(this.ability._maxStack, (int)(this.owner.health.maximumHealth / (double)this.ability._healthPerStack));
					double num2 = character.health.currentHealth * (double)this.ability._damagePercentPerStack * 0.009999999776482582;
					double num3 = num * num2;
					if (character.type == Character.Type.Boss || character.type == Character.Type.Adventurer)
					{
						num3 *= (double)this.ability._damageMultiplierPerStackToBoss;
					}
					else
					{
						num3 *= (double)this.ability._damageMultiplierPerStackToNormal;
					}
					Damage damage = this.owner.stat.GetDamage((double)Mathf.Clamp((float)num3, this.ability._damageRange.x, this.ability._damageRange.y), MMMaths.RandomPointWithinBounds(target.collider.bounds), this.ability._hitInfo);
					this.owner.TryAttackCharacter(target, ref damage);
					this.ability._targetPositionInfo.Attach(target.character, this.ability._targetPosition);
					this.owner.StartCoroutine(this.ability._onTarget.CRun(this.owner));
				}

				// Token: 0x0400270C RID: 9996
				private StackHistoryManager<Character> _historyManager;
			}
		}
	}
}
