using System;
using UnityEngine;

namespace BT
{
	// Token: 0x0200140E RID: 5134
	public class Repeat : Decorator
	{
		// Token: 0x06006504 RID: 25860 RVA: 0x00124795 File Offset: 0x00122995
		protected override void OnInitialize()
		{
			this._currentCount = 0;
			base.OnInitialize();
		}

		// Token: 0x06006505 RID: 25861 RVA: 0x001247A4 File Offset: 0x001229A4
		protected override NodeState UpdateDeltatime(Context context)
		{
			NodeState nodeState = this._subTree.Tick(context);
			if (nodeState == NodeState.Running)
			{
				return nodeState;
			}
			this._currentCount++;
			if (this._currentCount < this._count)
			{
				return NodeState.Running;
			}
			return NodeState.Success;
		}

		// Token: 0x06006506 RID: 25862 RVA: 0x001247E3 File Offset: 0x001229E3
		protected override void DoReset(NodeState state)
		{
			this._currentCount = 0;
			base.DoReset(state);
		}

		// Token: 0x0400515A RID: 20826
		[Header("성공이든 실패든 연속 실행 후 성공 반환")]
		[SerializeField]
		private int _count;

		// Token: 0x0400515B RID: 20827
		private int _currentCount;
	}
}
