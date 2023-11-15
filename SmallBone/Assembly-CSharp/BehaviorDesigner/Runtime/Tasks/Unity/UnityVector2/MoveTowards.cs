using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
	// Token: 0x020014FB RID: 5371
	[TaskDescription("Move from the current position to the target position.")]
	[TaskCategory("Unity/Vector2")]
	public class MoveTowards : Action
	{
		// Token: 0x06006830 RID: 26672 RVA: 0x0012CFE5 File Offset: 0x0012B1E5
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector2.MoveTowards(this.currentPosition.Value, this.targetPosition.Value, this.speed.Value * Time.deltaTime);
			return TaskStatus.Success;
		}

		// Token: 0x06006831 RID: 26673 RVA: 0x0012D020 File Offset: 0x0012B220
		public override void OnReset()
		{
			this.currentPosition = Vector2.zero;
			this.targetPosition = Vector2.zero;
			this.storeResult = Vector2.zero;
			this.speed = 0f;
		}

		// Token: 0x0400541A RID: 21530
		[Tooltip("The current position")]
		public SharedVector2 currentPosition;

		// Token: 0x0400541B RID: 21531
		[Tooltip("The target position")]
		public SharedVector2 targetPosition;

		// Token: 0x0400541C RID: 21532
		[Tooltip("The movement speed")]
		public SharedFloat speed;

		// Token: 0x0400541D RID: 21533
		[RequiredField]
		[Tooltip("The move resut")]
		public SharedVector2 storeResult;
	}
}
