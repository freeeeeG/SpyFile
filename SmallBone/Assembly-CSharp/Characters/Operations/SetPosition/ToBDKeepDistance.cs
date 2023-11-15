using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000ED9 RID: 3801
	public class ToBDKeepDistance : Policy
	{
		// Token: 0x17000F1D RID: 3869
		// (get) Token: 0x06004A9C RID: 19100 RVA: 0x000D9CB3 File Offset: 0x000D7EB3
		private Vector2 _default
		{
			get
			{
				return base.transform.position;
			}
		}

		// Token: 0x06004A9D RID: 19101 RVA: 0x000D950E File Offset: 0x000D770E
		public override Vector2 GetPosition(Character owner)
		{
			return this.GetPosition();
		}

		// Token: 0x06004A9E RID: 19102 RVA: 0x000D9CC8 File Offset: 0x000D7EC8
		public override Vector2 GetPosition()
		{
			SharedVariable<Character> sharedVariable = this._tree.GetVariable(this._ownerValueName) as SharedCharacter;
			Character value = (this._tree.GetVariable(this._targetValueName) as SharedCharacter).Value;
			Character value2 = sharedVariable.Value;
			BoxCollider2D collider = value2.collider;
			Collider2D lastStandingCollider = value2.movement.controller.collisionState.lastStandingCollider;
			if (lastStandingCollider == null)
			{
				value2.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, 100f);
				if (lastStandingCollider == null)
				{
					return this._default;
				}
			}
			Bounds bounds = lastStandingCollider.bounds;
			Vector2 lhs = (value.transform.position.x - value2.transform.position.x > 0f) ? Vector2.left : Vector2.right;
			float value3 = this._distance.value;
			Vector3 position = value2.transform.position;
			float x = (lhs == Vector2.left) ? Mathf.Max(bounds.min.x + collider.bounds.extents.x + value2.collider.offset.x, position.x - value3) : Mathf.Min(bounds.max.x - collider.bounds.extents.x + value2.collider.offset.x, position.x + value3);
			if (lhs == Vector2.right && bounds.max.x < position.x + value3)
			{
				x = Mathf.Max(bounds.min.x + collider.bounds.extents.x + value2.collider.offset.x, position.x - value3);
			}
			else if (lhs == Vector2.left && bounds.min.x > position.x - value3)
			{
				x = Mathf.Min(bounds.max.x - collider.bounds.extents.x + value2.collider.offset.x, position.x + value3);
			}
			return new Vector2(x, value2.transform.position.y);
		}

		// Token: 0x040039B7 RID: 14775
		[SerializeField]
		private BehaviorTree _tree;

		// Token: 0x040039B8 RID: 14776
		[SerializeField]
		private string _ownerValueName = "Owner";

		// Token: 0x040039B9 RID: 14777
		[SerializeField]
		private string _targetValueName = "Target";

		// Token: 0x040039BA RID: 14778
		[SerializeField]
		private CustomFloat _distance;
	}
}
