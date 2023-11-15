using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001493 RID: 5267
	[TaskIcon("{SkinColor}RandomSelectorIcon.png")]
	[TaskDescription("Similar to the selector task, the random selector task will return success as soon as a child task returns success.  The difference is that the random selector class will run its children in a random order. The selector task is deterministic in that it will always run the tasks from left to right within the tree. The random selector task shuffles the child tasks up and then begins execution in a random order. Other than that the random selector class is the same as the selector class. It will continue running tasks until a task completes successfully. If no child tasks return success then it will return failure.")]
	public class RandomSelector : Composite
	{
		// Token: 0x060066B1 RID: 26289 RVA: 0x00129144 File Offset: 0x00127344
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

		// Token: 0x060066B2 RID: 26290 RVA: 0x00129191 File Offset: 0x00127391
		public override void OnStart()
		{
			this.ShuffleChilden();
		}

		// Token: 0x060066B3 RID: 26291 RVA: 0x00129199 File Offset: 0x00127399
		public override int CurrentChildIndex()
		{
			return this.childrenExecutionOrder.Peek();
		}

		// Token: 0x060066B4 RID: 26292 RVA: 0x001291A6 File Offset: 0x001273A6
		public override bool CanExecute()
		{
			return this.childrenExecutionOrder.Count > 0 && this.executionStatus != TaskStatus.Success;
		}

		// Token: 0x060066B5 RID: 26293 RVA: 0x001291C4 File Offset: 0x001273C4
		public override void OnChildExecuted(TaskStatus childStatus)
		{
			if (this.childrenExecutionOrder.Count > 0)
			{
				this.childrenExecutionOrder.Pop();
			}
			this.executionStatus = childStatus;
		}

		// Token: 0x060066B6 RID: 26294 RVA: 0x001291E7 File Offset: 0x001273E7
		public override void OnConditionalAbort(int childIndex)
		{
			this.childrenExecutionOrder.Clear();
			this.executionStatus = TaskStatus.Inactive;
			this.ShuffleChilden();
		}

		// Token: 0x060066B7 RID: 26295 RVA: 0x00129201 File Offset: 0x00127401
		public override void OnEnd()
		{
			this.executionStatus = TaskStatus.Inactive;
			this.childrenExecutionOrder.Clear();
		}

		// Token: 0x060066B8 RID: 26296 RVA: 0x00129215 File Offset: 0x00127415
		public override void OnReset()
		{
			this.seed = 0;
			this.useSeed = false;
		}

		// Token: 0x060066B9 RID: 26297 RVA: 0x00129228 File Offset: 0x00127428
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

		// Token: 0x040052A5 RID: 21157
		[Tooltip("Seed the random number generator to make things easier to debug")]
		public int seed;

		// Token: 0x040052A6 RID: 21158
		[Tooltip("Do we want to use the seed?")]
		public bool useSeed;

		// Token: 0x040052A7 RID: 21159
		private List<int> childIndexList = new List<int>();

		// Token: 0x040052A8 RID: 21160
		private Stack<int> childrenExecutionOrder = new Stack<int>();

		// Token: 0x040052A9 RID: 21161
		private TaskStatus executionStatus;
	}
}
