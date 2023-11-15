using System;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EEA RID: 3818
	public class ToLookingEndPoint : Policy
	{
		// Token: 0x06004ADE RID: 19166 RVA: 0x000DBAF7 File Offset: 0x000D9CF7
		public override Vector2 GetPosition(Character owner)
		{
			if (this._owner == null)
			{
				this._owner = owner;
			}
			return this.GetPosition();
		}

		// Token: 0x06004ADF RID: 19167 RVA: 0x000DBB14 File Offset: 0x000D9D14
		public override Vector2 GetPosition()
		{
			Bounds bounds = this._owner.movement.controller.collisionState.lastStandingCollider.bounds;
			Character.LookingDirection lookingDirection = this._owner.lookingDirection;
			switch (this._point)
			{
			case ToLookingEndPoint.Point.Left:
				return new Vector2(this.ClampX(this._owner, bounds.min.x, bounds), bounds.max.y);
			case ToLookingEndPoint.Point.Center:
				return new Vector2(this.ClampX(this._owner, bounds.center.x, bounds), bounds.max.y);
			case ToLookingEndPoint.Point.Right:
				return new Vector2(this.ClampX(this._owner, bounds.max.x, bounds), bounds.max.y);
			default:
				return this._owner.transform.position;
			}
		}

		// Token: 0x06004AE0 RID: 19168 RVA: 0x000DBC04 File Offset: 0x000D9E04
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

		// Token: 0x04003A0F RID: 14863
		[SerializeField]
		private Character _owner;

		// Token: 0x04003A10 RID: 14864
		[SerializeField]
		private ToLookingEndPoint.Point _point;

		// Token: 0x02000EEB RID: 3819
		private enum Point
		{
			// Token: 0x04003A12 RID: 14866
			Left,
			// Token: 0x04003A13 RID: 14867
			Center,
			// Token: 0x04003A14 RID: 14868
			Right
		}
	}
}
