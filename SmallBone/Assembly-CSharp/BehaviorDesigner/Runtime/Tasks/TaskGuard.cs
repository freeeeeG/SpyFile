using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014D8 RID: 5336
	[TaskDescription("The task guard task is similar to a semaphore in multithreaded programming. The task guard task is there to ensure a limited resource is not being overused. \n\nFor example, you may place a task guard above a task that plays an animation. Elsewhere within your behavior tree you may also have another task that plays a different animation but uses the same bones for that animation. Because of this you don't want that animation to play twice at the same time. Placing a task guard will let you specify how many times a particular task can be accessed at the same time.\n\nIn the previous animation task example you would specify an access count of 1. With this setup the animation task can be only controlled by one task at a time. If the first task is playing the animation and a second task wants to control the animation as well, it will either have to wait or skip over the task completely.")]
	[TaskIcon("{SkinColor}TaskGuardIcon.png")]
	public class TaskGuard : Decorator
	{
		// Token: 0x060067C1 RID: 26561 RVA: 0x0012C082 File Offset: 0x0012A282
		public override bool CanExecute()
		{
			return this.executingTasks < this.maxTaskAccessCount.Value && !this.executing;
		}

		// Token: 0x060067C2 RID: 26562 RVA: 0x0012C0A4 File Offset: 0x0012A2A4
		public override void OnChildStarted()
		{
			this.executingTasks++;
			this.executing = true;
			for (int i = 0; i < this.linkedTaskGuards.Length; i++)
			{
				this.linkedTaskGuards[i].taskExecuting(true);
			}
		}

		// Token: 0x060067C3 RID: 26563 RVA: 0x0012C0E7 File Offset: 0x0012A2E7
		public override TaskStatus OverrideStatus(TaskStatus status)
		{
			if (this.executing || !this.waitUntilTaskAvailable.Value)
			{
				return status;
			}
			return TaskStatus.Running;
		}

		// Token: 0x060067C4 RID: 26564 RVA: 0x0012C101 File Offset: 0x0012A301
		public void taskExecuting(bool increase)
		{
			this.executingTasks += (increase ? 1 : -1);
		}

		// Token: 0x060067C5 RID: 26565 RVA: 0x0012C118 File Offset: 0x0012A318
		public override void OnEnd()
		{
			if (this.executing)
			{
				this.executingTasks--;
				for (int i = 0; i < this.linkedTaskGuards.Length; i++)
				{
					this.linkedTaskGuards[i].taskExecuting(false);
				}
				this.executing = false;
			}
		}

		// Token: 0x060067C6 RID: 26566 RVA: 0x0012C163 File Offset: 0x0012A363
		public override void OnReset()
		{
			this.maxTaskAccessCount = null;
			this.linkedTaskGuards = null;
			this.waitUntilTaskAvailable = true;
		}

		// Token: 0x040053B2 RID: 21426
		[Tooltip("The number of times the child tasks can be accessed by parallel tasks at once")]
		public SharedInt maxTaskAccessCount;

		// Token: 0x040053B3 RID: 21427
		[Tooltip("The linked tasks that also guard a task. If the task guard is not linked against any other tasks it doesn't have much purpose. Marked as LinkedTask to ensure all tasks linked are linked to the same set of tasks")]
		[LinkedTask]
		public TaskGuard[] linkedTaskGuards;

		// Token: 0x040053B4 RID: 21428
		[Tooltip("If true the task will wait until the child task is available. If false then any unavailable child tasks will be skipped over")]
		public SharedBool waitUntilTaskAvailable;

		// Token: 0x040053B5 RID: 21429
		private int executingTasks;

		// Token: 0x040053B6 RID: 21430
		private bool executing;
	}
}
