using System;
using Characters;
using UnityEngine;

namespace BT
{
	// Token: 0x02001417 RID: 5143
	public sealed class Despawn : Node
	{
		// Token: 0x06006523 RID: 25891 RVA: 0x00124DC9 File Offset: 0x00122FC9
		protected override NodeState UpdateDeltatime(Context context)
		{
			this._minion.Despawn();
			return NodeState.Success;
		}

		// Token: 0x04005171 RID: 20849
		[SerializeField]
		private Minion _minion;
	}
}
