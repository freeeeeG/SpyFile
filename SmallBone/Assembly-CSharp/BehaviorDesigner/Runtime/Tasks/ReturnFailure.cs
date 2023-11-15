using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014D5 RID: 5333
	[TaskIcon("{SkinColor}ReturnFailureIcon.png")]
	[TaskDescription("The return failure task will always return failure except when the child task is running.")]
	public class ReturnFailure : Decorator
	{
		// Token: 0x060067B3 RID: 26547 RVA: 0x0012C00E File Offset: 0x0012A20E
		public override bool CanExecute()
		{
			return this.executionStatus == TaskStatus.Inactive || this.executionStatus == TaskStatus.Running;
		}

		// Token: 0x060067B4 RID: 26548 RVA: 0x0012C023 File Offset: 0x0012A223
		public override void OnChildExecuted(TaskStatus childStatus)
		{
			this.executionStatus = childStatus;
		}

		// Token: 0x060067B5 RID: 26549 RVA: 0x0012C02C File Offset: 0x0012A22C
		public override TaskStatus Decorate(TaskStatus status)
		{
			if (status == TaskStatus.Success)
			{
				return TaskStatus.Failure;
			}
			return status;
		}

		// Token: 0x060067B6 RID: 26550 RVA: 0x0012C035 File Offset: 0x0012A235
		public override void OnEnd()
		{
			this.executionStatus = TaskStatus.Inactive;
		}

		// Token: 0x040053AF RID: 21423
		private TaskStatus executionStatus;
	}
}
