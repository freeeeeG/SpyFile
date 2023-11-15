using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTime
{
	// Token: 0x02001524 RID: 5412
	[TaskDescription("Returns the time in second since the start of the game.")]
	[TaskCategory("Unity/Time")]
	public class GetTime : Action
	{
		// Token: 0x060068C7 RID: 26823 RVA: 0x0012E616 File Offset: 0x0012C816
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Time.time;
			return TaskStatus.Success;
		}

		// Token: 0x060068C8 RID: 26824 RVA: 0x0012E629 File Offset: 0x0012C829
		public override void OnReset()
		{
			this.storeResult = 0f;
		}

		// Token: 0x040054BC RID: 21692
		[Tooltip("The variable to store the result")]
		public SharedFloat storeResult;
	}
}
