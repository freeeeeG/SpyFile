using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
	// Token: 0x020014E2 RID: 5346
	[TaskCategory("Unity/Vector3")]
	[TaskDescription("Stores the magnitude of the Vector3.")]
	public class GetMagnitude : Action
	{
		// Token: 0x060067E8 RID: 26600 RVA: 0x0012C678 File Offset: 0x0012A878
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.vector3Variable.Value.magnitude;
			return TaskStatus.Success;
		}

		// Token: 0x060067E9 RID: 26601 RVA: 0x0012C6A4 File Offset: 0x0012A8A4
		public override void OnReset()
		{
			this.vector3Variable = Vector3.zero;
			this.storeResult = 0f;
		}

		// Token: 0x040053D5 RID: 21461
		[Tooltip("The Vector3 to get the magnitude of")]
		public SharedVector3 vector3Variable;

		// Token: 0x040053D6 RID: 21462
		[Tooltip("The magnitude of the vector")]
		[RequiredField]
		public SharedFloat storeResult;
	}
}
