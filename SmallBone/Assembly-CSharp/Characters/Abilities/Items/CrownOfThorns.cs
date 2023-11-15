using System;
using System.Collections.Generic;
using Characters.Operations;
using FX;
using FX.BoundsAttackVisualEffect;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CAF RID: 3247
	[Serializable]
	public sealed class CrownOfThorns : Ability
	{
		// Token: 0x060041F4 RID: 16884 RVA: 0x000BFF89 File Offset: 0x000BE189
		public override void Initialize()
		{
			base.Initialize();
			this._onHit.Initialize();
		}

		// Token: 0x060041F5 RID: 16885 RVA: 0x000BFF9C File Offset: 0x000BE19C
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new CrownOfThorns.Instance(owner, this);
		}

		// Token: 0x04003284 RID: 12932
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onHit;

		// Token: 0x04003285 RID: 12933
		[Header("강화")]
		[SerializeField]
		private float _range;

		// Token: 0x04003286 RID: 12934
		[SerializeField]
		private bool _enhanced;

		// Token: 0x04003287 RID: 12935
		[Header("연타 스택")]
		[SerializeField]
		private int _maxStack;

		// Token: 0x04003288 RID: 12936
		[SerializeField]
		private float _mutiplierPerStack;

		// Token: 0x04003289 RID: 12937
		[SerializeField]
		private float _refreshCooldownTime;

		// Token: 0x0400328A RID: 12938
		[Header("추가 데미지")]
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x0400328B RID: 12939
		[SerializeField]
		private float _additionalDamageAmount;

		// Token: 0x0400328C RID: 12940
		[SerializeField]
		private HitInfo _additionalHit = new HitInfo(Damage.AttackType.Additional);

		// Token: 0x0400328D RID: 12941
		[SerializeField]
		private bool _needCritical;

		// Token: 0x0400328E RID: 12942
		[SerializeField]
		private Transform _targetPoint;

		// Token: 0x0400328F RID: 12943
		[SerializeField]
		private MotionTypeBoolArray _attackTypes;

		// Token: 0x04003290 RID: 12944
		[SerializeField]
		private AttackTypeBoolArray _damageTypes;

		// Token: 0x04003291 RID: 12945
		[SerializeField]
		[BoundsAttackVisualEffect.SubcomponentAttribute]
		private BoundsAttackVisualEffect.Subcomponents _hitEffect;

		// Token: 0x04003292 RID: 12946
		[SerializeField]
		private SoundInfo _attackSoundInfo;

		// Token: 0x02000CB0 RID: 3248
		public sealed class Instance : AbilityInstance<CrownOfThorns>
		{
			// Token: 0x17000DCF RID: 3535
			// (get) Token: 0x060041F7 RID: 16887 RVA: 0x000BFFB9 File Offset: 0x000BE1B9
			public override Sprite icon
			{
				get
				{
					if (this._remainRefreshTime <= 0f)
					{
						return null;
					}
					return base.icon;
				}
			}

			// Token: 0x17000DD0 RID: 3536
			// (get) Token: 0x060041F8 RID: 16888 RVA: 0x000BFFD0 File Offset: 0x000BE1D0
			public override int iconStacks
			{
				get
				{
					return this._stack;
				}
			}

			// Token: 0x17000DD1 RID: 3537
			// (get) Token: 0x060041F9 RID: 16889 RVA: 0x000BFFD8 File Offset: 0x000BE1D8
			public override float iconFillAmount
			{
				get
				{
					return this._remainRefreshTime / this.ability._refreshCooldownTime;
				}
			}

			// Token: 0x060041FA RID: 16890 RVA: 0x000BFFEC File Offset: 0x000BE1EC
			internal Instance(Character owner, CrownOfThorns ability) : base(owner, ability)
			{
				this._targets = new List<Target>(128);
			}

			// Token: 0x060041FB RID: 16891 RVA: 0x000C0011 File Offset: 0x000BE211
			protected override void OnAttach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
				this._stack = 0;
			}

			// Token: 0x060041FC RID: 16892 RVA: 0x000C0041 File Offset: 0x000BE241
			public override void UpdateTime(float deltaTime)
			{
				this._remainRefreshTime -= deltaTime;
				this._remainCooldownTime -= deltaTime;
				if (this._remainRefreshTime <= 0f)
				{
					this._stack = 0;
					this.UpdateStack();
				}
			}

			// Token: 0x060041FD RID: 16893 RVA: 0x000C0079 File Offset: 0x000BE279
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
			}

			// Token: 0x060041FE RID: 16894 RVA: 0x000C00A4 File Offset: 0x000BE2A4
			private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				if (this._remainCooldownTime > 0f || target.character.health.dead || !target.transform.gameObject.activeSelf || (this.ability._needCritical && !tookDamage.critical) || !this.ability._attackTypes[tookDamage.motionType] || !this.ability._damageTypes[tookDamage.attackType])
				{
					return;
				}
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._attackSoundInfo, this.owner.transform.position);
				if (this.ability._enhanced)
				{
					TargetFinder.FindTargetInRange(target.transform.position, this.ability._range, 1024, this._targets);
					this.ability._targetPoint.position = target.collider.bounds.center + MMMaths.Vector2ToVector3(UnityEngine.Random.insideUnitCircle * 0.3f);
					using (List<Target>.Enumerator enumerator = this._targets.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Target target2 = enumerator.Current;
							if (this.owner == null)
							{
								return;
							}
							if (!(target2.character == null) && !target2.character.health.dead && !(target2.character == this.owner))
							{
								float num = this.ability._additionalDamageAmount * this._multiplier;
								Damage damage = this.owner.stat.GetDamage((double)num, MMMaths.RandomPointWithinBounds(target2.collider.bounds), this.ability._additionalHit);
								this.owner.Attack(target2, ref damage);
								CoroutineProxy.instance.StartCoroutine(this.ability._onHit.CRun(target2.character));
							}
						}
						goto IL_2A5;
					}
				}
				float num2 = this.ability._additionalDamageAmount * this._multiplier;
				Damage damage2 = this.owner.stat.GetDamage((double)num2, MMMaths.RandomPointWithinBounds(target.collider.bounds), this.ability._additionalHit);
				this.owner.Attack(target, ref damage2);
				this.ability._hitEffect.Spawn(this.owner, target.collider.bounds, damage2, target);
				CoroutineProxy.instance.StartCoroutine(this.ability._onHit.CRun(target.character));
				IL_2A5:
				if (this._stack < this.ability._maxStack)
				{
					this._stack++;
					this.UpdateStack();
				}
				this._remainCooldownTime = this.ability._cooldownTime;
				this._remainRefreshTime = this.ability._refreshCooldownTime;
			}

			// Token: 0x060041FF RID: 16895 RVA: 0x000C03B0 File Offset: 0x000BE5B0
			private void UpdateStack()
			{
				this._multiplier = 1f + this.ability._mutiplierPerStack * (float)this._stack;
			}

			// Token: 0x04003293 RID: 12947
			private float _remainRefreshTime;

			// Token: 0x04003294 RID: 12948
			private float _remainCooldownTime;

			// Token: 0x04003295 RID: 12949
			private float _multiplier = 1f;

			// Token: 0x04003296 RID: 12950
			private int _stack;

			// Token: 0x04003297 RID: 12951
			private List<Target> _targets;
		}
	}
}
