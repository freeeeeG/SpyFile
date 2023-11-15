using System;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001498 RID: 5272
	[TaskIcon("{SkinColor}UtilitySelectorIcon.png")]
	[TaskDescription("The utility selector task evaluates the child tasks using Utility Theory AI. The child task can override the GetUtility method and return the utility value at that particular time. The task with the highest utility value will be selected and the existing running task will be aborted. The utility selector task reevaluates its children every tick.")]
	public class UtilitySelector : Composite
	{
		// Token: 0x060066DD RID: 26333 RVA: 0x00129658 File Offset: 0x00127858
		public override void OnStart()
		{
			this.highestUtility = float.MinValue;
			this.availableChildren.Clear();
			for (int i = 0; i < this.children.Count; i++)
			{
				float utility = this.children[i].GetUtility();
				if (utility > this.highestUtility)
				{
					this.highestUtility = utility;
					this.currentChildIndex = i;
				}
				this.availableChildren.Add(i);
			}
		}

		// Token: 0x060066DE RID: 26334 RVA: 0x001296C6 File Offset: 0x001278C6
		public override int CurrentChildIndex()
		{
			return this.currentChildIndex;
		}

		// Token: 0x060066DF RID: 26335 RVA: 0x001296CE File Offset: 0x001278CE
		public override void OnChildStarted(int childIndex)
		{
			this.executionStatus = TaskStatus.Running;
		}

		// Token: 0x060066E0 RID: 26336 RVA: 0x001296D7 File Offset: 0x001278D7
		public override bool CanExecute()
		{
			return this.executionStatus != TaskStatus.Success && this.executionStatus != TaskStatus.Running && !this.reevaluating && this.availableChildren.Count > 0;
		}

		// Token: 0x060066E1 RID: 26337 RVA: 0x00129704 File Offset: 0x00127904
		public override void OnChildExecuted(int childIndex, TaskStatus childStatus)
		{
			if (childStatus != TaskStatus.Inactive && childStatus != TaskStatus.Running)
			{
				this.executionStatus = childStatus;
				if (this.executionStatus == TaskStatus.Failure)
				{
					this.availableChildren.Remove(childIndex);
					this.highestUtility = float.MinValue;
					for (int i = 0; i < this.availableChildren.Count; i++)
					{
						float utility = this.children[this.availableChildren[i]].GetUtility();
						if (utility > this.highestUtility)
						{
							this.highestUtility = utility;
							this.currentChildIndex = this.availableChildren[i];
						}
					}
				}
			}
		}

		// Token: 0x060066E2 RID: 26338 RVA: 0x00129798 File Offset: 0x00127998
		public override void OnConditionalAbort(int childIndex)
		{
			this.currentChildIndex = childIndex;
			this.executionStatus = TaskStatus.Inactive;
		}

		// Token: 0x060066E3 RID: 26339 RVA: 0x001297A8 File Offset: 0x001279A8
		public override void OnEnd()
		{
			this.executionStatus = TaskStatus.Inactive;
			this.currentChildIndex = 0;
		}

		// Token: 0x060066E4 RID: 26340 RVA: 0x001297B8 File Offset: 0x001279B8
		public override TaskStatus OverrideStatus(TaskStatus status)
		{
			return this.executionStatus;
		}

		// Token: 0x060066E5 RID: 26341 RVA: 0x000076D4 File Offset: 0x000058D4
		public override bool CanRunParallelChildren()
		{
			return true;
		}

		// Token: 0x060066E6 RID: 26342 RVA: 0x000076D4 File Offset: 0x000058D4
		public override bool CanReevaluate()
		{
			return true;
		}

		// Token: 0x060066E7 RID: 26343 RVA: 0x001297C0 File Offset: 0x001279C0
		public override bool OnReevaluationStarted()
		{
			if (this.executionStatus == TaskStatus.Inactive)
			{
				return false;
			}
			this.reevaluating = true;
			return true;
		}

		// Token: 0x060066E8 RID: 26344 RVA: 0x001297D4 File Offset: 0x001279D4
		public override void OnReevaluationEnded(TaskStatus status)
		{
			this.reevaluating = false;
			int num = this.currentChildIndex;
			this.highestUtility = float.MinValue;
			for (int i = 0; i < this.availableChildren.Count; i++)
			{
				float utility = this.children[this.availableChildren[i]].GetUtility();
				if (utility > this.highestUtility)
				{
					this.highestUtility = utility;
					this.currentChildIndex = this.availableChildren[i];
				}
			}
			if (num != this.currentChildIndex)
			{
				BehaviorManager.instance.Interrupt(base.Owner, this.children[num], this, TaskStatus.Failure);
				this.executionStatus = TaskStatus.Inactive;
			}
		}

		// Token: 0x040052B7 RID: 21175
		private int currentChildIndex;

		// Token: 0x040052B8 RID: 21176
		private float highestUtility;

		// Token: 0x040052B9 RID: 21177
		private TaskStatus executionStatus;

		// Token: 0x040052BA RID: 21178
		private bool reevaluating;

		// Token: 0x040052BB RID: 21179
		private List<int> availableChildren = new List<int>();
	}
}
