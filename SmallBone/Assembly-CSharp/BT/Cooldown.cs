using System;
using UnityEngine;

namespace BT
{
	// Token: 0x02001409 RID: 5129
	public sealed class Cooldown : Decorator
	{
		// Token: 0x060064F1 RID: 25841 RVA: 0x0012465F File Offset: 0x0012285F
		protected override void OnInitialize()
		{
			this._duration = this._durationRange.value;
			base.OnInitialize();
		}

		// Token: 0x060064F2 RID: 25842 RVA: 0x00124678 File Offset: 0x00122878
		protected override NodeState UpdateDeltatime(Context context)
		{
			if (!this._onCoolDown)
			{
				return this.RegularBehaviour(context);
			}
			return this.CoolDownBehaviour(context);
		}

		// Token: 0x060064F3 RID: 25843 RVA: 0x00124691 File Offset: 0x00122891
		private NodeState RegularBehaviour(Context context)
		{
			NodeState nodeState = this._subTree.Tick(context);
			if (nodeState == NodeState.Success)
			{
				this.EnterCoolDown();
			}
			return nodeState;
		}

		// Token: 0x060064F4 RID: 25844 RVA: 0x001246A9 File Offset: 0x001228A9
		private NodeState CoolDownBehaviour(Context context)
		{
			if (Time.time - this._startTimeStamp >= this._duration)
			{
				this.ExitCoolDown();
				return this.RegularBehaviour(context);
			}
			return NodeState.Fail;
		}

		// Token: 0x060064F5 RID: 25845 RVA: 0x001246CE File Offset: 0x001228CE
		private void EnterCoolDown()
		{
			this._startTimeStamp = Time.time;
			this._onCoolDown = true;
		}

		// Token: 0x060064F6 RID: 25846 RVA: 0x001246E2 File Offset: 0x001228E2
		private void ExitCoolDown()
		{
			this._onCoolDown = false;
		}

		// Token: 0x060064F7 RID: 25847 RVA: 0x001246EB File Offset: 0x001228EB
		protected override void OnTerminate(NodeState state)
		{
			base.OnTerminate(state);
		}

		// Token: 0x04005154 RID: 20820
		[SerializeField]
		private CustomFloat _durationRange;

		// Token: 0x04005155 RID: 20821
		private float _duration;

		// Token: 0x04005156 RID: 20822
		private float _startTimeStamp;

		// Token: 0x04005157 RID: 20823
		private bool _onCoolDown;
	}
}
