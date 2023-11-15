using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTime
{
	// Token: 0x02001522 RID: 5410
	[TaskDescription("Returns the time in seconds it took to complete the last frame.")]
	[TaskCategory("Unity/Time")]
	public class GetDeltaTime : Action
	{
		// Token: 0x060068C1 RID: 26817 RVA: 0x0012E5CC File Offset: 0x0012C7CC
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Time.deltaTime;
			return TaskStatus.Success;
		}

		// Token: 0x060068C2 RID: 26818 RVA: 0x0012E5DF File Offset: 0x0012C7DF
		public override void OnReset()
		{
			this.storeResult = 0f;
		}

		// Token: 0x040054BA RID: 21690
		[Tooltip("The variable to store the result")]
		public SharedFloat storeResult;
	}
}
