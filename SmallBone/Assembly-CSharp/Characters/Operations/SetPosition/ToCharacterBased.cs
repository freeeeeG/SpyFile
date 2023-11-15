using System;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EE2 RID: 3810
	public class ToCharacterBased : Policy
	{
		// Token: 0x06004ABF RID: 19135 RVA: 0x000DA912 File Offset: 0x000D8B12
		public override Vector2 GetPosition(Character owner)
		{
			if (this._target == null)
			{
				this._target = owner;
			}
			if (this._collider == null)
			{
				this._collider = owner.collider;
			}
			return this.GetPosition();
		}

		// Token: 0x06004AC0 RID: 19136 RVA: 0x000DA94C File Offset: 0x000D8B4C
		public override Vector2 GetPosition()
		{
			Vector2 vector = this._target.transform.position;
			this.Clamp(ref vector, this._amount.value);
			if (!this._onPlatform)
			{
				return vector;
			}
			Collider2D lastStandingCollider;
			if (this._lastStandingCollider)
			{
				lastStandingCollider = this._target.movement.controller.collisionState.lastStandingCollider;
			}
			else
			{
				this._target.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, this._belowRayDistance);
			}
			float x = vector.x;
			float y = lastStandingCollider.bounds.max.y;
			return new Vector2(x, y);
		}

		// Token: 0x06004AC1 RID: 19137 RVA: 0x000DA9F0 File Offset: 0x000D8BF0
		private void Clamp(ref Vector2 result, float amount)
		{
			Collider2D lastStandingCollider;
			if (this._lastStandingCollider)
			{
				lastStandingCollider = this._target.movement.controller.collisionState.lastStandingCollider;
				if (lastStandingCollider == null)
				{
					this._target.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, this._belowRayDistance);
				}
			}
			else
			{
				this._target.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, this._belowRayDistance);
			}
			float min = lastStandingCollider.bounds.min.x + this._collider.bounds.extents.x;
			float max = lastStandingCollider.bounds.max.x - this._collider.bounds.extents.x;
			if (this._target.lookingDirection == Character.LookingDirection.Right)
			{
				result = this.ClampX(result, this._behind ? (result.x - amount) : (result.x + amount), min, max);
				return;
			}
			result = this.ClampX(result, this._behind ? (result.x + amount) : (result.x - amount), min, max);
		}

		// Token: 0x06004AC2 RID: 19138 RVA: 0x000DAB2C File Offset: 0x000D8D2C
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

		// Token: 0x040039DD RID: 14813
		[SerializeField]
		private Character _target;

		// Token: 0x040039DE RID: 14814
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x040039DF RID: 14815
		[SerializeField]
		private CustomFloat _amount;

		// Token: 0x040039E0 RID: 14816
		[SerializeField]
		private float _belowRayDistance = 100f;

		// Token: 0x040039E1 RID: 14817
		[SerializeField]
		private bool _behind;

		// Token: 0x040039E2 RID: 14818
		[SerializeField]
		private bool _onPlatform;

		// Token: 0x040039E3 RID: 14819
		[SerializeField]
		private bool _lastStandingCollider;
	}
}
