using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters.Projectiles.Movements;
using Characters.Projectiles.Operations;
using Characters.Utils;
using FX.ProjectileAttackVisualEffect;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Projectiles.Customs
{
	// Token: 0x020007E4 RID: 2020
	public class TerrainCollisionDetector : MonoBehaviour
	{
		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x060028D3 RID: 10451 RVA: 0x0007C2DC File Offset: 0x0007A4DC
		// (set) Token: 0x060028D4 RID: 10452 RVA: 0x0007C2E4 File Offset: 0x0007A4E4
		public Vector2 direction { get; private set; }

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x060028D5 RID: 10453 RVA: 0x0007C2ED File Offset: 0x0007A4ED
		// (set) Token: 0x060028D6 RID: 10454 RVA: 0x0007C2F5 File Offset: 0x0007A4F5
		public float speed { get; private set; }

		// Token: 0x060028D7 RID: 10455 RVA: 0x0007C2FE File Offset: 0x0007A4FE
		private void Awake()
		{
			this._collisionDetector.onTerrainHit += this.<Awake>g__onTerrainHit|19_0;
		}

		// Token: 0x060028D8 RID: 10456 RVA: 0x0007C317 File Offset: 0x0007A517
		public void Run()
		{
			base.StartCoroutine(this.CUpdate());
		}

		// Token: 0x060028D9 RID: 10457 RVA: 0x0007C326 File Offset: 0x0007A526
		private IEnumerator CUpdate()
		{
			this._time = 0f;
			while (this._time <= this._maxLifeTime)
			{
				yield return null;
				float deltaTime = Chronometer.global.deltaTime;
				this._time += deltaTime;
				ValueTuple<Vector2, float> speed = this._movement.GetSpeed(this._time, deltaTime);
				this.direction = speed.Item1;
				this.speed = speed.Item2;
				this.speed *= deltaTime;
				if (this._time >= this._collisionTime)
				{
					this._collisionDetector.Detect(base.transform.position, this.direction, this.speed);
				}
			}
			this._effect.SpawnExpire(this._projectile);
			this.Despawn();
			yield break;
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x0007C338 File Offset: 0x0007A538
		internal void Despawn()
		{
			for (int i = 0; i < this._onDespawn.components.Length; i++)
			{
				this._onDespawn.components[i].Run(this._projectile);
			}
			this._effect.SpawnDespawn(this._projectile);
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x0007C388 File Offset: 0x0007A588
		[CompilerGenerated]
		private void <Awake>g__onTerrainHit|19_0(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit)
		{
			for (int i = 0; i < this._onTerrainHit.components.Length; i++)
			{
				this._onTerrainHit.components[i].Run(this._projectile, raycastHit);
			}
			this._effect.Spawn(this._projectile, origin, direction, distance, raycastHit);
		}

		// Token: 0x04002349 RID: 9033
		[SerializeField]
		private Projectile _projectile;

		// Token: 0x0400234A RID: 9034
		[SerializeField]
		private float _maxLifeTime;

		// Token: 0x0400234B RID: 9035
		[SerializeField]
		private float _collisionTime;

		// Token: 0x0400234C RID: 9036
		[SerializeField]
		private TerrainCollisionDetector.CollisionDetector _collisionDetector;

		// Token: 0x0400234D RID: 9037
		[Operation.SubcomponentAttribute]
		[SerializeField]
		private Operation.Subcomponents _onDespawn;

		// Token: 0x0400234E RID: 9038
		[HitOperation.SubcomponentAttribute]
		[SerializeField]
		private HitOperation.Subcomponents _onTerrainHit;

		// Token: 0x0400234F RID: 9039
		[SerializeField]
		[ProjectileAttackVisualEffect.SubcomponentAttribute]
		private ProjectileAttackVisualEffect.Subcomponents _effect;

		// Token: 0x04002350 RID: 9040
		[SerializeField]
		private Movement _movement;

		// Token: 0x04002351 RID: 9041
		private float _direction;

		// Token: 0x04002352 RID: 9042
		private float _time;

		// Token: 0x020007E5 RID: 2021
		[Serializable]
		private class CollisionDetector
		{
			// Token: 0x14000063 RID: 99
			// (add) Token: 0x060028DD RID: 10461 RVA: 0x0007C3E0 File Offset: 0x0007A5E0
			// (remove) Token: 0x060028DE RID: 10462 RVA: 0x0007C418 File Offset: 0x0007A618
			public event TerrainCollisionDetector.CollisionDetector.onTerrainHitDelegate onTerrainHit;

			// Token: 0x1700085B RID: 2139
			// (get) Token: 0x060028DF RID: 10463 RVA: 0x0007C44D File Offset: 0x0007A64D
			public Collider2D collider
			{
				get
				{
					return this._collider;
				}
			}

			// Token: 0x060028E1 RID: 10465 RVA: 0x0007C463 File Offset: 0x0007A663
			internal void Initialize(Projectile projectile)
			{
				this.Initialize(projectile, this._internalHitHistory);
			}

			// Token: 0x060028E2 RID: 10466 RVA: 0x0007C472 File Offset: 0x0007A672
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

			// Token: 0x060028E3 RID: 10467 RVA: 0x0007C4AE File Offset: 0x0007A6AE
			internal void SetHitHistoryManager(HitHistoryManager hitHistory)
			{
				this.hitHistoryManager = hitHistory;
			}

			// Token: 0x060028E4 RID: 10468 RVA: 0x0007C4B8 File Offset: 0x0007A6B8
			internal void Detect(Vector2 origin, Vector2 direction, float distance)
			{
				TerrainCollisionDetector.CollisionDetector._caster.contactFilter.SetLayerMask(this._terrainLayer);
				if (this._collider != null)
				{
					this._collider.enabled = true;
					TerrainCollisionDetector.CollisionDetector._caster.ColliderCast(this._collider, direction, distance);
					this._collider.enabled = false;
				}
				else
				{
					TerrainCollisionDetector.CollisionDetector._caster.RayCast(origin, direction, distance);
				}
				for (int i = 0; i < TerrainCollisionDetector.CollisionDetector._caster.results.Count; i++)
				{
					RaycastHit2D raycastHit = TerrainCollisionDetector.CollisionDetector._caster.results[i];
					if (this._terrainLayer.Contains(raycastHit.collider.gameObject.layer))
					{
						this.onTerrainHit(origin, direction, distance, raycastHit);
					}
					else
					{
						Target component = raycastHit.collider.GetComponent<Target>();
						if (component == null)
						{
							Debug.LogError("Need a target component to: " + raycastHit.collider.name + "!");
						}
						else if (this.hitHistoryManager.CanAttack(component, this._maxHits + this._propPenetratingHits, this._maxHitsPerUnit, this._hitIntervalPerUnit) && this.hitHistoryManager.Count - this._propPenetratingHits >= this._maxHits)
						{
							this._projectile.Despawn();
							return;
						}
					}
				}
			}

			// Token: 0x04002355 RID: 9045
			private const int maxHits = 99;

			// Token: 0x04002357 RID: 9047
			private Projectile _projectile;

			// Token: 0x04002358 RID: 9048
			[SerializeField]
			private TargetLayer _layer;

			// Token: 0x04002359 RID: 9049
			[SerializeField]
			private LayerMask _terrainLayer = Layers.terrainMaskForProjectile;

			// Token: 0x0400235A RID: 9050
			[SerializeField]
			private Collider2D _collider;

			// Token: 0x0400235B RID: 9051
			[SerializeField]
			[Range(1f, 99f)]
			private int _maxHits = 1;

			// Token: 0x0400235C RID: 9052
			[SerializeField]
			private int _maxHitsPerUnit = 1;

			// Token: 0x0400235D RID: 9053
			[SerializeField]
			private float _hitIntervalPerUnit = 0.5f;

			// Token: 0x0400235E RID: 9054
			internal HitHistoryManager hitHistoryManager;

			// Token: 0x0400235F RID: 9055
			private HitHistoryManager _internalHitHistory = new HitHistoryManager(99);

			// Token: 0x04002360 RID: 9056
			private int _propPenetratingHits;

			// Token: 0x04002361 RID: 9057
			private static readonly NonAllocCaster _caster = new NonAllocCaster(15);

			// Token: 0x020007E6 RID: 2022
			// (Invoke) Token: 0x060028E7 RID: 10471
			public delegate void onTerrainHitDelegate(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit);

			// Token: 0x020007E7 RID: 2023
			// (Invoke) Token: 0x060028EB RID: 10475
			public delegate void onTargetHitDelegate(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target);
		}
	}
}
