using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters.Movements;
using Characters.Operations.Movement;
using Characters.Utils;
using FX.CastAttackVisualEffect;
using GameResources;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack
{
	// Token: 0x02000F95 RID: 3989
	public class SweepAttack : CharacterOperation, IAttack
	{
		// Token: 0x140000BF RID: 191
		// (add) Token: 0x06004D68 RID: 19816 RVA: 0x000E6F24 File Offset: 0x000E5124
		// (remove) Token: 0x06004D69 RID: 19817 RVA: 0x000E6F5C File Offset: 0x000E515C
		public event OnAttackHitDelegate onHit;

		// Token: 0x17000F61 RID: 3937
		// (get) Token: 0x06004D6A RID: 19818 RVA: 0x000E6F91 File Offset: 0x000E5191
		// (set) Token: 0x06004D6B RID: 19819 RVA: 0x000E6F99 File Offset: 0x000E5199
		internal Character owner { get; private set; }

		// Token: 0x17000F62 RID: 3938
		// (get) Token: 0x06004D6C RID: 19820 RVA: 0x000E6FA2 File Offset: 0x000E51A2
		public SweepAttack.CollisionDetector collisionDetector
		{
			get
			{
				return this._collisionDetector;
			}
		}

		// Token: 0x17000F63 RID: 3939
		// (get) Token: 0x06004D6D RID: 19821 RVA: 0x000E6FAA File Offset: 0x000E51AA
		public HitInfo hitInfo
		{
			get
			{
				return this._hitInfo;
			}
		}

		// Token: 0x17000F64 RID: 3940
		// (get) Token: 0x06004D6E RID: 19822 RVA: 0x000E6FB2 File Offset: 0x000E51B2
		// (set) Token: 0x06004D6F RID: 19823 RVA: 0x000E6FBA File Offset: 0x000E51BA
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

		// Token: 0x06004D70 RID: 19824 RVA: 0x000E6FC4 File Offset: 0x000E51C4
		private void Awake()
		{
			if (this._duration == 0f)
			{
				this._duration = float.PositiveInfinity;
			}
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			this._onTerrainHit.Initialize();
			this._onCharacterHit.Initialize();
			foreach (TargetedOperationInfo targetedOperationInfo in this._onCharacterHit.components)
			{
				Knockback knockback = targetedOperationInfo.operation as Knockback;
				if (knockback != null)
				{
					this._pushInfo = knockback.pushInfo;
					break;
				}
				Smash smash = targetedOperationInfo.operation as Smash;
				if (smash != null)
				{
					this._pushInfo = smash.pushInfo;
				}
			}
			this._collisionDetector.onTerrainHit = new SweepAttack.CollisionDetector.onTerrainHitDelegate(this.<Awake>g__onTerrainHit|31_0);
			this._collisionDetector.onHit = new SweepAttack.CollisionDetector.onTargetHitDelegate(this.<Awake>g__onTargetHit|31_1);
		}

		// Token: 0x06004D71 RID: 19825 RVA: 0x000E7092 File Offset: 0x000E5292
		public override void Run(Character owner)
		{
			this.owner = owner;
			this._collisionDetector.Initialize(this);
			this._detectReference.Stop();
			this._detectReference = owner.StartCoroutineWithReference(this.CDetect());
		}

		// Token: 0x06004D72 RID: 19826 RVA: 0x000E70C4 File Offset: 0x000E52C4
		public override void Stop()
		{
			base.Stop();
			this._operationToOwnerWhenHitInfo.StopAll();
			this._detectReference.Stop();
		}

		// Token: 0x06004D73 RID: 19827 RVA: 0x000E70E2 File Offset: 0x000E52E2
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

		// Token: 0x06004D75 RID: 19829 RVA: 0x000E710C File Offset: 0x000E530C
		[CompilerGenerated]
		private void <Awake>g__onTerrainHit|31_0(Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit)
		{
			base.StartCoroutine(this._onTerrainHit.CRun(this.owner));
			this._effect.Spawn(this.owner, collider, origin, direction, distance, raycastHit);
		}

		// Token: 0x06004D76 RID: 19830 RVA: 0x000E7140 File Offset: 0x000E5340
		[CompilerGenerated]
		private void <Awake>g__onTargetHit|31_1(Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
		{
			if (this.owner == null)
			{
				Debug.LogError("owner is null");
			}
			if (this._adaptiveForce)
			{
				this._hitInfo.ChangeAdaptiveDamageAttribute(this.owner);
			}
			Damage damage = this.owner.stat.GetDamage((double)this._attackDamage.amount, raycastHit.point, this._hitInfo);
			Vector2 force = Vector2.zero;
			if (this._pushInfo != null)
			{
				ValueTuple<Vector2, Vector2> valueTuple = this._pushInfo.EvaluateTimeIndependent(this.owner, target);
				force = valueTuple.Item1 + valueTuple.Item2;
			}
			if (target.character != null)
			{
				if (target.character.cinematic.value)
				{
					return;
				}
				this._chronoToGlobe.ApplyGlobe();
				this._chronoToOwner.ApplyTo(this.owner);
				this._chronoToTarget.ApplyTo(target.character);
				if (this._operationToOwnerWhenHitInfo.components.Length != 0)
				{
					base.StartCoroutine(this._operationToOwnerWhenHitInfo.CRun(this.owner));
				}
				if (this._hitInfo.attackType != Damage.AttackType.None)
				{
					CommonResource.instance.hitParticle.Emit(target.transform.position, target.collider.bounds, force, true);
				}
				if (this.owner.TryAttackCharacter(target, ref damage))
				{
					base.StartCoroutine(this._onCharacterHit.CRun(this.owner, target.character));
					OnAttackHitDelegate onAttackHitDelegate = this.onHit;
					if (onAttackHitDelegate != null)
					{
						onAttackHitDelegate(target, ref damage);
					}
					this._effect.Spawn(this.owner, collider, origin, direction, distance, raycastHit, damage, target);
					return;
				}
			}
			else if (target.damageable != null)
			{
				if (target.damageable.spawnEffectOnHit && this._hitInfo.attackType != Damage.AttackType.None)
				{
					CommonResource.instance.hitParticle.Emit(target.transform.position, target.collider.bounds, force, true);
					this._effect.Spawn(this.owner, collider, origin, direction, distance, raycastHit, damage, target);
				}
				if (this._hitInfo.attackType == Damage.AttackType.None)
				{
					return;
				}
				target.damageable.Hit(this.owner, ref damage, force);
			}
		}

		// Token: 0x04003D3B RID: 15675
		[SerializeField]
		private bool _adaptiveForce;

		// Token: 0x04003D3C RID: 15676
		[SerializeField]
		private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Melee);

		// Token: 0x04003D3D RID: 15677
		[SerializeField]
		private ChronoInfo _chronoToGlobe;

		// Token: 0x04003D3E RID: 15678
		[SerializeField]
		private ChronoInfo _chronoToOwner;

		// Token: 0x04003D3F RID: 15679
		[SerializeField]
		private ChronoInfo _chronoToTarget;

		// Token: 0x04003D40 RID: 15680
		[SerializeField]
		private float _duration;

		// Token: 0x04003D41 RID: 15681
		[SerializeField]
		private bool _timeIndependent;

		// Token: 0x04003D42 RID: 15682
		[SerializeField]
		private bool _trackMovement = true;

		// Token: 0x04003D43 RID: 15683
		[SerializeField]
		private SweepAttack.CollisionDetector _collisionDetector;

		// Token: 0x04003D44 RID: 15684
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _operationToOwnerWhenHitInfo;

		// Token: 0x04003D45 RID: 15685
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onTerrainHit;

		// Token: 0x04003D46 RID: 15686
		[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
		[SerializeField]
		private TargetedOperationInfo.Subcomponents _onCharacterHit;

		// Token: 0x04003D47 RID: 15687
		[CastAttackVisualEffect.SubcomponentAttribute]
		[SerializeField]
		private CastAttackVisualEffect.Subcomponents _effect;

		// Token: 0x04003D48 RID: 15688
		private CoroutineReference _detectReference;

		// Token: 0x04003D49 RID: 15689
		private PushInfo _pushInfo;

		// Token: 0x04003D4A RID: 15690
		private IAttackDamage _attackDamage;

		// Token: 0x02000F96 RID: 3990
		[Serializable]
		public class CollisionDetector
		{
			// Token: 0x17000F65 RID: 3941
			// (get) Token: 0x06004D77 RID: 19831 RVA: 0x000E738E File Offset: 0x000E558E
			// (set) Token: 0x06004D78 RID: 19832 RVA: 0x000E7396 File Offset: 0x000E5596
			public HitHistoryManager hits
			{
				get
				{
					return this._hits;
				}
				set
				{
					this._hits = value;
				}
			}

			// Token: 0x06004D7A RID: 19834 RVA: 0x000E73B0 File Offset: 0x000E55B0
			internal void Initialize(SweepAttack sweepAttack)
			{
				this._sweepAttack = sweepAttack;
				this._filter.layerMask = this._layer.Evaluate(sweepAttack.owner.gameObject);
				if (!this.group)
				{
					this._hits.Clear();
				}
				this._propPenetratingHits = 0;
				if (this._optimizedCollider && this._collider != null)
				{
					this._collider.enabled = false;
				}
				if (this._maxHitsPerUnit == 0)
				{
					this._maxHitsPerUnit = int.MaxValue;
				}
			}

			// Token: 0x06004D7B RID: 19835 RVA: 0x000E7434 File Offset: 0x000E5634
			internal void Detect(Vector2 origin, Vector2 distance)
			{
				this.Detect(origin, distance.normalized, distance.magnitude);
			}

			// Token: 0x06004D7C RID: 19836 RVA: 0x000E744C File Offset: 0x000E564C
			internal void Detect(Vector2 origin, Vector2 direction, float distance)
			{
				SweepAttack.CollisionDetector._caster.contactFilter.SetLayerMask(this._filter.layerMask);
				if (this._collider)
				{
					if (this._optimizedCollider)
					{
						this._collider.enabled = true;
					}
					SweepAttack.CollisionDetector._caster.ColliderCast(this._collider, direction, distance);
					if (this._optimizedCollider)
					{
						this._collider.enabled = false;
					}
				}
				else
				{
					SweepAttack.CollisionDetector._caster.RayCast(origin, direction, distance);
				}
				int i = 0;
				while (i < SweepAttack.CollisionDetector._caster.results.Count)
				{
					RaycastHit2D raycastHit = SweepAttack.CollisionDetector._caster.results[i];
					if (this._terrainLayer.Contains(raycastHit.collider.gameObject.layer))
					{
						this.onTerrainHit(this._collider, origin, direction, distance, raycastHit);
						goto IL_1AE;
					}
					Target component = raycastHit.collider.GetComponent<Target>();
					if (!(component == null) && this._hits.CanAttack(component, this._maxHits, this._maxHitsPerUnit, this._hitIntervalPerUnit))
					{
						if (component.character != null)
						{
							if ((this._selfTarget || !(component.character == this._sweepAttack.owner)) && component.character.liveAndActive)
							{
								this.onHit(this._collider, origin, direction, distance, raycastHit, component);
								this._hits.AddOrUpdate(component);
								goto IL_1AE;
							}
						}
						else
						{
							if (component.damageable != null)
							{
								this.onHit(this._collider, origin, direction, distance, raycastHit, component);
								if (!component.damageable.blockCast)
								{
									this._propPenetratingHits++;
								}
								this._hits.AddOrUpdate(component);
								goto IL_1AE;
							}
							goto IL_1AE;
						}
					}
					IL_1D3:
					i++;
					continue;
					IL_1AE:
					if (this._hits.Count - this._propPenetratingHits >= this._maxHits)
					{
						this._sweepAttack.Stop();
						goto IL_1D3;
					}
					goto IL_1D3;
				}
			}

			// Token: 0x04003D4C RID: 15692
			public SweepAttack.CollisionDetector.onTerrainHitDelegate onTerrainHit;

			// Token: 0x04003D4D RID: 15693
			public SweepAttack.CollisionDetector.onTargetHitDelegate onHit;

			// Token: 0x04003D4E RID: 15694
			private SweepAttack _sweepAttack;

			// Token: 0x04003D4F RID: 15695
			[SerializeField]
			private TargetLayer _layer = new TargetLayer(2048, false, true, false, false);

			// Token: 0x04003D50 RID: 15696
			[SerializeField]
			private bool _selfTarget;

			// Token: 0x04003D51 RID: 15697
			[SerializeField]
			private LayerMask _terrainLayer = Layers.groundMask;

			// Token: 0x04003D52 RID: 15698
			[SerializeField]
			private Collider2D _collider;

			// Token: 0x04003D53 RID: 15699
			[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
			[SerializeField]
			private bool _optimizedCollider = true;

			// Token: 0x04003D54 RID: 15700
			[SerializeField]
			private int _maxHits = 512;

			// Token: 0x04003D55 RID: 15701
			[SerializeField]
			private int _maxHitsPerUnit = 1;

			// Token: 0x04003D56 RID: 15702
			[SerializeField]
			private float _hitIntervalPerUnit = 0.5f;

			// Token: 0x04003D57 RID: 15703
			private HitHistoryManager _hits = new HitHistoryManager(32);

			// Token: 0x04003D58 RID: 15704
			private int _propPenetratingHits;

			// Token: 0x04003D59 RID: 15705
			private ContactFilter2D _filter;

			// Token: 0x04003D5A RID: 15706
			private static readonly NonAllocCaster _caster = new NonAllocCaster(99);

			// Token: 0x04003D5B RID: 15707
			[NonSerialized]
			public bool group;

			// Token: 0x02000F97 RID: 3991
			// (Invoke) Token: 0x06004D7F RID: 19839
			public delegate void onTerrainHitDelegate(Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit);

			// Token: 0x02000F98 RID: 3992
			// (Invoke) Token: 0x06004D83 RID: 19843
			public delegate void onTargetHitDelegate(Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target);
		}
	}
}
