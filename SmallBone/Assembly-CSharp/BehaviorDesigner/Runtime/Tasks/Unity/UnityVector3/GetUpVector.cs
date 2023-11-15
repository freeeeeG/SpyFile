using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
	// Token: 0x020014E5 RID: 5349
	[TaskCategory("Unity/Vector3")]
	[TaskDescription("Stores the up vector value.")]
	public class GetUpVector : Action
	{
		// Token: 0x060067F1 RID: 26609 RVA: 0x0012C73A File Offset: 0x0012A93A
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector3.up;
			return TaskStatus.Success;
		}

		// Token: 0x060067F2 RID: 26610 RVA: 0x0012C74D File Offset: 0x0012A94D
		public override void OnReset()
		{
			this.storeResult = Vector3.zero;
		}

		// Token: 0x040053DA RID: 21466
		[RequiredField]
		[Tooltip("The stored result")]
		public SharedVector3 storeResult;
	}
}
