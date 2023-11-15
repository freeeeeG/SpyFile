using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EE6 RID: 3814
	public class ToKeepDistance : Policy
	{
		// Token: 0x17000F21 RID: 3873
		// (get) Token: 0x06004AD6 RID: 19158 RVA: 0x000D9CB3 File Offset: 0x000D7EB3
		private Vector2 _default
		{
			get
			{
				return base.transform.position;
			}
		}

		// Token: 0x06004AD7 RID: 19159 RVA: 0x000D950E File Offset: 0x000D770E
		public override Vector2 GetPosition(Character owner)
		{
			return this.GetPosition();
		}

		// Token: 0x06004AD8 RID: 19160 RVA: 0x000DB608 File Offset: 0x000D9808
		public override Vector2 GetPosition()
		{
			Character value = this._communicator.GetVariable<SharedCharacter>(this._targetValueName).Value;
			Collider2D lastStandingCollider;
			if (this._lastStandingCollider)
			{
				lastStandingCollider = value.movement.controller.collisionState.lastStandingCollider;
			}
			else
			{
				value.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, 100f);
			}
			if (lastStandingCollider == null)
			{
				return this._default;
			}
			Bounds bounds = lastStandingCollider.bounds;
			float value2 = this._distance.value;
			BoxCollider2D collider = this._owner.collider;
			Vector2 lhs;
			if (this._basedCharacter == ToKeepDistance.BasedCharacter.Owner)
			{
				if (this._direction == ToKeepDistance.Direction.Looking)
				{
					lhs = ((this._owner.lookingDirection == Character.LookingDirection.Left) ? Vector2.left : Vector2.right);
				}
				else
				{
					lhs = ((this._owner.lookingDirection == Character.LookingDirection.Left) ? Vector2.right : Vector2.left);
				}
			}
			else if (this._direction == ToKeepDistance.Direction.Looking)
			{
				lhs = ((value.lookingDirection == Character.LookingDirection.Left) ? Vector2.left : Vector2.right);
			}
			else
			{
				lhs = ((value.lookingDirection == Character.LookingDirection.Left) ? Vector2.right : Vector2.left);
			}
			Vector2 vector;
			if (lhs == Vector2.left)
			{
				if (value.transform.position.x - value2 + collider.bounds.extents.x + collider.offset.x > bounds.min.x)
				{
					vector.x = value.transform.position.x - value2 + collider.bounds.extents.x + collider.offset.x + this._epsilon;
				}
				else if (value.transform.position.x + value2 - collider.bounds.extents.x + collider.offset.x < bounds.max.x)
				{
					vector.x = value.transform.position.x + value2 - collider.bounds.extents.x + collider.offset.x - this._epsilon;
				}
				else
				{
					vector.x = bounds.min.x + collider.bounds.extents.x + collider.offset.x + this._epsilon;
				}
			}
			else if (value.transform.position.x + value2 - collider.bounds.extents.x + collider.offset.x < bounds.max.x)
			{
				vector.x = value.transform.position.x + value2 - collider.bounds.extents.x + collider.offset.x - this._epsilon;
			}
			else if (value.transform.position.x - value2 + collider.bounds.extents.x + collider.offset.x > bounds.min.x)
			{
				vector.x = value.transform.position.x - value2 + collider.bounds.extents.x + collider.offset.x + this._epsilon;
			}
			else
			{
				vector.x = bounds.max.x - collider.bounds.extents.x + collider.offset.x - this._epsilon;
			}
			vector.x = this.ClampX(vector.x, bounds.max.x - collider.bounds.extents.x + collider.offset.x - this._epsilon, bounds.min.x + collider.bounds.extents.x + collider.offset.x + this._epsilon);
			if (this._lastStandingCollider)
			{
				vector.y = bounds.max.y;
			}
			else
			{
				vector.y = this._owner.transform.position.y;
			}
			return new Vector2(vector.x, vector.y);
		}

		// Token: 0x06004AD9 RID: 19161 RVA: 0x000DBA9D File Offset: 0x000D9C9D
		private float ClampX(float x, float max, float min)
		{
			if (x > max)
			{
				return max;
			}
			if (x < min)
			{
				return min;
			}
			return x;
		}

		// Token: 0x040039FE RID: 14846
		[SerializeField]
		private BehaviorDesignerCommunicator _communicator;

		// Token: 0x040039FF RID: 14847
		[SerializeField]
		private Character _owner;

		// Token: 0x04003A00 RID: 14848
		[SerializeField]
		private string _targetValueName = "Target";

		// Token: 0x04003A01 RID: 14849
		[SerializeField]
		private ToKeepDistance.BasedCharacter _basedCharacter;

		// Token: 0x04003A02 RID: 14850
		[SerializeField]
		private ToKeepDistance.Direction _direction;

		// Token: 0x04003A03 RID: 14851
		[SerializeField]
		private CustomFloat _distance;

		// Token: 0x04003A04 RID: 14852
		[SerializeField]
		private bool _lastStandingCollider;

		// Token: 0x04003A05 RID: 14853
		private float _epsilon = 0.05f;

		// Token: 0x02000EE7 RID: 3815
		private enum BasedCharacter
		{
			// Token: 0x04003A07 RID: 14855
			Owner,
			// Token: 0x04003A08 RID: 14856
			Target
		}

		// Token: 0x02000EE8 RID: 3816
		private enum Direction
		{
			// Token: 0x04003A0A RID: 14858
			Looking,
			// Token: 0x04003A0B RID: 14859
			ReverseLooking
		}
	}
}
