using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001491 RID: 5265
	[TaskDescription("하나라도 성공을 반환하면 종료 후 성공을 반환하고, 모두 실패하면 실패를 반환함")]
	[TaskIcon("{SkinColor}ParallelSelectorIcon.png")]
	public class ParallelSelector : Composite
	{
		// Token: 0x060066A0 RID: 26272 RVA: 0x00128F30 File Offset: 0x00127130
		public override void OnAwake()
		{
			this.executionStatus = new TaskStatus[this.children.Count];
		}

		// Token: 0x060066A1 RID: 26273 RVA: 0x00128F48 File Offset: 0x00127148
		public override void OnChildStarted(int childIndex)
		{
			this.currentChildIndex++;
			this.executionStatus[childIndex] = TaskStatus.Running;
		}

		// Token: 0x060066A2 RID: 26274 RVA: 0x000076D4 File Offset: 0x000058D4
		public override bool CanRunParallelChildren()
		{
			return true;
		}

		// Token: 0x060066A3 RID: 26275 RVA: 0x00128F61 File Offset: 0x00127161
		public override int CurrentChildIndex()
		{
			return this.currentChildIndex;
		}

		// Token: 0x060066A4 RID: 26276 RVA: 0x00128F69 File Offset: 0x00127169
		public override bool CanExecute()
		{
			return this.currentChildIndex < this.children.Count;
		}

		// Token: 0x060066A5 RID: 26277 RVA: 0x00128F7E File Offset: 0x0012717E
		public override void OnChildExecuted(int childIndex, TaskStatus childStatus)
		{
			this.executionStatus[childIndex] = childStatus;
		}

		// Token: 0x060066A6 RID: 26278 RVA: 0x00128F8C File Offset: 0x0012718C
		public override void OnConditionalAbort(int childIndex)
		{
			this.currentChildIndex = 0;
			for (int i = 0; i < this.executionStatus.Length; i++)
			{
				this.executionStatus[i] = TaskStatus.Inactive;
			}
		}

		// Token: 0x060066A7 RID: 26279 RVA: 0x00128FBC File Offset: 0x001271BC
		public override TaskStatus OverrideStatus(TaskStatus status)
		{
			bool flag = true;
			for (int i = 0; i < this.executionStatus.Length; i++)
			{
				if (this.executionStatus[i] == TaskStatus.Running)
				{
					flag = false;
				}
				else if (this.executionStatus[i] == TaskStatus.Success)
				{
					return TaskStatus.Success;
				}
			}
			if (!flag)
			{
				return TaskStatus.Running;
			}
			return TaskStatus.Failure;
		}

		// Token: 0x060066A8 RID: 26280 RVA: 0x00129000 File Offset: 0x00127200
		public override void OnEnd()
		{
			for (int i = 0; i < this.executionStatus.Length; i++)
			{
				this.executionStatus[i] = TaskStatus.Inactive;
			}
			this.currentChildIndex = 0;
		}

		// Token: 0x040052A0 RID: 21152
		private int currentChildIndex;

		// Token: 0x040052A1 RID: 21153
		private TaskStatus[] executionStatus;
	}
}
