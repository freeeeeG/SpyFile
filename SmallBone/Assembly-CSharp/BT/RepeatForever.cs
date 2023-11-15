using System;

namespace BT
{
	// Token: 0x0200140F RID: 5135
	public class RepeatForever : Decorator
	{
		// Token: 0x06006508 RID: 25864 RVA: 0x001247F3 File Offset: 0x001229F3
		protected override NodeState UpdateDeltatime(Context context)
		{
			if (this._subTree.Tick(context) != NodeState.Running)
			{
				this._subTree.node.ResetState();
			}
			return NodeState.Running;
		}
	}
}
