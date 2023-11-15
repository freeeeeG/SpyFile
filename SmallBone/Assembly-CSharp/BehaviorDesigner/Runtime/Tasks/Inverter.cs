using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014D3 RID: 5331
	[TaskDescription("The inverter task will invert the return value of the child task after it has finished executing. If the child returns success, the inverter task will return failure. If the child returns failure, the inverter task will return success.")]
	[TaskIcon("{SkinColor}InverterIcon.png")]
	public class Inverter : Decorator
	{
		// Token: 0x060067A9 RID: 26537 RVA: 0x0012BF20 File Offset: 0x0012A120
		public override bool CanExecute()
		{
			return this.executionStatus == TaskStatus.Inactive || this.executionStatus == TaskStatus.Running;
		}

		// Token: 0x060067AA RID: 26538 RVA: 0x0012BF35 File Offset: 0x0012A135
		public override void OnChildExecuted(TaskStatus childStatus)
		{
			this.executionStatus = childStatus;
		}

		// Token: 0x060067AB RID: 26539 RVA: 0x0012BF3E File Offset: 0x0012A13E
		public override TaskStatus Decorate(TaskStatus status)
		{
			if (status == TaskStatus.Success)
			{
				return TaskStatus.Failure;
			}
			if (status == TaskStatus.Failure)
			{
				return TaskStatus.Success;
			}
			return status;
		}

		// Token: 0x060067AC RID: 26540 RVA: 0x0012BF4D File Offset: 0x0012A14D
		public override void OnEnd()
		{
			this.executionStatus = TaskStatus.Inactive;
		}

		// Token: 0x040053A9 RID: 21417
		private TaskStatus executionStatus;
	}
}
