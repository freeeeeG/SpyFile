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
	// Token: 0x02000756 RID: 1878
	[RequireComponent(typeof(PoolObject))]
	public class BouncyProjectile : MonoBehaviour, IProjectile, IMonoBehaviour
	{
		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06002646 RID: 9798 RVA: 0x00073BAC File Offset: 0x00071DAC
		// (set) Token: 0x06002647 RID: 9799 RVA: 0x00073BB4 File Offset: 0x00071DB4
		public Character owner { get; private set; }

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06002648 RID: 9800 RVA: 0x00073BBD File Offset: 0x00071DBD
		public PoolObject reusable
		{
			get
			{
				return this._reusable;
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06002649 RID: 9801 RVA: 0x00073BC5 File Offset: 0x00071DC5
		// (set) Token: 0x0600264A RID: 9802 RVA: 0x00073BCD File Offset: 0x00071DCD
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

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x0600264B RID: 9803 RVA: 0x00073BD6 File Offset: 0x00071DD6
		public Collider2D collider
		{
			get
			{
				return this._collisionDetector.collider;
			}
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x0600264C RID: 9804 RVA: 0x00073BE3 File Offset: 0x00071DE3
		// (set) Token: 0x0600264D RID: 9805 RVA: 0x00073BEB File Offset: 0x00071DEB
		public float baseDamage { get; private set; }

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x0600264E RID: 9806 RVA: 0x00073BF4 File Offset: 0x00071DF4
		// (set) Token: 0x0600264F RID: 9807 RVA: 0x00073BFC File Offset: 0x00071DFC
		public float speedMultiplier { get; set; }

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06002650 RID: 9808 RVA: 0x00073C05 File Offset: 0x00071E05
		// (set) Token: 0x06002651 RID: 9809 RVA: 0x00073C0D File Offset: 0x00071E0D
		public Vector2 firedDirection { get; private set; }

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06002652 RID: 9810 RVA: 0x00073C16 File Offset: 0x00071E16
		// (set) Token: 0x06002653 RID: 9811 RVA: 0x00073C23 File Offset: 0x00071E23
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

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06002654 RID: 9812 RVA: 0x00073C31 File Offset: 0x00071E31
		// (set) Token: 0x06002655 RID: 9813 RVA: 0x00073C39 File Offset: 0x00071E39
		public float speed { get; private set; }

		// Token: 0x06002656 RID: 9814 RVA: 0x00073C44 File Offset: 0x00071E44
		private void Awake()
		{
			this._terrainDetector = new BouncyProjectile.TerrainDetector(this.collider, this._collisionDetector.terrainLayer);
			this._collisionDetector.onTerrainHit += this.<Awake>g__onTerrainHit|52_0;
			this._collisionDetector.onHit += this.<Awake>g__onTargetHit|52_1;
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x00073C9B File Offset: 0x00071E9B
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
				ValueTuple<Vector2, float> speed = this._movement.GetSpeed(num);
				this.firedDirection = speed.Item1;
				this.speed = speed.Item2;
				this.speed *= num;
				Vector2 firedDirection = this.firedDirection;
				if (this._rotatable != null)
				{
					this._rotatable.transform.rotation = Quaternion.FromToRotation(Vector3.right, firedDirection);
				}
				this._collisionDetector.Detect(base.transform.position, firedDirection, this.speed);
				this._collisionDetector.Trigger(base.transform.position, firedDirection, this.speed);
				if (this._time >= this._collisionTime && !this._disableCollisionDetect)
				{
					ValueTuple<bool, Vector2, bool, bool> valueTuple = this._terrainDetector.Cast(firedDirection, this.speed);
					if (valueTuple.Item1)
					{
						if (this._bounceHitPoint != null)
						{
							this._bounceHitPoint.position = valueTuple.Item2;
						}
						this._operationsOnBounce.Run(this.owner);
						if (valueTuple.Item4)
						{
							firedDirection.y *= -1f;
							this._movement.ySpeed = 0f;
						}
						if (valueTuple.Item3)
						{
							firedDirection.x *= -1f;
							this._movement.ySpeed = 0f;
						}
						this._movement.directionVector = firedDirection;
					}
				}
				base.transform.Translate(firedDirection * this.speed, Space.World);
				this.firedDirection = firedDirection;
				yield return null;
			}
			this._effect.SpawnExpire(this);
			this.Despawn();
			yield break;
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x00073CB4 File Offset: 0x00071EB4
		public void Despawn()
		{
			for (int i = 0; i < this._onDespawn.components.Length; i++)
			{
				this._onDespawn.components[i].Run(this);
			}
			this._effect.SpawnDespawn(this);
			this._reusable.Despawn();
		}

		// Token: 0x06002659 RID: 9817 RVA: 0x00073D04 File Offset: 0x00071F04
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

		// Token: 0x0600265A RID: 9818 RVA: 0x00073E3B File Offset: 0x0007203B
		public void ClearHitHistroy()
		{
			this._collisionDetector.hitHistoryManager.Clear();
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x00073E4D File Offset: 0x0007204D
		public void SetHitHistroyManager(HitHistoryManager hitHistoryManager)
		{
			this._collisionDetector.hitHistoryManager = hitHistoryManager;
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x00073E5B File Offset: 0x0007205B
		public void DetectCollision(Vector2 origin, Vector2 direction, float speed)
		{
			this._collisionDetector.Detect(base.transform.position, direction, speed);
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x00073E7A File Offset: 0x0007207A
		public override string ToString()
		{
			return base.name;
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x00073E7A File Offset: 0x0007207A
		string IMonoBehaviour.get_name()
		{
			return base.name;
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x00073E9D File Offset: 0x0007209D
		GameObject IMonoBehaviour.get_gameObject()
		{
			return base.gameObject;
		}

		// Token: 0x06002661 RID: 9825 RVA: 0x00070992 File Offset: 0x0006EB92
		Transform IMonoBehaviour.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x00073EA5 File Offset: 0x000720A5
		T IMonoBehaviour.GetComponent<T>()
		{
			return base.GetComponent<T>();
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x00073EAD File Offset: 0x000720AD
		T IMonoBehaviour.GetComponentInChildren<T>(bool includeInactive)
		{
			return base.GetComponentInChildren<T>(includeInactive);
		}

		// Token: 0x06002664 RID: 9828 RVA: 0x00073EB6 File Offset: 0x000720B6
		T IMonoBehaviour.GetComponentInChildren<T>()
		{
			return base.GetComponentInChildren<T>();
		}

		// Token: 0x06002665 RID: 9829 RVA: 0x00073EBE File Offset: 0x000720BE
		T IMonoBehaviour.GetComponentInParent<T>()
		{
			return base.GetComponentInParent<T>();
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x00073EC6 File Offset: 0x000720C6
		T[] IMonoBehaviour.GetComponents<T>()
		{
			return base.GetComponents<T>();
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x00073ECE File Offset: 0x000720CE
		void IMonoBehaviour.GetComponents<T>(List<T> results)
		{
			base.GetComponents<T>(results);
		}

		// Token: 0x06002668 RID: 9832 RVA: 0x00073ED7 File Offset: 0x000720D7
		void IMonoBehaviour.GetComponentsInChildren<T>(List<T> results)
		{
			base.GetComponentsInChildren<T>(results);
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x00073EE0 File Offset: 0x000720E0
		T[] IMonoBehaviour.GetComponentsInChildren<T>(bool includeInactive)
		{
			return base.GetComponentsInChildren<T>(includeInactive);
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x00073EE9 File Offset: 0x000720E9
		void IMonoBehaviour.GetComponentsInChildren<T>(bool includeInactive, List<T> result)
		{
			base.GetComponentsInChildren<T>(includeInactive, result);
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x00073EF3 File Offset: 0x000720F3
		T[] IMonoBehaviour.GetComponentsInChildren<T>()
		{
			return base.GetComponentsInChildren<T>();
		}

		// Token: 0x0600266C RID: 9836 RVA: 0x00073EFB File Offset: 0x000720FB
		T[] IMonoBehaviour.GetComponentsInParent<T>()
		{
			return base.GetComponentsInParent<T>();
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x00073F03 File Offset: 0x00072103
		T[] IMonoBehaviour.GetComponentsInParent<T>(bool includeInactive)
		{
			return base.GetComponentsInParent<T>(includeInactive);
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x00073F0C File Offset: 0x0007210C
		void IMonoBehaviour.GetComponentsInParent<T>(bool includeInactive, List<T> results)
		{
			base.GetComponentsInParent<T>(includeInactive, results);
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x00073F16 File Offset: 0x00072116
		bool IMonoBehaviour.get_enabled()
		{
			return base.enabled;
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x00073F1E File Offset: 0x0007211E
		void IMonoBehaviour.set_enabled(bool value)
		{
			base.enabled = value;
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x00073F27 File Offset: 0x00072127
		bool IMonoBehaviour.get_isActiveAndEnabled()
		{
			return base.isActiveAndEnabled;
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x00073F2F File Offset: 0x0007212F
		Coroutine IMonoBehaviour.StartCoroutine(IEnumerator routine)
		{
			return base.StartCoroutine(routine);
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x00048973 File Offset: 0x00046B73
		void IMonoBehaviour.StopAllCoroutines()
		{
			base.StopAllCoroutines();
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x00073F38 File Offset: 0x00072138
		void IMonoBehaviour.StopCoroutine(IEnumerator routine)
		{
			base.StopCoroutine(routine);
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x00073F41 File Offset: 0x00072141
		void IMonoBehaviour.StopCoroutine(Coroutine routine)
		{
			base.StopCoroutine(routine);
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x00073F4C File Offset: 0x0007214C
		[CompilerGenerated]
		private void <Awake>g__onTerrainHit|52_0(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit)
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

		// Token: 0x06002677 RID: 9847 RVA: 0x00073FA8 File Offset: 0x000721A8
		[CompilerGenerated]
		private void <Awake>g__onTargetHit|52_1(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
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

		// Token: 0x040020E4 RID: 8420
		[SerializeField]
		[GetComponent]
		private PoolObject _reusable;

		// Token: 0x040020E5 RID: 8421
		[SerializeField]
		private float _maxLifeTime;

		// Token: 0x040020E6 RID: 8422
		[SerializeField]
		private float _collisionTime;

		// Token: 0x040020E7 RID: 8423
		[SerializeField]
		private Transform _rotatable;

		// Token: 0x040020E8 RID: 8424
		[SerializeField]
		private bool _disableCollisionDetect;

		// Token: 0x040020E9 RID: 8425
		[SerializeField]
		private BouncyProjectile.CollisionDetector _collisionDetector;

		// Token: 0x040020EA RID: 8426
		[SerializeField]
		private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Projectile);

		// Token: 0x040020EB RID: 8427
		[Subcomponent(typeof(Characters.Projectiles.Operations.OperationInfo))]
		[SerializeField]
		[Space]
		private Characters.Projectiles.Operations.OperationInfo.Subcomponents _operations;

		// Token: 0x040020EC RID: 8428
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		[Space]
		private CharacterOperation.Subcomponents _operationsOnBounce;

		// Token: 0x040020ED RID: 8429
		[SerializeField]
		private Transform _bounceHitPoint;

		// Token: 0x040020EE RID: 8430
		[Space]
		[Characters.Projectiles.Operations.Operation.SubcomponentAttribute]
		[SerializeField]
		private Characters.Projectiles.Operations.Operation.Subcomponents _onSpawned;

		// Token: 0x040020EF RID: 8431
		[Characters.Projectiles.Operations.Operation.SubcomponentAttribute]
		[SerializeField]
		private Characters.Projectiles.Operations.Operation.Subcomponents _onDespawn;

		// Token: 0x040020F0 RID: 8432
		[SerializeField]
		private bool _despawnOnTerrainHit = true;

		// Token: 0x040020F1 RID: 8433
		[HitOperation.SubcomponentAttribute]
		[SerializeField]
		private HitOperation.Subcomponents _onTerrainHit;

		// Token: 0x040020F2 RID: 8434
		[CharacterHitOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterHitOperation.Subcomponents _onCharacterHit;

		// Token: 0x040020F3 RID: 8435
		[SerializeField]
		[ProjectileAttackVisualEffect.SubcomponentAttribute]
		private ProjectileAttackVisualEffect.Subcomponents _effect;

		// Token: 0x040020F4 RID: 8436
		[SerializeField]
		private BouncyProjectileMovement _movement;

		// Token: 0x040020F5 RID: 8437
		private BouncyProjectile.TerrainDetector _terrainDetector;

		// Token: 0x040020F6 RID: 8438
		private float _direction;

		// Token: 0x040020F7 RID: 8439
		private float _time;

		// Token: 0x02000757 RID: 1879
		[Serializable]
		private class CollisionDetector
		{
			// Token: 0x1400005E RID: 94
			// (add) Token: 0x06002678 RID: 9848 RVA: 0x00074144 File Offset: 0x00072344
			// (remove) Token: 0x06002679 RID: 9849 RVA: 0x0007417C File Offset: 0x0007237C
			public event BouncyProjectile.CollisionDetector.onTerrainHitDelegate onTerrainHit;

			// Token: 0x1400005F RID: 95
			// (add) Token: 0x0600267A RID: 9850 RVA: 0x000741B4 File Offset: 0x000723B4
			// (remove) Token: 0x0600267B RID: 9851 RVA: 0x000741EC File Offset: 0x000723EC
			public event BouncyProjectile.CollisionDetector.onTargetHitDelegate onHit;

			// Token: 0x170007FE RID: 2046
			// (get) Token: 0x0600267C RID: 9852 RVA: 0x00074221 File Offset: 0x00072421
			public Collider2D collider
			{
				get
				{
					return this._collider;
				}
			}

			// Token: 0x170007FF RID: 2047
			// (get) Token: 0x0600267D RID: 9853 RVA: 0x00074229 File Offset: 0x00072429
			public ReadonlyBoundedList<RaycastHit2D> results
			{
				get
				{
					return BouncyProjectile.CollisionDetector._caster.results;
				}
			}

			// Token: 0x17000800 RID: 2048
			// (get) Token: 0x0600267E RID: 9854 RVA: 0x00074235 File Offset: 0x00072435
			public LayerMask terrainLayer
			{
				get
				{
					return this._terrainLayer;
				}
			}

			// Token: 0x06002680 RID: 9856 RVA: 0x0007424B File Offset: 0x0007244B
			internal void Initialize(BouncyProjectile projectile)
			{
				this.Initialize(projectile, this._internalHitHistory);
			}

			// Token: 0x06002681 RID: 9857 RVA: 0x0007425A File Offset: 0x0007245A
			internal void Initialize(BouncyProjectile projectile, HitHistoryManager hitHistory)
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

			// Token: 0x06002682 RID: 9858 RVA: 0x00074296 File Offset: 0x00072496
			internal void SetHitHistoryManager(HitHistoryManager hitHistory)
			{
				this.hitHistoryManager = hitHistory;
			}

			// Token: 0x06002683 RID: 9859 RVA: 0x000742A0 File Offset: 0x000724A0
			internal void Detect(Vector2 origin, Vector2 direction, float distance)
			{
				if (this._projectile.owner == null)
				{
					return;
				}
				BouncyProjectile.CollisionDetector._caster.contactFilter.SetLayerMask(this._layer.Evaluate(this._projectile.owner.gameObject) | this._terrainLayer);
				if (this._collider != null)
				{
					this._collider.enabled = true;
					BouncyProjectile.CollisionDetector._caster.ColliderCast(this._collider, direction, distance);
					this._collider.enabled = false;
					return;
				}
				BouncyProjectile.CollisionDetector._caster.RayCast(origin, direction, distance);
			}

			// Token: 0x06002684 RID: 9860 RVA: 0x0007434C File Offset: 0x0007254C
			internal void Trigger(Vector2 origin, Vector2 direction, float distance)
			{
				for (int i = 0; i < BouncyProjectile.CollisionDetector._caster.results.Count; i++)
				{
					RaycastHit2D raycastHit = BouncyProjectile.CollisionDetector._caster.results[i];
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
								goto IL_16D;
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
					IL_16D:;
				}
			}

			// Token: 0x040020FD RID: 8445
			private const int maxHits = 99;

			// Token: 0x04002100 RID: 8448
			private BouncyProjectile _projectile;

			// Token: 0x04002101 RID: 8449
			[SerializeField]
			private TargetLayer _layer;

			// Token: 0x04002102 RID: 8450
			[SerializeField]
			private LayerMask _terrainLayer = Layers.terrainMaskForProjectile;

			// Token: 0x04002103 RID: 8451
			[SerializeField]
			private Collider2D _collider;

			// Token: 0x04002104 RID: 8452
			[Range(1f, 99f)]
			[SerializeField]
			private int _maxHits = 1;

			// Token: 0x04002105 RID: 8453
			[SerializeField]
			private int _maxHitsPerUnit = 1;

			// Token: 0x04002106 RID: 8454
			[SerializeField]
			private float _hitIntervalPerUnit = 0.5f;

			// Token: 0x04002107 RID: 8455
			internal HitHistoryManager hitHistoryManager;

			// Token: 0x04002108 RID: 8456
			private HitHistoryManager _internalHitHistory = new HitHistoryManager(99);

			// Token: 0x04002109 RID: 8457
			private int _propPenetratingHits;

			// Token: 0x0400210A RID: 8458
			private static readonly NonAllocCaster _caster = new NonAllocCaster(64);

			// Token: 0x02000758 RID: 1880
			// (Invoke) Token: 0x06002687 RID: 9863
			public delegate void onTerrainHitDelegate(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit);

			// Token: 0x02000759 RID: 1881
			// (Invoke) Token: 0x0600268B RID: 9867
			public delegate void onTargetHitDelegate(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target);
		}

		// Token: 0x0200075A RID: 1882
		private class TerrainDetector
		{
			// Token: 0x0600268E RID: 9870 RVA: 0x00074518 File Offset: 0x00072718
			public TerrainDetector(Collider2D collider, LayerMask terrainMask)
			{
				this._collider = collider;
				this._terrainMask = terrainMask;
			}

			// Token: 0x0600268F RID: 9871 RVA: 0x0007453C File Offset: 0x0007273C
			[return: TupleElementNames(new string[]
			{
				"hit",
				"hitPoint",
				"horizontalHit",
				"verticalHit"
			})]
			public ValueTuple<bool, Vector2, bool, bool> Cast(Vector2 direction, float distance)
			{
				this._caster.contactFilter.SetLayerMask(this._terrainMask);
				this._collider.enabled = true;
				this._caster.ColliderCast(this._collider, direction, distance);
				if (this._caster.results.Count == 0)
				{
					this._collider.enabled = false;
					return new ValueTuple<bool, Vector2, bool, bool>(false, Vector2.zero, false, false);
				}
				Vector2 point = this._caster.results[0].point;
				Vector2 direction2 = new Vector2(direction.x, 0f);
				Vector2 direction3 = new Vector2(0f, direction.y);
				this._caster.ColliderCast(this._collider, direction2, distance);
				bool item = this._caster.results.Count > 0;
				this._caster.ColliderCast(this._collider, direction3, distance);
				bool item2 = this._caster.results.Count > 0;
				this._collider.enabled = false;
				return new ValueTuple<bool, Vector2, bool, bool>(true, point, item, item2);
			}

			// Token: 0x0400210B RID: 8459
			private readonly NonAllocCaster _caster = new NonAllocCaster(1);

			// Token: 0x0400210C RID: 8460
			private readonly Collider2D _collider;

			// Token: 0x0400210D RID: 8461
			private readonly LayerMask _terrainMask;
		}
	}
}
