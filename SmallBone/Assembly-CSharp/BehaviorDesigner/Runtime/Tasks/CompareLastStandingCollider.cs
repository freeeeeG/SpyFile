using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014AB RID: 5291
	[TaskDescription("Owner와 Target의 lastStandingCollider를 비교합니다.lastStandingCollider이 둘중 하나라도 Null일 경우는 Failure를 반환합니다.")]
	public class CompareLastStandingCollider : Conditional
	{
		// Token: 0x06006718 RID: 26392 RVA: 0x0012A4E0 File Offset: 0x001286E0
		public override TaskStatus OnUpdate()
		{
			if (this._target == null || this._owner == null)
			{
				return TaskStatus.Failure;
			}
			Character value = this._owner.Value;
			Character value2 = this._target.Value;
			Collider2D lastStandingCollider = value.movement.controller.collisionState.lastStandingCollider;
			Collider2D lastStandingCollider2 = value2.movement.controller.collisionState.lastStandingCollider;
			if (lastStandingCollider2 == null || lastStandingCollider == null)
			{
				return TaskStatus.Failure;
			}
			CompareLastStandingCollider.Operation operation = this.operation;
			if (operation != CompareLastStandingCollider.Operation.Equal)
			{
				if (operation != CompareLastStandingCollider.Operation.NotEqual)
				{
					return TaskStatus.Failure;
				}
				if (!(lastStandingCollider != lastStandingCollider2))
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			}
			else
			{
				if (!(lastStandingCollider == lastStandingCollider2))
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			}
		}

		// Token: 0x04005310 RID: 21264
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x04005311 RID: 21265
		[SerializeField]
		private SharedCharacter _target;

		// Token: 0x04005312 RID: 21266
		public CompareLastStandingCollider.Operation operation;

		// Token: 0x020014AC RID: 5292
		public enum Operation
		{
			// Token: 0x04005314 RID: 21268
			Equal,
			// Token: 0x04005315 RID: 21269
			NotEqual
		}
	}
}
