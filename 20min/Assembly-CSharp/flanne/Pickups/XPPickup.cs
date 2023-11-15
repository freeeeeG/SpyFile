using System;
using flanne.Player;
using UnityEngine;

namespace flanne.Pickups
{
	// Token: 0x02000182 RID: 386
	public class XPPickup : Pickup
	{
		// Token: 0x06000970 RID: 2416 RVA: 0x0002689C File Offset: 0x00024A9C
		protected override void UsePickup(GameObject pickupper)
		{
			PlayerXP componentInChildren = pickupper.transform.root.GetComponentInChildren<PlayerXP>();
			if (componentInChildren != null)
			{
				componentInChildren.GainXP(this.amount);
			}
			this.PostNotification(XPPickup.XPPickupEvent, null);
		}

		// Token: 0x040006DB RID: 1755
		public static string XPPickupEvent = "XPPickup.XPPickupEvent";

		// Token: 0x040006DC RID: 1756
		public float amount;
	}
}
