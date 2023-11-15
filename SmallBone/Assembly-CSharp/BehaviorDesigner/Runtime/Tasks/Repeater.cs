using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014D4 RID: 5332
	[TaskDescription("The repeater task will repeat execution of its child task until the child task has been run a specified number of times. It has the option of continuing to execute the child task even if the child task returns a failure.")]
	[TaskIcon("{SkinColor}RepeaterIcon.png")]
	public class Repeater : Decorator
	{
		// Token: 0x060067AE RID: 26542 RVA: 0x0012BF60 File Offset: 0x0012A160
		public override bool CanExecute()
		{
			return (this.repeatForever.Value || this.executionCount < this.count.Value) && (!this.endOnFailure.Value || (this.endOnFailure.Value && this.executionStatus != TaskStatus.Failure));
		}

		// Token: 0x060067AF RID: 26543 RVA: 0x0012BFB9 File Offset: 0x0012A1B9
		public override void OnChildExecuted(TaskStatus childStatus)
		{
			this.executionCount++;
			this.executionStatus = childStatus;
		}

		// Token: 0x060067B0 RID: 26544 RVA: 0x0012BFD0 File Offset: 0x0012A1D0
		public override void OnEnd()
		{
			this.executionCount = 0;
			this.executionStatus = TaskStatus.Inactive;
		}

		// Token: 0x060067B1 RID: 26545 RVA: 0x0012BFE0 File Offset: 0x0012A1E0
		public override void OnReset()
		{
			this.count = 0;
			this.endOnFailure = true;
		}

		// Token: 0x040053AA RID: 21418
		[Tooltip("The number of times to repeat the execution of its child task")]
		public SharedInt count = 1;

		// Token: 0x040053AB RID: 21419
		[Tooltip("Allows the repeater to repeat forever")]
		public SharedBool repeatForever;

		// Token: 0x040053AC RID: 21420
		[Tooltip("Should the task return if the child task returns a failure")]
		public SharedBool endOnFailure;

		// Token: 0x040053AD RID: 21421
		private int executionCount;

		// Token: 0x040053AE RID: 21422
		private TaskStatus executionStatus;
	}
}
