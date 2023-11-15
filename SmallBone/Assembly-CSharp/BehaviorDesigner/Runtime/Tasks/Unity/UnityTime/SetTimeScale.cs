using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTime
{
	// Token: 0x02001526 RID: 5414
	[TaskCategory("Unity/Time")]
	[TaskDescription("Sets the scale at which time is passing.")]
	public class SetTimeScale : Action
	{
		// Token: 0x060068CD RID: 26829 RVA: 0x0012E660 File Offset: 0x0012C860
		public override TaskStatus OnUpdate()
		{
			Time.timeScale = this.timeScale.Value;
			return TaskStatus.Success;
		}

		// Token: 0x060068CE RID: 26830 RVA: 0x0012E673 File Offset: 0x0012C873
		public override void OnReset()
		{
			this.timeScale.Value = 0f;
		}

		// Token: 0x040054BE RID: 21694
		[Tooltip("The timescale")]
		public SharedFloat timeScale;
	}
}
