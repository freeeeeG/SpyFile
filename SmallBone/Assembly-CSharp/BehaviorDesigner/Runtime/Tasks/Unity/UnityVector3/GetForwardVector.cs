using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
	// Token: 0x020014E1 RID: 5345
	[TaskCategory("Unity/Vector3")]
	[TaskDescription("Stores the forward vector value.")]
	public class GetForwardVector : Action
	{
		// Token: 0x060067E5 RID: 26597 RVA: 0x0012C652 File Offset: 0x0012A852
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector3.forward;
			return TaskStatus.Success;
		}

		// Token: 0x060067E6 RID: 26598 RVA: 0x0012C665 File Offset: 0x0012A865
		public override void OnReset()
		{
			this.storeResult = Vector3.zero;
		}

		// Token: 0x040053D4 RID: 21460
		[RequiredField]
		[Tooltip("The stored result")]
		public SharedVector3 storeResult;
	}
}
