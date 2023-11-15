using System;
using System.Collections;
using Characters.Abilities.Weapons.Skeleton_Sword;
using Characters.Operations.Attack;
using GameResources;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Customs.Skeleton_Sword
{
	// Token: 0x02000FF6 RID: 4086
	public sealed class Skeleton_SwordInstantAttack : CharacterOperation, IAttack
	{
		// Token: 0x140000C4 RID: 196
		// (add) Token: 0x06004EE7 RID: 20199 RVA: 0x000ECE50 File Offset: 0x000EB050
		// (remove) Token: 0x06004EE8 RID: 20200 RVA: 0x000ECE88 File Offset: 0x000EB088
		public event OnAttackHitDelegate onHit;

		// Token: 0x06004EE9 RID: 20201 RVA: 0x000ECEC0 File Offset: 0x000EB0C0
		private void Awake()
		{
			if (this._optimizedCollider && this._collider != null)
			{
				this._collider.enabled = false;
			}
			this._maxHits = Math.Min(this._maxHits, 2048);
			this._overlapper = ((this._maxHits == Skeleton_SwordInstantAttack._sharedOverlapper.capacity) ? Skeleton_SwordInstantAttack._sharedOverlapper : new NonAllocOverlapper(this._maxHits));
		}

		// Token: 0x06004EEA RID: 20202 RVA: 0x000ECF2F File Offset: 0x000EB12F
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			this._attackAndEffect.Initialize();
		}

		// Token: 0x06004EEB RID: 20203 RVA: 0x000ECF48 File Offset: 0x000EB148
		public override void Stop()
		{
			this._attackAndEffect.StopAllOperationsToOwner();
		}

		// Token: 0x06004EEC RID: 20204 RVA: 0x000ECF58 File Offset: 0x000EB158
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
					BoundsAttackInfoSequence[] components;
					if (component.character.ability.GetInstance<Skeleton_SwordTatanusDamage>() == null)
					{
						components = this._attackAndEffect.components;
					}
					else
					{
						components = this._tetanusAttackAndEffect.components;
					}
					foreach (BoundsAttackInfoSequence boundsAttackInfoSequence in components)
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

		// Token: 0x06004EED RID: 20205 RVA: 0x000ED084 File Offset: 0x000EB284
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
				if (target.character.evasion.value)
				{
					owner.TryAttackCharacter(target, ref damage);
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

		// Token: 0x06004EEE RID: 20206 RVA: 0x000ED369 File Offset: 0x000EB569
		private IEnumerator CAttack(Character owner, Bounds bounds, Target target)
		{
			int index = 0;
			float time = 0f;
			BoundsAttackInfoSequence[] attackAndEfects;
			if (target.character.ability.GetInstance<Skeleton_SwordTatanusDamage>() == null)
			{
				attackAndEfects = this._attackAndEffect.components;
			}
			else
			{
				attackAndEfects = this._tetanusAttackAndEffect.components;
			}
			foreach (BoundsAttackInfoSequence boundsAttackInfoSequence in attackAndEfects)
			{
				while (this != null && index < attackAndEfects.Length)
				{
					BoundsAttackInfoSequence boundsAttackInfoSequence2;
					while (index < attackAndEfects.Length && time >= (boundsAttackInfoSequence2 = attackAndEfects[index]).timeToTrigger)
					{
						this.Attack(owner, bounds, target, boundsAttackInfoSequence2.attackInfo);
						int num = index;
						index = num + 1;
					}
					yield return null;
					time += owner.chronometer.animation.deltaTime;
				}
			}
			BoundsAttackInfoSequence[] array = null;
			yield break;
		}

		// Token: 0x04003F07 RID: 16135
		private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(2048);

		// Token: 0x04003F08 RID: 16136
		private NonAllocOverlapper _overlapper;

		// Token: 0x04003F09 RID: 16137
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x04003F0A RID: 16138
		[SerializeField]
		private TargetLayer _layer = new TargetLayer(2048, false, true, false, false);

		// Token: 0x04003F0B RID: 16139
		[SerializeField]
		[Tooltip("한 번에 공격 가능한 적의 수(프롭 포함), 특별한 경우가 아니면 기본값인 99로 두는 게 좋음.")]
		private int _maxHits = 512;

		// Token: 0x04003F0C RID: 16140
		[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
		[SerializeField]
		private bool _optimizedCollider = true;

		// Token: 0x04003F0D RID: 16141
		[SerializeField]
		[Tooltip("공격자 자기 자신을 대상에서 제외할지 여부")]
		private bool _excludeItself = true;

		// Token: 0x04003F0E RID: 16142
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(BoundsAttackInfoSequence))]
		private BoundsAttackInfoSequence.Subcomponents _attackAndEffect;

		// Token: 0x04003F0F RID: 16143
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(BoundsAttackInfoSequence))]
		private BoundsAttackInfoSequence.Subcomponents _tetanusAttackAndEffect;

		// Token: 0x04003F10 RID: 16144
		private IAttackDamage _attackDamage;
	}
}
