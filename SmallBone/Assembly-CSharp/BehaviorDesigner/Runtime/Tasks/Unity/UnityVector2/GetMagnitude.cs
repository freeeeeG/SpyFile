using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
	// Token: 0x020014F4 RID: 5364
	[TaskCategory("Unity/Vector2")]
	[TaskDescription("Stores the magnitude of the Vector2.")]
	public class GetMagnitude : Action
	{
		// Token: 0x0600681B RID: 26651 RVA: 0x0012CDCC File Offset: 0x0012AFCC
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.vector2Variable.Value.magnitude;
			return TaskStatus.Success;
		}

		// Token: 0x0600681C RID: 26652 RVA: 0x0012CDF8 File Offset: 0x0012AFF8
		public override void OnReset()
		{
			this.vector2Variable = Vector2.zero;
			this.storeResult = 0f;
		}

		// Token: 0x0400540B RID: 21515
		[Tooltip("The Vector2 to get the magnitude of")]
		public SharedVector2 vector2Variable;

		// Token: 0x0400540C RID: 21516
		[Tooltip("The magnitude of the vector")]
		[RequiredField]
		public SharedFloat storeResult;
	}
}
