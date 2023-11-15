using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters.Abilities.Weapons.Skeleton_Sword;
using Characters.Operations.Attack;
using Characters.Utils;
using GameResources;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Customs.Skeleton_Sword
{
	// Token: 0x02000FF8 RID: 4088
	public class Skeleton_SwordSweepAttack : CharacterOperation, IAttack
	{
		// Token: 0x140000C5 RID: 197
		// (add) Token: 0x06004EF7 RID: 20215 RVA: 0x000ED56C File Offset: 0x000EB76C
		// (remove) Token: 0x06004EF8 RID: 20216 RVA: 0x000ED5A4 File Offset: 0x000EB7A4
		public event OnAttackHitDelegate onHit;

		// Token: 0x17000F94 RID: 3988
		// (get) Token: 0x06004EF9 RID: 20217 RVA: 0x000ED5D9 File Offset: 0x000EB7D9
		// (set) Token: 0x06004EFA RID: 20218 RVA: 0x000ED5E1 File Offset: 0x000EB7E1
		internal Character owner { get; private set; }

		// Token: 0x17000F95 RID: 3989
		// (get) Token: 0x06004EFB RID: 20219 RVA: 0x000ED5EA File Offset: 0x000EB7EA
		private Skeleton_SwordSweepAttack.CollisionDetector collisionDetector
		{
			get
			{
				return this._collisionDetector;
			}
		}

		// Token: 0x17000F96 RID: 3990
		// (get) Token: 0x06004EFC RID: 20220 RVA: 0x000ED5F2 File Offset: 0x000EB7F2
		// (set) Token: 0x06004EFD RID: 20221 RVA: 0x000ED5FA File Offset: 0x000EB7FA
		public float duration
		{
			get
			{
				return this._duration;
			}
			set
			{
				this._duration = value;
			}
		}

		// Token: 0x17000F97 RID: 3991
		// (get) Token: 0x06004EFE RID: 20222 RVA: 0x000ED603 File Offset: 0x000EB803
		// (set) Token: 0x06004EFF RID: 20223 RVA: 0x000ED610 File Offset: 0x000EB810
		public Collider2D range
		{
			get
			{
				return this.collisionDetector.collider;
			}
			set
			{
				this.collisionDetector.collider = value;
			}
		}

		// Token: 0x06004F00 RID: 20224 RVA: 0x000ED620 File Offset: 0x000EB820
		private void Awake()
		{
			this._collisionDetector.onTerrainHit += this.<Awake>g__onTerrainHit|25_0;
			if (this._attackAndEffect.noDelay)
			{
				this._collisionDetector.onHit += this.<Awake>g__onTargetHitWithoutDelay|25_2;
				return;
			}
			this._collisionDetector.onHit += this.<Awake>g__onTargetHit|25_1;
		}

		// Token: 0x06004F01 RID: 20225 RVA: 0x000ED680 File Offset: 0x000EB880
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			this._attackAndEffect.Initialize();
		}

		// Token: 0x06004F02 RID: 20226 RVA: 0x000ED699 File Offset: 0x000EB899
		public override void Run(Character owner)
		{
			this.owner = owner;
			this._collisionDetector.Initialize(this);
			this._detectReference.Stop();
			this._detectReference = owner.StartCoroutineWithReference(this.CDetect());
		}

		// Token: 0x06004F03 RID: 20227 RVA: 0x000ED6CB File Offset: 0x000EB8CB
		public override void Stop()
		{
			base.Stop();
			this._attackAndEffect.StopAllOperationsToOwner();
			this._detectReference.Stop();
		}

		// Token: 0x06004F04 RID: 20228 RVA: 0x000ED6E9 File Offset: 0x000EB8E9
		private IEnumerator CDetect()
		{
			float time = 0f;
			Chronometer master = this.owner.chronometer.master;
			Chronometer animationChronometer = this.owner.chronometer.animation;
			while (time < this._duration)
			{
				if (this._timeIndependent || animationChronometer.timeScale > 1E-45f)
				{
					Vector2 distance = Vector2.zero;
					if (this._trackMovement && this.owner.movement != null)
					{
						distance = this.owner.movement.moved;
					}
					this._collisionDetector.Detect(base.transform.position, distance);
				}
				yield return null;
				if (this._timeIndependent)
				{
					time += Chronometer.global.deltaTime;
				}
				else
				{
					time += animationChronometer.deltaTime;
				}
			}
			yield break;
		}

		// Token: 0x06004F05 RID: 20229 RVA: 0x000ED6F8 File Offset: 0x000EB8F8
		protected void Attack(CastAttackInfo attackInfo, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
		{
			Vector2 force = Vector2.zero;
			if (attackInfo.pushInfo != null)
			{
				ValueTuple<Vector2, Vector2> valueTuple = attackInfo.pushInfo.EvaluateTimeIndependent(this.owner, target);
				force = valueTuple.Item1 + valueTuple.Item2;
			}
			if (target.character != null)
			{
				if (!target.character.liveAndActive || target.character == this.owner)
				{
					return;
				}
				if (target.character.cinematic.value)
				{
					return;
				}
				attackInfo.ApplyChrono(this.owner, target.character);
				if (attackInfo.operationsToOwner.components.Length != 0)
				{
					this.owner.StartCoroutine(attackInfo.operationsToOwner.CRun(this.owner));
				}
				Damage damage = this.owner.stat.GetDamage((double)this._attackDamage.amount, raycastHit.point, attackInfo.hitInfo);
				if (attackInfo.hitInfo.attackType != Damage.AttackType.None)
				{
					CommonResource.instance.hitParticle.Emit(target.transform.position, target.collider.bounds, force, true);
				}
				if (target.character.evasion.value)
				{
					this.owner.TryAttackCharacter(target, ref damage);
					return;
				}
				this.owner.StartCoroutine(attackInfo.operationsToCharacter.CRun(this.owner, target.character));
				this.owner.TryAttackCharacter(target, ref damage);
				attackInfo.effect.Spawn(this.owner, this._collisionDetector.collider, origin, direction, distance, raycastHit, damage, target);
				return;
			}
			else
			{
				if (!(target.damageable != null))
				{
					return;
				}
				attackInfo.ApplyChrono(this.owner);
				this.owner.StartCoroutine(attackInfo.operationsToOwner.CRun(this.owner));
				Damage damage2 = this.owner.stat.GetDamage((double)this._attackDamage.amount, raycastHit.point, attackInfo.hitInfo);
				if (target.damageable.spawnEffectOnHit && attackInfo.hitInfo.attackType != Damage.AttackType.None)
				{
					CommonResource.instance.hitParticle.Emit(target.transform.position, target.collider.bounds, force, true);
					attackInfo.effect.Spawn(this.owner, this._collisionDetector.collider, origin, direction, distance, raycastHit, damage2, target);
				}
				if (attackInfo.hitInfo.attackType == Damage.AttackType.None)
				{
					return;
				}
				target.damageable.Hit(this.owner, ref damage2, force);
				return;
			}
		}

		// Token: 0x06004F06 RID: 20230 RVA: 0x000ED99A File Offset: 0x000EBB9A
		protected virtual IEnumerator CAttack(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
		{
			int index = 0;
			float time = 0f;
			Vector3 originOffset = MMMaths.Vector2ToVector3(origin) - target.transform.position;
			Vector3 hitPointOffset = MMMaths.Vector2ToVector3(raycastHit.point) - target.transform.position;
			CastAttackInfoSequence[] attackAndEfects;
			if (target.character.ability.GetInstance<Skeleton_SwordTatanusDamage>() == null)
			{
				attackAndEfects = this._attackAndEffect.components;
			}
			else
			{
				attackAndEfects = this._tetanusAttackAndEffect.components;
			}
			foreach (CastAttackInfoSequence castAttackInfoSequence in attackAndEfects)
			{
				this.Attack(castAttackInfoSequence.attackInfo, origin, direction, distance, raycastHit, target);
			}
			while (this != null && index < attackAndEfects.Length)
			{
				CastAttackInfoSequence castAttackInfoSequence2;
				while (index < attackAndEfects.Length && time >= (castAttackInfoSequence2 = attackAndEfects[index]).timeToTrigger)
				{
					raycastHit.point = target.transform.position + hitPointOffset;
					this.Attack(castAttackInfoSequence2.attackInfo, target.transform.position + originOffset, direction, distance, raycastHit, target);
					int i = index;
					index = i + 1;
				}
				yield return null;
				time += this.owner.chronometer.animation.deltaTime;
			}
			yield break;
		}

		// Token: 0x06004F08 RID: 20232 RVA: 0x000ED9DD File Offset: 0x000EBBDD
		[CompilerGenerated]
		private void <Awake>g__onTerrainHit|25_0(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit)
		{
			base.StartCoroutine(this._onTerrainHit.CRun(this.owner));
		}

		// Token: 0x06004F09 RID: 20233 RVA: 0x000ED9F7 File Offset: 0x000EBBF7
		[CompilerGenerated]
		private void <Awake>g__onTargetHit|25_1(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
		{
			target.StartCoroutine(this.CAttack(origin, direction, distance, raycastHit, target));
		}

		// Token: 0x06004F0A RID: 20234 RVA: 0x000EDA10 File Offset: 0x000EBC10
		[CompilerGenerated]
		private void <Awake>g__onTargetHitWithoutDelay|25_2(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
		{
			CastAttackInfoSequence[] components;
			if (target.character == null || target.character.ability.GetInstance<Skeleton_SwordTatanusDamage>() == null)
			{
				components = this._attackAndEffect.components;
			}
			else
			{
				components = this._tetanusAttackAndEffect.components;
			}
			foreach (CastAttackInfoSequence castAttackInfoSequence in components)
			{
				this.Attack(castAttackInfoSequence.attackInfo, origin, direction, distance, raycastHit, target);
			}
		}

		// Token: 0x04003F1D RID: 16157
		[SerializeField]
		private float _duration;

		// Token: 0x04003F1E RID: 16158
		[SerializeField]
		private bool _timeIndependent;

		// Token: 0x04003F1F RID: 16159
		[SerializeField]
		private bool _trackMovement = true;

		// Token: 0x04003F20 RID: 16160
		[SerializeField]
		private Skeleton_SwordSweepAttack.CollisionDetector _collisionDetector;

		// Token: 0x04003F21 RID: 16161
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onTerrainHit;

		// Token: 0x04003F22 RID: 16162
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(CastAttackInfoSequence))]
		protected CastAttackInfoSequence.Subcomponents _attackAndEffect;

		// Token: 0x04003F23 RID: 16163
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(CastAttackInfoSequence))]
		protected CastAttackInfoSequence.Subcomponents _tetanusAttackAndEffect;

		// Token: 0x04003F24 RID: 16164
		private IAttackDamage _attackDamage;

		// Token: 0x04003F25 RID: 16165
		private CoroutineReference _detectReference;

		// Token: 0x02000FF9 RID: 4089
		[Serializable]
		public class CollisionDetector
		{
			// Token: 0x140000C6 RID: 198
			// (add) Token: 0x06004F0B RID: 20235 RVA: 0x000EDA80 File Offset: 0x000EBC80
			// (remove) Token: 0x06004F0C RID: 20236 RVA: 0x000EDAB8 File Offset: 0x000EBCB8
			public event Skeleton_SwordSweepAttack.CollisionDetector.onTerrainHitDelegate onTerrainHit;

			// Token: 0x140000C7 RID: 199
			// (add) Token: 0x06004F0D RID: 20237 RVA: 0x000EDAF0 File Offset: 0x000EBCF0
			// (remove) Token: 0x06004F0E RID: 20238 RVA: 0x000EDB28 File Offset: 0x000EBD28
			public event Skeleton_SwordSweepAttack.CollisionDetector.onTargetHitDelegate onHit;

			// Token: 0x17000F98 RID: 3992
			// (get) Token: 0x06004F0F RID: 20239 RVA: 0x000EDB5D File Offset: 0x000EBD5D
			// (set) Token: 0x06004F10 RID: 20240 RVA: 0x000EDB65 File Offset: 0x000EBD65
			public Collider2D collider
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

			// Token: 0x06004F11 RID: 20241 RVA: 0x000EDB70 File Offset: 0x000EBD70
			internal void Initialize(Skeleton_SwordSweepAttack sweepAttack)
			{
				this._sweepAttack = sweepAttack;
				this._filter.layerMask = this._layer.Evaluate(sweepAttack.owner.gameObject);
				this._hits.Clear();
				this._propPenetratingHits = 0;
				if (this._optimizedCollider && this._collider != null)
				{
					this._collider.enabled = false;
				}
			}

			// Token: 0x06004F12 RID: 20242 RVA: 0x000EDBD9 File Offset: 0x000EBDD9
			internal void Detect(Vector2 origin, Vector2 distance)
			{
				this.Detect(origin, distance.normalized, distance.magnitude);
			}

			// Token: 0x06004F13 RID: 20243 RVA: 0x000EDBF0 File Offset: 0x000EBDF0
			internal void Detect(Vector2 origin, Vector2 direction, float distance)
			{
				Skeleton_SwordSweepAttack.CollisionDetector.<>c__DisplayClass25_0 CS$<>8__locals1;
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.origin = origin;
				CS$<>8__locals1.direction = direction;
				CS$<>8__locals1.distance = distance;
				Skeleton_SwordSweepAttack.CollisionDetector._caster.contactFilter.SetLayerMask(this._filter.layerMask);
				if (this._collider != null)
				{
					if (this._optimizedCollider)
					{
						this._collider.enabled = true;
						Skeleton_SwordSweepAttack.CollisionDetector._caster.ColliderCast(this._collider, CS$<>8__locals1.direction, CS$<>8__locals1.distance);
						this._collider.enabled = false;
					}
					else
					{
						Skeleton_SwordSweepAttack.CollisionDetector._caster.ColliderCast(this._collider, CS$<>8__locals1.direction, CS$<>8__locals1.distance);
					}
				}
				else
				{
					Skeleton_SwordSweepAttack.CollisionDetector._caster.RayCast(CS$<>8__locals1.origin, CS$<>8__locals1.direction, CS$<>8__locals1.distance);
				}
				for (int i = 0; i < Skeleton_SwordSweepAttack.CollisionDetector._caster.results.Count; i++)
				{
					Skeleton_SwordSweepAttack.CollisionDetector.<>c__DisplayClass25_1 CS$<>8__locals2;
					CS$<>8__locals2.result = Skeleton_SwordSweepAttack.CollisionDetector._caster.results[i];
					this.<Detect>g__HandleResult|25_0(ref CS$<>8__locals1, ref CS$<>8__locals2);
					if (this._hits.Count - this._propPenetratingHits >= this._maxHits)
					{
						this._sweepAttack.Stop();
					}
				}
			}

			// Token: 0x06004F16 RID: 20246 RVA: 0x000EDD98 File Offset: 0x000EBF98
			[CompilerGenerated]
			private void <Detect>g__HandleResult|25_0(ref Skeleton_SwordSweepAttack.CollisionDetector.<>c__DisplayClass25_0 A_1, ref Skeleton_SwordSweepAttack.CollisionDetector.<>c__DisplayClass25_1 A_2)
			{
				if (this._terrainLayer.Contains(A_2.result.collider.gameObject.layer))
				{
					this.onTerrainHit(A_1.origin, A_1.direction, A_1.distance, A_2.result);
					return;
				}
				A_1.target = A_2.result.collider.GetComponent<Target>();
				if (A_1.target == null)
				{
					Debug.LogError(A_2.result.collider.name + " : Character has no Target component");
					return;
				}
				if (!this._hits.CanAttack(A_1.target, this._maxHits, this._maxHitsPerUnit, this._hitIntervalPerUnit))
				{
					return;
				}
				if (A_1.target.character != null)
				{
					if (!A_1.target.character.liveAndActive)
					{
						return;
					}
					if (A_1.target.character.cinematic.value)
					{
						return;
					}
					this.onHit(A_1.origin, A_1.direction, A_1.distance, A_2.result, A_1.target);
					this._hits.AddOrUpdate(A_1.target);
					return;
				}
				else
				{
					if (A_1.target.damageable != null)
					{
						this.onHit(A_1.origin, A_1.direction, A_1.distance, A_2.result, A_1.target);
						if (!A_1.target.damageable.blockCast)
						{
							this._propPenetratingHits++;
						}
						this._hits.AddOrUpdate(A_1.target);
						return;
					}
					return;
				}
			}

			// Token: 0x04003F28 RID: 16168
			private static readonly NonAllocCaster _caster = new NonAllocCaster(99);

			// Token: 0x04003F2B RID: 16171
			private Skeleton_SwordSweepAttack _sweepAttack;

			// Token: 0x04003F2C RID: 16172
			[SerializeField]
			private TargetLayer _layer = new TargetLayer(2048, false, true, false, false);

			// Token: 0x04003F2D RID: 16173
			[SerializeField]
			private LayerMask _terrainLayer = Layers.groundMask;

			// Token: 0x04003F2E RID: 16174
			[SerializeField]
			private Collider2D _collider;

			// Token: 0x04003F2F RID: 16175
			[SerializeField]
			[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
			private bool _optimizedCollider = true;

			// Token: 0x04003F30 RID: 16176
			[SerializeField]
			private int _maxHits = 512;

			// Token: 0x04003F31 RID: 16177
			[SerializeField]
			private int _maxHitsPerUnit = 1;

			// Token: 0x04003F32 RID: 16178
			[SerializeField]
			private float _hitIntervalPerUnit = 0.5f;

			// Token: 0x04003F33 RID: 16179
			private HitHistoryManager _hits = new HitHistoryManager(32);

			// Token: 0x04003F34 RID: 16180
			private int _propPenetratingHits;

			// Token: 0x04003F35 RID: 16181
			private ContactFilter2D _filter;

			// Token: 0x02000FFA RID: 4090
			// (Invoke) Token: 0x06004F18 RID: 20248
			public delegate void onTerrainHitDelegate(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit);

			// Token: 0x02000FFB RID: 4091
			// (Invoke) Token: 0x06004F1C RID: 20252
			public delegate void onTargetHitDelegate(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target);
		}
	}
}
