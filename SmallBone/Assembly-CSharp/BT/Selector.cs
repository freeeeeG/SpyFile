using System;

namespace BT
{
	// Token: 0x02001403 RID: 5123
	public class Selector : Composite
	{
		// Token: 0x060064DC RID: 25820 RVA: 0x00124439 File Offset: 0x00122639
		protected virtual Node GetChild(int i)
		{
			if (i >= this._child.components.Length || i < 0)
			{
				throw new ArgumentException(string.Format("{0} : invalid child index", i));
			}
			return this._child.components[i].node;
		}

		// Token: 0x060064DD RID: 25821 RVA: 0x00124478 File Offset: 0x00122678
		protected override NodeState UpdateDeltatime(Context context)
		{
			NodeState nodeState;
			for (;;)
			{
				nodeState = this.GetChild(this._currentIndex).Tick(context);
				if (nodeState != NodeState.Fail)
				{
					break;
				}
				int num = this._currentIndex + 1;
				this._currentIndex = num;
				if (num >= this._child.components.Length)
				{
					return NodeState.Fail;
				}
			}
			return nodeState;
		}

		// Token: 0x060064DE RID: 25822 RVA: 0x001244BE File Offset: 0x001226BE
		protected override void DoReset(NodeState state)
		{
			this._currentIndex = 0;
			base.DoReset(state);
		}

		// Token: 0x0400514D RID: 20813
		private int _currentIndex;
	}
}
