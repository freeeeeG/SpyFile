using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001483 RID: 5251
	public class SetSharedVariable : Action
	{
		// Token: 0x06006652 RID: 26194 RVA: 0x00127FF3 File Offset: 0x001261F3
		public override TaskStatus OnUpdate()
		{
			this._to.SetValue(this._from.GetValue());
			return TaskStatus.Success;
		}

		// Token: 0x04005266 RID: 21094
		[SerializeField]
		private SharedVariable _to;

		// Token: 0x04005267 RID: 21095
		[SerializeField]
		private SharedVariable _from;
	}
}
