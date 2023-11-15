using System;
using System.Collections;
using Characters.Actions;
using Characters.Operations;
using FX;
using GameResources;
using PhysicsUtils;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CD5 RID: 3285
	[Serializable]
	public sealed class ManeOfBeastKing : Ability
	{
		// Token: 0x06004287 RID: 17031 RVA: 0x000C1AB8 File Offset: 0x000BFCB8
		public override void Initialize()
		{
			base.Initialize();
			this._debuff.Initialize();
		}

		// Token: 0x06004288 RID: 17032 RVA: 0x000C1ACB File Offset: 0x000BFCCB
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ManeOfBeastKing.Instance(owner, this);
		}

		// Token: 0x040032EA RID: 13034
		[SerializeField]
		private ManeOfBeastKing.Debuff _debuff;

		// Token: 0x02000CD6 RID: 3286
		[Serializable]
		public sealed class Debuff : Ability
		{
			// Token: 0x0600428A RID: 17034 RVA: 0x000C1AD4 File Offset: 0x000BFCD4
			public override IAbilityInstance CreateInstance(Character owner)
			{
				return new ManeOfBeastKing.Debuff.Instance(owner, this);
			}

			// Token: 0x040032EB RID: 13035
			[SerializeField]
			private CustomFloat _additionalHitDamage;

			// Token: 0x040032EC RID: 13036
			[SerializeField]
			private HitInfo _additionalHitInfo;

			// Token: 0x040032ED RID: 13037
			[SerializeField]
			private MotionTypeBoolArray _motionType;

			// Token: 0x040032EE RID: 13038
			[SerializeField]
			private AttackTypeBoolArray _attackType;

			// Token: 0x040032EF RID: 13039
			[SerializeField]
			private CharacterTypeBoolArray _attackerType;

			// Token: 0x040032F0 RID: 13040
			[SerializeField]
			private EffectInfo _hitEffect;

			// Token: 0x040032F1 RID: 13041
			[SerializeField]
			private SoundInfo _hitSound;

			// Token: 0x02000CD7 RID: 3287
			public sealed class Instance : AbilityInstance<ManeOfBeastKing.Debuff>
			{
				// Token: 0x0600428C RID: 17036 RVA: 0x000C1ADD File Offset: 0x000BFCDD
				public Instance(Character owner, ManeOfBeastKing.Debuff ability) : base(owner, ability)
				{
					this._hitParticle = CommonResource.instance.hitParticle;
				}

				// Token: 0x0600428D RID: 17037 RVA: 0x000C1AF8 File Offset: 0x000BFCF8
				protected override void OnAttach()
				{
					this._canUse = true;
					this.owner.health.onTakeDamage.Add(int.MinValue, new TakeDamageDelegate(this.OnTakeDamage));
					this.owner.health.onTookDamage += new TookDamageDelegate(this.OnTookDamage);
				}

				// Token: 0x0600428E RID: 17038 RVA: 0x000C1B4E File Offset: 0x000BFD4E
				private bool OnTakeDamage(ref Damage damage)
				{
					if (!damage.canCritical)
					{
						return false;
					}
					if (!this._canUse)
					{
						return false;
					}
					damage.criticalChance = 1.0;
					damage.Evaluate(false);
					this._canUse = false;
					return false;
				}

				// Token: 0x0600428F RID: 17039 RVA: 0x000C1B84 File Offset: 0x000BFD84
				private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
				{
					if (!this.ability._attackType[tookDamage.attackType] || !this.ability._motionType[tookDamage.motionType] || !this.ability._attackerType[tookDamage.attacker.character.type])
					{
						return;
					}
					this.owner.health.onTookDamage -= new TookDamageDelegate(this.OnTookDamage);
					Character character = tookDamage.attacker.character;
					Damage damage = character.stat.GetDamage((double)this.ability._additionalHitDamage.value, MMMaths.RandomPointWithinBounds(this.owner.collider.bounds), this.ability._additionalHitInfo);
					character.Attack(this.owner, ref damage);
					this.ability._hitEffect.Spawn(MMMaths.RandomPointWithinBounds(this.owner.collider.bounds), 0f, 1f);
					PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._hitSound, this.owner.transform.position);
					this._hitParticle.Emit(this.owner.transform.position, this.owner.collider.bounds, Vector2.zero, true);
					this.owner.ability.Remove(this);
				}

				// Token: 0x06004290 RID: 17040 RVA: 0x000C1CF8 File Offset: 0x000BFEF8
				protected override void OnDetach()
				{
					this.owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.OnTakeDamage));
					this.owner.health.onTookDamage -= new TookDamageDelegate(this.OnTookDamage);
				}

				// Token: 0x040032F2 RID: 13042
				private ParticleEffectInfo _hitParticle;

				// Token: 0x040032F3 RID: 13043
				private bool _canUse;
			}
		}

		// Token: 0x02000CD8 RID: 3288
		public sealed class Instance : AbilityInstance<ManeOfBeastKing>
		{
			// Token: 0x06004291 RID: 17041 RVA: 0x000C1D38 File Offset: 0x000BFF38
			public Instance(Character owner, ManeOfBeastKing ability) : base(owner, ability)
			{
				this._caster = new NonAllocCaster(99);
			}

			// Token: 0x06004292 RID: 17042 RVA: 0x000C1D4F File Offset: 0x000BFF4F
			protected override void OnAttach()
			{
				this.owner.onStartAction += this.OnStartAction;
			}

			// Token: 0x06004293 RID: 17043 RVA: 0x000C1D68 File Offset: 0x000BFF68
			private void OnStartAction(Characters.Actions.Action action)
			{
				if (action.type != Characters.Actions.Action.Type.Dash)
				{
					return;
				}
				this.owner.StartCoroutine(this.CCheckTargetCollision(action));
			}

			// Token: 0x06004294 RID: 17044 RVA: 0x000C1D86 File Offset: 0x000BFF86
			private IEnumerator CCheckTargetCollision(Characters.Actions.Action dash)
			{
				Chronometer animationChronometer = this.owner.chronometer.animation;
				while (dash.running)
				{
					if (animationChronometer.timeScale > 1E-45f)
					{
						Vector2 vector = Vector2.zero;
						if (this.owner.movement != null)
						{
							vector = this.owner.movement.moved;
						}
						this.Detect(vector.normalized, vector.magnitude);
					}
					yield return null;
				}
				yield break;
			}

			// Token: 0x06004295 RID: 17045 RVA: 0x000C1D9C File Offset: 0x000BFF9C
			private void Detect(Vector2 direction, float distance)
			{
				this._caster.contactFilter.SetLayerMask(1024);
				this._caster.ColliderCast(this.owner.collider, direction, distance);
				for (int i = 0; i < this._caster.results.Count; i++)
				{
					RaycastHit2D raycastHit2D = this._caster.results[i];
					Target component = raycastHit2D.collider.GetComponent<Target>();
					if (component == null)
					{
						Debug.LogError(raycastHit2D.collider.name + " : Character has no Target component");
					}
					else
					{
						if (component.character == null)
						{
							return;
						}
						component.character.ability.Add(this.ability._debuff);
					}
				}
			}

			// Token: 0x06004296 RID: 17046 RVA: 0x000C1E6A File Offset: 0x000C006A
			protected override void OnDetach()
			{
				this.owner.onStartAction -= this.OnStartAction;
				this.owner.StopCoroutine("CCheckTargetCollision");
			}

			// Token: 0x040032F4 RID: 13044
			private readonly NonAllocCaster _caster;
		}
	}
}
