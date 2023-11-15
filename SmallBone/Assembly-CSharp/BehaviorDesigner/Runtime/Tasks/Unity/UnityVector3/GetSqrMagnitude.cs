using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
	// Token: 0x020014E4 RID: 5348
	[TaskCategory("Unity/Vector3")]
	[TaskDescription("Stores the square magnitude of the Vector3.")]
	public class GetSqrMagnitude : Action
	{
		// Token: 0x060067EE RID: 26606 RVA: 0x0012C6EC File Offset: 0x0012A8EC
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.vector3Variable.Value.sqrMagnitude;
			return TaskStatus.Success;
		}

		// Token: 0x060067EF RID: 26607 RVA: 0x0012C718 File Offset: 0x0012A918
		public override void OnReset()
		{
			this.vector3Variable = Vector3.zero;
			this.storeResult = 0f;
		}

		// Token: 0x040053D8 RID: 21464
		[Tooltip("The Vector3 to get the square magnitude of")]
		public SharedVector3 vector3Variable;

		// Token: 0x040053D9 RID: 21465
		[Tooltip("The square magnitude of the vector")]
		[RequiredField]
		public SharedFloat storeResult;
	}
}
