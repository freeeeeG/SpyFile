using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion
{
	// Token: 0x0200156C RID: 5484
	[TaskCategory("Unity/Quaternion")]
	[TaskDescription("Stores the inverse of the specified quaternion.")]
	public class Inverse : Action
	{
		// Token: 0x060069BC RID: 27068 RVA: 0x0013032B File Offset: 0x0012E52B
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Quaternion.Inverse(this.targetQuaternion.Value);
			return TaskStatus.Success;
		}

		// Token: 0x060069BD RID: 27069 RVA: 0x0013034C File Offset: 0x0012E54C
		public override void OnReset()
		{
			this.targetQuaternion = (this.storeResult = Quaternion.identity);
		}

		// Token: 0x0400557F RID: 21887
		[Tooltip("The target quaternion")]
		public SharedQuaternion targetQuaternion;

		// Token: 0x04005580 RID: 21888
		[RequiredField]
		[Tooltip("The stored quaternion")]
		public SharedQuaternion storeResult;
	}
}
