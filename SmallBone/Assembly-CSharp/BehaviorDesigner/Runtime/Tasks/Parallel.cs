using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x0200148F RID: 5263
	[TaskDescription("하부 테스크가 실패를 반환하면 모든 테스크를 종료하고 실패를 반환함")]
	[TaskIcon("{SkinColor}ParallelIcon.png")]
	public class Parallel : Composite
	{
		// Token: 0x0600668C RID: 26252 RVA: 0x00128D2F File Offset: 0x00126F2F
		public override void OnAwake()
		{
			this.executionStatus = new TaskStatus[this.children.Count];
		}

		// Token: 0x0600668D RID: 26253 RVA: 0x00128D47 File Offset: 0x00126F47
		public override void OnChildStarted(int childIndex)
		{
			this.currentChildIndex++;
			this.executionStatus[childIndex] = TaskStatus.Running;
		}

		// Token: 0x0600668E RID: 26254 RVA: 0x000076D4 File Offset: 0x000058D4
		public override bool CanRunParallelChildren()
		{
			return true;
		}

		// Token: 0x0600668F RID: 26255 RVA: 0x00128D60 File Offset: 0x00126F60
		public override int CurrentChildIndex()
		{
			return this.currentChildIndex;
		}

		// Token: 0x06006690 RID: 26256 RVA: 0x00128D68 File Offset: 0x00126F68
		public override bool CanExecute()
		{
			return this.currentChildIndex < this.children.Count;
		}

		// Token: 0x06006691 RID: 26257 RVA: 0x00128D7D File Offset: 0x00126F7D
		public override void OnChildExecuted(int childIndex, TaskStatus childStatus)
		{
			this.executionStatus[childIndex] = childStatus;
		}

		// Token: 0x06006692 RID: 26258 RVA: 0x00128D88 File Offset: 0x00126F88
		public override TaskStatus OverrideStatus(TaskStatus status)
		{
			bool flag = true;
			for (int i = 0; i < this.executionStatus.Length; i++)
			{
				if (this.executionStatus[i] == TaskStatus.Running)
				{
					flag = false;
				}
				else if (this.executionStatus[i] == TaskStatus.Failure)
				{
					return TaskStatus.Failure;
				}
			}
			if (!flag)
			{
				return TaskStatus.Running;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006693 RID: 26259 RVA: 0x00128DCC File Offset: 0x00126FCC
		public override void OnConditionalAbort(int childIndex)
		{
			this.currentChildIndex = 0;
			for (int i = 0; i < this.executionStatus.Length; i++)
			{
				this.executionStatus[i] = TaskStatus.Inactive;
			}
		}

		// Token: 0x06006694 RID: 26260 RVA: 0x00128DFC File Offset: 0x00126FFC
		public override void OnEnd()
		{
			for (int i = 0; i < this.executionStatus.Length; i++)
			{
				this.executionStatus[i] = TaskStatus.Inactive;
			}
			this.currentChildIndex = 0;
		}

		// Token: 0x0400529C RID: 21148
		private int currentChildIndex;

		// Token: 0x0400529D RID: 21149
		private TaskStatus[] executionStatus;
	}
}
