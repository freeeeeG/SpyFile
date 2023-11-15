using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
	// Token: 0x020014EB RID: 5355
	[TaskDescription("Normalize the Vector3.")]
	[TaskCategory("Unity/Vector3")]
	public class Normalize : Action
	{
		// Token: 0x06006803 RID: 26627 RVA: 0x0012C9A8 File Offset: 0x0012ABA8
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector3.Normalize(this.vector3Variable.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006804 RID: 26628 RVA: 0x0012C9C6 File Offset: 0x0012ABC6
		public override void OnReset()
		{
			this.vector3Variable = Vector3.zero;
			this.storeResult = Vector3.zero;
		}

		// Token: 0x040053EC RID: 21484
		[Tooltip("The Vector3 to normalize")]
		public SharedVector3 vector3Variable;

		// Token: 0x040053ED RID: 21485
		[Tooltip("The normalized resut")]
		[RequiredField]
		public SharedVector3 storeResult;
	}
}
