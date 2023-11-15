using System;

namespace BT
{
	// Token: 0x02001412 RID: 5138
	public class UntilFail : Decorator
	{
		// Token: 0x06006510 RID: 25872 RVA: 0x001248AA File Offset: 0x00122AAA
		protected override NodeState UpdateDeltatime(Context context)
		{
			if (this._subTree.Tick(context) != NodeState.Fail)
			{
				return NodeState.Running;
			}
			return NodeState.Success;
		}
	}
}
