using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014CD RID: 5325
	[TaskDescription("Evaluates the specified conditional task. If the conditional task returns success then the child task is run and the child status is returned. If the conditional task does not return success then the child task is not run and a failure status is immediately returned.")]
	[TaskIcon("{SkinColor}ConditionalEvaluatorIcon.png")]
	public class ConditionalEvaluator : Decorator
	{
		// Token: 0x0600678A RID: 26506 RVA: 0x0012BB18 File Offset: 0x00129D18
		public override void OnAwake()
		{
			if (this.conditionalTask != null)
			{
				this.conditionalTask.Owner = base.Owner;
				this.conditionalTask.GameObject = this.gameObject;
				this.conditionalTask.Transform = this.transform;
				this.conditionalTask.OnAwake();
			}
		}

		// Token: 0x0600678B RID: 26507 RVA: 0x0012BB6B File Offset: 0x00129D6B
		public override void OnStart()
		{
			if (this.conditionalTask != null)
			{
				this.conditionalTask.OnStart();
			}
		}

		// Token: 0x0600678C RID: 26508 RVA: 0x0012BB80 File Offset: 0x00129D80
		public override bool CanExecute()
		{
			if (this.checkConditionalTask)
			{
				this.checkConditionalTask = false;
				this.OnUpdate();
			}
			return !this.conditionalTaskFailed && (this.executionStatus == TaskStatus.Inactive || this.executionStatus == TaskStatus.Running);
		}

		// Token: 0x0600678D RID: 26509 RVA: 0x0012BBB5 File Offset: 0x00129DB5
		public override bool CanReevaluate()
		{
			return this.reevaluate.Value;
		}

		// Token: 0x0600678E RID: 26510 RVA: 0x0012BBC4 File Offset: 0x00129DC4
		public override TaskStatus OnUpdate()
		{
			TaskStatus taskStatus = this.conditionalTask.OnUpdate();
			this.conditionalTaskFailed = (this.conditionalTask == null || taskStatus == TaskStatus.Failure);
			return taskStatus;
		}

		// Token: 0x0600678F RID: 26511 RVA: 0x0012BBF3 File Offset: 0x00129DF3
		public override void OnChildExecuted(TaskStatus childStatus)
		{
			this.executionStatus = childStatus;
		}

		// Token: 0x06006790 RID: 26512 RVA: 0x000076D4 File Offset: 0x000058D4
		public override TaskStatus OverrideStatus()
		{
			return TaskStatus.Failure;
		}

		// Token: 0x06006791 RID: 26513 RVA: 0x0012BBFC File Offset: 0x00129DFC
		public override TaskStatus OverrideStatus(TaskStatus status)
		{
			if (this.conditionalTaskFailed)
			{
				return TaskStatus.Failure;
			}
			return status;
		}

		// Token: 0x06006792 RID: 26514 RVA: 0x0012BC09 File Offset: 0x00129E09
		public override void OnEnd()
		{
			this.executionStatus = TaskStatus.Inactive;
			this.checkConditionalTask = true;
			this.conditionalTaskFailed = false;
			if (this.conditionalTask != null)
			{
				this.conditionalTask.OnEnd();
			}
		}

		// Token: 0x06006793 RID: 26515 RVA: 0x0012BC33 File Offset: 0x00129E33
		public override string OnDrawNodeText()
		{
			if (this.conditionalTask == null || !this.graphLabel)
			{
				return string.Empty;
			}
			return this.conditionalTask.GetType().Name;
		}

		// Token: 0x06006794 RID: 26516 RVA: 0x0012BC5B File Offset: 0x00129E5B
		public override void OnReset()
		{
			this.conditionalTask = null;
		}

		// Token: 0x0400538B RID: 21387
		[Tooltip("Should the conditional task be reevaluated every tick?")]
		public SharedBool reevaluate;

		// Token: 0x0400538C RID: 21388
		[Tooltip("The conditional task to evaluate")]
		[InspectTask]
		public Conditional conditionalTask;

		// Token: 0x0400538D RID: 21389
		[Tooltip("Should the inspected conditional task be labeled within the graph?")]
		public bool graphLabel;

		// Token: 0x0400538E RID: 21390
		private TaskStatus executionStatus;

		// Token: 0x0400538F RID: 21391
		private bool checkConditionalTask = true;

		// Token: 0x04005390 RID: 21392
		private bool conditionalTaskFailed;
	}
}
