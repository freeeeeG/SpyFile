using System;
using Characters;
using Characters.Utils;
using PhysicsUtils;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001471 RID: 5233
	public class MoveToDestination : Action
	{
		// Token: 0x06006615 RID: 26133 RVA: 0x00126F50 File Offset: 0x00125150
		public override void OnStart()
		{
			this._ownerValue = this._owner.Value;
			this._destinationTransformValue = this._destinationTransform.Value;
			this._destinationCollider = PlatformUtils.GetClosestPlatform(this._destinationTransformValue.position, Vector2.down, MoveToDestination._belowCaster, this._groundMask, 100f);
		}

		// Token: 0x06006616 RID: 26134 RVA: 0x00126FB0 File Offset: 0x001251B0
		public override TaskStatus OnUpdate()
		{
			if (this._ownerValue == null || this._destinationTransformValue == null)
			{
				return TaskStatus.Failure;
			}
			if (this._ownerValue.movement.controller.collisionState.lastStandingCollider == null)
			{
				return TaskStatus.Running;
			}
			if (this._destinationCollider != this._ownerValue.movement.controller.collisionState.lastStandingCollider)
			{
				return TaskStatus.Failure;
			}
			Vector2 vector = (this._destinationTransformValue.position - this._ownerValue.transform.position).normalized;
			float num = this._ownerValue.stat.GetInterpolatedMovementSpeed() * this._ownerValue.chronometer.master.deltaTime;
			Vector2 b = vector * num;
			float num2 = Vector2.Distance(this._destinationTransformValue.position, this._ownerValue.transform.position);
			float num3 = Vector2.Distance(this._destinationTransformValue.position, this._ownerValue.transform.position + b);
			if (num2 <= num3)
			{
				return TaskStatus.Success;
			}
			if (num2 <= this.minimumDistanceValue)
			{
				return TaskStatus.Success;
			}
			if (this._stopOnPlatformEdge)
			{
				if (this._ownerValue.collider.bounds.max.x + num >= this._ownerValue.movement.controller.collisionState.lastStandingCollider.bounds.max.x && this._ownerValue.lookingDirection == Character.LookingDirection.Right)
				{
					return TaskStatus.Failure;
				}
				if (this._ownerValue.collider.bounds.min.x + num <= this._ownerValue.movement.controller.collisionState.lastStandingCollider.bounds.min.x && this._ownerValue.lookingDirection == Character.LookingDirection.Left)
				{
					return TaskStatus.Failure;
				}
			}
			this._ownerValue.movement.Move(vector);
			return TaskStatus.Running;
		}

		// Token: 0x0400520C RID: 21004
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x0400520D RID: 21005
		[SerializeField]
		private SharedTransform _destinationTransform;

		// Token: 0x0400520E RID: 21006
		[SerializeField]
		private float minimumDistanceValue = 0.1f;

		// Token: 0x0400520F RID: 21007
		[SerializeField]
		private bool _stopOnPlatformEdge;

		// Token: 0x04005210 RID: 21008
		private Character _ownerValue;

		// Token: 0x04005211 RID: 21009
		private Transform _destinationTransformValue;

		// Token: 0x04005212 RID: 21010
		private static NonAllocCaster _belowCaster = new NonAllocCaster(1);

		// Token: 0x04005213 RID: 21011
		private LayerMask _groundMask = Layers.groundMask;

		// Token: 0x04005214 RID: 21012
		private Collider2D _destinationCollider;
	}
}
