using System;
using UnityEngine;

namespace flanne.Pickups
{
	// Token: 0x0200017E RID: 382
	public class HPPickup : Pickup
	{
		// Token: 0x06000965 RID: 2405 RVA: 0x00026768 File Offset: 0x00024968
		protected override void UsePickup(GameObject pickupper)
		{
			PlayerHealth componentInChildren = pickupper.transform.root.GetComponentInChildren<PlayerHealth>();
			if (componentInChildren != null)
			{
				componentInChildren.Heal(this.amount);
			}
		}

		// Token: 0x040006D5 RID: 1749
		public int amount;
	}
}
