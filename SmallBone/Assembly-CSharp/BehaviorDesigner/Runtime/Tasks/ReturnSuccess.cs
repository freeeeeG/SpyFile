using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014D6 RID: 5334
	[TaskDescription("The return success task will always return success except when the child task is running.")]
	[TaskIcon("{SkinColor}ReturnSuccessIcon.png")]
	public class ReturnSuccess : Decorator
	{
		// Token: 0x060067B8 RID: 26552 RVA: 0x0012C03E File Offset: 0x0012A23E
		public override bool CanExecute()
		{
			return this.executionStatus == TaskStatus.Inactive || this.executionStatus == TaskStatus.Running;
		}

		// Token: 0x060067B9 RID: 26553 RVA: 0x0012C053 File Offset: 0x0012A253
		public override void OnChildExecuted(TaskStatus childStatus)
		{
			this.executionStatus = childStatus;
		}

		// Token: 0x060067BA RID: 26554 RVA: 0x0012C05C File Offset: 0x0012A25C
		public override TaskStatus Decorate(TaskStatus status)
		{
			if (status == TaskStatus.Failure)
			{
				return TaskStatus.Success;
			}
			return status;
		}

		// Token: 0x060067BB RID: 26555 RVA: 0x0012C065 File Offset: 0x0012A265
		public override void OnEnd()
		{
			this.executionStatus = TaskStatus.Inactive;
		}

		// Token: 0x040053B0 RID: 21424
		private TaskStatus executionStatus;
	}
}
