using System;
using System.Collections;
using GameResources;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack
{
	// Token: 0x02000F8B RID: 3979
	public sealed class InstantAttack2 : CharacterOperation, IAttack
	{
		// Token: 0x140000BD RID: 189
		// (add) Token: 0x06004D31 RID: 19761 RVA: 0x000E5A70 File Offset: 0x000E3C70
		// (remove) Token: 0x06004D32 RID: 19762 RVA: 0x000E5AA8 File Offset: 0x000E3CA8
		public event OnAttackHitDelegate onHit;

		// Token: 0x06004D33 RID: 19763 RVA: 0x000E5AE0 File Offset: 0x000E3CE0
		private void Awake()
		{
			if (this._optimizedCollider && this._collider != null)
			{
				this._collider.enabled = false;
			}
			this._maxHits = Math.Min(this._maxHits, 2048);
			this._overlapper = ((this._maxHits == InstantAttack2._sharedOverlapper.capacity) ? InstantAttack2._sharedOverlapper : new NonAllocOverlapper(this._maxHits));
		}

		// Token: 0x06004D34 RID: 19764 RVA: 0x000E5B4F File Offset: 0x000E3D4F
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			this._attackAndEffect.Initialize();
		}

		// Token: 0x06004D35 RID: 19765 RVA: 0x000E5B68 File Offset: 0x000E3D68
		public override void Stop()
		{
			this._attackAndEffect.StopAllOperationsToOwner();
		}

		// Token: 0x06004D36 RID: 19766 RVA: 0x000E5B78 File Offset: 0x000E3D78
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
			for (int i = 0; i < results.Count; i++)
			{
				Target component = results[i].GetComponent<Target>();
				if (component == null)
				{
					Debug.LogError("Target is null in InstantAttack2");
					return;
				}
				if (this._attackAndEffect.noDelay)
				{
					foreach (BoundsAttackInfoSequence boundsAttackInfoSequence in this._attackAndEffect.components)
					{
						this.Attack(owner, bounds, component, boundsAttackInfoSequence.attackInfo);
					}
				}
				else
				{
					component.StartCoroutine(this.CAttack(owner, bounds, component));
				}
			}
		}

		// Token: 0x06004D37 RID: 19767 RVA: 0x000E5C7C File Offset: 0x000E3E7C
		private void Attack(Character owner, Bounds bounds, Target target, BoundsAttackInfo attackInfo)
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
				if (!target.character.liveAndActive || (this._excludeItself && target.character == owner))
				{
					return;
				}
				if (target.character.cinematic.value)
				{
					return;
				}
				attackInfo.ApplyChrono(owner, target.character);
				if (attackInfo.operationsToOwner.components.Length != 0)
				{
					owner.StartCoroutine(attackInfo.operationsToOwner.CRun(owner));
				}
				Damage damage = owner.stat.GetDamage((double)this._attackDamage.amount, hitPoint, attackInfo.hitInfo);
				if (attackInfo.hitInfo.attackType != Damage.AttackType.None)
				{
					CommonResource.instance.hitParticle.Emit(target.transform.position, target.collider.bounds, force, true);
				}
				if (owner.TryAttackCharacter(target, ref damage))
				{
					owner.StartCoroutine(attackInfo.operationInfo.CRun(owner, target.character));
					OnAttackHitDelegate onAttackHitDelegate = this.onHit;
					if (onAttackHitDelegate != null)
					{
						onAttackHitDelegate(target, ref damage);
					}
					attackInfo.effect.Spawn(owner, bounds4, damage, target);
				}
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

		// Token: 0x06004D38 RID: 19768 RVA: 0x000E5F45 File Offset: 0x000E4145
		private IEnumerator CAttack(Character owner, Bounds bounds, Target target)
		{
			int index = 0;
			float time = 0f;
			while (this != null && index < this._attackAndEffect.components.Length)
			{
				BoundsAttackInfoSequence boundsAttackInfoSequence;
				while (index < this._attackAndEffect.components.Length && time >= (boundsAttackInfoSequence = this._attackAndEffect.components[index]).timeToTrigger)
				{
					this.Attack(owner, bounds, target, boundsAttackInfoSequence.attackInfo);
					int num = index;
					index = num + 1;
				}
				yield return null;
				time += owner.chronometer.animation.deltaTime;
			}
			yield break;
		}

		// Token: 0x04003CED RID: 15597
		private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(2048);

		// Token: 0x04003CEE RID: 15598
		private NonAllocOverlapper _overlapper;

		// Token: 0x04003CEF RID: 15599
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x04003CF0 RID: 15600
		[SerializeField]
		private TargetLayer _layer = new TargetLayer(2048, false, true, false, false);

		// Token: 0x04003CF1 RID: 15601
		[Tooltip("한 번에 공격 가능한 적의 수(프롭 포함), 특별한 경우가 아니면 기본값인 99로 두는 게 좋음.")]
		[SerializeField]
		private int _maxHits = 512;

		// Token: 0x04003CF2 RID: 15602
		[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
		[SerializeField]
		private bool _optimizedCollider = true;

		// Token: 0x04003CF3 RID: 15603
		[SerializeField]
		[Tooltip("공격자 자기 자신을 대상에서 제외할지 여부")]
		private bool _excludeItself = true;

		// Token: 0x04003CF4 RID: 15604
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(BoundsAttackInfoSequence))]
		private BoundsAttackInfoSequence.Subcomponents _attackAndEffect;

		// Token: 0x04003CF5 RID: 15605
		private IAttackDamage _attackDamage;
	}
}
