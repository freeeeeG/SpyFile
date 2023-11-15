using System;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EEE RID: 3822
	public class ToOppositionPlatform : Policy
	{
		// Token: 0x06004AEB RID: 19179 RVA: 0x000DBF19 File Offset: 0x000DA119
		public override Vector2 GetPosition(Character owner)
		{
			if (this._owner == null)
			{
				this._owner = owner;
			}
			return this.GetPosition();
		}

		// Token: 0x06004AEC RID: 19180 RVA: 0x000DBF38 File Offset: 0x000DA138
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
			Bounds bounds = lastStandingCollider.bounds;
			Vector3 center = bounds.center;
			float x = this.CalculateX(this._owner, ref bounds, center);
			float y = this.CalculateY(this._owner, bounds);
			if (this._colliderInterpolate)
			{
				x = this.ClampX(this._owner, x, bounds);
			}
			return new Vector2(x, y);
		}

		// Token: 0x06004AED RID: 19181 RVA: 0x000DC018 File Offset: 0x000DA218
		private float ClampX(Character owner, float x, Bounds platform)
		{
			if (x <= platform.min.x + owner.collider.bounds.extents.x)
			{
				x = platform.min.x + owner.collider.bounds.extents.x;
			}
			else if (x >= platform.max.x - owner.collider.bounds.extents.x)
			{
				x = platform.max.x - owner.collider.bounds.extents.x;
			}
			return x;
		}

		// Token: 0x06004AEE RID: 19182 RVA: 0x000DC0C6 File Offset: 0x000DA2C6
		private float CalculateY(Character target, Bounds platform)
		{
			if (!this._onPlatform)
			{
				return target.transform.position.y;
			}
			return platform.max.y;
		}

		// Token: 0x06004AEF RID: 19183 RVA: 0x000DC0F0 File Offset: 0x000DA2F0
		private float CalculateX(Character target, ref Bounds platform, Vector3 center)
		{
			float result;
			if (target.transform.position.x > center.x)
			{
				result = (this._randomX ? UnityEngine.Random.Range(platform.min.x, platform.center.x) : platform.min.x);
			}
			else
			{
				result = (this._randomX ? UnityEngine.Random.Range(platform.center.x, platform.max.x) : platform.max.x);
			}
			return result;
		}

		// Token: 0x04003A1E RID: 14878
		[SerializeField]
		private Character _owner;

		// Token: 0x04003A1F RID: 14879
		[SerializeField]
		private bool _onPlatform = true;

		// Token: 0x04003A20 RID: 14880
		[SerializeField]
		private bool _lastStandingCollider = true;

		// Token: 0x04003A21 RID: 14881
		[SerializeField]
		private bool _randomX;

		// Token: 0x04003A22 RID: 14882
		[SerializeField]
		private bool _colliderInterpolate;
	}
}
