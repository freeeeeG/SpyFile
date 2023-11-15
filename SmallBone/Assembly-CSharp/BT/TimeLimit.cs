using System;
using UnityEngine;

namespace BT
{
	// Token: 0x02001411 RID: 5137
	public class TimeLimit : Decorator
	{
		// Token: 0x0600650C RID: 25868 RVA: 0x0012483C File Offset: 0x00122A3C
		protected override void OnInitialize()
		{
			this._duration = this._durationRange.value;
			base.OnInitialize();
		}

		// Token: 0x0600650D RID: 25869 RVA: 0x00124858 File Offset: 0x00122A58
		protected override NodeState UpdateDeltatime(Context context)
		{
			float deltaTime = context.deltaTime;
			this._elapsed += deltaTime;
			if (this._elapsed >= this._duration)
			{
				return NodeState.Fail;
			}
			return this._subTree.Tick(context);
		}

		// Token: 0x0600650E RID: 25870 RVA: 0x00124896 File Offset: 0x00122A96
		protected override void DoReset(NodeState state)
		{
			this._elapsed = 0f;
			base.DoReset(state);
		}

		// Token: 0x0400515C RID: 20828
		[SerializeField]
		private CustomFloat _durationRange;

		// Token: 0x0400515D RID: 20829
		private float _elapsed;

		// Token: 0x0400515E RID: 20830
		private float _duration;
	}
}
