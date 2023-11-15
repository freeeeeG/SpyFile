using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Operations;
using FX;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000AE0 RID: 2784
	[Serializable]
	public sealed class KingSlayer : Ability
	{
		// Token: 0x060038FB RID: 14587 RVA: 0x000A7DA4 File Offset: 0x000A5FA4
		public override void Initialize()
		{
			base.Initialize();
			this._kingMark.Initialize();
		}

		// Token: 0x060038FC RID: 14588 RVA: 0x000A7DB7 File Offset: 0x000A5FB7
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new KingSlayer.Instance(owner, this);
		}

		// Token: 0x04002D3C RID: 11580
		[SerializeField]
		private CharacterTypeBoolArray _targetFilter = new CharacterTypeBoolArray(new bool[]
		{
			true,
			true,
			true,
			true,
			true
		});

		// Token: 0x04002D3D RID: 11581
		[SerializeField]
		private MotionTypeBoolArray _motionTypeFilter;

		// Token: 0x04002D3E RID: 11582
		[SerializeField]
		private AttackTypeBoolArray _attackTypeFilter;

		// Token: 0x04002D3F RID: 11583
		[SerializeField]
		private string _attackKey;

		// Token: 0x04002D40 RID: 11584
		[SerializeField]
		private KingSlayer.Instance.KingMark _kingMark;

		// Token: 0x02000AE1 RID: 2785
		public sealed class Instance : AbilityInstance<KingSlayer>
		{
			// Token: 0x060038FE RID: 14590 RVA: 0x000A7DE4 File Offset: 0x000A5FE4
			public Instance(Character owner, KingSlayer ability) : base(owner, ability)
			{
			}

			// Token: 0x060038FF RID: 14591 RVA: 0x000A7DF0 File Offset: 0x000A5FF0
			protected override void OnAttach()
			{
				this._targets = new HashSet<Character>();
				Singleton<Service>.Instance.levelManager.onMapLoaded += this.HandleOnMapLoaded;
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x06003900 RID: 14592 RVA: 0x000A7E4A File Offset: 0x000A604A
			private void HandleOnMapLoaded()
			{
				this._targets.Clear();
			}

			// Token: 0x06003901 RID: 14593 RVA: 0x000A7E58 File Offset: 0x000A6058
			protected override void OnDetach()
			{
				if (Service.quitting)
				{
					return;
				}
				Singleton<Service>.Instance.levelManager.onMapLoaded -= this.HandleOnMapLoaded;
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x06003902 RID: 14594 RVA: 0x000A7EB0 File Offset: 0x000A60B0
			private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				if (target.character == null || target.character.health.dead)
				{
					return;
				}
				if (!string.IsNullOrWhiteSpace(this.ability._attackKey) && !gaveDamage.key.Equals(this.ability._attackKey, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
				if (!this.ability._targetFilter[target.character.type] || !this.ability._motionTypeFilter[gaveDamage.motionType] || !this.ability._attackTypeFilter[gaveDamage.attackType])
				{
					return;
				}
				if (this._targets.Contains(target.character))
				{
					return;
				}
				(target.character.ability.Add(this.ability._kingMark) as KingSlayer.Instance.KingMark.Instance).ability.SetAttacker(this.owner);
				this._targets.Add(target.character);
			}

			// Token: 0x04002D41 RID: 11585
			private HashSet<Character> _targets;

			// Token: 0x02000AE2 RID: 2786
			[Serializable]
			public sealed class KingMark : Ability
			{
				// Token: 0x06003903 RID: 14595 RVA: 0x000A7FAE File Offset: 0x000A61AE
				public void SetAttacker(Character attacker)
				{
					this._attacker = attacker;
				}

				// Token: 0x06003904 RID: 14596 RVA: 0x000A7FB7 File Offset: 0x000A61B7
				public override void Initialize()
				{
					base.Initialize();
					this._onKilled.Initialize();
				}

				// Token: 0x06003905 RID: 14597 RVA: 0x000A7FCA File Offset: 0x000A61CA
				public override IAbilityInstance CreateInstance(Character owner)
				{
					return new KingSlayer.Instance.KingMark.Instance(owner, this);
				}

				// Token: 0x04002D42 RID: 11586
				[SerializeField]
				private KingSlayerComponent _kinSlayer;

				// Token: 0x04002D43 RID: 11587
				[Header("처형 연출")]
				[SerializeField]
				private EffectInfo _markEffect = new EffectInfo
				{
					subordinated = true
				};

				// Token: 0x04002D44 RID: 11588
				[SerializeField]
				private SoundInfo _markSound;

				// Token: 0x04002D45 RID: 11589
				[SerializeField]
				private EffectInfo _killEffect;

				// Token: 0x04002D46 RID: 11590
				[SerializeField]
				[Subcomponent(typeof(OperationInfo))]
				private OperationInfo.Subcomponents _onKilled;

				// Token: 0x04002D47 RID: 11591
				[SerializeField]
				private float _timeScaleDuringKilling = 0.3f;

				// Token: 0x04002D48 RID: 11592
				private Character _attacker;

				// Token: 0x02000AE3 RID: 2787
				public sealed class Instance : AbilityInstance<KingSlayer.Instance.KingMark>
				{
					// Token: 0x06003907 RID: 14599 RVA: 0x000A7FF8 File Offset: 0x000A61F8
					public Instance(Character owner, KingSlayer.Instance.KingMark ability) : base(owner, ability)
					{
						this._target = owner.GetComponentInChildren<ITarget>();
						owner.health.onTookDamage += new TookDamageDelegate(this.HandleOnTookDamage);
					}

					// Token: 0x06003908 RID: 14600 RVA: 0x000A8028 File Offset: 0x000A6228
					private void HandleOnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
					{
						if (this.ability._attacker == null)
						{
							return;
						}
						if (this._killing)
						{
							return;
						}
						if (this.owner.health.percent * 100.0 <= (double)this.ability._kinSlayer.triggerPercent)
						{
							this.owner.health.onTookDamage -= new TookDamageDelegate(this.HandleOnTookDamage);
							this._killing = true;
							this.Invoke();
						}
					}

					// Token: 0x06003909 RID: 14601 RVA: 0x000A80A8 File Offset: 0x000A62A8
					private void Invoke()
					{
						if (this.ability._attacker == null)
						{
							return;
						}
						this.ability._attacker.StartCoroutine(this.CDelayedKill());
					}

					// Token: 0x0600390A RID: 14602 RVA: 0x000A80D5 File Offset: 0x000A62D5
					private IEnumerator CDelayedKill()
					{
						Vector3 center = this.owner.collider.bounds.center;
						this._markEffect = this.ability._markEffect.Spawn(center, this.owner, 0f, 1f);
						this.owner.chronometer.animation.AttachTimeScale(this, this.ability._timeScaleDuringKilling);
						this.ability._killEffect.Spawn(this.owner.collider.bounds.center, this.owner, 0f, 1f);
						PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._markSound, this.owner.gameObject.transform.position);
						yield return this.owner.chronometer.master.WaitForSeconds(1f);
						if (this._markEffect != null)
						{
							this._markEffect.Stop();
						}
						this._markEffect = null;
						if (this.owner == null || !this.owner.gameObject.activeSelf)
						{
							yield break;
						}
						if (this.owner.health.dead)
						{
							this.owner.chronometer.animation.DetachTimeScale(this);
							yield break;
						}
						this.owner.chronometer.animation.DetachTimeScale(this);
						Damage damage = new Damage(this.ability._attacker, this.owner.health.currentHealth, this.owner.collider.bounds.center, Damage.Attribute.Fixed, Damage.AttackType.Additional, Damage.MotionType.DarkAbility, 1.0, 0f, 0.0, 1.0, 1.0, true, false, 0.0, 0.0, 0, null, 1.0);
						this.ability._attacker.StartCoroutine(this.ability._onKilled.CRun(this.ability._attacker));
						this.ability._attacker.TryKillTarget(this._target, ref damage);
						if (this.owner == null || this.owner.health.dead || !this.owner.gameObject.activeSelf)
						{
							yield break;
						}
						this._killing = false;
						this.owner.ability.Remove(this);
						yield break;
					}

					// Token: 0x0600390B RID: 14603 RVA: 0x00002191 File Offset: 0x00000391
					protected override void OnAttach()
					{
					}

					// Token: 0x0600390C RID: 14604 RVA: 0x000A80E4 File Offset: 0x000A62E4
					protected override void OnDetach()
					{
						this.owner.health.onTookDamage -= new TookDamageDelegate(this.HandleOnTookDamage);
						if (this._markEffect != null)
						{
							this._markEffect.Stop();
						}
						this._markEffect = null;
					}

					// Token: 0x04002D49 RID: 11593
					private ITarget _target;

					// Token: 0x04002D4A RID: 11594
					private EffectPoolInstance _markEffect;

					// Token: 0x04002D4B RID: 11595
					private bool _killing;
				}
			}
		}
	}
}
