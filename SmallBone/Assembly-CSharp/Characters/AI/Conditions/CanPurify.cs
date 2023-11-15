using System;
using Level.Chapter4;
using UnityEngine;

namespace Characters.AI.Conditions
{
	// Token: 0x020011CD RID: 4557
	public sealed class CanPurify : Condition
	{
		// Token: 0x06005991 RID: 22929 RVA: 0x0010A7D4 File Offset: 0x001089D4
		protected override bool Check(AIController controller)
		{
			return this._platformContainer.CanPurify();
		}

		// Token: 0x04004853 RID: 18515
		[SerializeField]
		private PlatformContainer _platformContainer;
	}
}
