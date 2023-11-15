using System;

namespace BT
{
	// Token: 0x0200140C RID: 5132
	public class Inverter : Decorator
	{
		// Token: 0x060064FD RID: 25853 RVA: 0x00124718 File Offset: 0x00122918
		protected override NodeState UpdateDeltatime(Context context)
		{
			NodeState nodeState = this._subTree.Tick(context);
			if (nodeState == NodeState.Success)
			{
				return NodeState.Fail;
			}
			if (nodeState != NodeState.Fail)
			{
				return nodeState;
			}
			return NodeState.Success;
		}
	}
}
