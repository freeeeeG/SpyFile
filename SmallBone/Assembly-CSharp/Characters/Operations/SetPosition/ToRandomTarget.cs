using System;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EF6 RID: 3830
	public class ToRandomTarget : Policy
	{
		// Token: 0x17000F22 RID: 3874
		// (get) Token: 0x06004B09 RID: 19209 RVA: 0x000D9CB3 File Offset: 0x000D7EB3
		private Vector2 _default
		{
			get
			{
				return base.transform.position;
			}
		}

		// Token: 0x06004B0A RID: 19210 RVA: 0x000DCA22 File Offset: 0x000DAC22
		public override Vector2 GetPosition(Character owner)
		{
			if (this._ownerCollider == null)
			{
				this._ownerCollider = owner.collider;
			}
			return this.GetPosition();
		}

		// Token: 0x06004B0B RID: 19211 RVA: 0x000DCA44 File Offset: 0x000DAC44
		public override Vector2 GetPosition()
		{
			Character randomTarget = TargetFinder.GetRandomTarget(this._findRange, this._targetLayer.Evaluate(this._ownerCollider.gameObject));
			if (randomTarget == null)
			{
				Debug.Log("Target is null");
				return base.transform.position;
			}
			Vector2 result = randomTarget.transform.position;
			this.Clamp(ref result, this._amount.value);
			if (!this._onPlatform)
			{
				return result;
			}
			Collider2D lastStandingCollider;
			if (this._lastStandingCollider)
			{
				lastStandingCollider = randomTarget.movement.controller.collisionState.lastStandingCollider;
				if (lastStandingCollider == null)
				{
					randomTarget.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, 100f);
					if (lastStandingCollider == null)
					{
						return this._default;
					}
				}
			}
			else
			{
				randomTarget.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, 100f);
				if (lastStandingCollider == null)
				{
					return this._default;
				}
			}
			float x = randomTarget.transform.position.x;
			float y = lastStandingCollider.bounds.max.y;
			return new Vector2(x, y);
		}

		// Token: 0x06004B0C RID: 19212 RVA: 0x000DCB68 File Offset: 0x000DAD68
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
			float min = lastStandingCollider.bounds.min.x + this._ownerCollider.bounds.size.x;
			float max = lastStandingCollider.bounds.max.x - this._ownerCollider.bounds.size.x;
			if (player.lookingDirection == Character.LookingDirection.Right)
			{
				result = this.ClampX(result, this._behind ? (result.x - amount) : (result.x + amount), min, max);
				return;
			}
			result = this.ClampX(result, this._behind ? (result.x + amount) : (result.x - amount), min, max);
		}

		// Token: 0x06004B0D RID: 19213 RVA: 0x000DCCA4 File Offset: 0x000DAEA4
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

		// Token: 0x04003A3D RID: 14909
		[SerializeField]
		private Collider2D _findRange;

		// Token: 0x04003A3E RID: 14910
		[SerializeField]
		private Collider2D _ownerCollider;

		// Token: 0x04003A3F RID: 14911
		[SerializeField]
		private CustomFloat _amount;

		// Token: 0x04003A40 RID: 14912
		[SerializeField]
		private float _belowRayDistance = 100f;

		// Token: 0x04003A41 RID: 14913
		[SerializeField]
		private TargetLayer _targetLayer;

		// Token: 0x04003A42 RID: 14914
		[SerializeField]
		private bool _behind;

		// Token: 0x04003A43 RID: 14915
		[SerializeField]
		private bool _onPlatform;

		// Token: 0x04003A44 RID: 14916
		[SerializeField]
		private bool _lastStandingCollider;
	}
}
