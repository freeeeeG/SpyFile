using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
	// Token: 0x020014E3 RID: 5347
	[TaskDescription("Stores the right vector value.")]
	[TaskCategory("Unity/Vector3")]
	public class GetRightVector : Action
	{
		// Token: 0x060067EB RID: 26603 RVA: 0x0012C6C6 File Offset: 0x0012A8C6
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector3.right;
			return TaskStatus.Success;
		}

		// Token: 0x060067EC RID: 26604 RVA: 0x0012C6D9 File Offset: 0x0012A8D9
		public override void OnReset()
		{
			this.storeResult = Vector3.zero;
		}

		// Token: 0x040053D7 RID: 21463
		[RequiredField]
		[Tooltip("The stored result")]
		public SharedVector3 storeResult;
	}
}
