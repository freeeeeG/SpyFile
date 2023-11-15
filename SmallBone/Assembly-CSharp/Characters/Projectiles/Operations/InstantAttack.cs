using System;
using Characters.Movements;
using Characters.Operations;
using Characters.Operations.Movement;
using Characters.Player;
using FX.BoundsAttackVisualEffect;
using GameResources;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000785 RID: 1925
	public class InstantAttack : Operation
	{
		// Token: 0x0600278A RID: 10122 RVA: 0x00076990 File Offset: 0x00074B90
		private void Awake()
		{
			this._limit = Math.Min(this._limit, 2048);
			this._overlapper = ((this._limit == InstantAttack._sharedOverlapper.capacity) ? InstantAttack._sharedOverlapper : new NonAllocOverlapper(this._limit));
			Array.Sort<TargetedOperationInfo>(this._operationInfo.components, (TargetedOperationInfo x, TargetedOperationInfo y) => x.timeToTrigger.CompareTo(y.timeToTrigger));
			this._collider.enabled = false;
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			this._operationInfo.Initialize();
			foreach (TargetedOperationInfo targetedOperationInfo in this._operationInfo.components)
			{
				Knockback knockback = targetedOperationInfo.operation as Knockback;
				if (knockback != null)
				{
					this._pushInfo = knockback.pushInfo;
					return;
				}
				Smash smash = targetedOperationInfo.operation as Smash;
				if (smash != null)
				{
					this._pushInfo = smash.pushInfo;
				}
			}
		}

		// Token: 0x0600278B RID: 10123 RVA: 0x00076A88 File Offset: 0x00074C88
		public override void Run(IProjectile projectile)
		{
			Character owner = projectile.owner;
			if (owner == null || !owner.gameObject.activeSelf)
			{
				return;
			}
			this._overlapper.contactFilter.SetLayerMask(this._layer.Evaluate(owner.gameObject));
			if (this._attackDamage == null)
			{
				PlayerComponents playerComponents = owner.playerComponents;
				if (((playerComponents != null) ? playerComponents.inventory.weapon : null) != null)
				{
					this._attackDamage = owner.playerComponents.inventory.weapon.weaponAttackDamage;
				}
				else
				{
					this._attackDamage = owner.GetComponent<IAttackDamage>();
				}
			}
			this._collider.enabled = true;
			this._overlapper.OverlapCollider(this._collider);
			Bounds bounds = this._collider.bounds;
			this._collider.enabled = false;
			for (int i = 0; i < this._overlapper.results.Count; i++)
			{
				Target component = this._overlapper.results[i].GetComponent<Target>();
				if (!(component == null))
				{
					Bounds bounds2 = component.collider.bounds;
					Bounds bounds3 = new Bounds
					{
						min = MMMaths.Max(bounds.min, bounds2.min),
						max = MMMaths.Min(bounds.max, bounds2.max)
					};
					Vector2 hitPoint = MMMaths.RandomPointWithinBounds(bounds3);
					if (!(projectile.owner == null))
					{
						Vector2 force = Vector2.zero;
						if (this._pushInfo != null)
						{
							ValueTuple<Vector2, Vector2> valueTuple = this._pushInfo.EvaluateTimeIndependent(owner, component);
							force = valueTuple.Item1 + valueTuple.Item2;
						}
						if (component.character != null && component.character.liveAndActive && component.character != owner)
						{
							if (!component.character.cinematic.value)
							{
								Damage damage = owner.stat.GetDamage((double)this._attackDamage.amount, hitPoint, this._hitInfo);
								this._chrono.ApplyTo(component.character);
								if (this._hitInfo.attackType != Damage.AttackType.None)
								{
									CommonResource.instance.hitParticle.Emit(component.transform.position, bounds3, force, true);
								}
								if (owner.TryAttackCharacter(component, ref damage))
								{
									base.StartCoroutine(this._operationInfo.CRun(owner, component.character));
									this._effect.Spawn(owner, bounds3, damage, component);
								}
							}
						}
						else if (component.damageable != null)
						{
							Damage damage2 = owner.stat.GetDamage((double)this._attackDamage.amount, hitPoint, this._hitInfo);
							if (component.damageable.spawnEffectOnHit && this._hitInfo.attackType != Damage.AttackType.None)
							{
								CommonResource.instance.hitParticle.Emit(component.transform.position, bounds3, force, true);
								this._effect.Spawn(owner, bounds3, damage2, component);
							}
							if (this._hitInfo.attackType == Damage.AttackType.None)
							{
								return;
							}
							component.damageable.Hit(owner, ref damage2, force);
						}
					}
				}
			}
		}

		// Token: 0x040021A7 RID: 8615
		private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(2048);

		// Token: 0x040021A8 RID: 8616
		[SerializeField]
		private int _limit = 15;

		// Token: 0x040021A9 RID: 8617
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x040021AA RID: 8618
		[SerializeField]
		private TargetLayer _layer = new TargetLayer(2048, false, true, false, false);

		// Token: 0x040021AB RID: 8619
		[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
		[SerializeField]
		private TargetedOperationInfo.Subcomponents _operationInfo;

		// Token: 0x040021AC RID: 8620
		[BoundsAttackVisualEffect.SubcomponentAttribute]
		[SerializeField]
		private BoundsAttackVisualEffect.Subcomponents _effect;

		// Token: 0x040021AD RID: 8621
		[SerializeField]
		protected HitInfo _hitInfo = new HitInfo(Damage.AttackType.Ranged);

		// Token: 0x040021AE RID: 8622
		[SerializeField]
		protected ChronoInfo _chrono;

		// Token: 0x040021AF RID: 8623
		private NonAllocOverlapper _overlapper;

		// Token: 0x040021B0 RID: 8624
		private PushInfo _pushInfo;

		// Token: 0x040021B1 RID: 8625
		private IAttackDamage _attackDamage;
	}
}
