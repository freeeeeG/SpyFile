using System;

namespace BT
{
	// Token: 0x02001410 RID: 5136
	public class Succeder : Decorator
	{
		// Token: 0x0600650A RID: 25866 RVA: 0x00124818 File Offset: 0x00122A18
		protected override NodeState UpdateDeltatime(Context context)
		{
			NodeState nodeState = this._subTree.Tick(context);
			if (nodeState == NodeState.Success || nodeState == NodeState.Fail)
			{
				return NodeState.Success;
			}
			return nodeState;
		}
	}
}
