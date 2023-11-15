using System;
using Characters.AI.YggdrasillElderEnt;
using UnityEngine;

namespace Characters.AI.Conditions
{
	// Token: 0x020011CF RID: 4559
	public class SelectedHand : Condition
	{
		// Token: 0x06005997 RID: 22935 RVA: 0x0010A8B5 File Offset: 0x00108AB5
		protected override bool Check(AIController controller)
		{
			return this._controller.left == this._left;
		}

		// Token: 0x04004859 RID: 18521
		[SerializeField]
		private SweepHandController _controller;

		// Token: 0x0400485A RID: 18522
		[SerializeField]
		private bool _left;
	}
}
