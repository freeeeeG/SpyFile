using System;

namespace BT
{
	// Token: 0x0200140B RID: 5131
	public class Failer : Decorator
	{
		// Token: 0x060064FB RID: 25851 RVA: 0x001246F4 File Offset: 0x001228F4
		protected override NodeState UpdateDeltatime(Context context)
		{
			NodeState nodeState = this._subTree.Tick(context);
			if (nodeState == NodeState.Success || nodeState == NodeState.Fail)
			{
				return NodeState.Fail;
			}
			return nodeState;
		}
	}
}
