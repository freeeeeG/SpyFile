using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Characters.Operations;
using Characters.Projectiles.Movements;
using Characters.Projectiles.Operations;
using Characters.Utils;
using FX.ProjectileAttackVisualEffect;
using GameResources;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Projectiles
{
	// Token: 0x02000764 RID: 1892
	[RequireComponent(typeof(PoolObject))]
	public class Projectile : MonoBehaviour, IProjectile, IMonoBehaviour
	{
		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x060026F6 RID: 9974 RVA: 0x00075287 File Offset: 0x00073487
		// (set) Token: 0x060026F7 RID: 9975 RVA: 0x0007528F File Offset: 0x0007348F
		public Character owner { get; private set; }

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x060026F8 RID: 9976 RVA: 0x00075298 File Offset: 0x00073498
		public PoolObject reusable
		{
			get
			{
				return this._reusable;
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x060026F9 RID: 9977 RVA: 0x000752A0 File Offset: 0x000734A0
		// (set) Token: 0x060026FA RID: 9978 RVA: 0x000752A8 File Offset: 0x000734A8
		public float maxLifeTime
		{
			get
			{
				return this._maxLifeTime;
			}
			set
			{
				this._maxLifeTime = value;
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x060026FB RID: 9979 RVA: 0x000752B1 File Offset: 0x000734B1
		public Movement movement
		{
			get
			{
				return this._movement;
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x060026FC RID: 9980 RVA: 0x000752B9 File Offset: 0x000734B9
		public Collider2D collider
		{
			get
			{
				return this._collisionDetector.collider;
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x060026FD RID: 9981 RVA: 0x000752C6 File Offset: 0x000734C6
		// (set) Token: 0x060026FE RID: 9982 RVA: 0x000752CE File Offset: 0x000734CE
		public float baseDamage { get; private set; }

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x060026FF RID: 9983 RVA: 0x000752D7 File Offset: 0x000734D7
		// (set) Token: 0x06002700 RID: 9984 RVA: 0x000752DF File Offset: 0x000734DF
		public float speedMultiplier { get; private set; }

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06002701 RID: 9985 RVA: 0x000752E8 File Offset: 0x000734E8
		// (set) Token: 0x06002702 RID: 9986 RVA: 0x000752F0 File Offset: 0x000734F0
		public Vector2 firedDirection { get; private set; }

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06002703 RID: 9987 RVA: 0x000752F9 File Offset: 0x000734F9
		// (set) Token: 0x06002704 RID: 9988 RVA: 0x00075306 File Offset: 0x00073506
		public Vector2 direction
		{
			get
			{
				return this._movement.directionVector;
			}
			set
			{
				this._movement.directionVector = value;
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06002705 RID: 9989 RVA: 0x00075314 File Offset: 0x00073514
		// (set) Token: 0x06002706 RID: 9990 RVA: 0x0007531C File Offset: 0x0007351C
		public float speed { get; private set; }

		// Token: 0x06002707 RID: 9991 RVA: 0x00075328 File Offset: 0x00073528
		private void Awake()
		{
			if (this._operations == null)
			{
				this._operations = new Characters.Projectiles.Operations.OperationInfo.Subcomponents();
			}
			else
			{
				this._operations.Sort();
			}
			this._collisionDetector.onTerrainHit += this.<Awake>g__onTerrainHit|50_0;
			this._collisionDetector.onHit += this.<Awake>g__onTargetHit|50_1;
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x00075384 File Offset: 0x00073584
		private void OnDestroy()
		{
			this.owner = null;
			this._reusable = null;
			this._movement = null;
			this._collisionDetector.Dispose();
			this._collisionDetector = null;
			this._operations = null;
			this._onSpawned = null;
			this._onDespawn = null;
			this._onTerrainHit = null;
			this._onCharacterHit = null;
			this._effect = null;
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x000753E2 File Offset: 0x000735E2
		private IEnumerator CUpdate(float delay)
		{
			while (delay > 0f)
			{
				delay -= ((this.owner != null) ? this.owner.chronometer.projectile.deltaTime : Chronometer.global.deltaTime);
				yield return null;
			}
			this._time = 0f;
			while (this._time <= this._maxLifeTime)
			{
				float num = (this.owner != null) ? this.owner.chronometer.projectile.deltaTime : Chronometer.global.deltaTime;
				this._time += num;
				ValueTuple<Vector2, float> speed = this._movement.GetSpeed(this._time, num);
				this.firedDirection = speed.Item1;
				this.speed = speed.Item2;
				this.speed *= num;
				if (this._rotatable != null)
				{
					this._rotatable.transform.rotation = Quaternion.FromToRotation(Vector3.right, this.firedDirection);
				}
				if (this._time >= this._collisionTime && !this._disableCollisionDetect)
				{
					this._collisionDetector.Detect(base.transform.position, this.firedDirection, this.speed);
				}
				base.transform.Translate(this.firedDirection * this.speed, Space.World);
				yield return null;
			}
			this._effect.SpawnExpire(this);
			this.Despawn();
			yield break;
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x000753F8 File Offset: 0x000735F8
		public void Despawn()
		{
			for (int i = 0; i < this._onDespawn.components.Length; i++)
			{
				this._onDespawn.components[i].Run(this);
			}
			this._effect.SpawnDespawn(this);
			this._reusable.Despawn();
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x00075448 File Offset: 0x00073648
		public void Fire(Character owner, float attackDamage, float direction, bool flipX = false, bool flipY = false, float speedMultiplier = 1f, HitHistoryManager hitHistoryManager = null, float delay = 0f)
		{
			this.owner = owner;
			this._direction = direction;
			Vector3 localScale = base.transform.localScale;
			if (flipX)
			{
				localScale.x *= -1f;
			}
			if (flipY)
			{
				localScale.y *= -1f;
			}
			base.transform.localScale = localScale;
			this.baseDamage = attackDamage;
			this.speedMultiplier = speedMultiplier;
			base.gameObject.layer = (TargetLayer.IsPlayer(owner.gameObject.layer) ? 15 : 16);
			if (this._rotatable != null)
			{
				this._rotatable.transform.rotation = Quaternion.Euler(0f, 0f, this._direction);
			}
			this._movement.Initialize(this, this._direction);
			this._collisionDetector.Initialize(this);
			if (hitHistoryManager != null)
			{
				this.SetHitHistroyManager(hitHistoryManager);
			}
			for (int i = 0; i < this._onSpawned.components.Length; i++)
			{
				this._onSpawned.components[i].Run(this);
			}
			base.StartCoroutine(this._operations.CRun(this));
			base.StartCoroutine(this.CUpdate(delay));
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x0007557F File Offset: 0x0007377F
		public void ClearHitHistroy()
		{
			this._collisionDetector.hitHistoryManager.Clear();
		}

		// Token: 0x0600270D RID: 9997 RVA: 0x00075591 File Offset: 0x00073791
		public void SetHitHistroyManager(HitHistoryManager hitHistoryManager)
		{
			this._collisionDetector.hitHistoryManager = hitHistoryManager;
		}

		// Token: 0x0600270E RID: 9998 RVA: 0x0007559F File Offset: 0x0007379F
		public void DetectCollision(Vector2 origin, Vector2 direction, float speed)
		{
			this._collisionDetector.Detect(base.transform.position, direction, speed);
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x00073E7A File Offset: 0x0007207A
		public override string ToString()
		{
			return base.name;
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x00073E7A File Offset: 0x0007207A
		string IMonoBehaviour.get_name()
		{
			return base.name;
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x00073E9D File Offset: 0x0007209D
		GameObject IMonoBehaviour.get_gameObject()
		{
			return base.gameObject;
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x00070992 File Offset: 0x0006EB92
		Transform IMonoBehaviour.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x00073EA5 File Offset: 0x000720A5
		T IMonoBehaviour.GetComponent<T>()
		{
			return base.GetComponent<T>();
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x00073EAD File Offset: 0x000720AD
		T IMonoBehaviour.GetComponentInChildren<T>(bool includeInactive)
		{
			return base.GetComponentInChildren<T>(includeInactive);
		}

		// Token: 0x06002716 RID: 10006 RVA: 0x00073EB6 File Offset: 0x000720B6
		T IMonoBehaviour.GetComponentInChildren<T>()
		{
			return base.GetComponentInChildren<T>();
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x00073EBE File Offset: 0x000720BE
		T IMonoBehaviour.GetComponentInParent<T>()
		{
			return base.GetComponentInParent<T>();
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x00073EC6 File Offset: 0x000720C6
		T[] IMonoBehaviour.GetComponents<T>()
		{
			return base.GetComponents<T>();
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x00073ECE File Offset: 0x000720CE
		void IMonoBehaviour.GetComponents<T>(List<T> results)
		{
			base.GetComponents<T>(results);
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x00073ED7 File Offset: 0x000720D7
		void IMonoBehaviour.GetComponentsInChildren<T>(List<T> results)
		{
			base.GetComponentsInChildren<T>(results);
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x00073EE0 File Offset: 0x000720E0
		T[] IMonoBehaviour.GetComponentsInChildren<T>(bool includeInactive)
		{
			return base.GetComponentsInChildren<T>(includeInactive);
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x00073EE9 File Offset: 0x000720E9
		void IMonoBehaviour.GetComponentsInChildren<T>(bool includeInactive, List<T> result)
		{
			base.GetComponentsInChildren<T>(includeInactive, result);
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x00073EF3 File Offset: 0x000720F3
		T[] IMonoBehaviour.GetComponentsInChildren<T>()
		{
			return base.GetComponentsInChildren<T>();
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x00073EFB File Offset: 0x000720FB
		T[] IMonoBehaviour.GetComponentsInParent<T>()
		{
			return base.GetComponentsInParent<T>();
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x00073F03 File Offset: 0x00072103
		T[] IMonoBehaviour.GetComponentsInParent<T>(bool includeInactive)
		{
			return base.GetComponentsInParent<T>(includeInactive);
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x00073F0C File Offset: 0x0007210C
		void IMonoBehaviour.GetComponentsInParent<T>(bool includeInactive, List<T> results)
		{
			base.GetComponentsInParent<T>(includeInactive, results);
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x00073F16 File Offset: 0x00072116
		bool IMonoBehaviour.get_enabled()
		{
			return base.enabled;
		}

		// Token: 0x06002722 RID: 10018 RVA: 0x00073F1E File Offset: 0x0007211E
		void IMonoBehaviour.set_enabled(bool value)
		{
			base.enabled = value;
		}

		// Token: 0x06002723 RID: 10019 RVA: 0x00073F27 File Offset: 0x00072127
		bool IMonoBehaviour.get_isActiveAndEnabled()
		{
			return base.isActiveAndEnabled;
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x00073F2F File Offset: 0x0007212F
		Coroutine IMonoBehaviour.StartCoroutine(IEnumerator routine)
		{
			return base.StartCoroutine(routine);
		}

		// Token: 0x06002725 RID: 10021 RVA: 0x00048973 File Offset: 0x00046B73
		void IMonoBehaviour.StopAllCoroutines()
		{
			base.StopAllCoroutines();
		}

		// Token: 0x06002726 RID: 10022 RVA: 0x00073F38 File Offset: 0x00072138
		void IMonoBehaviour.StopCoroutine(IEnumerator routine)
		{
			base.StopCoroutine(routine);
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x00073F41 File Offset: 0x00072141
		void IMonoBehaviour.StopCoroutine(Coroutine routine)
		{
			base.StopCoroutine(routine);
		}

		// Token: 0x06002728 RID: 10024 RVA: 0x000755DC File Offset: 0x000737DC
		[CompilerGenerated]
		private void <Awake>g__onTerrainHit|50_0(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit)
		{
			for (int i = 0; i < this._onTerrainHit.components.Length; i++)
			{
				this._onTerrainHit.components[i].Run(this, raycastHit);
			}
			this._effect.Spawn(this, origin, direction, distance, raycastHit);
			if (this._despawnOnTerrainHit)
			{
				this.Despawn();
			}
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x00075638 File Offset: 0x00073838
		[CompilerGenerated]
		private void <Awake>g__onTargetHit|50_1(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
		{
			Damage projectileDamage = this.owner.stat.GetProjectileDamage(this, (double)this.baseDamage, raycastHit.point, this._hitInfo);
			Vector2 force = direction * this.speed * 10f;
			if (target.character != null)
			{
				if (target.character.liveAndActive && !target.character.cinematic.value && this.owner.TryAttackCharacter(target, ref projectileDamage))
				{
					for (int i = 0; i < this._onCharacterHit.components.Length; i++)
					{
						this._onCharacterHit.components[i].Run(this, raycastHit, target.character);
					}
					if (this._hitInfo.attackType != Damage.AttackType.None)
					{
						CommonResource.instance.hitParticle.Emit(target.transform.position, target.collider.bounds, force, true);
					}
					this._effect.Spawn(this, origin, direction, distance, raycastHit, projectileDamage, target);
					return;
				}
			}
			else if (target.damageable != null)
			{
				target.damageable.Hit(this.owner, ref projectileDamage, force);
				if (target.damageable.spawnEffectOnHit && this._hitInfo.attackType != Damage.AttackType.None)
				{
					CommonResource.instance.hitParticle.Emit(target.transform.position, target.collider.bounds, force, true);
					this._effect.Spawn(this, origin, direction, distance, raycastHit, projectileDamage, target);
				}
			}
		}

		// Token: 0x0400213C RID: 8508
		[GetComponent]
		[SerializeField]
		private PoolObject _reusable;

		// Token: 0x0400213D RID: 8509
		[SerializeField]
		private float _maxLifeTime;

		// Token: 0x0400213E RID: 8510
		[SerializeField]
		private float _collisionTime;

		// Token: 0x0400213F RID: 8511
		[SerializeField]
		private Transform _rotatable;

		// Token: 0x04002140 RID: 8512
		[SerializeField]
		private bool _disableCollisionDetect;

		// Token: 0x04002141 RID: 8513
		[SerializeField]
		private Projectile.CollisionDetector _collisionDetector;

		// Token: 0x04002142 RID: 8514
		[SerializeField]
		private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Projectile);

		// Token: 0x04002143 RID: 8515
		[Subcomponent(typeof(Characters.Projectiles.Operations.OperationInfo))]
		[Space]
		[SerializeField]
		private Characters.Projectiles.Operations.OperationInfo.Subcomponents _operations;

		// Token: 0x04002144 RID: 8516
		[Characters.Projectiles.Operations.Operation.SubcomponentAttribute]
		[SerializeField]
		[Space]
		private Characters.Projectiles.Operations.Operation.Subcomponents _onSpawned;

		// Token: 0x04002145 RID: 8517
		[SerializeField]
		[Characters.Projectiles.Operations.Operation.SubcomponentAttribute]
		private Characters.Projectiles.Operations.Operation.Subcomponents _onDespawn;

		// Token: 0x04002146 RID: 8518
		[SerializeField]
		private bool _despawnOnTerrainHit = true;

		// Token: 0x04002147 RID: 8519
		[HitOperation.SubcomponentAttribute]
		[SerializeField]
		private HitOperation.Subcomponents _onTerrainHit;

		// Token: 0x04002148 RID: 8520
		[SerializeField]
		[CharacterHitOperation.SubcomponentAttribute]
		private CharacterHitOperation.Subcomponents _onCharacterHit;

		// Token: 0x04002149 RID: 8521
		[SerializeField]
		[ProjectileAttackVisualEffect.SubcomponentAttribute]
		private ProjectileAttackVisualEffect.Subcomponents _effect;

		// Token: 0x0400214A RID: 8522
		[SerializeField]
		[Movement.SubcomponentAttribute]
		private Movement _movement;

		// Token: 0x0400214B RID: 8523
		private float _direction;

		// Token: 0x0400214C RID: 8524
		private float _time;

		// Token: 0x02000765 RID: 1893
		[Serializable]
		private class CollisionDetector
		{
			// Token: 0x14000061 RID: 97
			// (add) Token: 0x0600272A RID: 10026 RVA: 0x000757D4 File Offset: 0x000739D4
			// (remove) Token: 0x0600272B RID: 10027 RVA: 0x0007580C File Offset: 0x00073A0C
			public event Projectile.CollisionDetector.onTerrainHitDelegate onTerrainHit;

			// Token: 0x14000062 RID: 98
			// (add) Token: 0x0600272C RID: 10028 RVA: 0x00075844 File Offset: 0x00073A44
			// (remove) Token: 0x0600272D RID: 10029 RVA: 0x0007587C File Offset: 0x00073A7C
			public event Projectile.CollisionDetector.onTargetHitDelegate onHit;

			// Token: 0x17000824 RID: 2084
			// (get) Token: 0x0600272E RID: 10030 RVA: 0x000758B1 File Offset: 0x00073AB1
			public Collider2D collider
			{
				get
				{
					return this._collider;
				}
			}

			// Token: 0x06002730 RID: 10032 RVA: 0x000758C7 File Offset: 0x00073AC7
			internal void Initialize(Projectile projectile)
			{
				this.Initialize(projectile, this._internalHitHistory);
			}

			// Token: 0x06002731 RID: 10033 RVA: 0x000758D6 File Offset: 0x00073AD6
			internal void Initialize(Projectile projectile, HitHistoryManager hitHistory)
			{
				this._projectile = projectile;
				this.hitHistoryManager = hitHistory;
				this.hitHistoryManager.Clear();
				this._propPenetratingHits = 0;
				if (this._collider != null)
				{
					this._collider.enabled = false;
				}
			}

			// Token: 0x06002732 RID: 10034 RVA: 0x00075912 File Offset: 0x00073B12
			internal void Dispose()
			{
				this._projectile = null;
				this._collider = null;
			}

			// Token: 0x06002733 RID: 10035 RVA: 0x00075922 File Offset: 0x00073B22
			internal void SetHitHistoryManager(HitHistoryManager hitHistory)
			{
				this.hitHistoryManager = hitHistory;
			}

			// Token: 0x06002734 RID: 10036 RVA: 0x0007592C File Offset: 0x00073B2C
			internal void Detect(Vector2 origin, Vector2 direction, float distance)
			{
				if (this._projectile.owner == null)
				{
					return;
				}
				Projectile.CollisionDetector._caster.contactFilter.SetLayerMask(this._layer.Evaluate(this._projectile.owner.gameObject) | this._terrainLayer);
				if (this._collider != null)
				{
					this._collider.enabled = true;
					Projectile.CollisionDetector._caster.ColliderCast(this._collider, direction, distance);
					this._collider.enabled = false;
				}
				else
				{
					Projectile.CollisionDetector._caster.RayCast(origin, direction, distance);
				}
				for (int i = 0; i < Projectile.CollisionDetector._caster.results.Count; i++)
				{
					RaycastHit2D raycastHit = Projectile.CollisionDetector._caster.results[i];
					if (this._terrainLayer.Contains(raycastHit.collider.gameObject.layer))
					{
						this.onTerrainHit(origin, direction, distance, raycastHit);
						return;
					}
					Target component = raycastHit.collider.GetComponent<Target>();
					if (component == null)
					{
						Debug.LogError("Need a target component to: " + raycastHit.collider.name + "!");
					}
					else if (this.hitHistoryManager.CanAttack(component, this._maxHits + this._propPenetratingHits, this._maxHitsPerUnit, this._hitIntervalPerUnit))
					{
						if (component.character != null)
						{
							if (component.character == this._projectile.owner || !component.character.liveAndActive)
							{
								goto IL_20A;
							}
							this.onHit(origin, direction, distance, raycastHit, component);
							this.hitHistoryManager.AddOrUpdate(component);
						}
						else if (component.damageable != null)
						{
							this.onHit(origin, direction, distance, raycastHit, component);
							if (!component.damageable.blockCast)
							{
								this._propPenetratingHits++;
							}
							this.hitHistoryManager.AddOrUpdate(component);
						}
						if (this.hitHistoryManager.Count - this._propPenetratingHits >= this._maxHits)
						{
							this._projectile.Despawn();
							return;
						}
					}
					IL_20A:;
				}
			}

			// Token: 0x04002152 RID: 8530
			private const int maxHits = 99;

			// Token: 0x04002155 RID: 8533
			private Projectile _projectile;

			// Token: 0x04002156 RID: 8534
			[SerializeField]
			private TargetLayer _layer;

			// Token: 0x04002157 RID: 8535
			[SerializeField]
			private LayerMask _terrainLayer = Layers.terrainMaskForProjectile;

			// Token: 0x04002158 RID: 8536
			[SerializeField]
			private Collider2D _collider;

			// Token: 0x04002159 RID: 8537
			[Range(1f, 99f)]
			[SerializeField]
			private int _maxHits = 1;

			// Token: 0x0400215A RID: 8538
			[SerializeField]
			private int _maxHitsPerUnit = 1;

			// Token: 0x0400215B RID: 8539
			[SerializeField]
			private float _hitIntervalPerUnit = 0.5f;

			// Token: 0x0400215C RID: 8540
			internal HitHistoryManager hitHistoryManager;

			// Token: 0x0400215D RID: 8541
			private HitHistoryManager _internalHitHistory = new HitHistoryManager(99);

			// Token: 0x0400215E RID: 8542
			private int _propPenetratingHits;

			// Token: 0x0400215F RID: 8543
			private static readonly NonAllocCaster _caster = new NonAllocCaster(64);

			// Token: 0x02000766 RID: 1894
			// (Invoke) Token: 0x06002737 RID: 10039
			public delegate void onTerrainHitDelegate(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit);

			// Token: 0x02000767 RID: 1895
			// (Invoke) Token: 0x0600273B RID: 10043
			public delegate void onTargetHitDelegate(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target);
		}
	}
}
