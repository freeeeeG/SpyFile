using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters.Utils;
using GameResources;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack
{
	// Token: 0x02000F9A RID: 3994
	public class SweepAttack2 : CharacterOperation, IAttack
	{
		// Token: 0x140000C0 RID: 192
		// (add) Token: 0x06004D8C RID: 19852 RVA: 0x000E77F8 File Offset: 0x000E59F8
		// (remove) Token: 0x06004D8D RID: 19853 RVA: 0x000E7830 File Offset: 0x000E5A30
		public event OnAttackHitDelegate onHit;

		// Token: 0x17000F68 RID: 3944
		// (get) Token: 0x06004D8E RID: 19854 RVA: 0x000E7865 File Offset: 0x000E5A65
		// (set) Token: 0x06004D8F RID: 19855 RVA: 0x000E786D File Offset: 0x000E5A6D
		internal Character owner { get; private set; }

		// Token: 0x17000F69 RID: 3945
		// (get) Token: 0x06004D90 RID: 19856 RVA: 0x000E7876 File Offset: 0x000E5A76
		private SweepAttack2.CollisionDetector collisionDetector
		{
			get
			{
				return this._collisionDetector;
			}
		}

		// Token: 0x17000F6A RID: 3946
		// (get) Token: 0x06004D91 RID: 19857 RVA: 0x000E787E File Offset: 0x000E5A7E
		// (set) Token: 0x06004D92 RID: 19858 RVA: 0x000E7886 File Offset: 0x000E5A86
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

		// Token: 0x17000F6B RID: 3947
		// (get) Token: 0x06004D93 RID: 19859 RVA: 0x000E788F File Offset: 0x000E5A8F
		// (set) Token: 0x06004D94 RID: 19860 RVA: 0x000E789C File Offset: 0x000E5A9C
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

		// Token: 0x06004D95 RID: 19861 RVA: 0x000E78AC File Offset: 0x000E5AAC
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

		// Token: 0x06004D96 RID: 19862 RVA: 0x000E790C File Offset: 0x000E5B0C
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			this._attackAndEffect.Initialize();
		}

		// Token: 0x06004D97 RID: 19863 RVA: 0x000E7925 File Offset: 0x000E5B25
		public override void Run(Character owner)
		{
			this.owner = owner;
			this._collisionDetector.Initialize(this);
			this._detectReference.Stop();
			this._detectReference = owner.StartCoroutineWithReference(this.CDetect());
		}

		// Token: 0x06004D98 RID: 19864 RVA: 0x000E7957 File Offset: 0x000E5B57
		public override void Stop()
		{
			base.Stop();
			this._attackAndEffect.StopAllOperationsToOwner();
			this._detectReference.Stop();
		}

		// Token: 0x06004D99 RID: 19865 RVA: 0x000E7975 File Offset: 0x000E5B75
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

		// Token: 0x06004D9A RID: 19866 RVA: 0x000E7984 File Offset: 0x000E5B84
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
				if (this._adaptiveForce)
				{
					attackInfo.hitInfo.ChangeAdaptiveDamageAttribute(this.owner);
				}
				Damage damage = this.owner.stat.GetDamage((double)this._attackDamage.amount, raycastHit.point, attackInfo.hitInfo);
				if (attackInfo.hitInfo.attackType != Damage.AttackType.None)
				{
					CommonResource.instance.hitParticle.Emit(target.transform.position, target.collider.bounds, force, true);
				}
				if (this.owner.TryAttackCharacter(target, ref damage))
				{
					this.owner.StartCoroutine(attackInfo.operationsToCharacter.CRun(this.owner, target.character));
					attackInfo.effect.Spawn(this.owner, this._collisionDetector.collider, origin, direction, distance, raycastHit, damage, target);
				}
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

		// Token: 0x06004D9B RID: 19867 RVA: 0x000E7C1C File Offset: 0x000E5E1C
		protected virtual IEnumerator CAttack(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
		{
			int index = 0;
			float time = 0f;
			Vector3 originOffset = MMMaths.Vector2ToVector3(origin) - target.transform.position;
			Vector3 hitPointOffset = MMMaths.Vector2ToVector3(raycastHit.point) - target.transform.position;
			while (this != null && index < this._attackAndEffect.components.Length)
			{
				CastAttackInfoSequence castAttackInfoSequence;
				while (index < this._attackAndEffect.components.Length && time >= (castAttackInfoSequence = this._attackAndEffect.components[index]).timeToTrigger)
				{
					raycastHit.point = target.transform.position + hitPointOffset;
					this.Attack(castAttackInfoSequence.attackInfo, target.transform.position + originOffset, direction, distance, raycastHit, target);
					int num = index;
					index = num + 1;
				}
				yield return null;
				time += this.owner.chronometer.animation.deltaTime;
			}
			yield break;
		}

		// Token: 0x06004D9D RID: 19869 RVA: 0x000E7C5F File Offset: 0x000E5E5F
		[CompilerGenerated]
		private void <Awake>g__onTerrainHit|25_0(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit)
		{
			base.StartCoroutine(this._onTerrainHit.CRun(this.owner));
		}

		// Token: 0x06004D9E RID: 19870 RVA: 0x000E7C79 File Offset: 0x000E5E79
		[CompilerGenerated]
		private void <Awake>g__onTargetHit|25_1(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
		{
			target.StartCoroutine(this.CAttack(origin, direction, distance, raycastHit, target));
		}

		// Token: 0x06004D9F RID: 19871 RVA: 0x000E7C90 File Offset: 0x000E5E90
		[CompilerGenerated]
		private void <Awake>g__onTargetHitWithoutDelay|25_2(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
		{
			foreach (CastAttackInfoSequence castAttackInfoSequence in this._attackAndEffect.components)
			{
				this.Attack(castAttackInfoSequence.attackInfo, origin, direction, distance, raycastHit, target);
			}
		}

		// Token: 0x04003D61 RID: 15713
		[SerializeField]
		private bool _adaptiveForce;

		// Token: 0x04003D62 RID: 15714
		[SerializeField]
		private float _duration;

		// Token: 0x04003D63 RID: 15715
		[SerializeField]
		private bool _timeIndependent;

		// Token: 0x04003D64 RID: 15716
		[SerializeField]
		private bool _trackMovement = true;

		// Token: 0x04003D65 RID: 15717
		[SerializeField]
		private SweepAttack2.CollisionDetector _collisionDetector;

		// Token: 0x04003D66 RID: 15718
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onTerrainHit;

		// Token: 0x04003D67 RID: 15719
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(CastAttackInfoSequence))]
		protected CastAttackInfoSequence.Subcomponents _attackAndEffect;

		// Token: 0x04003D68 RID: 15720
		private IAttackDamage _attackDamage;

		// Token: 0x04003D69 RID: 15721
		private CoroutineReference _detectReference;

		// Token: 0x02000F9B RID: 3995
		[Serializable]
		public class CollisionDetector
		{
			// Token: 0x140000C1 RID: 193
			// (add) Token: 0x06004DA0 RID: 19872 RVA: 0x000E7CD0 File Offset: 0x000E5ED0
			// (remove) Token: 0x06004DA1 RID: 19873 RVA: 0x000E7D08 File Offset: 0x000E5F08
			public event SweepAttack2.CollisionDetector.onTerrainHitDelegate onTerrainHit;

			// Token: 0x140000C2 RID: 194
			// (add) Token: 0x06004DA2 RID: 19874 RVA: 0x000E7D40 File Offset: 0x000E5F40
			// (remove) Token: 0x06004DA3 RID: 19875 RVA: 0x000E7D78 File Offset: 0x000E5F78
			public event SweepAttack2.CollisionDetector.onTargetHitDelegate onHit;

			// Token: 0x17000F6C RID: 3948
			// (get) Token: 0x06004DA4 RID: 19876 RVA: 0x000E7DAD File Offset: 0x000E5FAD
			// (set) Token: 0x06004DA5 RID: 19877 RVA: 0x000E7DB5 File Offset: 0x000E5FB5
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

			// Token: 0x06004DA6 RID: 19878 RVA: 0x000E7DC0 File Offset: 0x000E5FC0
			internal void Initialize(SweepAttack2 sweepAttack)
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

			// Token: 0x06004DA7 RID: 19879 RVA: 0x000E7E29 File Offset: 0x000E6029
			internal void Detect(Vector2 origin, Vector2 distance)
			{
				this.Detect(origin, distance.normalized, distance.magnitude);
			}

			// Token: 0x06004DA8 RID: 19880 RVA: 0x000E7E40 File Offset: 0x000E6040
			internal void Detect(Vector2 origin, Vector2 direction, float distance)
			{
				SweepAttack2.CollisionDetector.<>c__DisplayClass25_0 CS$<>8__locals1;
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.origin = origin;
				CS$<>8__locals1.direction = direction;
				CS$<>8__locals1.distance = distance;
				SweepAttack2.CollisionDetector._caster.contactFilter.SetLayerMask(this._filter.layerMask);
				if (this._collider != null)
				{
					if (this._optimizedCollider)
					{
						this._collider.enabled = true;
						SweepAttack2.CollisionDetector._caster.ColliderCast(this._collider, CS$<>8__locals1.direction, CS$<>8__locals1.distance);
						this._collider.enabled = false;
					}
					else
					{
						SweepAttack2.CollisionDetector._caster.ColliderCast(this._collider, CS$<>8__locals1.direction, CS$<>8__locals1.distance);
					}
				}
				else
				{
					SweepAttack2.CollisionDetector._caster.RayCast(CS$<>8__locals1.origin, CS$<>8__locals1.direction, CS$<>8__locals1.distance);
				}
				for (int i = 0; i < SweepAttack2.CollisionDetector._caster.results.Count; i++)
				{
					SweepAttack2.CollisionDetector.<>c__DisplayClass25_1 CS$<>8__locals2;
					CS$<>8__locals2.result = SweepAttack2.CollisionDetector._caster.results[i];
					this.<Detect>g__HandleResult|25_0(ref CS$<>8__locals1, ref CS$<>8__locals2);
					if (this._hits.Count - this._propPenetratingHits >= this._maxHits)
					{
						this._sweepAttack.Stop();
					}
				}
			}

			// Token: 0x06004DAB RID: 19883 RVA: 0x000E7FE8 File Offset: 0x000E61E8
			[CompilerGenerated]
			private void <Detect>g__HandleResult|25_0(ref SweepAttack2.CollisionDetector.<>c__DisplayClass25_0 A_1, ref SweepAttack2.CollisionDetector.<>c__DisplayClass25_1 A_2)
			{
				if (this._terrainLayer.Contains(A_2.result.collider.gameObject.layer))
				{
					this.onTerrainHit(A_1.origin, A_1.direction, A_1.distance, A_2.result);
					return;
				}
				A_1.target = A_2.result.collider.GetComponent<Target>();
				if (A_1.target == null)
				{
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

			// Token: 0x04003D6C RID: 15724
			private static readonly NonAllocCaster _caster = new NonAllocCaster(99);

			// Token: 0x04003D6F RID: 15727
			private SweepAttack2 _sweepAttack;

			// Token: 0x04003D70 RID: 15728
			[SerializeField]
			private TargetLayer _layer = new TargetLayer(2048, false, true, false, false);

			// Token: 0x04003D71 RID: 15729
			[SerializeField]
			private LayerMask _terrainLayer = Layers.groundMask;

			// Token: 0x04003D72 RID: 15730
			[SerializeField]
			private Collider2D _collider;

			// Token: 0x04003D73 RID: 15731
			[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
			[SerializeField]
			private bool _optimizedCollider = true;

			// Token: 0x04003D74 RID: 15732
			[SerializeField]
			private int _maxHits = 512;

			// Token: 0x04003D75 RID: 15733
			[SerializeField]
			private int _maxHitsPerUnit = 1;

			// Token: 0x04003D76 RID: 15734
			[SerializeField]
			private float _hitIntervalPerUnit = 0.5f;

			// Token: 0x04003D77 RID: 15735
			private HitHistoryManager _hits = new HitHistoryManager(32);

			// Token: 0x04003D78 RID: 15736
			private int _propPenetratingHits;

			// Token: 0x04003D79 RID: 15737
			private ContactFilter2D _filter;

			// Token: 0x02000F9C RID: 3996
			// (Invoke) Token: 0x06004DAD RID: 19885
			public delegate void onTerrainHitDelegate(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit);

			// Token: 0x02000F9D RID: 3997
			// (Invoke) Token: 0x06004DB1 RID: 19889
			public delegate void onTargetHitDelegate(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target);
		}
	}
}
