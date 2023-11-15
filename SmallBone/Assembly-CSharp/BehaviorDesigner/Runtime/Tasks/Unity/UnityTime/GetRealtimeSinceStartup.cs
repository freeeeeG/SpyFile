using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTime
{
	// Token: 0x02001523 RID: 5411
	[TaskCategory("Unity/Time")]
	[TaskDescription("Returns the real time in seconds since the game started.")]
	public class GetRealtimeSinceStartup : Action
	{
		// Token: 0x060068C4 RID: 26820 RVA: 0x0012E5F1 File Offset: 0x0012C7F1
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Time.realtimeSinceStartup;
			return TaskStatus.Success;
		}

		// Token: 0x060068C5 RID: 26821 RVA: 0x0012E604 File Offset: 0x0012C804
		public override void OnReset()
		{
			this.storeResult = 0f;
		}

		// Token: 0x040054BB RID: 21691
		[Tooltip("The variable to store the result")]
		public SharedFloat storeResult;
	}
}
