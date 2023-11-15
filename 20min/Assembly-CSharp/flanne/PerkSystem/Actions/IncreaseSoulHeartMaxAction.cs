using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001B2 RID: 434
	public class IncreaseSoulHeartMaxAction : Action
	{
		// Token: 0x06000A06 RID: 2566 RVA: 0x00027860 File Offset: 0x00025A60
		public override void Activate(GameObject target)
		{
			PlayerController.Instance.playerHealth.maxSHP += this.shpIncrease;
		}

		// Token: 0x04000714 RID: 1812
		[SerializeField]
		private int shpIncrease;
	}
}
