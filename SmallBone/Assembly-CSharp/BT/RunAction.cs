using System;
using Characters.Actions;
using UnityEngine;

namespace BT
{
	// Token: 0x0200141C RID: 5148
	public sealed class RunAction : Node
	{
		// Token: 0x06006534 RID: 25908 RVA: 0x00125287 File Offset: 0x00123487
		protected override NodeState UpdateDeltatime(Context context)
		{
			if (this._action.running)
			{
				return NodeState.Running;
			}
			if (this._running)
			{
				return NodeState.Success;
			}
			if (!this._action.TryStart())
			{
				return NodeState.Fail;
			}
			this._running = true;
			return NodeState.Running;
		}

		// Token: 0x06006535 RID: 25909 RVA: 0x001252B9 File Offset: 0x001234B9
		protected override void OnTerminate(NodeState state)
		{
			this._running = false;
			base.OnTerminate(state);
		}

		// Token: 0x06006536 RID: 25910 RVA: 0x001252C9 File Offset: 0x001234C9
		protected override void DoReset(NodeState state)
		{
			this._running = false;
			base.DoReset(state);
		}

		// Token: 0x04005188 RID: 20872
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x04005189 RID: 20873
		private bool _running;
	}
}
