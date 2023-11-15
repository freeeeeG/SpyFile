using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014BE RID: 5310
	[TaskDescription("타겟이 Grounded인지 판별합니다.")]
	public class IsTargetGrounded : Conditional
	{
		// Token: 0x06006746 RID: 26438 RVA: 0x0012AFB4 File Offset: 0x001291B4
		public override TaskStatus OnUpdate()
		{
			Character value = this._target.Value;
			if (value == null)
			{
				return TaskStatus.Failure;
			}
			if (!value.movement.isGrounded)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x0400535E RID: 21342
		[SerializeField]
		private SharedCharacter _target;
	}
}
