using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Characters.Operations;
using Characters.Projectiles.Movements;
using Characters.Projectiles.Operations;
using Characters.Utils;
using FX.ProjectileAttackVisualEffect;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Projectiles
{
	// Token: 0x0200075F RID: 1887
	[RequireComponent(typeof(PoolObject))]
	public class WeaponMasterProjectile : MonoBehaviour, IProjectile, IMonoBehaviour
	{
		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x060026AD RID: 9901 RVA: 0x00074AED File Offset: 0x00072CED
		// (set) Token: 0x060026AE RID: 9902 RVA: 0x00074AF5 File Offset: 0x00072CF5
		public Character owner { get; private set; }

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x060026AF RID: 9903 RVA: 0x00074AFE File Offset: 0x00072CFE
		public PoolObject reusable
		{
			get
			{
				return this._reusable;
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x060026B0 RID: 9904 RVA: 0x00074B06 File Offset: 0x00072D06
		// (set) Token: 0x060026B1 RID: 9905 RVA: 0x00074B0E File Offset: 0x00072D0E
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

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x060026B2 RID: 9906 RVA: 0x00074B17 File Offset: 0x00072D17
		public Movement movement
		{
			get
			{
				return this._movement;
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x060026B3 RID: 9907 RVA: 0x00074B1F File Offset: 0x00072D1F
		public Collider2D collider
		{
			get
			{
				return this._collisionDetector.collider;
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x060026B4 RID: 9908 RVA: 0x00074B2C File Offset: 0x00072D2C
		// (set) Token: 0x060026B5 RID: 9909 RVA: 0x00074B34 File Offset: 0x00072D34
		public float baseDamage { get; private set; }

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x060026B6 RID: 9910 RVA: 0x00074B3D File Offset: 0x00072D3D
		// (set) Token: 0x060026B7 RID: 9911 RVA: 0x00074B45 File Offset: 0x00072D45
		public float speedMultiplier { get; private set; }

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x060026B8 RID: 9912 RVA: 0x00074B4E File Offset: 0x00072D4E
		// (set) Token: 0x060026B9 RID: 9913 RVA: 0x00074B56 File Offset: 0x00072D56
		public Vector2 firedDirection { get; private set; }

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x060026BA RID: 9914 RVA: 0x00074B5F File Offset: 0x00072D5F
		// (set) Token: 0x060026BB RID: 9915 RVA: 0x00074B6C File Offset: 0x00072D6C
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

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x060026BC RID: 9916 RVA: 0x00074B7A File Offset: 0x00072D7A
		// (set) Token: 0x060026BD RID: 9917 RVA: 0x00074B82 File Offset: 0x00072D82
		public float speed { get; private set; }

		// Token: 0x060026BE RID: 9918 RVA: 0x00074B8B File Offset: 0x00072D8B
		private void Awake()
		{
			this._collisionDetector.onHit += this.<Awake>g__onTargetHit|43_0;
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x00074BA4 File Offset: 0x00072DA4
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
				if (this._time >= this._collisionTime)
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

		// Token: 0x060026C0 RID: 9920 RVA: 0x00074BBA File Offset: 0x00072DBA
		public void Despawn()
		{
			this._effect.SpawnDespawn(this);
			this._reusable.Despawn();
		}

		// Token: 0x060026C1 RID: 9921 RVA: 0x00074BD4 File Offset: 0x00072DD4
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
			this._movement.Initialize(this, this._direction);
			this._collisionDetector.Initialize(this);
			if (hitHistoryManager != null)
			{
				this.SetHitHistroyManager(hitHistoryManager);
			}
			base.StartCoroutine(this.CUpdate(delay));
		}

		// Token: 0x060026C2 RID: 9922 RVA: 0x00074C9A File Offset: 0x00072E9A
		public void ClearHitHistroy()
		{
			this._collisionDetector.hitHistoryManager.Clear();
		}

		// Token: 0x060026C3 RID: 9923 RVA: 0x00074CAC File Offset: 0x00072EAC
		public void SetHitHistroyManager(HitHistoryManager hitHistoryManager)
		{
			this._collisionDetector.hitHistoryManager = hitHistoryManager;
		}

		// Token: 0x060026C4 RID: 9924 RVA: 0x00074CBA File Offset: 0x00072EBA
		public void DetectCollision(Vector2 origin, Vector2 direction, float speed)
		{
			this._collisionDetector.Detect(base.transform.position, direction, speed);
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x00073E7A File Offset: 0x0007207A
		public override string ToString()
		{
			return base.name;
		}

		// Token: 0x060026C7 RID: 9927 RVA: 0x00073E7A File Offset: 0x0007207A
		string IMonoBehaviour.get_name()
		{
			return base.name;
		}

		// Token: 0x060026C8 RID: 9928 RVA: 0x00073E9D File Offset: 0x0007209D
		GameObject IMonoBehaviour.get_gameObject()
		{
			return base.gameObject;
		}

		// Token: 0x060026C9 RID: 9929 RVA: 0x00070992 File Offset: 0x0006EB92
		Transform IMonoBehaviour.get_transform()
		{
			return base.transform;
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x00073EA5 File Offset: 0x000720A5
		T IMonoBehaviour.GetComponent<T>()
		{
			return base.GetComponent<T>();
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x00073EAD File Offset: 0x000720AD
		T IMonoBehaviour.GetComponentInChildren<T>(bool includeInactive)
		{
			return base.GetComponentInChildren<T>(includeInactive);
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x00073EB6 File Offset: 0x000720B6
		T IMonoBehaviour.GetComponentInChildren<T>()
		{
			return base.GetComponentInChildren<T>();
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x00073EBE File Offset: 0x000720BE
		T IMonoBehaviour.GetComponentInParent<T>()
		{
			return base.GetComponentInParent<T>();
		}

		// Token: 0x060026CE RID: 9934 RVA: 0x00073EC6 File Offset: 0x000720C6
		T[] IMonoBehaviour.GetComponents<T>()
		{
			return base.GetComponents<T>();
		}

		// Token: 0x060026CF RID: 9935 RVA: 0x00073ECE File Offset: 0x000720CE
		void IMonoBehaviour.GetComponents<T>(List<T> results)
		{
			base.GetComponents<T>(results);
		}

		// Token: 0x060026D0 RID: 9936 RVA: 0x00073ED7 File Offset: 0x000720D7
		void IMonoBehaviour.GetComponentsInChildren<T>(List<T> results)
		{
			base.GetComponentsInChildren<T>(results);
		}

		// Token: 0x060026D1 RID: 9937 RVA: 0x00073EE0 File Offset: 0x000720E0
		T[] IMonoBehaviour.GetComponentsInChildren<T>(bool includeInactive)
		{
			return base.GetComponentsInChildren<T>(includeInactive);
		}

		// Token: 0x060026D2 RID: 9938 RVA: 0x00073EE9 File Offset: 0x000720E9
		void IMonoBehaviour.GetComponentsInChildren<T>(bool includeInactive, List<T> result)
		{
			base.GetComponentsInChildren<T>(includeInactive, result);
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x00073EF3 File Offset: 0x000720F3
		T[] IMonoBehaviour.GetComponentsInChildren<T>()
		{
			return base.GetComponentsInChildren<T>();
		}

		// Token: 0x060026D4 RID: 9940 RVA: 0x00073EFB File Offset: 0x000720FB
		T[] IMonoBehaviour.GetComponentsInParent<T>()
		{
			return base.GetComponentsInParent<T>();
		}

		// Token: 0x060026D5 RID: 9941 RVA: 0x00073F03 File Offset: 0x00072103
		T[] IMonoBehaviour.GetComponentsInParent<T>(bool includeInactive)
		{
			return base.GetComponentsInParent<T>(includeInactive);
		}

		// Token: 0x060026D6 RID: 9942 RVA: 0x00073F0C File Offset: 0x0007210C
		void IMonoBehaviour.GetComponentsInParent<T>(bool includeInactive, List<T> results)
		{
			base.GetComponentsInParent<T>(includeInactive, results);
		}

		// Token: 0x060026D7 RID: 9943 RVA: 0x00073F16 File Offset: 0x00072116
		bool IMonoBehaviour.get_enabled()
		{
			return base.enabled;
		}

		// Token: 0x060026D8 RID: 9944 RVA: 0x00073F1E File Offset: 0x0007211E
		void IMonoBehaviour.set_enabled(bool value)
		{
			base.enabled = value;
		}

		// Token: 0x060026D9 RID: 9945 RVA: 0x00073F27 File Offset: 0x00072127
		bool IMonoBehaviour.get_isActiveAndEnabled()
		{
			return base.isActiveAndEnabled;
		}

		// Token: 0x060026DA RID: 9946 RVA: 0x00073F2F File Offset: 0x0007212F
		Coroutine IMonoBehaviour.StartCoroutine(IEnumerator routine)
		{
			return base.StartCoroutine(routine);
		}

		// Token: 0x060026DB RID: 9947 RVA: 0x00048973 File Offset: 0x00046B73
		void IMonoBehaviour.StopAllCoroutines()
		{
			base.StopAllCoroutines();
		}

		// Token: 0x060026DC RID: 9948 RVA: 0x00073F38 File Offset: 0x00072138
		void IMonoBehaviour.StopCoroutine(IEnumerator routine)
		{
			base.StopCoroutine(routine);
		}

		// Token: 0x060026DD RID: 9949 RVA: 0x00073F41 File Offset: 0x00072141
		void IMonoBehaviour.StopCoroutine(Coroutine routine)
		{
			base.StopCoroutine(routine);
		}

		// Token: 0x060026DE RID: 9950 RVA: 0x00074CF0 File Offset: 0x00072EF0
		[CompilerGenerated]
		private void <Awake>g__onTargetHit|43_0(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
		{
			Damage damage = this.owner.stat.GetDamage((double)this.baseDamage, raycastHit.point, this._hitInfo);
			direction * this.speed * 10f;
			if (target.character == null)
			{
				return;
			}
			if (target.character.liveAndActive && !target.character.invulnerable.value)
			{
				this.owner.Attack(target, ref damage);
				for (int i = 0; i < this._onCharacterHit.components.Length; i++)
				{
					this._onCharacterHit.components[i].Run(this, raycastHit, target.character);
				}
				this._effect.Spawn(this, origin, direction, distance, raycastHit, damage, target);
			}
		}

		// Token: 0x0400211D RID: 8477
		[SerializeField]
		[GetComponent]
		private PoolObject _reusable;

		// Token: 0x0400211E RID: 8478
		[SerializeField]
		private float _maxLifeTime;

		// Token: 0x0400211F RID: 8479
		[SerializeField]
		private float _collisionTime;

		// Token: 0x04002120 RID: 8480
		[SerializeField]
		private WeaponMasterProjectile.CollisionDetector _collisionDetector;

		// Token: 0x04002121 RID: 8481
		[SerializeField]
		private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Projectile);

		// Token: 0x04002122 RID: 8482
		[CharacterHitOperation.SubcomponentAttribute]
		[SerializeField]
		[Space]
		private CharacterHitOperation.Subcomponents _onCharacterHit;

		// Token: 0x04002123 RID: 8483
		[SerializeField]
		[ProjectileAttackVisualEffect.SubcomponentAttribute]
		private ProjectileAttackVisualEffect.Subcomponents _effect;

		// Token: 0x04002124 RID: 8484
		[SerializeField]
		[Movement.SubcomponentAttribute]
		private Movement _movement;

		// Token: 0x04002125 RID: 8485
		private float _direction;

		// Token: 0x04002126 RID: 8486
		private float _time;

		// Token: 0x02000760 RID: 1888
		[Serializable]
		private class CollisionDetector
		{
			// Token: 0x14000060 RID: 96
			// (add) Token: 0x060026DF RID: 9951 RVA: 0x00074DC4 File Offset: 0x00072FC4
			// (remove) Token: 0x060026E0 RID: 9952 RVA: 0x00074DFC File Offset: 0x00072FFC
			public event WeaponMasterProjectile.CollisionDetector.onTargetHitDelegate onHit;

			// Token: 0x17000817 RID: 2071
			// (get) Token: 0x060026E1 RID: 9953 RVA: 0x00074E31 File Offset: 0x00073031
			public Collider2D collider
			{
				get
				{
					return this._collider;
				}
			}

			// Token: 0x060026E3 RID: 9955 RVA: 0x00074E47 File Offset: 0x00073047
			internal void Initialize(WeaponMasterProjectile projectile)
			{
				this.Initialize(projectile, this._internalHitHistory);
			}

			// Token: 0x060026E4 RID: 9956 RVA: 0x00074E56 File Offset: 0x00073056
			internal void Initialize(WeaponMasterProjectile projectile, HitHistoryManager hitHistory)
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

			// Token: 0x060026E5 RID: 9957 RVA: 0x00074E92 File Offset: 0x00073092
			internal void SetHitHistoryManager(HitHistoryManager hitHistory)
			{
				this.hitHistoryManager = hitHistory;
			}

			// Token: 0x060026E6 RID: 9958 RVA: 0x00074E9C File Offset: 0x0007309C
			internal void Detect(Vector2 origin, Vector2 direction, float distance)
			{
				if (this._projectile.owner == null)
				{
					return;
				}
				WeaponMasterProjectile.CollisionDetector._caster.contactFilter.SetLayerMask(this._layer.Evaluate(this._projectile.owner.gameObject));
				if (this._collider != null)
				{
					this._collider.enabled = true;
					WeaponMasterProjectile.CollisionDetector._caster.ColliderCast(this._collider, direction, distance);
					this._collider.enabled = false;
				}
				else
				{
					WeaponMasterProjectile.CollisionDetector._caster.RayCast(origin, direction, distance);
				}
				for (int i = 0; i < WeaponMasterProjectile.CollisionDetector._caster.results.Count; i++)
				{
					RaycastHit2D raycastHit = WeaponMasterProjectile.CollisionDetector._caster.results[i];
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
								goto IL_1C6;
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
					IL_1C6:;
				}
			}

			// Token: 0x0400212C RID: 8492
			private const int maxHits = 99;

			// Token: 0x0400212E RID: 8494
			private WeaponMasterProjectile _projectile;

			// Token: 0x0400212F RID: 8495
			[SerializeField]
			private TargetLayer _layer;

			// Token: 0x04002130 RID: 8496
			[SerializeField]
			private Collider2D _collider;

			// Token: 0x04002131 RID: 8497
			[SerializeField]
			[Range(1f, 99f)]
			private int _maxHits = 1;

			// Token: 0x04002132 RID: 8498
			[SerializeField]
			private int _maxHitsPerUnit = 1;

			// Token: 0x04002133 RID: 8499
			[SerializeField]
			private float _hitIntervalPerUnit = 0.5f;

			// Token: 0x04002134 RID: 8500
			internal HitHistoryManager hitHistoryManager;

			// Token: 0x04002135 RID: 8501
			private HitHistoryManager _internalHitHistory = new HitHistoryManager(99);

			// Token: 0x04002136 RID: 8502
			private int _propPenetratingHits;

			// Token: 0x04002137 RID: 8503
			private static readonly NonAllocCaster _caster = new NonAllocCaster(64);

			// Token: 0x02000761 RID: 1889
			// (Invoke) Token: 0x060026E9 RID: 9961
			public delegate void onTerrainHitDelegate(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit);

			// Token: 0x02000762 RID: 1890
			// (Invoke) Token: 0x060026ED RID: 9965
			public delegate void onTargetHitDelegate(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target);
		}
	}
}
