using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x0200148E RID: 5262
	[TaskIcon("{SkinColor}PrioritySelectorIcon.png")]
	[TaskDescription("자식 노드가 실행 횟수만큼 랜덤하게 실행됩니다.")]
	public class CountingRandomSelector : Composite
	{
		// Token: 0x06006681 RID: 26241 RVA: 0x00128AF7 File Offset: 0x00126CF7
		public override void OnAwake()
		{
			base.OnAwake();
			this._childIndics = new List<ValueTuple<int, int>>(this.children.Count);
			this.UpdateCount();
		}

		// Token: 0x06006682 RID: 26242 RVA: 0x00128B1C File Offset: 0x00126D1C
		public override void OnStart()
		{
			int index;
			if (this.TryTakeOne(out index))
			{
				this.currentChildIndex = this._childIndics[index].Item1;
				return;
			}
			this.UpdateCount();
			if (this.TryTakeOne(out index))
			{
				this.currentChildIndex = this._childIndics[index].Item1;
				return;
			}
			Debug.LogError("count는 최소 1 이상 이어야 합니다");
		}

		// Token: 0x06006683 RID: 26243 RVA: 0x00128B80 File Offset: 0x00126D80
		private void UpdateCount()
		{
			this._childIndics.Clear();
			for (int i = 0; i < this.children.Count; i++)
			{
				this._childIndics.Add(new ValueTuple<int, int>(i, UnityEngine.Random.Range(this._counts[i].x, this._counts[i].y + 1)));
			}
		}

		// Token: 0x06006684 RID: 26244 RVA: 0x00128BF0 File Offset: 0x00126DF0
		private bool TryTakeOne(out int index)
		{
			this._childIndics.Shuffle<ValueTuple<int, int>>();
			for (int i = 0; i < this._childIndics.Count; i++)
			{
				if (this._childIndics[i].Item2 > 0)
				{
					index = i;
					this._childIndics[index] = new ValueTuple<int, int>(this._childIndics[index].Item1, this._childIndics[index].Item2 - 1);
					return true;
				}
			}
			index = -1;
			return false;
		}

		// Token: 0x06006685 RID: 26245 RVA: 0x00128C73 File Offset: 0x00126E73
		public override int CurrentChildIndex()
		{
			return this.currentChildIndex;
		}

		// Token: 0x06006686 RID: 26246 RVA: 0x00128C7B File Offset: 0x00126E7B
		public override bool CanExecute()
		{
			return this.executionStatus != TaskStatus.Failure && this.executionStatus != TaskStatus.Success;
		}

		// Token: 0x06006687 RID: 26247 RVA: 0x00128C94 File Offset: 0x00126E94
		public override void OnChildExecuted(TaskStatus childStatus)
		{
			this.currentChildIndex++;
			this.executionStatus = childStatus;
		}

		// Token: 0x06006688 RID: 26248 RVA: 0x00128CAB File Offset: 0x00126EAB
		public override void OnConditionalAbort(int childIndex)
		{
			this.currentChildIndex = childIndex;
			this.executionStatus = TaskStatus.Inactive;
		}

		// Token: 0x06006689 RID: 26249 RVA: 0x00128CBB File Offset: 0x00126EBB
		public override void OnEnd()
		{
			this.executionStatus = TaskStatus.Inactive;
			this.currentChildIndex = 0;
		}

		// Token: 0x0600668A RID: 26250 RVA: 0x00128CCC File Offset: 0x00126ECC
		public override string OnDrawNodeText()
		{
			string result = base.OnDrawNodeText();
			if (this._counts == null)
			{
				return result;
			}
			if (this.children == null)
			{
				return result;
			}
			if (this._counts.Count == this.children.Count)
			{
				base.NodeData.ColorIndex = 0;
			}
			else
			{
				base.NodeData.ColorIndex = 1;
			}
			return result;
		}

		// Token: 0x04005298 RID: 21144
		[SerializeField]
		private List<Vector2Int> _counts;

		// Token: 0x04005299 RID: 21145
		private int currentChildIndex;

		// Token: 0x0400529A RID: 21146
		private TaskStatus executionStatus;

		// Token: 0x0400529B RID: 21147
		private List<ValueTuple<int, int>> _childIndics;
	}
}
