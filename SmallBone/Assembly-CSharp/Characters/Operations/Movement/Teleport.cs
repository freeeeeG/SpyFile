using System;
using System.Collections.Generic;
using Level;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E72 RID: 3698
	public class Teleport : CharacterOperation
	{
		// Token: 0x06004959 RID: 18777 RVA: 0x000D6210 File Offset: 0x000D4410
		private void Awake()
		{
			if (this._maxDistance <= 0f)
			{
				this._maxDistance = float.PositiveInfinity;
			}
		}

		// Token: 0x0600495A RID: 18778 RVA: 0x000D622A File Offset: 0x000D442A
		public override void Run(Character owner)
		{
			if (this._distanceType == Teleport.DistanceType.Range)
			{
				this.TeleportByRange(owner);
				return;
			}
			if (this._distanceType == Teleport.DistanceType.Constant)
			{
				this.TeleportByDistanceInPlatform(owner);
				return;
			}
			if (this._distanceType == Teleport.DistanceType.TargetDistance)
			{
				this.TeleportByTarget(owner);
			}
		}

		// Token: 0x0600495B RID: 18779 RVA: 0x000D6260 File Offset: 0x000D4460
		private void TeleportByRange(Character owner)
		{
			if (this._type == Teleport.Type.Teleport)
			{
				this._targetPosition.position = MMMaths.RandomVector3(this._distanceArea.bounds.min, this._distanceArea.bounds.max);
				owner.movement.controller.Teleport(this._targetPosition.position);
				return;
			}
			Teleport._nonAllocOverlapper.contactFilter.SetLayerMask(Layers.groundMask);
			ReadonlyBoundedList<Collider2D> results = Teleport._nonAllocOverlapper.OverlapCollider(this._distanceArea).results;
			if (results.Count == 0)
			{
				Debug.LogError("Failed to teleport, you can widen distanceArea");
				return;
			}
			List<Collider2D> list = new List<Collider2D>(results.Count);
			Collider2D lastStandingCollider = owner.movement.controller.collisionState.lastStandingCollider;
			foreach (Collider2D collider2D in results)
			{
				if (Map.Instance.bounds.min.x <= collider2D.bounds.min.x && Map.Instance.bounds.max.x >= collider2D.bounds.max.x)
				{
					ColliderDistance2D colliderDistance2D = Physics2D.Distance(collider2D, owner.collider);
					if (lastStandingCollider == collider2D)
					{
						if (owner.transform.position.x + this._minDistance < lastStandingCollider.bounds.max.x || owner.transform.position.x - this._minDistance > lastStandingCollider.bounds.min.x)
						{
							list.Add(collider2D);
						}
					}
					else if (colliderDistance2D.distance >= this._minDistance)
					{
						list.Add(collider2D);
					}
				}
			}
			if (list.Count == 0)
			{
				Debug.LogError("Failed to teleport, you can widen distanceArea");
				return;
			}
			int index = UnityEngine.Random.Range(0, list.Count);
			Bounds bounds = list[index].bounds;
			if (bounds == lastStandingCollider.bounds)
			{
				bool flag = owner.transform.position.x + this._minDistance < lastStandingCollider.bounds.max.x;
				bool flag2 = owner.transform.position.x - this._minDistance > lastStandingCollider.bounds.min.x;
				float x;
				if (flag && (!flag2 || MMMaths.RandomBool()))
				{
					x = UnityEngine.Random.Range(Mathf.Max(lastStandingCollider.bounds.min.x, owner.transform.position.x - this._maxDistance), owner.transform.position.x - this._minDistance);
				}
				else
				{
					x = UnityEngine.Random.Range(owner.transform.position.x + this._minDistance, Mathf.Min(lastStandingCollider.bounds.max.x, owner.transform.position.x + this._maxDistance));
				}
				this._targetPosition.position = new Vector2(x, lastStandingCollider.bounds.max.y);
			}
			else
			{
				this._targetPosition.position = new Vector2(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), bounds.max.y);
			}
			owner.movement.controller.TeleportUponGround(this._targetPosition.position, 4f);
		}

		// Token: 0x0600495C RID: 18780 RVA: 0x000D6640 File Offset: 0x000D4840
		private void TeleportByTarget(Character owner)
		{
			if (this._type == Teleport.Type.Teleport)
			{
				owner.movement.controller.Teleport(this._targetPosition.position);
				return;
			}
			owner.movement.controller.TeleportUponGround(this.FilterDest(owner, this._targetPosition.position), 4f);
		}

		// Token: 0x0600495D RID: 18781 RVA: 0x000D66A8 File Offset: 0x000D48A8
		private float FilterDestX(float extends, float position)
		{
			Bounds bounds = Map.Instance.bounds;
			if (position + extends >= bounds.max.x)
			{
				return position - extends;
			}
			if (position - extends <= bounds.min.x)
			{
				return position + extends;
			}
			return position;
		}

		// Token: 0x0600495E RID: 18782 RVA: 0x000D66EC File Offset: 0x000D48EC
		private Vector2 FilterDest(Character owner, Vector2 position)
		{
			return new Vector2(this.FilterDestX(owner.collider.bounds.extents.x, position.x), position.y);
		}

		// Token: 0x0600495F RID: 18783 RVA: 0x00002191 File Offset: 0x00000391
		private void TeleportByDistanceInPlatform(Character owner)
		{
		}

		// Token: 0x04003889 RID: 14473
		[SerializeField]
		private Teleport.Type _type;

		// Token: 0x0400388A RID: 14474
		[SerializeField]
		private Teleport.DistanceType _distanceType;

		// Token: 0x0400388B RID: 14475
		[SerializeField]
		private Collider2D _distanceArea;

		// Token: 0x0400388C RID: 14476
		[SerializeField]
		private float _minDistance;

		// Token: 0x0400388D RID: 14477
		[SerializeField]
		private float _maxDistance;

		// Token: 0x0400388E RID: 14478
		[SerializeField]
		private Transform _targetPosition;

		// Token: 0x0400388F RID: 14479
		private static readonly NonAllocOverlapper _nonAllocOverlapper = new NonAllocOverlapper(15);

		// Token: 0x02000E73 RID: 3699
		private enum Type
		{
			// Token: 0x04003891 RID: 14481
			TeleportUponGround,
			// Token: 0x04003892 RID: 14482
			Teleport
		}

		// Token: 0x02000E74 RID: 3700
		private enum DistanceType
		{
			// Token: 0x04003894 RID: 14484
			Constant,
			// Token: 0x04003895 RID: 14485
			TargetDistance,
			// Token: 0x04003896 RID: 14486
			Range
		}
	}
}
