using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
	// Token: 0x020014EE RID: 5358
	[TaskCategory("Unity/Vector3")]
	[TaskDescription("Rotate the current rotation to the target rotation.")]
	public class RotateTowards : Action
	{
		// Token: 0x06006809 RID: 26633 RVA: 0x0012CAC0 File Offset: 0x0012ACC0
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector3.RotateTowards(this.currentRotation.Value, this.targetRotation.Value, this.maxDegreesDelta.Value * 0.017453292f * Time.deltaTime, this.maxMagnitudeDelta.Value);
			return TaskStatus.Success;
		}

		// Token: 0x0600680A RID: 26634 RVA: 0x0012CB18 File Offset: 0x0012AD18
		public override void OnReset()
		{
			this.currentRotation = Vector3.zero;
			this.targetRotation = Vector3.zero;
			this.storeResult = Vector3.zero;
			this.maxDegreesDelta = 0f;
			this.maxMagnitudeDelta = 0f;
		}

		// Token: 0x040053F6 RID: 21494
		[Tooltip("The current rotation in euler angles")]
		public SharedVector3 currentRotation;

		// Token: 0x040053F7 RID: 21495
		[Tooltip("The target rotation in euler angles")]
		public SharedVector3 targetRotation;

		// Token: 0x040053F8 RID: 21496
		[Tooltip("The maximum delta of the degrees")]
		public SharedFloat maxDegreesDelta;

		// Token: 0x040053F9 RID: 21497
		[Tooltip("The maximum delta of the magnitude")]
		public SharedFloat maxMagnitudeDelta;

		// Token: 0x040053FA RID: 21498
		[Tooltip("The rotation resut")]
		[RequiredField]
		public SharedVector3 storeResult;
	}
}
