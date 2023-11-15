using System;
using UnityEngine;

namespace BT
{
	// Token: 0x02001420 RID: 5152
	public class WaitForDuration : Node
	{
		// Token: 0x06006540 RID: 25920 RVA: 0x0012540C File Offset: 0x0012360C
		protected override void OnInitialize()
		{
			this._duration = this._durationRange.value;
		}

		// Token: 0x06006541 RID: 25921 RVA: 0x0012541F File Offset: 0x0012361F
		protected override NodeState UpdateDeltatime(Context context)
		{
			this._elapsed += context.deltaTime;
			if (this._elapsed >= this._duration)
			{
				return NodeState.Success;
			}
			return NodeState.Running;
		}

		// Token: 0x06006542 RID: 25922 RVA: 0x00125445 File Offset: 0x00123645
		protected override void OnTerminate(NodeState state)
		{
			this._elapsed = 0f;
			base.OnTerminate(state);
		}

		// Token: 0x06006543 RID: 25923 RVA: 0x00125459 File Offset: 0x00123659
		protected override void DoReset(NodeState state)
		{
			this._elapsed = 0f;
			base.DoReset(state);
		}

		// Token: 0x04005193 RID: 20883
		[SerializeField]
		private CustomFloat _durationRange;

		// Token: 0x04005194 RID: 20884
		private float _duration;

		// Token: 0x04005195 RID: 20885
		private float _elapsed;
	}
}
