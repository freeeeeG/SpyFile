using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTime
{
	// Token: 0x02001525 RID: 5413
	[TaskCategory("Unity/Time")]
	[TaskDescription("Returns the scale at which time is passing.")]
	public class GetTimeScale : Action
	{
		// Token: 0x060068CA RID: 26826 RVA: 0x0012E63B File Offset: 0x0012C83B
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Time.timeScale;
			return TaskStatus.Success;
		}

		// Token: 0x060068CB RID: 26827 RVA: 0x0012E64E File Offset: 0x0012C84E
		public override void OnReset()
		{
			this.storeResult = 0f;
		}

		// Token: 0x040054BD RID: 21693
		[Tooltip("The variable to store the result")]
		public SharedFloat storeResult;
	}
}
