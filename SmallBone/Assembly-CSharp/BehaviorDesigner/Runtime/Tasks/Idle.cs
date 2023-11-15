using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x0200146A RID: 5226
	[TaskDescription("Returns a TaskStatus of running. Will only stop when interrupted or a conditional abort is triggered.")]
	[TaskIcon("{SkinColor}IdleIcon.png")]
	public class Idle : Action
	{
		// Token: 0x06006603 RID: 26115 RVA: 0x00126CBB File Offset: 0x00124EBB
		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Running;
		}
	}
}
