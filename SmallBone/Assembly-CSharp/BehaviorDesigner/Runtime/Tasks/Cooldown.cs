using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014CE RID: 5326
	[TaskIcon("{SkinColor}CooldownIcon.png")]
	[TaskDescription("Waits the specified duration after the child has completed before returning the child's status of success or failure.")]
	public class Cooldown : Decorator
	{
		// Token: 0x06006796 RID: 26518 RVA: 0x0012BC73 File Offset: 0x00129E73
		public override bool CanExecute()
		{
			return this.cooldownTime == -1f || this.cooldownTime + this.duration.Value > Time.time;
		}

		// Token: 0x06006797 RID: 26519 RVA: 0x0012BC9D File Offset: 0x00129E9D
		public override int CurrentChildIndex()
		{
			if (this.cooldownTime == -1f)
			{
				return 0;
			}
			return -1;
		}

		// Token: 0x06006798 RID: 26520 RVA: 0x0012BCAF File Offset: 0x00129EAF
		public override void OnChildExecuted(TaskStatus childStatus)
		{
			this.executionStatus = childStatus;
			if (this.executionStatus == TaskStatus.Failure || this.executionStatus == TaskStatus.Success)
			{
				this.cooldownTime = Time.time;
			}
		}

		// Token: 0x06006799 RID: 26521 RVA: 0x0012BCD5 File Offset: 0x00129ED5
		public override TaskStatus OverrideStatus()
		{
			if (!this.CanExecute())
			{
				return TaskStatus.Running;
			}
			return this.executionStatus;
		}

		// Token: 0x0600679A RID: 26522 RVA: 0x0012BCE7 File Offset: 0x00129EE7
		public override TaskStatus OverrideStatus(TaskStatus status)
		{
			if (status == TaskStatus.Running)
			{
				return status;
			}
			return this.executionStatus;
		}

		// Token: 0x0600679B RID: 26523 RVA: 0x0012BCF5 File Offset: 0x00129EF5
		public override void OnEnd()
		{
			this.executionStatus = TaskStatus.Inactive;
			this.cooldownTime = -1f;
		}

		// Token: 0x04005391 RID: 21393
		public SharedFloat duration = 2f;

		// Token: 0x04005392 RID: 21394
		private TaskStatus executionStatus;

		// Token: 0x04005393 RID: 21395
		private float cooldownTime = -1f;
	}
}
