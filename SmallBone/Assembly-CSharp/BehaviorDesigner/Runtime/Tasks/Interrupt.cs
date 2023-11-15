using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014D2 RID: 5330
	[TaskDescription("The interrupt task will stop all child tasks from running if it is interrupted. The interruption can be triggered by the perform interruption task. The interrupt task will keep running its child until this interruption is called. If no interruption happens and the child task completed its execution the interrupt task will return the value assigned by the child task.")]
	[TaskIcon("{SkinColor}InterruptIcon.png")]
	public class Interrupt : Decorator
	{
		// Token: 0x060067A3 RID: 26531 RVA: 0x0012BEC0 File Offset: 0x0012A0C0
		public override bool CanExecute()
		{
			return this.executionStatus == TaskStatus.Inactive || this.executionStatus == TaskStatus.Running;
		}

		// Token: 0x060067A4 RID: 26532 RVA: 0x0012BED5 File Offset: 0x0012A0D5
		public override void OnChildExecuted(TaskStatus childStatus)
		{
			this.executionStatus = childStatus;
		}

		// Token: 0x060067A5 RID: 26533 RVA: 0x0012BEDE File Offset: 0x0012A0DE
		public void DoInterrupt(TaskStatus status)
		{
			this.interruptStatus = status;
			BehaviorManager.instance.Interrupt(base.Owner, this, status);
		}

		// Token: 0x060067A6 RID: 26534 RVA: 0x0012BEF9 File Offset: 0x0012A0F9
		public override TaskStatus OverrideStatus()
		{
			return this.interruptStatus;
		}

		// Token: 0x060067A7 RID: 26535 RVA: 0x0012BF01 File Offset: 0x0012A101
		public override void OnEnd()
		{
			this.interruptStatus = TaskStatus.Failure;
			this.executionStatus = TaskStatus.Inactive;
		}

		// Token: 0x040053A7 RID: 21415
		private TaskStatus interruptStatus = TaskStatus.Failure;

		// Token: 0x040053A8 RID: 21416
		private TaskStatus executionStatus;
	}
}
