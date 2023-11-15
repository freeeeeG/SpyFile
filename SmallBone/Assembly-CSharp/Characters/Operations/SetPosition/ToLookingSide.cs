using System;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EEC RID: 3820
	public class ToLookingSide : Policy
	{
		// Token: 0x06004AE2 RID: 19170 RVA: 0x000DBC92 File Offset: 0x000D9E92
		public override Vector2 GetPosition(Character owner)
		{
			if (this._owner == null)
			{
				this._owner = owner;
			}
			return this.GetPosition();
		}

		// Token: 0x06004AE3 RID: 19171 RVA: 0x000DBCB0 File Offset: 0x000D9EB0
		public override Vector2 GetPosition()
		{
			if (this._owner == null)
			{
				return this._owner.transform.position;
			}
			Collider2D lastStandingCollider = this._owner.movement.controller.collisionState.lastStandingCollider;
			if (!this._lastStandingCollider)
			{
				this._owner.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, 100f);
			}
			if (lastStandingCollider == null)
			{
				return this._owner.transform.position;
			}
			Character.LookingDirection lookingDirection = this._owner.lookingDirection;
			float x = this.ClampX(this._owner, lookingDirection, lastStandingCollider.bounds);
			float y = this.CalculateY(this._owner, lastStandingCollider.bounds);
			return new Vector2(x, y);
		}

		// Token: 0x06004AE4 RID: 19172 RVA: 0x000DBD78 File Offset: 0x000D9F78
		private float ClampX(Character owner, Character.LookingDirection direciton, Bounds platform)
		{
			if (!this._opposition)
			{
				if (direciton == Character.LookingDirection.Right)
				{
					return platform.max.x + (this._colliderInterpolate ? (-owner.collider.bounds.extents.x) : 0f);
				}
				return platform.min.x + (this._colliderInterpolate ? owner.collider.bounds.extents.x : 0f);
			}
			else
			{
				if (direciton == Character.LookingDirection.Right)
				{
					return platform.min.x + (this._colliderInterpolate ? owner.collider.bounds.extents.x : 0f);
				}
				return platform.max.x + (this._colliderInterpolate ? (-owner.collider.bounds.extents.x) : 0f);
			}
		}

		// Token: 0x06004AE5 RID: 19173 RVA: 0x000DBE68 File Offset: 0x000DA068
		private float CalculateY(Character target, Bounds platform)
		{
			if (!this._onPlatform)
			{
				return target.transform.position.y;
			}
			return platform.max.y;
		}

		// Token: 0x04003A15 RID: 14869
		[SerializeField]
		private Character _owner;

		// Token: 0x04003A16 RID: 14870
		[SerializeField]
		private bool _onPlatform;

		// Token: 0x04003A17 RID: 14871
		[SerializeField]
		private bool _lastStandingCollider = true;

		// Token: 0x04003A18 RID: 14872
		[SerializeField]
		private bool _opposition;

		// Token: 0x04003A19 RID: 14873
		[SerializeField]
		private bool _colliderInterpolate;
	}
}
