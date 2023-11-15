using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001497 RID: 5271
	[TaskIcon("{SkinColor}SequenceIcon.png")]
	[TaskDescription("The sequence task is similar to an \"and\" operation. It will return failure as soon as one of its child tasks return failure. If a child task returns success then it will sequentially run the next task. If all child tasks return success then it will return success.")]
	public class Sequence : Composite
	{
		// Token: 0x060066D7 RID: 26327 RVA: 0x001295F6 File Offset: 0x001277F6
		public override int CurrentChildIndex()
		{
			return this.currentChildIndex;
		}

		// Token: 0x060066D8 RID: 26328 RVA: 0x001295FE File Offset: 0x001277FE
		public override bool CanExecute()
		{
			return this.currentChildIndex < this.children.Count && this.executionStatus != TaskStatus.Failure;
		}

		// Token: 0x060066D9 RID: 26329 RVA: 0x00129621 File Offset: 0x00127821
		public override void OnChildExecuted(TaskStatus childStatus)
		{
			this.currentChildIndex++;
			this.executionStatus = childStatus;
		}

		// Token: 0x060066DA RID: 26330 RVA: 0x00129638 File Offset: 0x00127838
		public override void OnConditionalAbort(int childIndex)
		{
			this.currentChildIndex = childIndex;
			this.executionStatus = TaskStatus.Inactive;
		}

		// Token: 0x060066DB RID: 26331 RVA: 0x00129648 File Offset: 0x00127848
		public override void OnEnd()
		{
			this.executionStatus = TaskStatus.Inactive;
			this.currentChildIndex = 0;
		}

		// Token: 0x040052B5 RID: 21173
		private int currentChildIndex;

		// Token: 0x040052B6 RID: 21174
		private TaskStatus executionStatus;
	}
}
