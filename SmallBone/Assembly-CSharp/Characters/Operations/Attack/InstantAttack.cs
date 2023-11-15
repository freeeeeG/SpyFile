using System;
using Characters.Movements;
using Characters.Operations.Movement;
using FX.BoundsAttackVisualEffect;
using GameResources;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack
{
	// Token: 0x02000F89 RID: 3977
	public sealed class InstantAttack : CharacterOperation, IAttack
	{
		// Token: 0x140000BC RID: 188
		// (add) Token: 0x06004D24 RID: 19748 RVA: 0x000E54D8 File Offset: 0x000E36D8
		// (remove) Token: 0x06004D25 RID: 19749 RVA: 0x000E5510 File Offset: 0x000E3710
		public event OnAttackHitDelegate onHit;

		// Token: 0x17000F5C RID: 3932
		// (get) Token: 0x06004D26 RID: 19750 RVA: 0x000E5545 File Offset: 0x000E3745
		// (set) Token: 0x06004D27 RID: 19751 RVA: 0x000E554D File Offset: 0x000E374D
		public Collider2D range
		{
			get
			{
				return this._collider;
			}
			set
			{
				this._collider = value;
			}
		}

		// Token: 0x06004D28 RID: 19752 RVA: 0x000E5558 File Offset: 0x000E3758
		private void Awake()
		{
			Array.Sort<TargetedOperationInfo>(this._operationInfo.components, (TargetedOperationInfo x, TargetedOperationInfo y) => x.timeToTrigger.CompareTo(y.timeToTrigger));
			if (this._optimizedCollider && this._collider != null)
			{
				this._collider.enabled = false;
			}
			this._maxHits = Math.Min(this._maxHits, 2048);
			this._overlapper = ((this._maxHits == InstantAttack._sharedOverlapper.capacity) ? InstantAttack._sharedOverlapper : new NonAllocOverlapper(this._maxHits));
		}

		// Token: 0x06004D29 RID: 19753 RVA: 0x000E55F8 File Offset: 0x000E37F8
		public override void Initialize()
		{
			base.Initialize();
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			this._operationInfo.Initialize();
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

		// Token: 0x06004D2A RID: 19754 RVA: 0x000E567C File Offset: 0x000E387C
		public override void Run(Character owner)
		{
			this._overlapper.contactFilter.SetLayerMask(this._layer.Evaluate(owner.gameObject));
			this._collider.enabled = true;
			Bounds bounds = this._collider.bounds;
			this._overlapper.OverlapCollider(this._collider);
			if (this._optimizedCollider)
			{
				this._collider.enabled = false;
			}
			bool flag = false;
			for (int i = 0; i < this._overlapper.results.Count; i++)
			{
				Target component = this._overlapper.results[i].GetComponent<Target>();
				if (!(component == null))
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

		// Token: 0x06004D2B RID: 19755 RVA: 0x000E59E1 File Offset: 0x000E3BE1
		public override void Stop()
		{
			this._operationToOwnerWhenHitInfo.StopAll();
		}

		// Token: 0x04003CDA RID: 15578
		private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(2048);

		// Token: 0x04003CDB RID: 15579
		private NonAllocOverlapper _overlapper;

		// Token: 0x04003CDC RID: 15580
		[SerializeField]
		private bool _adaptiveForce;

		// Token: 0x04003CDD RID: 15581
		[SerializeField]
		private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Melee);

		// Token: 0x04003CDE RID: 15582
		[SerializeField]
		private ChronoInfo _chronoToGlobe;

		// Token: 0x04003CDF RID: 15583
		[SerializeField]
		private ChronoInfo _chronoToOwner;

		// Token: 0x04003CE0 RID: 15584
		[SerializeField]
		private ChronoInfo _chronoToTarget;

		// Token: 0x04003CE1 RID: 15585
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		internal OperationInfo.Subcomponents _operationToOwnerWhenHitInfo;

		// Token: 0x04003CE2 RID: 15586
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x04003CE3 RID: 15587
		[SerializeField]
		private TargetLayer _layer = new TargetLayer(2048, false, true, false, false);

		// Token: 0x04003CE4 RID: 15588
		[Tooltip("한 번에 공격 가능한 적의 수(프롭 포함), 특별한 경우가 아니면 기본값인 512로 두는 게 좋음.")]
		[SerializeField]
		private int _maxHits = 512;

		// Token: 0x04003CE5 RID: 15589
		[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
		[SerializeField]
		private bool _optimizedCollider = true;

		// Token: 0x04003CE6 RID: 15590
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
		private TargetedOperationInfo.Subcomponents _operationInfo;

		// Token: 0x04003CE7 RID: 15591
		[SerializeField]
		[BoundsAttackVisualEffect.SubcomponentAttribute]
		private BoundsAttackVisualEffect.Subcomponents _effect;

		// Token: 0x04003CE8 RID: 15592
		private PushInfo _pushInfo;

		// Token: 0x04003CEA RID: 15594
		private IAttackDamage _attackDamage;
	}
}
