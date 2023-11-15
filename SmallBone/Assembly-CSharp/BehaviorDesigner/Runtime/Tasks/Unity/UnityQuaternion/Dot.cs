using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion
{
	// Token: 0x02001568 RID: 5480
	[TaskDescription("Stores the dot product between two rotations.")]
	[TaskCategory("Unity/Quaternion")]
	public class Dot : Action
	{
		// Token: 0x060069B0 RID: 27056 RVA: 0x00130205 File Offset: 0x0012E405
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Quaternion.Dot(this.leftRotation.Value, this.rightRotation.Value);
			return TaskStatus.Success;
		}

		// Token: 0x060069B1 RID: 27057 RVA: 0x00130230 File Offset: 0x0012E430
		public override void OnReset()
		{
			this.leftRotation = (this.rightRotation = Quaternion.identity);
			this.storeResult = 0f;
		}

		// Token: 0x04005576 RID: 21878
		[Tooltip("The first rotation")]
		public SharedQuaternion leftRotation;

		// Token: 0x04005577 RID: 21879
		[Tooltip("The second rotation")]
		public SharedQuaternion rightRotation;

		// Token: 0x04005578 RID: 21880
		[RequiredField]
		[Tooltip("The stored result")]
		public SharedFloat storeResult;
	}
}
