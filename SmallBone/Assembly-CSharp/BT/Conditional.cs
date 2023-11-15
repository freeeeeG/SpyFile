using System;
using BT.Conditions;
using UnityEngine;

namespace BT
{
	// Token: 0x02001408 RID: 5128
	public sealed class Conditional : Node
	{
		// Token: 0x060064EF RID: 25839 RVA: 0x0012464C File Offset: 0x0012284C
		protected override NodeState UpdateDeltatime(Context context)
		{
			if (!this._condition.IsSatisfied(context))
			{
				return NodeState.Fail;
			}
			return NodeState.Success;
		}

		// Token: 0x04005153 RID: 20819
		[SerializeField]
		[Condition.SubcomponentAttribute(true)]
		private Condition _condition;
	}
}
