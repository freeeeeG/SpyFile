using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
	// Token: 0x020014EA RID: 5354
	[TaskDescription("Multiply the Vector3 by a float.")]
	[TaskCategory("Unity/Vector3")]
	public class Multiply : Action
	{
		// Token: 0x06006800 RID: 26624 RVA: 0x0012C94D File Offset: 0x0012AB4D
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.vector3Variable.Value * this.multiplyBy.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006801 RID: 26625 RVA: 0x0012C976 File Offset: 0x0012AB76
		public override void OnReset()
		{
			this.vector3Variable = Vector3.zero;
			this.storeResult = Vector3.zero;
			this.multiplyBy = 0f;
		}

		// Token: 0x040053E9 RID: 21481
		[Tooltip("The Vector3 to multiply of")]
		public SharedVector3 vector3Variable;

		// Token: 0x040053EA RID: 21482
		[Tooltip("The value to multiply the Vector3 of")]
		public SharedFloat multiplyBy;

		// Token: 0x040053EB RID: 21483
		[Tooltip("The multiplication resut")]
		[RequiredField]
		public SharedVector3 storeResult;
	}
}
