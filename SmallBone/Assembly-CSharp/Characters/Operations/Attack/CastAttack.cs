using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameResources;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack
{
	// Token: 0x02000F71 RID: 3953
	public sealed class CastAttack : CharacterOperation, IAttack
	{
		// Token: 0x140000B6 RID: 182
		// (add) Token: 0x06004CB1 RID: 19633 RVA: 0x000E3848 File Offset: 0x000E1A48
		// (remove) Token: 0x06004CB2 RID: 19634 RVA: 0x000E3880 File Offset: 0x000E1A80
		public event OnAttackHitDelegate onHit;

		// Token: 0x17000F4A RID: 3914
		// (get) Token: 0x06004CB3 RID: 19635 RVA: 0x000E38B5 File Offset: 0x000E1AB5
		// (set) Token: 0x06004CB4 RID: 19636 RVA: 0x000E38BD File Offset: 0x000E1ABD
		internal Character owner { get; private set; }

		// Token: 0x06004CB5 RID: 19637 RVA: 0x000E38C8 File Offset: 0x000E1AC8
		private void Awake()
		{
			this._collisionDetector.onTerrainHit += this.<Awake>g__onTerrainHit|13_0;
			if (this._attackAndEffect.noDelay)
			{
				this._collisionDetector.onHit += this.<Awake>g__onTargetHitWithoutDelay|13_2;
				return;
			}
			this._collisionDetector.onHit += this.<Awake>g__onTargetHit|13_1;
		}

		// Token: 0x06004CB6 RID: 19638 RVA: 0x000E3928 File Offset: 0x000E1B28
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			this._attackAndEffect.Initialize();
		}

		// Token: 0x06004CB7 RID: 19639 RVA: 0x000E3941 File Offset: 0x000E1B41
		public override void Stop()
		{
			this._attackAndEffect.StopAllOperationsToOwner();
		}

		// Token: 0x06004CB8 RID: 19640 RVA: 0x000E3950 File Offset: 0x000E1B50
		public override void Run(Character owner)
		{
			this.owner = owner;
			this._collisionDetector.Initialize(this);
			this._collisionDetector.Detect(base.transform.position, (owner.lookingDirection == Character.LookingDirection.Right) ? Vector2.right : Vector2.left, this._distance);
		}

		// Token: 0x06004CB9 RID: 19641 RVA: 0x000E39A8 File Offset: 0x000E1BA8
		private void Attack(CastAttackInfo attackInfo, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
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
				attackInfo.effect.Spawn(this.owner, this._collisionDetector.collider, origin, direction, distance, raycastHit, damage, target);
				if (this.owner.TryAttackCharacter(target, ref damage))
				{
					this.owner.StartCoroutine(attackInfo.operationsToCharacter.CRun(this.owner, target.character));
					OnAttackHitDelegate onAttackHitDelegate = this.onHit;
					if (onAttackHitDelegate == null)
					{
						return;
					}
					onAttackHitDelegate(target, ref damage);
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

		// Token: 0x06004CBA RID: 19642 RVA: 0x000E3C3B File Offset: 0x000E1E3B
		private IEnumerator CAttack(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
		{
			int index = 0;
			float time = 0f;
			while (this != null && index < this._attackAndEffect.components.Length)
			{
				CastAttackInfoSequence castAttackInfoSequence;
				while (index < this._attackAndEffect.components.Length && time >= (castAttackInfoSequence = this._attackAndEffect.components[index]).timeToTrigger)
				{
					this.Attack(castAttackInfoSequence.attackInfo, origin, direction, distance, raycastHit, target);
					int num = index;
					index = num + 1;
				}
				yield return null;
				time += this.owner.chronometer.animation.deltaTime;
			}
			yield break;
		}

		// Token: 0x06004CBC RID: 19644 RVA: 0x000E3C6F File Offset: 0x000E1E6F
		[CompilerGenerated]
		private void <Awake>g__onTerrainHit|13_0(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit)
		{
			base.StartCoroutine(this._onTerrainHit.CRun(this.owner));
		}

		// Token: 0x06004CBD RID: 19645 RVA: 0x000E3C89 File Offset: 0x000E1E89
		[CompilerGenerated]
		private void <Awake>g__onTargetHit|13_1(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
		{
			target.StartCoroutine(this.CAttack(origin, direction, distance, raycastHit, target));
		}

		// Token: 0x06004CBE RID: 19646 RVA: 0x000E3CA0 File Offset: 0x000E1EA0
		[CompilerGenerated]
		private void <Awake>g__onTargetHitWithoutDelay|13_2(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
		{
			foreach (CastAttackInfoSequence castAttackInfoSequence in this._attackAndEffect.components)
			{
				this.Attack(castAttackInfoSequence.attackInfo, origin, direction, distance, raycastHit, target);
			}
		}

		// Token: 0x04003C57 RID: 15447
		[SerializeField]
		private float _distance;

		// Token: 0x04003C58 RID: 15448
		[SerializeField]
		private CastAttack.CollisionDetector _collisionDetector;

		// Token: 0x04003C59 RID: 15449
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onTerrainHit;

		// Token: 0x04003C5A RID: 15450
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(CastAttackInfoSequence))]
		private CastAttackInfoSequence.Subcomponents _attackAndEffect;

		// Token: 0x04003C5B RID: 15451
		private IAttackDamage _attackDamage;

		// Token: 0x02000F72 RID: 3954
		[Serializable]
		private class CollisionDetector
		{
			// Token: 0x140000B7 RID: 183
			// (add) Token: 0x06004CBF RID: 19647 RVA: 0x000E3CE0 File Offset: 0x000E1EE0
			// (remove) Token: 0x06004CC0 RID: 19648 RVA: 0x000E3D18 File Offset: 0x000E1F18
			public event CastAttack.CollisionDetector.onTerrainHitDelegate onTerrainHit;

			// Token: 0x140000B8 RID: 184
			// (add) Token: 0x06004CC1 RID: 19649 RVA: 0x000E3D50 File Offset: 0x000E1F50
			// (remove) Token: 0x06004CC2 RID: 19650 RVA: 0x000E3D88 File Offset: 0x000E1F88
			public event CastAttack.CollisionDetector.onTargetHitDelegate onHit;

			// Token: 0x17000F4B RID: 3915
			// (get) Token: 0x06004CC3 RID: 19651 RVA: 0x000E3DBD File Offset: 0x000E1FBD
			public Collider2D collider
			{
				get
				{
					return this._collider;
				}
			}

			// Token: 0x06004CC4 RID: 19652 RVA: 0x000E3DC8 File Offset: 0x000E1FC8
			internal void Initialize(CastAttack castAttack)
			{
				this._castAttack = castAttack;
				this._filter.layerMask = this._layer.Evaluate(castAttack.owner.gameObject);
				this._hits.Clear();
				this._propPenetratingHits = 0;
				if (this._collider != null)
				{
					this._collider.enabled = false;
				}
			}

			// Token: 0x06004CC5 RID: 19653 RVA: 0x000E3E2C File Offset: 0x000E202C
			internal void Detect(Vector2 origin, Vector2 direction, float distance)
			{
				CastAttack.CollisionDetector.<>c__DisplayClass19_0 CS$<>8__locals1;
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.origin = origin;
				CS$<>8__locals1.direction = direction;
				CS$<>8__locals1.distance = distance;
				CastAttack.CollisionDetector._caster.contactFilter.SetLayerMask(this._filter.layerMask);
				CastAttack.CollisionDetector._caster.RayCast(CS$<>8__locals1.origin, CS$<>8__locals1.direction, CS$<>8__locals1.distance);
				if (this._collider)
				{
					this._collider.enabled = true;
					CastAttack.CollisionDetector._caster.ColliderCast(this._collider, CS$<>8__locals1.direction, CS$<>8__locals1.distance);
					this._collider.enabled = false;
				}
				else
				{
					CastAttack.CollisionDetector._caster.RayCast(CS$<>8__locals1.origin, CS$<>8__locals1.direction, CS$<>8__locals1.distance);
				}
				for (int i = 0; i < CastAttack.CollisionDetector._caster.results.Count; i++)
				{
					CastAttack.CollisionDetector.<>c__DisplayClass19_1 CS$<>8__locals2;
					CS$<>8__locals2.result = CastAttack.CollisionDetector._caster.results[i];
					this.<Detect>g__HandleResult|19_0(ref CS$<>8__locals1, ref CS$<>8__locals2);
					if (this._hits.Count - this._propPenetratingHits >= this._maxHits)
					{
						return;
					}
				}
			}

			// Token: 0x06004CC8 RID: 19656 RVA: 0x000E3F8C File Offset: 0x000E218C
			[CompilerGenerated]
			private void <Detect>g__HandleResult|19_0(ref CastAttack.CollisionDetector.<>c__DisplayClass19_0 A_1, ref CastAttack.CollisionDetector.<>c__DisplayClass19_1 A_2)
			{
				if (A_2.result.collider.gameObject.layer == 8)
				{
					this.onTerrainHit(A_1.origin, A_1.direction, A_1.distance, A_2.result);
					return;
				}
				A_1.target = A_2.result.collider.GetComponent<Target>();
				if (this._hits.Contains(A_1.target))
				{
					return;
				}
				if (A_1.target.character != null)
				{
					if (!A_1.target.character.liveAndActive)
					{
						return;
					}
					this.onHit(A_1.origin, A_1.direction, A_1.distance, A_2.result, A_1.target);
					this._hits.Add(A_1.target);
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
						this._hits.Add(A_1.target);
						return;
					}
					return;
				}
			}

			// Token: 0x04003C5E RID: 15454
			private static readonly NonAllocCaster _caster = new NonAllocCaster(15);

			// Token: 0x04003C61 RID: 15457
			private CastAttack _castAttack;

			// Token: 0x04003C62 RID: 15458
			[SerializeField]
			private TargetLayer _layer = new TargetLayer(2048, false, true, false, false);

			// Token: 0x04003C63 RID: 15459
			[SerializeField]
			private Collider2D _collider;

			// Token: 0x04003C64 RID: 15460
			[Range(1f, 15f)]
			[SerializeField]
			private int _maxHits = 15;

			// Token: 0x04003C65 RID: 15461
			private List<Target> _hits = new List<Target>(15);

			// Token: 0x04003C66 RID: 15462
			private int _propPenetratingHits;

			// Token: 0x04003C67 RID: 15463
			private ContactFilter2D _filter;

			// Token: 0x02000F73 RID: 3955
			// (Invoke) Token: 0x06004CCA RID: 19658
			public delegate void onTerrainHitDelegate(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit);

			// Token: 0x02000F74 RID: 3956
			// (Invoke) Token: 0x06004CCE RID: 19662
			public delegate void onTargetHitDelegate(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target);
		}
	}
}
