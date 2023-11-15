using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001496 RID: 5270
	[TaskIcon("{SkinColor}SelectorEvaluatorIcon.png")]
	[TaskDescription("The selector evaluator is a selector task which reevaluates its children every tick. It will run the lowest priority child which returns a task status of running. This is done each tick. If a higher priority child is running and the next frame a lower priority child wants to run it will interrupt the higher priority child. The selector evaluator will return success as soon as the first child returns success otherwise it will keep trying higher priority children. This task mimics the conditional abort functionality except the child tasks don't always have to be conditional tasks.")]
	public class SelectorEvaluator : Composite
	{
		// Token: 0x060066CB RID: 26315 RVA: 0x00129485 File Offset: 0x00127685
		public override int CurrentChildIndex()
		{
			return this.currentChildIndex;
		}

		// Token: 0x060066CC RID: 26316 RVA: 0x0012948D File Offset: 0x0012768D
		public override void OnChildStarted(int childIndex)
		{
			this.currentChildIndex++;
			this.executionStatus = TaskStatus.Running;
		}

		// Token: 0x060066CD RID: 26317 RVA: 0x001294A4 File Offset: 0x001276A4
		public override bool CanExecute()
		{
			if (this.executionStatus == TaskStatus.Success || this.executionStatus == TaskStatus.Running)
			{
				return false;
			}
			if (this.storedCurrentChildIndex != -1)
			{
				return this.currentChildIndex < this.storedCurrentChildIndex - 1;
			}
			return this.currentChildIndex < this.children.Count;
		}

		// Token: 0x060066CE RID: 26318 RVA: 0x001294F2 File Offset: 0x001276F2
		public override void OnChildExecuted(int childIndex, TaskStatus childStatus)
		{
			if (childStatus == TaskStatus.Inactive && this.children[childIndex].Disabled)
			{
				this.executionStatus = TaskStatus.Failure;
			}
			if (childStatus != TaskStatus.Inactive && childStatus != TaskStatus.Running)
			{
				this.executionStatus = childStatus;
			}
		}

		// Token: 0x060066CF RID: 26319 RVA: 0x0012951F File Offset: 0x0012771F
		public override void OnConditionalAbort(int childIndex)
		{
			this.currentChildIndex = childIndex;
			this.executionStatus = TaskStatus.Inactive;
		}

		// Token: 0x060066D0 RID: 26320 RVA: 0x0012952F File Offset: 0x0012772F
		public override void OnEnd()
		{
			this.executionStatus = TaskStatus.Inactive;
			this.currentChildIndex = 0;
		}

		// Token: 0x060066D1 RID: 26321 RVA: 0x0012953F File Offset: 0x0012773F
		public override TaskStatus OverrideStatus(TaskStatus status)
		{
			return this.executionStatus;
		}

		// Token: 0x060066D2 RID: 26322 RVA: 0x000076D4 File Offset: 0x000058D4
		public override bool CanRunParallelChildren()
		{
			return true;
		}

		// Token: 0x060066D3 RID: 26323 RVA: 0x000076D4 File Offset: 0x000058D4
		public override bool CanReevaluate()
		{
			return true;
		}

		// Token: 0x060066D4 RID: 26324 RVA: 0x00129547 File Offset: 0x00127747
		public override bool OnReevaluationStarted()
		{
			if (this.executionStatus == TaskStatus.Inactive)
			{
				return false;
			}
			this.storedCurrentChildIndex = this.currentChildIndex;
			this.storedExecutionStatus = this.executionStatus;
			this.currentChildIndex = 0;
			this.executionStatus = TaskStatus.Inactive;
			return true;
		}

		// Token: 0x060066D5 RID: 26325 RVA: 0x0012957C File Offset: 0x0012777C
		public override void OnReevaluationEnded(TaskStatus status)
		{
			if (this.executionStatus != TaskStatus.Failure && this.executionStatus != TaskStatus.Inactive)
			{
				BehaviorManager.instance.Interrupt(base.Owner, this.children[this.storedCurrentChildIndex - 1], this, TaskStatus.Inactive);
			}
			else
			{
				this.currentChildIndex = this.storedCurrentChildIndex;
				this.executionStatus = this.storedExecutionStatus;
			}
			this.storedCurrentChildIndex = -1;
			this.storedExecutionStatus = TaskStatus.Inactive;
		}

		// Token: 0x040052B1 RID: 21169
		private int currentChildIndex;

		// Token: 0x040052B2 RID: 21170
		private TaskStatus executionStatus;

		// Token: 0x040052B3 RID: 21171
		private int storedCurrentChildIndex = -1;

		// Token: 0x040052B4 RID: 21172
		private TaskStatus storedExecutionStatus;
	}
}
