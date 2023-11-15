using System;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001492 RID: 5266
	[TaskDescription("Similar to the selector task, the priority selector task will return success as soon as a child task returns success. Instead of running the tasks sequentially from left to right within the tree, the priority selector will ask the task what its priority is to determine the order. The higher priority tasks have a higher chance at being run first.")]
	[TaskIcon("{SkinColor}PrioritySelectorIcon.png")]
	public class PrioritySelector : Composite
	{
		// Token: 0x060066AA RID: 26282 RVA: 0x00129030 File Offset: 0x00127230
		public override void OnStart()
		{
			this.childrenExecutionOrder.Clear();
			for (int i = 0; i < this.children.Count; i++)
			{
				float priority = this.children[i].GetPriority();
				int index = this.childrenExecutionOrder.Count;
				for (int j = 0; j < this.childrenExecutionOrder.Count; j++)
				{
					if (this.children[this.childrenExecutionOrder[j]].GetPriority() < priority)
					{
						index = j;
						break;
					}
				}
				this.childrenExecutionOrder.Insert(index, i);
			}
		}

		// Token: 0x060066AB RID: 26283 RVA: 0x001290C2 File Offset: 0x001272C2
		public override int CurrentChildIndex()
		{
			return this.childrenExecutionOrder[this.currentChildIndex];
		}

		// Token: 0x060066AC RID: 26284 RVA: 0x001290D5 File Offset: 0x001272D5
		public override bool CanExecute()
		{
			return this.currentChildIndex < this.children.Count && this.executionStatus != TaskStatus.Success;
		}

		// Token: 0x060066AD RID: 26285 RVA: 0x001290F8 File Offset: 0x001272F8
		public override void OnChildExecuted(TaskStatus childStatus)
		{
			this.currentChildIndex++;
			this.executionStatus = childStatus;
		}

		// Token: 0x060066AE RID: 26286 RVA: 0x0012910F File Offset: 0x0012730F
		public override void OnConditionalAbort(int childIndex)
		{
			this.currentChildIndex = childIndex;
			this.executionStatus = TaskStatus.Inactive;
		}

		// Token: 0x060066AF RID: 26287 RVA: 0x0012911F File Offset: 0x0012731F
		public override void OnEnd()
		{
			this.executionStatus = TaskStatus.Inactive;
			this.currentChildIndex = 0;
		}

		// Token: 0x040052A2 RID: 21154
		private int currentChildIndex;

		// Token: 0x040052A3 RID: 21155
		private TaskStatus executionStatus;

		// Token: 0x040052A4 RID: 21156
		private List<int> childrenExecutionOrder = new List<int>();
	}
}
