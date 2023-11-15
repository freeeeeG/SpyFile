using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014D9 RID: 5337
	[TaskDescription("The until failure task will keep executing its child task until the child task returns failure.")]
	[TaskIcon("{SkinColor}UntilFailureIcon.png")]
	public class UntilFailure : Decorator
	{
		// Token: 0x060067C8 RID: 26568 RVA: 0x0012C17F File Offset: 0x0012A37F
		public override bool CanExecute()
		{
			return this.executionStatus == TaskStatus.Success || this.executionStatus == TaskStatus.Inactive;
		}

		// Token: 0x060067C9 RID: 26569 RVA: 0x0012C195 File Offset: 0x0012A395
		public override void OnChildExecuted(TaskStatus childStatus)
		{
			this.executionStatus = childStatus;
		}

		// Token: 0x060067CA RID: 26570 RVA: 0x0012C19E File Offset: 0x0012A39E
		public override void OnEnd()
		{
			this.executionStatus = TaskStatus.Inactive;
		}

		// Token: 0x040053B7 RID: 21431
		private TaskStatus executionStatus;
	}
}
