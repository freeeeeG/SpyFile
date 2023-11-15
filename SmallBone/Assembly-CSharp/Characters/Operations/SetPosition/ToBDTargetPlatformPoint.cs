using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EDC RID: 3804
	public class ToBDTargetPlatformPoint : Policy
	{
		// Token: 0x06004AAB RID: 19115 RVA: 0x000D950E File Offset: 0x000D770E
		public override Vector2 GetPosition(Character owner)
		{
			return this.GetPosition();
		}

		// Token: 0x06004AAC RID: 19116 RVA: 0x000DA340 File Offset: 0x000D8540
		public override Vector2 GetPosition()
		{
			Character value = this._communicator.GetVariable<SharedCharacter>(this._targetName).Value;
			if (value == null)
			{
				return base.transform.position;
			}
			Collider2D lastStandingCollider;
			if (this._lastStandingCollider)
			{
				lastStandingCollider = value.movement.controller.collisionState.lastStandingCollider;
				if (lastStandingCollider == null)
				{
					value.movement.TryGetClosestBelowCollider(out lastStandingCollider, this._platformLayer, 100f);
				}
			}
			else
			{
				value.movement.TryGetClosestBelowCollider(out lastStandingCollider, this._platformLayer, 100f);
			}
			Bounds bounds = lastStandingCollider.bounds;
			switch (this._point)
			{
			case ToBDTargetPlatformPoint.Point.Left:
			{
				Vector2 result = new Vector2(bounds.min.x, bounds.max.y);
				if (this._colliderInterpolation)
				{
					result = new Vector2(this.ClampX(value, bounds.min.x, bounds), bounds.max.y);
				}
				return result;
			}
			case ToBDTargetPlatformPoint.Point.Center:
			{
				Vector2 result = new Vector2(bounds.center.x, bounds.max.y);
				if (this._colliderInterpolation)
				{
					result = new Vector2(this.ClampX(value, bounds.center.x, bounds), bounds.max.y);
				}
				return result;
			}
			case ToBDTargetPlatformPoint.Point.Right:
			{
				Vector2 result = new Vector2(bounds.max.x, bounds.max.y);
				if (this._colliderInterpolation)
				{
					result = new Vector2(this.ClampX(value, bounds.max.x, bounds), bounds.max.y);
				}
				return result;
			}
			case ToBDTargetPlatformPoint.Point.Random:
			{
				float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
				Vector2 result = new Vector2(x, bounds.max.y);
				if (this._colliderInterpolation)
				{
					result = new Vector2(this.ClampX(value, x, bounds), bounds.max.y);
				}
				return result;
			}
			default:
				return value.transform.position;
			}
		}

		// Token: 0x06004AAD RID: 19117 RVA: 0x000DA560 File Offset: 0x000D8760
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

		// Token: 0x040039C5 RID: 14789
		[SerializeField]
		private BehaviorDesignerCommunicator _communicator;

		// Token: 0x040039C6 RID: 14790
		[SerializeField]
		private string _targetName = "Target";

		// Token: 0x040039C7 RID: 14791
		[SerializeField]
		private LayerMask _platformLayer = Layers.footholdMask;

		// Token: 0x040039C8 RID: 14792
		[SerializeField]
		private bool _lastStandingCollider = true;

		// Token: 0x040039C9 RID: 14793
		[SerializeField]
		private bool _colliderInterpolation = true;

		// Token: 0x040039CA RID: 14794
		[SerializeField]
		private ToBDTargetPlatformPoint.Point _point;

		// Token: 0x02000EDD RID: 3805
		private enum Point
		{
			// Token: 0x040039CC RID: 14796
			Left,
			// Token: 0x040039CD RID: 14797
			Center,
			// Token: 0x040039CE RID: 14798
			Right,
			// Token: 0x040039CF RID: 14799
			Random
		}
	}
}
