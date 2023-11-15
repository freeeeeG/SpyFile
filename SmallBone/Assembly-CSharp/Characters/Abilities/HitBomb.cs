using System;
using Characters.Operations;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Abilities
{
	// Token: 0x02000A48 RID: 2632
	[Serializable]
	public sealed class HitBomb : Ability
	{
		// Token: 0x0600373D RID: 14141 RVA: 0x000A2F9B File Offset: 0x000A119B
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new HitBomb.Instance(owner, this);
		}

		// Token: 0x04002C01 RID: 11265
		[Header("받는 공격 설정")]
		[SerializeField]
		private CharacterTypeBoolArray _attackerFilter;

		// Token: 0x04002C02 RID: 11266
		[SerializeField]
		private MotionTypeBoolArray _motionTypeFilter;

		// Token: 0x04002C03 RID: 11267
		[SerializeField]
		private AttackTypeBoolArray _attackTypeFilter;

		// Token: 0x04002C04 RID: 11268
		[Header("터지는 공격 설정")]
		[SerializeField]
		private bool _bombOnDetached;

		// Token: 0x04002C05 RID: 11269
		[SerializeField]
		private float _hitDamageMultiplier;

		// Token: 0x04002C06 RID: 11270
		[SerializeField]
		private float _maxDamage;

		// Token: 0x04002C07 RID: 11271
		[SerializeField]
		private HitInfo _bombHitInfo;

		// Token: 0x04002C08 RID: 11272
		[SerializeField]
		private PositionInfo _positionInfo;

		// Token: 0x04002C09 RID: 11273
		[SerializeField]
		private Transform _targetPoint;

		// Token: 0x04002C0A RID: 11274
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onBomb;

		// Token: 0x02000A49 RID: 2633
		public sealed class Instance : AbilityInstance<HitBomb>
		{
			// Token: 0x0600373E RID: 14142 RVA: 0x000A2FA4 File Offset: 0x000A11A4
			public Instance(Character owner, HitBomb ability) : base(owner, ability)
			{
			}

			// Token: 0x0600373F RID: 14143 RVA: 0x000A2FAE File Offset: 0x000A11AE
			protected override void OnAttach()
			{
				this.owner.health.onTookDamage += new TookDamageDelegate(this.HandleOnTookDamage);
				this._startTime = Time.time;
			}

			// Token: 0x06003740 RID: 14144 RVA: 0x000A2FD8 File Offset: 0x000A11D8
			private void HandleOnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				if (tookDamage.attacker.character == null)
				{
					return;
				}
				if (!this.ability._attackerFilter[tookDamage.attacker.character.type])
				{
					return;
				}
				if (!this.ability._motionTypeFilter[tookDamage.motionType])
				{
					return;
				}
				if (!this.ability._attackTypeFilter[tookDamage.attackType])
				{
					return;
				}
				this._baseDamage += damageDealt;
			}

			// Token: 0x06003741 RID: 14145 RVA: 0x000A305C File Offset: 0x000A125C
			private void Bomb()
			{
				float num = Mathf.Clamp((float)this._baseDamage * this.ability._hitDamageMultiplier, 1f, this.ability._maxDamage);
				Damage damage = this.owner.stat.GetDamage((double)num, MMMaths.RandomPointWithinBounds(this.owner.collider.bounds), this.ability._bombHitInfo);
				Character player = Singleton<Service>.Instance.levelManager.player;
				player.Attack(this.owner, ref damage);
				this.ability._positionInfo.Attach(this.owner, this.ability._targetPoint);
				player.StartCoroutine(this.ability._onBomb.CRun(player));
			}

			// Token: 0x06003742 RID: 14146 RVA: 0x000A311C File Offset: 0x000A131C
			protected override void OnDetach()
			{
				this.owner.health.onTookDamage -= new TookDamageDelegate(this.HandleOnTookDamage);
				float num = Time.time - this._startTime;
				if (!this.ability._bombOnDetached && num < this.ability.duration)
				{
					return;
				}
				this.Bomb();
			}

			// Token: 0x04002C0B RID: 11275
			private double _baseDamage;

			// Token: 0x04002C0C RID: 11276
			private float _startTime;
		}
	}
}
