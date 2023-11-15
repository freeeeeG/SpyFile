using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Operations;
using Characters.Operations.Attack;
using FX.CastAttackVisualEffect;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt
{
	// Token: 0x02001148 RID: 4424
	public class YggdrasillElderEntCollisionDetector : MonoBehaviour
	{
		// Token: 0x140000CA RID: 202
		// (add) Token: 0x06005616 RID: 22038 RVA: 0x00100468 File Offset: 0x000FE668
		// (remove) Token: 0x06005617 RID: 22039 RVA: 0x001004A0 File Offset: 0x000FE6A0
		public event Action<RaycastHit2D> onTerrainHit;

		// Token: 0x140000CB RID: 203
		// (add) Token: 0x06005618 RID: 22040 RVA: 0x001004D8 File Offset: 0x000FE6D8
		// (remove) Token: 0x06005619 RID: 22041 RVA: 0x00100510 File Offset: 0x000FE710
		public event YggdrasillElderEntCollisionDetector.onTargetHitDelegate onHit;

		// Token: 0x0600561B RID: 22043 RVA: 0x00100553 File Offset: 0x000FE753
		private void Awake()
		{
			this.onHit += delegate(Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
			{
				if (target.character == null)
				{
					return;
				}
				if (target.character.cinematic.value)
				{
					return;
				}
				Damage damage = this._owner.stat.GetDamage((double)this._attackDamage.amount, raycastHit.point, this._hitInfo);
				if (this._owner.TryAttackCharacter(target, ref damage))
				{
					this._chronoToOwner.ApplyTo(this._owner);
					this._chronoToTarget.ApplyTo(target.character);
					base.StartCoroutine(this._onCharacterHit.CRun(this._owner, target.character));
					base.StartCoroutine(this._operationToOwnerWhenHitInfo.CRun(this._owner));
				}
			};
		}

		// Token: 0x0600561C RID: 22044 RVA: 0x00100568 File Offset: 0x000FE768
		internal void Initialize(GameObject owner, Collider2D collider)
		{
			this._filter.layerMask = this._layer.Evaluate(owner);
			this._propHits = 0;
			this._hits.Clear();
			this._collider = collider;
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			this._attackAndEffect.Initialize();
			this._onCharacterHit.Initialize();
		}

		// Token: 0x0600561D RID: 22045 RVA: 0x001005C7 File Offset: 0x000FE7C7
		private void Detect(Vector2 origin, Vector2 distance)
		{
			this.Detect(origin, distance.normalized, distance.magnitude);
		}

		// Token: 0x0600561E RID: 22046 RVA: 0x001005E0 File Offset: 0x000FE7E0
		private void Detect(Vector2 origin, Vector2 direction, float distance)
		{
			YggdrasillElderEntCollisionDetector._caster.contactFilter.SetLayerMask(this._filter.layerMask);
			YggdrasillElderEntCollisionDetector._caster.RayCast(origin, direction, distance);
			if (this._collider)
			{
				YggdrasillElderEntCollisionDetector._caster.ColliderCast(this._collider, direction, distance);
			}
			else
			{
				YggdrasillElderEntCollisionDetector._caster.RayCast(origin, direction, distance);
			}
			for (int i = 0; i < YggdrasillElderEntCollisionDetector._caster.results.Count; i++)
			{
				Target component = YggdrasillElderEntCollisionDetector._caster.results[i].collider.GetComponent<Target>();
				if (component == null)
				{
					return;
				}
				if (!this._hits.Contains(component))
				{
					if (component.character != null)
					{
						if (component.character.liveAndActive)
						{
							this.onHit(this._collider, origin, direction, distance, YggdrasillElderEntCollisionDetector._caster.results[i], component);
							this._hits.Add(component);
						}
					}
					else if (component.damageable != null)
					{
						Damage damage = this._owner.stat.GetDamage((double)this._attackDamage.amount, YggdrasillElderEntCollisionDetector._caster.results[i].point, this._hitInfo);
						component.damageable.Hit(this._owner, ref damage);
						this.onHit(this._collider, origin, direction, distance, YggdrasillElderEntCollisionDetector._caster.results[i], component);
						this._hits.Add(component);
					}
				}
				if (this._hits.Count >= this._maxHits)
				{
					this.Stop();
				}
			}
		}

		// Token: 0x0600561F RID: 22047 RVA: 0x00100798 File Offset: 0x000FE998
		public void Stop()
		{
			this._running = false;
			this._attackAndEffect.StopAllOperationsToOwner();
		}

		// Token: 0x06005620 RID: 22048 RVA: 0x001007AC File Offset: 0x000FE9AC
		public IEnumerator CRun(Transform moveTarget)
		{
			Vector2 vector = moveTarget.position;
			this._running = true;
			while (this._running)
			{
				Vector2 nextPosition = moveTarget.position;
				if (this._owner.chronometer.animation.deltaTime > 1E-45f)
				{
					this.Detect(vector, nextPosition - vector);
				}
				yield return null;
				vector = nextPosition;
				nextPosition = default(Vector2);
			}
			yield break;
		}

		// Token: 0x0400451B RID: 17691
		[SerializeField]
		protected HitInfo _hitInfo = new HitInfo(Damage.AttackType.Melee);

		// Token: 0x0400451C RID: 17692
		[SerializeField]
		[Subcomponent(typeof(TargetedOperationInfo))]
		private TargetedOperationInfo.Subcomponents _onCharacterHit;

		// Token: 0x0400451D RID: 17693
		[SerializeField]
		private Character _owner;

		// Token: 0x0400451E RID: 17694
		[SerializeField]
		private TargetLayer _layer;

		// Token: 0x0400451F RID: 17695
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x04004520 RID: 17696
		[SerializeField]
		[Range(1f, 15f)]
		private int _maxHits = 1;

		// Token: 0x04004521 RID: 17697
		private List<Target> _hits = new List<Target>(15);

		// Token: 0x04004522 RID: 17698
		private ContactFilter2D _filter;

		// Token: 0x04004523 RID: 17699
		[SerializeField]
		protected ChronoInfo _chronoToGlobe;

		// Token: 0x04004524 RID: 17700
		[SerializeField]
		protected ChronoInfo _chronoToOwner;

		// Token: 0x04004525 RID: 17701
		[SerializeField]
		protected ChronoInfo _chronoToTarget;

		// Token: 0x04004526 RID: 17702
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		internal OperationInfo.Subcomponents _operationToOwnerWhenHitInfo;

		// Token: 0x04004527 RID: 17703
		[Subcomponent(typeof(CastAttackInfoSequence))]
		[SerializeField]
		private CastAttackInfoSequence.Subcomponents _attackAndEffect;

		// Token: 0x04004528 RID: 17704
		[SerializeField]
		[CastAttackVisualEffect.SubcomponentAttribute]
		private CastAttackVisualEffect.Subcomponents _effect;

		// Token: 0x04004529 RID: 17705
		private CoroutineReference _expireReference;

		// Token: 0x0400452A RID: 17706
		private IAttackDamage _attackDamage;

		// Token: 0x0400452B RID: 17707
		private int _propHits;

		// Token: 0x0400452C RID: 17708
		private static readonly NonAllocCaster _caster = new NonAllocCaster(15);

		// Token: 0x0400452D RID: 17709
		private bool _running;

		// Token: 0x02001149 RID: 4425
		// (Invoke) Token: 0x06005624 RID: 22052
		public delegate void onTerrainHitDelegate(Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit);

		// Token: 0x0200114A RID: 4426
		// (Invoke) Token: 0x06005628 RID: 22056
		public delegate void onTargetHitDelegate(Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target);
	}
}
