using System;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EF3 RID: 3827
	public class ToPlayerBased : Policy
	{
		// Token: 0x06004AFB RID: 19195 RVA: 0x000DC50E File Offset: 0x000DA70E
		public override Vector2 GetPosition(Character owner)
		{
			if (this._collider == null)
			{
				this._collider = owner.collider;
			}
			return this.GetPosition();
		}

		// Token: 0x06004AFC RID: 19196 RVA: 0x000DC530 File Offset: 0x000DA730
		public override Vector2 GetPosition()
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			Vector2 vector = player.transform.position;
			this.Clamp(ref vector, this._amount.value);
			if (!this._onPlatform)
			{
				return vector;
			}
			Collider2D lastStandingCollider;
			if (this._lastStandingCollider)
			{
				lastStandingCollider = player.movement.controller.collisionState.lastStandingCollider;
			}
			else
			{
				player.movement.TryGetClosestBelowCollider(out lastStandingCollider, this._platformLayerMask, this._belowRayDistance);
			}
			float x = vector.x;
			float y = lastStandingCollider.bounds.max.y;
			return new Vector2(x, y);
		}

		// Token: 0x06004AFD RID: 19197 RVA: 0x000DC5D8 File Offset: 0x000DA7D8
		private void Clamp(ref Vector2 result, float amount)
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			Collider2D lastStandingCollider;
			if (this._lastStandingCollider)
			{
				lastStandingCollider = player.movement.controller.collisionState.lastStandingCollider;
				if (lastStandingCollider == null)
				{
					player.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, this._belowRayDistance);
				}
			}
			else
			{
				player.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, this._belowRayDistance);
			}
			float min = lastStandingCollider.bounds.min.x + this._collider.bounds.extents.x;
			float max = lastStandingCollider.bounds.max.x - this._collider.bounds.extents.x;
			if (player.lookingDirection == Character.LookingDirection.Right)
			{
				result = this.ClampX(result, this._behind ? (result.x - amount) : (result.x + amount), min, max);
				return;
			}
			result = this.ClampX(result, this._behind ? (result.x + amount) : (result.x - amount), min, max);
		}

		// Token: 0x06004AFE RID: 19198 RVA: 0x000DC714 File Offset: 0x000DA914
		private Vector2 ClampX(Vector2 result, float x, float min, float max)
		{
			float num = 0.05f;
			if (x <= min)
			{
				result = new Vector2(min + num, result.y);
			}
			else if (x >= max)
			{
				result = new Vector2(max - num, result.y);
			}
			else
			{
				result = new Vector2(x, result.y);
			}
			return result;
		}

		// Token: 0x04003A30 RID: 14896
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x04003A31 RID: 14897
		[SerializeField]
		private CustomFloat _amount;

		// Token: 0x04003A32 RID: 14898
		[SerializeField]
		private float _belowRayDistance = 100f;

		// Token: 0x04003A33 RID: 14899
		[SerializeField]
		private bool _behind;

		// Token: 0x04003A34 RID: 14900
		[SerializeField]
		private bool _onPlatform;

		// Token: 0x04003A35 RID: 14901
		[SerializeField]
		private bool _lastStandingCollider;

		// Token: 0x04003A36 RID: 14902
		[SerializeField]
		private LayerMask _platformLayerMask = Layers.footholdMask;
	}
}
