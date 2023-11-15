using System;
using UnityEngine;

namespace BT
{
	// Token: 0x02001402 RID: 5122
	public class Parallel : Composite
	{
		// Token: 0x060064D9 RID: 25817 RVA: 0x000147BD File Offset: 0x000129BD
		protected override NodeState UpdateDeltatime(Context context)
		{
			return NodeState.Running;
		}

		// Token: 0x060064DA RID: 25818 RVA: 0x00124428 File Offset: 0x00122628
		protected override void DoReset(NodeState state)
		{
			base.DoReset(state);
		}

		// Token: 0x0400514B RID: 20811
		[SerializeField]
		private int _successCount;

		// Token: 0x0400514C RID: 20812
		[SerializeField]
		private int _failCount;
	}
}
