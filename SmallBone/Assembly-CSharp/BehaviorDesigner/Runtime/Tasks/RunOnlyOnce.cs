using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014D7 RID: 5335
	[TaskDescription("이 노드의 자식노드는 오직 한번만 실행, 두번째 실행부터는 실패를 반환")]
	public class RunOnlyOnce : Decorator
	{
		// Token: 0x060067BD RID: 26557 RVA: 0x0012C06E File Offset: 0x0012A26E
		public override bool CanExecute()
		{
			return this.executionStatus == TaskStatus.Inactive;
		}

		// Token: 0x060067BE RID: 26558 RVA: 0x0012C079 File Offset: 0x0012A279
		public override void OnChildExecuted(TaskStatus childStatus)
		{
			this.executionStatus = childStatus;
		}

		// Token: 0x060067BF RID: 26559 RVA: 0x0012C02C File Offset: 0x0012A22C
		public override TaskStatus OverrideStatus(TaskStatus status)
		{
			if (status == TaskStatus.Success)
			{
				return TaskStatus.Failure;
			}
			return status;
		}

		// Token: 0x040053B1 RID: 21425
		private TaskStatus executionStatus;
	}
}
