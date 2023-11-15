using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001494 RID: 5268
	[TaskDescription("Similar to the sequence task, the random sequence task will return success as soon as every child task returns success.  The difference is that the random sequence class will run its children in a random order. The sequence task is deterministic in that it will always run the tasks from left to right within the tree. The random sequence task shuffles the child tasks up and then begins execution in a random order. Other than that the random sequence class is the same as the sequence class. It will stop running tasks as soon as a single task ends in failure. On a task failure it will stop executing all of the child tasks and return failure. If no child returns failure then it will return success.")]
	[TaskIcon("{SkinColor}RandomSequenceIcon.png")]
	public class RandomSequence : Composite
	{
		// Token: 0x060066BB RID: 26299 RVA: 0x001292B4 File Offset: 0x001274B4
		public override void OnAwake()
		{
			if (this.useSeed)
			{
				UnityEngine.Random.InitState(this.seed);
			}
			this.childIndexList.Clear();
			for (int i = 0; i < this.children.Count; i++)
			{
				this.childIndexList.Add(i);
			}
		}

		// Token: 0x060066BC RID: 26300 RVA: 0x00129301 File Offset: 0x00127501
		public override void OnStart()
		{
			this.ShuffleChilden();
		}

		// Token: 0x060066BD RID: 26301 RVA: 0x00129309 File Offset: 0x00127509
		public override int CurrentChildIndex()
		{
			return this.childrenExecutionOrder.Peek();
		}

		// Token: 0x060066BE RID: 26302 RVA: 0x00129316 File Offset: 0x00127516
		public override bool CanExecute()
		{
			return this.childrenExecutionOrder.Count > 0 && this.executionStatus != TaskStatus.Failure;
		}

		// Token: 0x060066BF RID: 26303 RVA: 0x00129334 File Offset: 0x00127534
		public override void OnChildExecuted(TaskStatus childStatus)
		{
			if (this.childrenExecutionOrder.Count > 0)
			{
				this.childrenExecutionOrder.Pop();
			}
			this.executionStatus = childStatus;
		}

		// Token: 0x060066C0 RID: 26304 RVA: 0x00129357 File Offset: 0x00127557
		public override void OnConditionalAbort(int childIndex)
		{
			this.childrenExecutionOrder.Clear();
			this.executionStatus = TaskStatus.Inactive;
			this.ShuffleChilden();
		}

		// Token: 0x060066C1 RID: 26305 RVA: 0x00129371 File Offset: 0x00127571
		public override void OnEnd()
		{
			this.executionStatus = TaskStatus.Inactive;
			this.childrenExecutionOrder.Clear();
		}

		// Token: 0x060066C2 RID: 26306 RVA: 0x00129385 File Offset: 0x00127585
		public override void OnReset()
		{
			this.seed = 0;
			this.useSeed = false;
		}

		// Token: 0x060066C3 RID: 26307 RVA: 0x00129398 File Offset: 0x00127598
		private void ShuffleChilden()
		{
			for (int i = this.childIndexList.Count; i > 0; i--)
			{
				int index = UnityEngine.Random.Range(0, i);
				int num = this.childIndexList[index];
				this.childrenExecutionOrder.Push(num);
				this.childIndexList[index] = this.childIndexList[i - 1];
				this.childIndexList[i - 1] = num;
			}
		}

		// Token: 0x040052AA RID: 21162
		[Tooltip("Seed the random number generator to make things easier to debug")]
		public int seed;

		// Token: 0x040052AB RID: 21163
		[Tooltip("Do we want to use the seed?")]
		public bool useSeed;

		// Token: 0x040052AC RID: 21164
		private List<int> childIndexList = new List<int>();

		// Token: 0x040052AD RID: 21165
		private Stack<int> childrenExecutionOrder = new Stack<int>();

		// Token: 0x040052AE RID: 21166
		private TaskStatus executionStatus;
	}
}
