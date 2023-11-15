using System;
using System.Collections;
using Characters.Movements;
using Characters.Operations.Movement;
using Characters.Utils;
using FX.BoundsAttackVisualEffect;
using GameResources;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack
{
	// Token: 0x02000F92 RID: 3986
	public sealed class RingAttack : CharacterOperation, IAttack
	{
		// Token: 0x140000BE RID: 190
		// (add) Token: 0x06004D55 RID: 19797 RVA: 0x000E6704 File Offset: 0x000E4904
		// (remove) Token: 0x06004D56 RID: 19798 RVA: 0x000E673C File Offset: 0x000E493C
		public event OnAttackHitDelegate onHit;

		// Token: 0x06004D57 RID: 19799 RVA: 0x000E6774 File Offset: 0x000E4974
		private void Awake()
		{
			Array.Sort<TargetedOperationInfo>(this._operationInfo.components, (TargetedOperationInfo x, TargetedOperationInfo y) => x.timeToTrigger.CompareTo(y.timeToTrigger));
			this._maxHits = Math.Min(this._maxHits, 2048);
			this._overlapper = ((this._maxHits == RingAttack._sharedOverlapper.capacity) ? RingAttack._sharedOverlapper : new NonAllocOverlapper(this._maxHits));
		}

		// Token: 0x06004D58 RID: 19800 RVA: 0x000E67F0 File Offset: 0x000E49F0
		public override void Initialize()
		{
			base.Initialize();
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			this._operationInfo.Initialize();
			this._hits.Clear();
			foreach (TargetedOperationInfo targetedOperationInfo in this._operationInfo.components)
			{
				Knockback knockback = targetedOperationInfo.operation as Knockback;
				if (knockback != null)
				{
					this._pushInfo = knockback.pushInfo;
					return;
				}
				Smash smash = targetedOperationInfo.operation as Smash;
				if (smash != null)
				{
					this._pushInfo = smash.pushInfo;
				}
			}
		}

		// Token: 0x06004D59 RID: 19801 RVA: 0x000E687D File Offset: 0x000E4A7D
		public override void Run(Character owner)
		{
			this._hits.Clear();
			this.coroutineReference = this.StartCoroutineWithReference(this.CDetect(owner));
		}

		// Token: 0x06004D5A RID: 19802 RVA: 0x000E689D File Offset: 0x000E4A9D
		private IEnumerator CDetect(Character owner)
		{
			float elapsed = 0f;
			Chronometer animationChronometer = owner.chronometer.animation;
			while (elapsed < this._duration)
			{
				if (this._timeIndependent || animationChronometer.timeScale > 1E-45f)
				{
					this.Detect(owner);
				}
				yield return null;
				if (this._timeIndependent)
				{
					elapsed += Chronometer.global.deltaTime;
				}
				else
				{
					elapsed += animationChronometer.deltaTime;
				}
			}
			yield break;
		}

		// Token: 0x06004D5B RID: 19803 RVA: 0x000E68B4 File Offset: 0x000E4AB4
		private void Detect(Character owner)
		{
			this._overlapper.contactFilter.SetLayerMask(this._layer.Evaluate(owner.gameObject));
			Bounds bounds = this._outCollider.bounds;
			this._overlapper.OverlapCollider(this._outCollider);
			bool flag = false;
			for (int i = 0; i < this._overlapper.results.Count; i++)
			{
				Target component = this._overlapper.results[i].GetComponent<Target>();
				if (!(component == null) && this._hits.CanAttack(component, this._maxHits, this._maxHitsPerUnit, this._hitIntervalPerUnit))
				{
					float num = Vector2.Distance(this._inCollider.bounds.center, component.collider.bounds.GetMostRightTop());
					float num2 = Vector2.Distance(this._inCollider.bounds.center, component.collider.bounds.GetMostRightBottom());
					float num3 = Vector2.Distance(this._inCollider.bounds.center, component.collider.bounds.GetMostLeftBottom());
					float num4 = Vector2.Distance(this._inCollider.bounds.center, component.collider.bounds.GetMostLeftTop());
					if (num >= this._inCollider.radius * this._inCollider.transform.lossyScale.x || num2 >= this._inCollider.radius * this._inCollider.transform.lossyScale.x || num3 >= this._inCollider.radius * this._inCollider.transform.lossyScale.x || num4 >= this._inCollider.radius * this._inCollider.transform.lossyScale.x)
					{
						Bounds bounds2 = component.collider.bounds;
						Bounds bounds3 = new Bounds
						{
							min = MMMaths.Max(bounds.min, bounds2.min),
							max = MMMaths.Min(bounds.max, bounds2.max)
						};
						Vector2 hitPoint = MMMaths.RandomPointWithinBounds(bounds3);
						Vector2 force = Vector2.zero;
						if (this._pushInfo != null)
						{
							ValueTuple<Vector2, Vector2> valueTuple = this._pushInfo.EvaluateTimeIndependent(owner, component);
							force = valueTuple.Item1 + valueTuple.Item2;
						}
						if (this._adaptiveForce)
						{
							this._hitInfo.ChangeAdaptiveDamageAttribute(owner);
						}
						if (component.character != null)
						{
							if (component.character.liveAndActive && !(component.character == owner) && !component.character.cinematic.value)
							{
								this._chronoToTarget.ApplyTo(component.character);
								Damage damage = owner.stat.GetDamage((double)this._attackDamage.amount, hitPoint, this._hitInfo);
								if (this._hitInfo.attackType != Damage.AttackType.None)
								{
									CommonResource.instance.hitParticle.Emit(component.transform.position, bounds3, force, true);
								}
								flag = owner.TryAttackCharacter(component, ref damage);
								this._hits.AddOrUpdate(component);
								if (flag)
								{
									base.StartCoroutine(this._operationInfo.CRun(owner, component.character));
									OnAttackHitDelegate onAttackHitDelegate = this.onHit;
									if (onAttackHitDelegate != null)
									{
										onAttackHitDelegate(component, ref damage);
									}
									this._effect.Spawn(owner, bounds3, damage, component);
								}
							}
						}
						else if (component.damageable != null)
						{
							Damage damage2 = owner.stat.GetDamage((double)this._attackDamage.amount, hitPoint, this._hitInfo);
							if (component.damageable.spawnEffectOnHit && this._hitInfo.attackType != Damage.AttackType.None)
							{
								CommonResource.instance.hitParticle.Emit(component.transform.position, bounds3, force, true);
								this._effect.Spawn(owner, bounds3, damage2, component);
							}
							if (this._hitInfo.attackType == Damage.AttackType.None)
							{
								return;
							}
							if (component.damageable.blockCast)
							{
								flag = true;
								OnAttackHitDelegate onAttackHitDelegate2 = this.onHit;
								if (onAttackHitDelegate2 != null)
								{
									onAttackHitDelegate2(component, ref damage2);
								}
							}
							component.damageable.Hit(owner, ref damage2, force);
						}
					}
				}
			}
			if (flag)
			{
				this._chronoToGlobe.ApplyGlobe();
				this._chronoToOwner.ApplyTo(owner);
				if (this._operationToOwnerWhenHitInfo.components.Length != 0)
				{
					base.StartCoroutine(this._operationToOwnerWhenHitInfo.CRun(owner));
				}
			}
		}

		// Token: 0x06004D5C RID: 19804 RVA: 0x000E6D79 File Offset: 0x000E4F79
		public override void Stop()
		{
			this._operationToOwnerWhenHitInfo.StopAll();
			this.coroutineReference.Stop();
		}

		// Token: 0x04003D1B RID: 15643
		private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(2048);

		// Token: 0x04003D1C RID: 15644
		private NonAllocOverlapper _overlapper;

		// Token: 0x04003D1D RID: 15645
		[SerializeField]
		private bool _adaptiveForce;

		// Token: 0x04003D1E RID: 15646
		[SerializeField]
		private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Melee);

		// Token: 0x04003D1F RID: 15647
		[SerializeField]
		private ChronoInfo _chronoToGlobe;

		// Token: 0x04003D20 RID: 15648
		[SerializeField]
		private ChronoInfo _chronoToOwner;

		// Token: 0x04003D21 RID: 15649
		[SerializeField]
		private ChronoInfo _chronoToTarget;

		// Token: 0x04003D22 RID: 15650
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		internal OperationInfo.Subcomponents _operationToOwnerWhenHitInfo;

		// Token: 0x04003D23 RID: 15651
		[SerializeField]
		private float _duration;

		// Token: 0x04003D24 RID: 15652
		[SerializeField]
		private CircleCollider2D _outCollider;

		// Token: 0x04003D25 RID: 15653
		[SerializeField]
		private CircleCollider2D _inCollider;

		// Token: 0x04003D26 RID: 15654
		[SerializeField]
		private TargetLayer _layer = new TargetLayer(2048, false, true, false, false);

		// Token: 0x04003D27 RID: 15655
		[Tooltip("한 번에 공격 가능한 적의 수(프롭 포함), 특별한 경우가 아니면 기본값인 512로 두는 게 좋음.")]
		[SerializeField]
		private int _maxHits = 512;

		// Token: 0x04003D28 RID: 15656
		[SerializeField]
		private int _maxHitsPerUnit = 1;

		// Token: 0x04003D29 RID: 15657
		[SerializeField]
		private float _hitIntervalPerUnit = 0.5f;

		// Token: 0x04003D2A RID: 15658
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
		private TargetedOperationInfo.Subcomponents _operationInfo;

		// Token: 0x04003D2B RID: 15659
		[BoundsAttackVisualEffect.SubcomponentAttribute]
		[SerializeField]
		private BoundsAttackVisualEffect.Subcomponents _effect;

		// Token: 0x04003D2C RID: 15660
		private PushInfo _pushInfo;

		// Token: 0x04003D2E RID: 15662
		private IAttackDamage _attackDamage;

		// Token: 0x04003D2F RID: 15663
		private CoroutineReference coroutineReference;

		// Token: 0x04003D30 RID: 15664
		private bool _timeIndependent;

		// Token: 0x04003D31 RID: 15665
		private HitHistoryManager _hits = new HitHistoryManager(32);
	}
}
