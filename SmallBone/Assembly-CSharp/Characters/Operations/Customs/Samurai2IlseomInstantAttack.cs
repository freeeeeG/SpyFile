using System;
using System.Collections;
using Characters.Marks;
using Characters.Operations.Attack;
using GameResources;
using PhysicsUtils;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FE6 RID: 4070
	public sealed class Samurai2IlseomInstantAttack : CharacterOperation, IAttack
	{
		// Token: 0x140000C3 RID: 195
		// (add) Token: 0x06004EA3 RID: 20131 RVA: 0x000EBBA8 File Offset: 0x000E9DA8
		// (remove) Token: 0x06004EA4 RID: 20132 RVA: 0x000EBBE0 File Offset: 0x000E9DE0
		public event OnAttackHitDelegate onHit;

		// Token: 0x17000F8B RID: 3979
		// (get) Token: 0x06004EA5 RID: 20133 RVA: 0x000EBC15 File Offset: 0x000E9E15
		// (set) Token: 0x06004EA6 RID: 20134 RVA: 0x000EBC1D File Offset: 0x000E9E1D
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

		// Token: 0x06004EA7 RID: 20135 RVA: 0x000EBC28 File Offset: 0x000E9E28
		private void Awake()
		{
			Array.Sort<TargetedOperationInfo>(this._operationInfo.components, (TargetedOperationInfo x, TargetedOperationInfo y) => x.timeToTrigger.CompareTo(y.timeToTrigger));
			if (this._optimizedCollider && this._collider != null)
			{
				this._collider.enabled = false;
			}
			this._maxHits = Math.Min(this._maxHits, 2048);
			this._overlapper = ((this._maxHits == Samurai2IlseomInstantAttack._sharedOverlapper.capacity) ? Samurai2IlseomInstantAttack._sharedOverlapper : new NonAllocOverlapper(this._maxHits));
		}

		// Token: 0x06004EA8 RID: 20136 RVA: 0x000EBCC6 File Offset: 0x000E9EC6
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			this._attack1.Initialize();
			this._attack2.Initialize();
		}

		// Token: 0x06004EA9 RID: 20137 RVA: 0x000EBCEA File Offset: 0x000E9EEA
		public override void Stop()
		{
			this._operationOnMaxStackHit.StopAll();
		}

		// Token: 0x06004EAA RID: 20138 RVA: 0x000EBCF8 File Offset: 0x000E9EF8
		public override void Run(Character owner)
		{
			this._overlapper.contactFilter.SetLayerMask(this._layer.Evaluate(owner.gameObject));
			this._collider.enabled = true;
			this._overlapper.OverlapCollider(this._collider);
			Bounds bounds = this._collider.bounds;
			if (this._optimizedCollider)
			{
				this._collider.enabled = false;
			}
			ReadonlyBoundedList<Collider2D> results = this._overlapper.results;
			bool flag = false;
			for (int i = 0; i < results.Count; i++)
			{
				Target component = results[i].GetComponent<Target>();
				if (component == null)
				{
					Debug.LogError("Target is null in InstantAttack2");
					return;
				}
				float num = 0f;
				if (component.character != null)
				{
					num = component.character.mark.TakeAllStack(this._mark);
					if (num == (float)this._mark.maxStack)
					{
						flag = true;
					}
				}
				component.StartCoroutine(this.CAttack(owner, bounds, component, num));
			}
			if (flag && this._operationOnMaxStackHit.components.Length != 0)
			{
				base.StartCoroutine(this._operationOnMaxStackHit.CRun(owner));
			}
		}

		// Token: 0x06004EAB RID: 20139 RVA: 0x000EBE24 File Offset: 0x000EA024
		private void Attack(Character owner, Bounds bounds, Target target, BoundsAttackInfo attackInfo, double multiplier = 1.0)
		{
			if (target == null)
			{
				Debug.LogError("Target is null in InstantAttack2");
				return;
			}
			if (!target.isActiveAndEnabled)
			{
				return;
			}
			Bounds bounds2 = bounds;
			Bounds bounds3 = target.collider.bounds;
			Bounds bounds4 = new Bounds
			{
				min = MMMaths.Max(bounds2.min, bounds3.min),
				max = MMMaths.Min(bounds2.max, bounds3.max)
			};
			Vector2 hitPoint = MMMaths.RandomPointWithinBounds(bounds4);
			Vector2 force = Vector2.zero;
			if (attackInfo.pushInfo != null)
			{
				ValueTuple<Vector2, Vector2> valueTuple = attackInfo.pushInfo.EvaluateTimeIndependent(owner, target);
				force = valueTuple.Item1 + valueTuple.Item2;
			}
			if (target.character != null)
			{
				if (!target.character.liveAndActive)
				{
					return;
				}
				attackInfo.ApplyChrono(owner, target.character);
				if (attackInfo.operationsToOwner.components.Length != 0)
				{
					owner.StartCoroutine(attackInfo.operationsToOwner.CRun(owner));
				}
				Damage damage = owner.stat.GetDamage((double)this._attackDamage.amount * multiplier, hitPoint, attackInfo.hitInfo);
				if (attackInfo.hitInfo.attackType != Damage.AttackType.None)
				{
					CommonResource.instance.hitParticle.Emit(target.transform.position, target.collider.bounds, force, true);
				}
				if (target.character.cinematic.value)
				{
					attackInfo.effect.Spawn(owner, bounds4, damage, target);
					return;
				}
				owner.StartCoroutine(attackInfo.operationInfo.CRun(owner, target.character));
				OnAttackHitDelegate onAttackHitDelegate = this.onHit;
				if (onAttackHitDelegate != null)
				{
					onAttackHitDelegate(target, ref damage);
				}
				owner.TryAttackCharacter(target, ref damage);
				attackInfo.effect.Spawn(owner, bounds4, damage, target);
				return;
			}
			else
			{
				if (!(target.damageable != null))
				{
					return;
				}
				attackInfo.ApplyChrono(owner);
				owner.StartCoroutine(attackInfo.operationsToOwner.CRun(owner));
				Damage damage2 = owner.stat.GetDamage((double)this._attackDamage.amount, hitPoint, attackInfo.hitInfo);
				if (target.damageable.spawnEffectOnHit && attackInfo.hitInfo.attackType != Damage.AttackType.None)
				{
					CommonResource.instance.hitParticle.Emit(target.transform.position, target.collider.bounds, force, true);
					attackInfo.effect.Spawn(owner, bounds4, damage2, target);
				}
				if (attackInfo.hitInfo.attackType == Damage.AttackType.None)
				{
					return;
				}
				OnAttackHitDelegate onAttackHitDelegate2 = this.onHit;
				if (onAttackHitDelegate2 != null)
				{
					onAttackHitDelegate2(target, ref damage2);
				}
				target.damageable.Hit(owner, ref damage2, force);
				return;
			}
		}

		// Token: 0x06004EAC RID: 20140 RVA: 0x000EC0EA File Offset: 0x000EA2EA
		private IEnumerator CAttack(Character owner, Bounds bounds, Target target, float stacks)
		{
			float time = 0f;
			while (this != null && time < this._attack1Time)
			{
				yield return null;
				time += owner.chronometer.animation.deltaTime;
			}
			this.Attack(owner, bounds, target, this._attack1, 1.0);
			while (this != null && time < this._attack2Time)
			{
				yield return null;
				time += owner.chronometer.animation.deltaTime;
			}
			float damageMultiplier = this._attack2.hitInfo.damageMultiplier;
			int num = (int)math.min(stacks, (float)this._mark.maxStack);
			this.Attack(owner, bounds, target, this._attack2, this._damagePercents[num]);
			this._attack2.hitInfo.damageMultiplier = damageMultiplier;
			yield break;
		}

		// Token: 0x04003EB5 RID: 16053
		private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(2048);

		// Token: 0x04003EB6 RID: 16054
		private NonAllocOverlapper _overlapper;

		// Token: 0x04003EB7 RID: 16055
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x04003EB8 RID: 16056
		[SerializeField]
		private TargetLayer _layer = new TargetLayer(2048, false, true, false, false);

		// Token: 0x04003EB9 RID: 16057
		[Tooltip("한 번에 공격 가능한 적의 수(프롭 포함), 특별한 경우가 아니면 기본값인 512로 두는 게 좋음.")]
		[SerializeField]
		private int _maxHits = 512;

		// Token: 0x04003EBA RID: 16058
		[SerializeField]
		[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
		private bool _optimizedCollider = true;

		// Token: 0x04003EBB RID: 16059
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
		private TargetedOperationInfo.Subcomponents _operationInfo;

		// Token: 0x04003EBC RID: 16060
		[SerializeField]
		[Space]
		private MarkInfo _mark;

		// Token: 0x04003EBD RID: 16061
		[SerializeField]
		[Tooltip("표식이 없을 때인 0개부터 시작")]
		private double[] _damagePercents;

		// Token: 0x04003EBE RID: 16062
		[SerializeField]
		[FrameTime]
		private float _attack1Time;

		// Token: 0x04003EBF RID: 16063
		[UnityEditor.Subcomponent(typeof(BoundsAttackInfo))]
		[SerializeField]
		private BoundsAttackInfo _attack1;

		// Token: 0x04003EC0 RID: 16064
		[FrameTime]
		[SerializeField]
		private float _attack2Time;

		// Token: 0x04003EC1 RID: 16065
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(BoundsAttackInfo))]
		private BoundsAttackInfo _attack2;

		// Token: 0x04003EC2 RID: 16066
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		internal OperationInfo.Subcomponents _operationOnMaxStackHit;

		// Token: 0x04003EC4 RID: 16068
		private IAttackDamage _attackDamage;
	}
}
