using System;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EE5 RID: 3813
	public sealed class ToColliderBased : Policy
	{
		// Token: 0x06004ACD RID: 19149 RVA: 0x000D950E File Offset: 0x000D770E
		public override Vector2 GetPosition(Character owner)
		{
			return this.GetPosition();
		}

		// Token: 0x06004ACE RID: 19150 RVA: 0x000DAED8 File Offset: 0x000D90D8
		private void Awake()
		{
			this._topLineRaycaster = new LineSequenceNonAllocCaster(this._verticalRayCount, this._verticalRayCount)
			{
				caster = new RayCaster
				{
					direction = Vector2.down
				}
			};
			this._bottomLineRaycaster = new LineSequenceNonAllocCaster(this._verticalRayCount, this._verticalRayCount)
			{
				caster = new RayCaster
				{
					direction = Vector2.up
				}
			};
			this._leftLineRaycaster = new LineSequenceNonAllocCaster(this._horizontalRayCount, this._horizontalRayCount)
			{
				caster = new RayCaster
				{
					direction = Vector2.right
				}
			};
			this._rightLineRaycaster = new LineSequenceNonAllocCaster(this._horizontalRayCount, this._horizontalRayCount)
			{
				caster = new RayCaster
				{
					direction = Vector2.left
				}
			};
		}

		// Token: 0x06004ACF RID: 19151 RVA: 0x000DAF9C File Offset: 0x000D919C
		public override Vector2 GetPosition()
		{
			this._horizontalMoveDelta = 0f;
			this._verticalMoveDelta = 0f;
			this._hit = false;
			this.SetBounds();
			this.CheckTop();
			this.CheckBottom();
			this.CheckRight();
			this.CheckLeft();
			if (!this._hit)
			{
				return (this._defaultPosition == null) ? base.transform.position : this._defaultPosition.transform.position;
			}
			return new Vector2(this._targetPoint.transform.position.x + this._horizontalMoveDelta, this._targetPoint.transform.position.y + this._verticalMoveDelta);
		}

		// Token: 0x06004AD0 RID: 19152 RVA: 0x000DB05C File Offset: 0x000D925C
		private void SetBounds()
		{
			Bounds bounds = this._targetCollider.bounds;
			this._topLineRaycaster.start = new Vector2(bounds.min.x, bounds.max.y);
			this._topLineRaycaster.end = new Vector2(bounds.max.x, bounds.max.y);
			this._bottomLineRaycaster.start = new Vector2(bounds.min.x, bounds.min.y);
			this._bottomLineRaycaster.end = new Vector2(bounds.max.x, bounds.min.y);
			this._leftLineRaycaster.start = new Vector2(bounds.min.x, bounds.min.y);
			this._leftLineRaycaster.end = new Vector2(bounds.min.x, bounds.max.y);
			this._rightLineRaycaster.start = new Vector2(bounds.max.x, bounds.min.y);
			this._rightLineRaycaster.end = new Vector2(bounds.max.x, bounds.max.y);
		}

		// Token: 0x06004AD1 RID: 19153 RVA: 0x000DB1B8 File Offset: 0x000D93B8
		private void CheckTop()
		{
			Bounds bounds = this._targetCollider.bounds;
			LineSequenceNonAllocCaster topLineRaycaster = this._topLineRaycaster;
			topLineRaycaster.caster.contactFilter.SetLayerMask(256);
			topLineRaycaster.caster.origin = Vector2.zero;
			topLineRaycaster.caster.distance = bounds.size.y;
			topLineRaycaster.Cast();
			float num = bounds.size.y;
			this._upMinDistance = 0f;
			for (int i = 0; i < topLineRaycaster.nonAllocCasters.Count; i++)
			{
				ReadonlyBoundedList<RaycastHit2D> results = topLineRaycaster.nonAllocCasters[i].results;
				if (results.Count != 0)
				{
					RaycastHit2D raycastHit2D = results[0];
					if (raycastHit2D.distance < bounds.size.y && raycastHit2D.distance > 1E-45f)
					{
						this._hit = true;
						num = Mathf.Min(num, raycastHit2D.distance);
						this._upMinDistance = num;
						this._verticalMoveDelta = bounds.size.y - num;
					}
				}
			}
		}

		// Token: 0x06004AD2 RID: 19154 RVA: 0x000DB2CC File Offset: 0x000D94CC
		private void CheckBottom()
		{
			Bounds bounds = this._targetCollider.bounds;
			LineSequenceNonAllocCaster bottomLineRaycaster = this._bottomLineRaycaster;
			bottomLineRaycaster.caster.contactFilter.SetLayerMask(256);
			bottomLineRaycaster.caster.origin = Vector2.zero;
			bottomLineRaycaster.caster.distance = bounds.size.y;
			bottomLineRaycaster.Cast();
			float num = bounds.size.y;
			for (int i = 0; i < bottomLineRaycaster.nonAllocCasters.Count; i++)
			{
				ReadonlyBoundedList<RaycastHit2D> results = bottomLineRaycaster.nonAllocCasters[i].results;
				if (results.Count != 0)
				{
					RaycastHit2D raycastHit2D = results[0];
					if (raycastHit2D.distance < bounds.size.y && raycastHit2D.distance > 1E-45f)
					{
						this._hit = true;
						num = Mathf.Min(num, raycastHit2D.distance);
						if (num > this._upMinDistance)
						{
							this._verticalMoveDelta = -1f * (bounds.size.y - num);
						}
					}
				}
			}
		}

		// Token: 0x06004AD3 RID: 19155 RVA: 0x000DB3E0 File Offset: 0x000D95E0
		private void CheckRight()
		{
			Bounds bounds = this._targetCollider.bounds;
			LineSequenceNonAllocCaster rightLineRaycaster = this._rightLineRaycaster;
			rightLineRaycaster.caster.contactFilter.SetLayerMask(256);
			rightLineRaycaster.caster.origin = Vector2.zero;
			rightLineRaycaster.caster.distance = bounds.size.x;
			rightLineRaycaster.Cast();
			float num = bounds.size.x;
			this._rightMinDistance = 0f;
			for (int i = 0; i < rightLineRaycaster.nonAllocCasters.Count; i++)
			{
				ReadonlyBoundedList<RaycastHit2D> results = rightLineRaycaster.nonAllocCasters[i].results;
				if (results.Count != 0)
				{
					RaycastHit2D raycastHit2D = results[0];
					if (raycastHit2D.distance < bounds.size.x && raycastHit2D.distance > 1E-45f)
					{
						this._hit = true;
						num = Mathf.Min(num, raycastHit2D.distance);
						this._rightMinDistance = num;
						this._horizontalMoveDelta = bounds.size.x - num;
					}
				}
			}
		}

		// Token: 0x06004AD4 RID: 19156 RVA: 0x000DB4F4 File Offset: 0x000D96F4
		private void CheckLeft()
		{
			Bounds bounds = this._targetCollider.bounds;
			LineSequenceNonAllocCaster leftLineRaycaster = this._leftLineRaycaster;
			leftLineRaycaster.caster.contactFilter.SetLayerMask(256);
			leftLineRaycaster.caster.origin = Vector2.zero;
			leftLineRaycaster.caster.distance = bounds.size.x;
			leftLineRaycaster.Cast();
			float num = bounds.size.x;
			for (int i = 0; i < leftLineRaycaster.nonAllocCasters.Count; i++)
			{
				ReadonlyBoundedList<RaycastHit2D> results = leftLineRaycaster.nonAllocCasters[i].results;
				if (results.Count != 0)
				{
					RaycastHit2D raycastHit2D = results[0];
					if (raycastHit2D.distance < bounds.size.x && raycastHit2D.distance != 0f)
					{
						this._hit = true;
						num = Mathf.Min(num, raycastHit2D.distance);
						if (num > this._rightMinDistance)
						{
							this._horizontalMoveDelta = -1f * (bounds.size.x - num);
						}
					}
				}
			}
		}

		// Token: 0x040039EF RID: 14831
		[SerializeField]
		private Transform _targetPoint;

		// Token: 0x040039F0 RID: 14832
		[SerializeField]
		private Collider2D _targetCollider;

		// Token: 0x040039F1 RID: 14833
		[SerializeField]
		private float _maxRetryDistance;

		// Token: 0x040039F2 RID: 14834
		[SerializeField]
		private Transform _defaultPosition;

		// Token: 0x040039F3 RID: 14835
		[SerializeField]
		[Range(1f, 10f)]
		private int _horizontalRayCount;

		// Token: 0x040039F4 RID: 14836
		[Range(1f, 10f)]
		[SerializeField]
		private int _verticalRayCount;

		// Token: 0x040039F5 RID: 14837
		private LineSequenceNonAllocCaster _topLineRaycaster;

		// Token: 0x040039F6 RID: 14838
		private LineSequenceNonAllocCaster _bottomLineRaycaster;

		// Token: 0x040039F7 RID: 14839
		private LineSequenceNonAllocCaster _leftLineRaycaster;

		// Token: 0x040039F8 RID: 14840
		private LineSequenceNonAllocCaster _rightLineRaycaster;

		// Token: 0x040039F9 RID: 14841
		private float _upMinDistance;

		// Token: 0x040039FA RID: 14842
		private float _rightMinDistance;

		// Token: 0x040039FB RID: 14843
		private float _horizontalMoveDelta;

		// Token: 0x040039FC RID: 14844
		private float _verticalMoveDelta;

		// Token: 0x040039FD RID: 14845
		private bool _hit;
	}
}
