using System;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EE4 RID: 3812
	public class ToClosestTarget : Policy
	{
		// Token: 0x17000F20 RID: 3872
		// (get) Token: 0x06004AC7 RID: 19143 RVA: 0x000D9CB3 File Offset: 0x000D7EB3
		private Vector2 _default
		{
			get
			{
				return base.transform.position;
			}
		}

		// Token: 0x06004AC8 RID: 19144 RVA: 0x000DABE7 File Offset: 0x000D8DE7
		public override Vector2 GetPosition(Character owner)
		{
			if (this._ownerCollider == null)
			{
				this._ownerCollider = owner.collider;
			}
			return this.GetPosition();
		}

		// Token: 0x06004AC9 RID: 19145 RVA: 0x000DAC0C File Offset: 0x000D8E0C
		public override Vector2 GetPosition()
		{
			Character character = TargetFinder.FindClosestTarget(this._findRange, this._ownerCollider, this._targetLayer.Evaluate(this._ownerCollider.gameObject));
			if (character == null)
			{
				Debug.Log("Target is null");
				return base.transform.position;
			}
			Vector2 result = character.transform.position;
			this.Clamp(ref result, this._amount.value);
			if (!this._onPlatform)
			{
				return result;
			}
			Collider2D lastStandingCollider;
			if (this._lastStandingCollider)
			{
				lastStandingCollider = character.movement.controller.collisionState.lastStandingCollider;
				if (lastStandingCollider == null)
				{
					character.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, 100f);
					if (lastStandingCollider == null)
					{
						return this._default;
					}
				}
			}
			else
			{
				character.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, 100f);
				if (lastStandingCollider == null)
				{
					return this._default;
				}
			}
			float x = character.transform.position.x;
			float y = lastStandingCollider.bounds.max.y;
			return new Vector2(x, y);
		}

		// Token: 0x06004ACA RID: 19146 RVA: 0x000DAD38 File Offset: 0x000D8F38
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

		// Token: 0x06004ACB RID: 19147 RVA: 0x000DAE74 File Offset: 0x000D9074
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

		// Token: 0x040039E7 RID: 14823
		[SerializeField]
		private Collider2D _findRange;

		// Token: 0x040039E8 RID: 14824
		[SerializeField]
		private Collider2D _ownerCollider;

		// Token: 0x040039E9 RID: 14825
		[SerializeField]
		private CustomFloat _amount;

		// Token: 0x040039EA RID: 14826
		[SerializeField]
		private float _belowRayDistance = 100f;

		// Token: 0x040039EB RID: 14827
		[SerializeField]
		private TargetLayer _targetLayer;

		// Token: 0x040039EC RID: 14828
		[SerializeField]
		private bool _behind;

		// Token: 0x040039ED RID: 14829
		[SerializeField]
		private bool _onPlatform;

		// Token: 0x040039EE RID: 14830
		[SerializeField]
		private bool _lastStandingCollider;
	}
}
