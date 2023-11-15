using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001495 RID: 5269
	[TaskDescription("The selector task is similar to an \"or\" operation. It will return success as soon as one of its child tasks return success. If a child task returns failure then it will sequentially run the next task. If no child task returns success then it will return failure.")]
	[TaskIcon("{SkinColor}SelectorIcon.png")]
	public class Selector : Composite
	{
		// Token: 0x060066C5 RID: 26309 RVA: 0x00129423 File Offset: 0x00127623
		public override int CurrentChildIndex()
		{
			return this.currentChildIndex;
		}

		// Token: 0x060066C6 RID: 26310 RVA: 0x0012942B File Offset: 0x0012762B
		public override bool CanExecute()
		{
			return this.currentChildIndex < this.children.Count && this.executionStatus != TaskStatus.Success;
		}

		// Token: 0x060066C7 RID: 26311 RVA: 0x0012944E File Offset: 0x0012764E
		public override void OnChildExecuted(TaskStatus childStatus)
		{
			this.currentChildIndex++;
			this.executionStatus = childStatus;
		}

		// Token: 0x060066C8 RID: 26312 RVA: 0x00129465 File Offset: 0x00127665
		public override void OnConditionalAbort(int childIndex)
		{
			this.currentChildIndex = childIndex;
			this.executionStatus = TaskStatus.Inactive;
		}

		// Token: 0x060066C9 RID: 26313 RVA: 0x00129475 File Offset: 0x00127675
		public override void OnEnd()
		{
			this.executionStatus = TaskStatus.Inactive;
			this.currentChildIndex = 0;
		}

		// Token: 0x040052AF RID: 21167
		private int currentChildIndex;

		// Token: 0x040052B0 RID: 21168
		private TaskStatus executionStatus;
	}
}
