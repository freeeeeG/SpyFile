using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
	// Token: 0x020014DF RID: 5343
	[TaskDescription("Returns the distance between two Vector3s.")]
	[TaskCategory("Unity/Vector3")]
	public class Distance : Action
	{
		// Token: 0x060067DF RID: 26591 RVA: 0x0012C59C File Offset: 0x0012A79C
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector3.Distance(this.firstVector3.Value, this.secondVector3.Value);
			return TaskStatus.Success;
		}

		// Token: 0x060067E0 RID: 26592 RVA: 0x0012C5C5 File Offset: 0x0012A7C5
		public override void OnReset()
		{
			this.firstVector3 = Vector3.zero;
			this.secondVector3 = Vector3.zero;
			this.storeResult = 0f;
		}

		// Token: 0x040053CE RID: 21454
		[Tooltip("The first Vector3")]
		public SharedVector3 firstVector3;

		// Token: 0x040053CF RID: 21455
		[Tooltip("The second Vector3")]
		public SharedVector3 secondVector3;

		// Token: 0x040053D0 RID: 21456
		[RequiredField]
		[Tooltip("The distance")]
		public SharedFloat storeResult;
	}
}
