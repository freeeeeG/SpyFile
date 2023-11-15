using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014DA RID: 5338
	[TaskDescription("The until success task will keep executing its child task until the child task returns success.")]
	[TaskIcon("{SkinColor}UntilSuccessIcon.png")]
	public class UntilSuccess : Decorator
	{
		// Token: 0x060067CC RID: 26572 RVA: 0x0012C1A7 File Offset: 0x0012A3A7
		public override bool CanExecute()
		{
			return this.executionStatus == TaskStatus.Failure || this.executionStatus == TaskStatus.Inactive;
		}

		// Token: 0x060067CD RID: 26573 RVA: 0x0012C1BD File Offset: 0x0012A3BD
		public override void OnChildExecuted(TaskStatus childStatus)
		{
			this.executionStatus = childStatus;
		}

		// Token: 0x060067CE RID: 26574 RVA: 0x0012C1C6 File Offset: 0x0012A3C6
		public override void OnEnd()
		{
			this.executionStatus = TaskStatus.Inactive;
		}

		// Token: 0x040053B8 RID: 21432
		private TaskStatus executionStatus;
	}
}
