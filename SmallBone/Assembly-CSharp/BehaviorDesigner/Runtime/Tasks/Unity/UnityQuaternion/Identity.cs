using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion
{
	// Token: 0x0200156B RID: 5483
	[TaskCategory("Unity/Quaternion")]
	[TaskDescription("Stores the quaternion identity.")]
	public class Identity : Action
	{
		// Token: 0x060069B9 RID: 27065 RVA: 0x00130306 File Offset: 0x0012E506
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Quaternion.identity;
			return TaskStatus.Success;
		}

		// Token: 0x060069BA RID: 27066 RVA: 0x00130319 File Offset: 0x0012E519
		public override void OnReset()
		{
			this.storeResult = Quaternion.identity;
		}

		// Token: 0x0400557E RID: 21886
		[RequiredField]
		[Tooltip("The identity")]
		public SharedQuaternion storeResult;
	}
}
