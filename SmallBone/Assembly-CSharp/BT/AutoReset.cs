using System;

namespace BT
{
	// Token: 0x02001407 RID: 5127
	public class AutoReset : Decorator
	{
		// Token: 0x060064EC RID: 25836 RVA: 0x00124624 File Offset: 0x00122824
		protected override NodeState UpdateDeltatime(Context context)
		{
			return this._subTree.Tick(context);
		}

		// Token: 0x060064ED RID: 25837 RVA: 0x00124632 File Offset: 0x00122832
		protected override void OnTerminate(NodeState state)
		{
			this._subTree.node.ResetState();
		}
	}
}
