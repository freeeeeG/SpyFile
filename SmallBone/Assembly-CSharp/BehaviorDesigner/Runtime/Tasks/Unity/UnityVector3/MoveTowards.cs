using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
	// Token: 0x020014E9 RID: 5353
	[TaskDescription("Move from the current position to the target position.")]
	[TaskCategory("Unity/Vector3")]
	public class MoveTowards : Action
	{
		// Token: 0x060067FD RID: 26621 RVA: 0x0012C8C3 File Offset: 0x0012AAC3
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector3.MoveTowards(this.currentPosition.Value, this.targetPosition.Value, this.speed.Value * Time.deltaTime);
			return TaskStatus.Success;
		}

		// Token: 0x060067FE RID: 26622 RVA: 0x0012C900 File Offset: 0x0012AB00
		public override void OnReset()
		{
			this.currentPosition = Vector3.zero;
			this.targetPosition = Vector3.zero;
			this.storeResult = Vector3.zero;
			this.speed = 0f;
		}

		// Token: 0x040053E5 RID: 21477
		[Tooltip("The current position")]
		public SharedVector3 currentPosition;

		// Token: 0x040053E6 RID: 21478
		[Tooltip("The target position")]
		public SharedVector3 targetPosition;

		// Token: 0x040053E7 RID: 21479
		[Tooltip("The movement speed")]
		public SharedFloat speed;

		// Token: 0x040053E8 RID: 21480
		[Tooltip("The move resut")]
		[RequiredField]
		public SharedVector3 storeResult;
	}
}
