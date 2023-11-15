using System;
using Characters.Utils;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EF7 RID: 3831
	public sealed class ToRayPoint : Policy
	{
		// Token: 0x06004B0F RID: 19215 RVA: 0x000D950E File Offset: 0x000D770E
		public override Vector2 GetPosition(Character owner)
		{
			return this.GetPosition();
		}

		// Token: 0x06004B10 RID: 19216 RVA: 0x000DCD07 File Offset: 0x000DAF07
		private void Awake()
		{
			this._nonAllocCaster = new NonAllocCaster(1);
			this._nonAllocCaster.contactFilter.SetLayerMask(this._targetLayerMask);
			if (this._onPlatform || this._onPlatformWhenHitTerrain)
			{
				this._belowCaster = new NonAllocCaster(1);
			}
		}

		// Token: 0x06004B11 RID: 19217 RVA: 0x000DCD48 File Offset: 0x000DAF48
		public override Vector2 GetPosition()
		{
			Vector2 vector = this._target.transform.position - this._origin.transform.position;
			float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			if (num < 0f)
			{
				num += 360f;
			}
			if (this._target.transform.position.x > this._origin.transform.position.x)
			{
				if (num > 90f && num < 270f)
				{
					vector = Vector2.down;
				}
				else if (num > 0f && num < 90f)
				{
					vector = Vector2.right;
				}
			}
			else if (num < 90f || num > 270f)
			{
				vector = Vector2.down;
			}
			else if (num > 90f && num < 180f)
			{
				vector = Vector2.left;
			}
			this._nonAllocCaster.RayCast(this._origin.transform.position, vector, this._distance);
			if (this._nonAllocCaster.results.Count == 0)
			{
				return this._target.transform.position;
			}
			Vector2 point = this._nonAllocCaster.results[0].point;
			if (this._onPlatform)
			{
				return PlatformUtils.GetProjectionPointToPlatform(point, Vector2.down, this._belowCaster, 262144, 100f);
			}
			if (this._onPlatformWhenHitTerrain)
			{
				if (this._nonAllocCaster.results[0].collider.gameObject.layer == 8)
				{
					return PlatformUtils.GetProjectionPointToPlatform(point, Vector2.down, this._belowCaster, 262144, 100f);
				}
			}
			else if (point.y > this._origin.position.y)
			{
				point = new Vector2(point.x, this._origin.position.y);
			}
			return point;
		}

		// Token: 0x04003A45 RID: 14917
		[SerializeField]
		private Transform _origin;

		// Token: 0x04003A46 RID: 14918
		[SerializeField]
		private Transform _target;

		// Token: 0x04003A47 RID: 14919
		[SerializeField]
		private LayerMask _targetLayerMask;

		// Token: 0x04003A48 RID: 14920
		[SerializeField]
		private float _distance;

		// Token: 0x04003A49 RID: 14921
		[SerializeField]
		private bool _onPlatform;

		// Token: 0x04003A4A RID: 14922
		[SerializeField]
		private bool _onPlatformWhenHitTerrain;

		// Token: 0x04003A4B RID: 14923
		private NonAllocCaster _nonAllocCaster;

		// Token: 0x04003A4C RID: 14924
		private NonAllocCaster _belowCaster;
	}
}
