using System;
using Characters.AI.Behaviours.Pope;
using UnityEngine;

namespace Characters.AI.Conditions
{
	// Token: 0x020011CC RID: 4556
	public sealed class CanChoice : Condition
	{
		// Token: 0x0600598F RID: 22927 RVA: 0x0010A7C7 File Offset: 0x001089C7
		protected override bool Check(AIController controller)
		{
			return this._choice.CanUse();
		}

		// Token: 0x04004852 RID: 18514
		[SerializeField]
		private Choice _choice;
	}
}
