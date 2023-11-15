using System;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EF0 RID: 3824
	public class ToPlatformPoint : Policy
	{
		// Token: 0x06004AF4 RID: 19188 RVA: 0x000DC1D1 File Offset: 0x000DA3D1
		public override Vector2 GetPosition(Character owner)
		{
			if (this._owner == null)
			{
				this._owner = owner;
			}
			return this.GetPosition();
		}

		// Token: 0x06004AF5 RID: 19189 RVA: 0x000DC1F0 File Offset: 0x000DA3F0
		public override Vector2 GetPosition()
		{
			Collider2D lastStandingCollider;
			if (this._lastStandingCollider)
			{
				lastStandingCollider = this._owner.movement.controller.collisionState.lastStandingCollider;
				if (lastStandingCollider == null)
				{
					this._owner.movement.TryGetClosestBelowCollider(out lastStandingCollider, this._platformLayer, 100f);
				}
			}
			else
			{
				this._owner.movement.TryGetClosestBelowCollider(out lastStandingCollider, this._platformLayer, 100f);
			}
			Bounds bounds = lastStandingCollider.bounds;
			switch (this._point)
			{
			case ToPlatformPoint.Point.Left:
			{
				Vector2 result = new Vector2(bounds.min.x, bounds.max.y);
				if (this._colliderInterpolation)
				{
					result = new Vector2(this.ClampX(this._owner, bounds.min.x, bounds), bounds.max.y);
				}
				return result;
			}
			case ToPlatformPoint.Point.Center:
			{
				Vector2 result = new Vector2(bounds.center.x, bounds.max.y);
				if (this._colliderInterpolation)
				{
					result = new Vector2(this.ClampX(this._owner, bounds.center.x, bounds), bounds.max.y);
				}
				return result;
			}
			case ToPlatformPoint.Point.Right:
			{
				Vector2 result = new Vector2(bounds.max.x, bounds.max.y);
				if (this._colliderInterpolation)
				{
					result = new Vector2(this.ClampX(this._owner, bounds.max.x, bounds), bounds.max.y);
				}
				return result;
			}
			default:
				return this._owner.transform.position;
			}
		}

		// Token: 0x06004AF6 RID: 19190 RVA: 0x000DC3A0 File Offset: 0x000DA5A0
		private float ClampX(Character owner, float x, Bounds platform)
		{
			if (x <= platform.min.x + owner.collider.size.x)
			{
				x = platform.min.x + owner.collider.size.x;
			}
			else if (x >= platform.max.x - owner.collider.size.x)
			{
				x = platform.max.x - owner.collider.size.x;
			}
			return x;
		}

		// Token: 0x04003A24 RID: 14884
		[SerializeField]
		private Character _owner;

		// Token: 0x04003A25 RID: 14885
		[SerializeField]
		private LayerMask _platformLayer = Layers.footholdMask;

		// Token: 0x04003A26 RID: 14886
		[SerializeField]
		private bool _lastStandingCollider = true;

		// Token: 0x04003A27 RID: 14887
		[SerializeField]
		private bool _colliderInterpolation = true;

		// Token: 0x04003A28 RID: 14888
		[SerializeField]
		private ToPlatformPoint.Point _point;

		// Token: 0x02000EF1 RID: 3825
		private enum Point
		{
			// Token: 0x04003A2A RID: 14890
			Left,
			// Token: 0x04003A2B RID: 14891
			Center,
			// Token: 0x04003A2C RID: 14892
			Right
		}
	}
}
