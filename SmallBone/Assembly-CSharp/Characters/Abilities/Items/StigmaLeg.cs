using System;
using System.Collections;
using Characters.Actions;
using Characters.Operations;
using FX;
using GameResources;
using PhysicsUtils;
using Singletons;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CF7 RID: 3319
	[Serializable]
	public sealed class StigmaLeg : Ability
	{
		// Token: 0x0600430D RID: 17165 RVA: 0x000C37C4 File Offset: 0x000C19C4
		public override void Initialize()
		{
			base.Initialize();
			this._debuff.Initialize();
		}

		// Token: 0x0600430E RID: 17166 RVA: 0x000C37D7 File Offset: 0x000C19D7
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this._debuff.attacker = owner;
			return new StigmaLeg.Instance(owner, this);
		}

		// Token: 0x0400334C RID: 13132
		[SerializeField]
		private StigmaLeg.Debuff _debuff;

		// Token: 0x02000CF8 RID: 3320
		[Serializable]
		public sealed class Debuff : Ability
		{
			// Token: 0x17000DEF RID: 3567
			// (get) Token: 0x06004310 RID: 17168 RVA: 0x000C37EC File Offset: 0x000C19EC
			// (set) Token: 0x06004311 RID: 17169 RVA: 0x000C37F4 File Offset: 0x000C19F4
			public Character attacker { get; set; }

			// Token: 0x06004312 RID: 17170 RVA: 0x000C37FD File Offset: 0x000C19FD
			public override void Initialize()
			{
				base.Initialize();
				this._operations.Initialize();
			}

			// Token: 0x06004313 RID: 17171 RVA: 0x000C3810 File Offset: 0x000C1A10
			public override IAbilityInstance CreateInstance(Character owner)
			{
				return new StigmaLeg.Debuff.Instance(owner, this);
			}

			// Token: 0x0400334D RID: 13133
			[SerializeField]
			private float _coodownTime;

			// Token: 0x0400334E RID: 13134
			[SerializeField]
			private CustomFloat _additionalHitDamage;

			// Token: 0x0400334F RID: 13135
			[SerializeField]
			private HitInfo _hitInfo;

			// Token: 0x04003350 RID: 13136
			[SerializeField]
			private SoundInfo _attachSoundInfo;

			// Token: 0x04003351 RID: 13137
			[SerializeField]
			private SoundInfo _attackSoundInfo;

			// Token: 0x04003352 RID: 13138
			[SerializeField]
			private PositionInfo _positionInfo;

			// Token: 0x04003353 RID: 13139
			[SerializeField]
			private Transform _targetPoint;

			// Token: 0x04003354 RID: 13140
			[Subcomponent(typeof(OperationInfo))]
			[SerializeField]
			private OperationInfo.Subcomponents _operations;

			// Token: 0x02000CF9 RID: 3321
			public sealed class Instance : AbilityInstance<StigmaLeg.Debuff>
			{
				// Token: 0x06004315 RID: 17173 RVA: 0x000C3819 File Offset: 0x000C1A19
				public Instance(Character owner, StigmaLeg.Debuff ability) : base(owner, ability)
				{
					this._hitParticle = CommonResource.instance.hitParticle;
				}

				// Token: 0x06004316 RID: 17174 RVA: 0x000C3834 File Offset: 0x000C1A34
				protected override void OnAttach()
				{
					if (this.ability.attacker != null)
					{
						PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._attachSoundInfo, this.owner.transform.position);
						this.ability.attacker.onStartAction += this.HandleOnStartAction;
					}
				}

				// Token: 0x06004317 RID: 17175 RVA: 0x000C3898 File Offset: 0x000C1A98
				private void HandleOnStartAction(Characters.Actions.Action action)
				{
					if (action.type != Characters.Actions.Action.Type.Skill && action.type != Characters.Actions.Action.Type.Dash)
					{
						return;
					}
					if (this._remainCooldownTime > 0f)
					{
						return;
					}
					if (this.owner == null)
					{
						this.ability.attacker.onStartAction -= this.HandleOnStartAction;
						return;
					}
					if (this.ability._targetPoint != null)
					{
						Vector3 center = this.owner.collider.bounds.center;
						Vector3 size = this.owner.collider.bounds.size;
						size.x *= this.ability._positionInfo.pivotValue.x;
						size.y *= this.ability._positionInfo.pivotValue.y;
						Vector3 position = center + size;
						this.ability._targetPoint.position = position;
					}
					Character attacker = this.ability.attacker;
					Damage damage = attacker.stat.GetDamage((double)this.ability._additionalHitDamage.value, MMMaths.RandomPointWithinBounds(this.owner.collider.bounds), this.ability._hitInfo);
					this._remainCooldownTime = this.ability._coodownTime;
					attacker.StartCoroutine(this.ability._operations.CRun(attacker));
					attacker.Attack(this.owner, ref damage);
					PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._attackSoundInfo, this.owner.transform.position);
					this._hitParticle.Emit(this.owner.transform.position, this.owner.collider.bounds, Vector2.zero, true);
				}

				// Token: 0x06004318 RID: 17176 RVA: 0x000C3A6D File Offset: 0x000C1C6D
				protected override void OnDetach()
				{
					if (this.ability.attacker != null)
					{
						this.ability.attacker.onStartAction -= this.HandleOnStartAction;
					}
				}

				// Token: 0x06004319 RID: 17177 RVA: 0x000C3A9E File Offset: 0x000C1C9E
				public override void UpdateTime(float deltaTime)
				{
					base.UpdateTime(deltaTime);
					this._remainCooldownTime -= deltaTime;
				}

				// Token: 0x04003356 RID: 13142
				private ParticleEffectInfo _hitParticle;

				// Token: 0x04003357 RID: 13143
				private float _remainCooldownTime;
			}
		}

		// Token: 0x02000CFA RID: 3322
		public sealed class Instance : AbilityInstance<StigmaLeg>
		{
			// Token: 0x0600431A RID: 17178 RVA: 0x000C3AB5 File Offset: 0x000C1CB5
			public Instance(Character owner, StigmaLeg ability) : base(owner, ability)
			{
				this._caster = new NonAllocCaster(12);
			}

			// Token: 0x0600431B RID: 17179 RVA: 0x000C3ACC File Offset: 0x000C1CCC
			protected override void OnAttach()
			{
				this.owner.onStartAction += this.OnStartAction;
			}

			// Token: 0x0600431C RID: 17180 RVA: 0x000C3AE5 File Offset: 0x000C1CE5
			private void OnStartAction(Characters.Actions.Action action)
			{
				if (action.type != Characters.Actions.Action.Type.Dash)
				{
					return;
				}
				this.owner.StartCoroutine(this.CCheckTargetCollision(action));
			}

			// Token: 0x0600431D RID: 17181 RVA: 0x000C3B03 File Offset: 0x000C1D03
			private IEnumerator CCheckTargetCollision(Characters.Actions.Action dash)
			{
				Chronometer animationChronometer = this.owner.chronometer.animation;
				while (dash.running)
				{
					if (this._stigmaTarget != null && !this._stigmaTarget.health.dead)
					{
						yield return null;
					}
					else
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
				}
				yield break;
			}

			// Token: 0x0600431E RID: 17182 RVA: 0x000C3B1C File Offset: 0x000C1D1C
			private void Detect(Vector2 direction, float distance)
			{
				this._caster.contactFilter.SetLayerMask(1024);
				this._caster.ColliderCast(this.owner.collider, direction, distance);
				int i = 0;
				while (i < this._caster.results.Count)
				{
					RaycastHit2D raycastHit2D = this._caster.results[i];
					Target component = raycastHit2D.collider.GetComponent<Target>();
					if (component == null)
					{
						Debug.LogError(raycastHit2D.collider.name + " : Character has no Target component");
						i++;
					}
					else
					{
						if (component.character == null)
						{
							return;
						}
						component.character.ability.Add(this.ability._debuff);
						this._stigmaTarget = component.character;
						return;
					}
				}
			}

			// Token: 0x0600431F RID: 17183 RVA: 0x000C3BFA File Offset: 0x000C1DFA
			protected override void OnDetach()
			{
				this.owner.onStartAction -= this.OnStartAction;
				this.owner.StopCoroutine("CCheckTargetCollision");
			}

			// Token: 0x04003358 RID: 13144
			private readonly NonAllocCaster _caster;

			// Token: 0x04003359 RID: 13145
			private Character _stigmaTarget;
		}
	}
}
