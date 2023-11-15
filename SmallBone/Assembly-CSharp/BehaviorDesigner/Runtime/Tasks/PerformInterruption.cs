using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001472 RID: 5234
	[TaskDescription("Perform the actual interruption. This will immediately stop the specified tasks from running and will return success or failure depending on the value of interrupt success.")]
	[TaskIcon("{SkinColor}PerformInterruptionIcon.png")]
	public class PerformInterruption : Action
	{
		// Token: 0x06006618 RID: 26136 RVA: 0x001271F0 File Offset: 0x001253F0
		public override TaskStatus OnUpdate()
		{
			for (int i = 0; i < this.interruptTasks.Length; i++)
			{
				this.interruptTasks[i].DoInterrupt(this.interruptSuccess.Value ? TaskStatus.Success : TaskStatus.Failure);
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006619 RID: 26137 RVA: 0x0012722F File Offset: 0x0012542F
		public override void OnReset()
		{
			this.interruptTasks = null;
			this.interruptSuccess = false;
		}

		// Token: 0x04005215 RID: 21013
		[Tooltip("The list of tasks to interrupt. Can be any number of tasks")]
		public Interrupt[] interruptTasks;

		// Token: 0x04005216 RID: 21014
		[Tooltip("When we interrupt the task should we return a task status of success?")]
		public SharedBool interruptSuccess;
	}
}
