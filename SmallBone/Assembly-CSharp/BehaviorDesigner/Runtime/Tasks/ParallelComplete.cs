using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001490 RID: 5264
	[TaskDescription("ParalleSelector와 유사하지만 성공 또는 실패를 반환하는 즉시 그 상태를 반환함")]
	[TaskIcon("{SkinColor}ParallelCompleteIcon.png")]
	public class ParallelComplete : Composite
	{
		// Token: 0x06006696 RID: 26262 RVA: 0x00128E2C File Offset: 0x0012702C
		public override void OnAwake()
		{
			this.executionStatus = new TaskStatus[this.children.Count];
		}

		// Token: 0x06006697 RID: 26263 RVA: 0x00128E44 File Offset: 0x00127044
		public override void OnChildStarted(int childIndex)
		{
			this.currentChildIndex++;
			this.executionStatus[childIndex] = TaskStatus.Running;
		}

		// Token: 0x06006698 RID: 26264 RVA: 0x000076D4 File Offset: 0x000058D4
		public override bool CanRunParallelChildren()
		{
			return true;
		}

		// Token: 0x06006699 RID: 26265 RVA: 0x00128E5D File Offset: 0x0012705D
		public override int CurrentChildIndex()
		{
			return this.currentChildIndex;
		}

		// Token: 0x0600669A RID: 26266 RVA: 0x00128E65 File Offset: 0x00127065
		public override bool CanExecute()
		{
			return this.currentChildIndex < this.children.Count;
		}

		// Token: 0x0600669B RID: 26267 RVA: 0x00128E7A File Offset: 0x0012707A
		public override void OnChildExecuted(int childIndex, TaskStatus childStatus)
		{
			this.executionStatus[childIndex] = childStatus;
		}

		// Token: 0x0600669C RID: 26268 RVA: 0x00128E88 File Offset: 0x00127088
		public override void OnConditionalAbort(int childIndex)
		{
			this.currentChildIndex = 0;
			for (int i = 0; i < this.executionStatus.Length; i++)
			{
				this.executionStatus[i] = TaskStatus.Inactive;
			}
		}

		// Token: 0x0600669D RID: 26269 RVA: 0x00128EB8 File Offset: 0x001270B8
		public override TaskStatus OverrideStatus(TaskStatus status)
		{
			if (this.currentChildIndex == 0)
			{
				return TaskStatus.Success;
			}
			for (int i = 0; i < this.currentChildIndex; i++)
			{
				if (this.executionStatus[i] == TaskStatus.Success || this.executionStatus[i] == TaskStatus.Failure)
				{
					return this.executionStatus[i];
				}
			}
			return TaskStatus.Running;
		}

		// Token: 0x0600669E RID: 26270 RVA: 0x00128F00 File Offset: 0x00127100
		public override void OnEnd()
		{
			for (int i = 0; i < this.executionStatus.Length; i++)
			{
				this.executionStatus[i] = TaskStatus.Inactive;
			}
			this.currentChildIndex = 0;
		}

		// Token: 0x0400529E RID: 21150
		private int currentChildIndex;

		// Token: 0x0400529F RID: 21151
		private TaskStatus[] executionStatus;
	}
}
