using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001499 RID: 5273
	[TaskIcon("{SkinColor}PrioritySelectorIcon.png")]
	[TaskDescription("가중치에 따라 실행되는 Selector, 한 번 선택이후 자식 노드의 성공 여부와 상관없이 선택된 노드만 실행됨")]
	public class WeightedSelector : Composite
	{
		// Token: 0x060066EA RID: 26346 RVA: 0x00129890 File Offset: 0x00127A90
		public override void OnStart()
		{
			List<ValueTuple<int, float>> list = new List<ValueTuple<int, float>>(this.children.Count);
			for (int i = 0; i < this.children.Count; i++)
			{
				list.Add(new ValueTuple<int, float>(i, this._weights[i]));
			}
			this._randomizer = new WeightedRandomizer<int>(list);
			this.currentChildIndex = this._randomizer.TakeOne();
		}

		// Token: 0x060066EB RID: 26347 RVA: 0x001298F9 File Offset: 0x00127AF9
		public override int CurrentChildIndex()
		{
			return this.currentChildIndex;
		}

		// Token: 0x060066EC RID: 26348 RVA: 0x00129901 File Offset: 0x00127B01
		public override bool CanExecute()
		{
			return this.executionStatus != TaskStatus.Failure && this.executionStatus != TaskStatus.Success;
		}

		// Token: 0x060066ED RID: 26349 RVA: 0x0012991A File Offset: 0x00127B1A
		public override void OnChildExecuted(TaskStatus childStatus)
		{
			this.currentChildIndex++;
			this.executionStatus = childStatus;
		}

		// Token: 0x060066EE RID: 26350 RVA: 0x00129931 File Offset: 0x00127B31
		public override void OnConditionalAbort(int childIndex)
		{
			this.currentChildIndex = childIndex;
			this.executionStatus = TaskStatus.Inactive;
		}

		// Token: 0x060066EF RID: 26351 RVA: 0x00129941 File Offset: 0x00127B41
		public override void OnEnd()
		{
			this.executionStatus = TaskStatus.Inactive;
			this.currentChildIndex = 0;
		}

		// Token: 0x060066F0 RID: 26352 RVA: 0x00129954 File Offset: 0x00127B54
		public override string OnDrawNodeText()
		{
			string result = base.OnDrawNodeText();
			if (this._weights == null)
			{
				return result;
			}
			if (this.children == null)
			{
				return result;
			}
			if (this._weights.Count == this.children.Count)
			{
				base.NodeData.ColorIndex = 0;
			}
			else
			{
				base.NodeData.ColorIndex = 1;
			}
			return result;
		}

		// Token: 0x040052BC RID: 21180
		[SerializeField]
		private List<float> _weights;

		// Token: 0x040052BD RID: 21181
		private int currentChildIndex;

		// Token: 0x040052BE RID: 21182
		private TaskStatus executionStatus;

		// Token: 0x040052BF RID: 21183
		private WeightedRandomizer<int> _randomizer;
	}
}
