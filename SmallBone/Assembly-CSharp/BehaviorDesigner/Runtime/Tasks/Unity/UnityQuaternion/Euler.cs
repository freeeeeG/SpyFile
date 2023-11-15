using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion
{
	// Token: 0x02001569 RID: 5481
	[TaskDescription("Stores the quaternion of a euler vector.")]
	[TaskCategory("Unity/Quaternion")]
	public class Euler : Action
	{
		// Token: 0x060069B3 RID: 27059 RVA: 0x00130266 File Offset: 0x0012E466
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Quaternion.Euler(this.eulerVector.Value);
			return TaskStatus.Success;
		}

		// Token: 0x060069B4 RID: 27060 RVA: 0x00130284 File Offset: 0x0012E484
		public override void OnReset()
		{
			this.eulerVector = Vector3.zero;
			this.storeResult = Quaternion.identity;
		}

		// Token: 0x04005579 RID: 21881
		[Tooltip("The euler vector")]
		public SharedVector3 eulerVector;

		// Token: 0x0400557A RID: 21882
		[Tooltip("The stored quaternion")]
		[RequiredField]
		public SharedQuaternion storeResult;
	}
}
