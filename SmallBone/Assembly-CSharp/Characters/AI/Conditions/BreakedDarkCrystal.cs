using System;
using UnityEngine;

namespace Characters.AI.Conditions
{
	// Token: 0x020011CB RID: 4555
	public sealed class BreakedDarkCrystal : Condition
	{
		// Token: 0x0600598D RID: 22925 RVA: 0x0010A7A1 File Offset: 0x001089A1
		protected override bool Check(AIController controller)
		{
			return this._left.health.dead || this._right.health.dead;
		}

		// Token: 0x04004850 RID: 18512
		[SerializeField]
		private Character _left;

		// Token: 0x04004851 RID: 18513
		[SerializeField]
		private Character _right;
	}
}
