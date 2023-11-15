using System;

namespace BT
{
	// Token: 0x02001404 RID: 5124
	public class Sequence : Composite
	{
		// Token: 0x060064E0 RID: 25824 RVA: 0x001244CE File Offset: 0x001226CE
		protected virtual Node GetChild(int i)
		{
			if (i >= this._child.components.Length || i < 0)
			{
				throw new ArgumentException("invalid child index");
			}
			return this._child.components[i].node;
		}

		// Token: 0x060064E1 RID: 25825 RVA: 0x00124504 File Offset: 0x00122704
		protected override NodeState UpdateDeltatime(Context context)
		{
			NodeState nodeState;
			for (;;)
			{
				nodeState = this.GetChild(this._currentIndex).Tick(context);
				if (nodeState != NodeState.Success)
				{
					break;
				}
				int num = this._currentIndex + 1;
				this._currentIndex = num;
				if (num >= this._child.components.Length)
				{
					return NodeState.Success;
				}
			}
			return nodeState;
		}

		// Token: 0x060064E2 RID: 25826 RVA: 0x0012454B File Offset: 0x0012274B
		protected override void DoReset(NodeState state)
		{
			this._currentIndex = 0;
			base.DoReset(state);
		}

		// Token: 0x0400514E RID: 20814
		private int _currentIndex;
	}
}
