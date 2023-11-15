using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EDA RID: 3802
	public class ToBDTarget : Policy
	{
		// Token: 0x17000F1E RID: 3870
		// (get) Token: 0x06004AA0 RID: 19104 RVA: 0x000D9CB3 File Offset: 0x000D7EB3
		private Vector2 _default
		{
			get
			{
				return base.transform.position;
			}
		}

		// Token: 0x06004AA1 RID: 19105 RVA: 0x000D950E File Offset: 0x000D770E
		public override Vector2 GetPosition(Character owner)
		{
			return this.GetPosition();
		}

		// Token: 0x06004AA2 RID: 19106 RVA: 0x000D9F50 File Offset: 0x000D8150
		public override Vector2 GetPosition()
		{
			Character value = this._communicator.GetVariable<SharedCharacter>(this._targetName).Value;
			if (value == null)
			{
				return base.transform.position;
			}
			if (!this._onPlatform)
			{
				return value.transform.position;
			}
			Collider2D lastStandingCollider;
			if (this._lastStandingCollider)
			{
				lastStandingCollider = value.movement.controller.collisionState.lastStandingCollider;
				if (lastStandingCollider == null)
				{
					value.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, 100f);
					if (lastStandingCollider == null)
					{
						return this._default;
					}
				}
			}
			else
			{
				value.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, 100f);
				if (lastStandingCollider == null)
				{
					return this._default;
				}
			}
			float x;
			if (this._interplateCollider != null)
			{
				float min = lastStandingCollider.bounds.min.x + this._interplateCollider.bounds.extents.x;
				float max = lastStandingCollider.bounds.max.x - this._interplateCollider.bounds.extents.x;
				x = this.ClampX(value.transform.position.x, min, max);
			}
			else
			{
				x = value.transform.position.x;
			}
			float y = lastStandingCollider.bounds.max.y;
			return new Vector2(x, y);
		}

		// Token: 0x06004AA3 RID: 19107 RVA: 0x000DA0DC File Offset: 0x000D82DC
		private float ClampX(float x, float min, float max)
		{
			float num = 0.05f;
			if (x <= min)
			{
				return min + num;
			}
			if (x >= max)
			{
				return max - num;
			}
			return x;
		}

		// Token: 0x040039BB RID: 14779
		[SerializeField]
		private BehaviorDesignerCommunicator _communicator;

		// Token: 0x040039BC RID: 14780
		[SerializeField]
		private string _targetName = "Target";

		// Token: 0x040039BD RID: 14781
		[SerializeField]
		private bool _onPlatform;

		// Token: 0x040039BE RID: 14782
		[SerializeField]
		private bool _lastStandingCollider;

		// Token: 0x040039BF RID: 14783
		[SerializeField]
		private Collider2D _interplateCollider;
	}
}
