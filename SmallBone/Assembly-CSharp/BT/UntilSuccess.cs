using System;

namespace BT
{
	// Token: 0x02001413 RID: 5139
	public class UntilSuccess : Decorator
	{
		// Token: 0x06006512 RID: 25874 RVA: 0x001248BD File Offset: 0x00122ABD
		protected override NodeState UpdateDeltatime(Context context)
		{
			if (this._subTree.Tick(context) != NodeState.Success)
			{
				return NodeState.Running;
			}
			return NodeState.Success;
		}
	}
}
