using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion
{
	// Token: 0x0200156E RID: 5486
	[TaskCategory("Unity/Quaternion")]
	[TaskDescription("Stores the quaternion of a forward vector.")]
	public class LookRotation : Action
	{
		// Token: 0x060069C2 RID: 27074 RVA: 0x001303E7 File Offset: 0x0012E5E7
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Quaternion.LookRotation(this.forwardVector.Value);
			return TaskStatus.Success;
		}

		// Token: 0x060069C3 RID: 27075 RVA: 0x00130405 File Offset: 0x0012E605
		public override void OnReset()
		{
			this.forwardVector = Vector3.zero;
			this.storeResult = Quaternion.identity;
		}

		// Token: 0x04005585 RID: 21893
		[Tooltip("The forward vector")]
		public SharedVector3 forwardVector;

		// Token: 0x04005586 RID: 21894
		[Tooltip("The second Vector3")]
		public SharedVector3 secondVector3;

		// Token: 0x04005587 RID: 21895
		[Tooltip("The stored quaternion")]
		[RequiredField]
		public SharedQuaternion storeResult;
	}
}
