using System;
using UnityEngine;

namespace flanne.Pickups
{
	// Token: 0x02000181 RID: 385
	public class SHPPickup : Pickup
	{
		// Token: 0x0600096E RID: 2414 RVA: 0x00026864 File Offset: 0x00024A64
		protected override void UsePickup(GameObject pickupper)
		{
			PlayerHealth componentInChildren = pickupper.transform.root.GetComponentInChildren<PlayerHealth>();
			if (componentInChildren != null)
			{
				PlayerHealth playerHealth = componentInChildren;
				int shp = playerHealth.shp;
				playerHealth.shp = shp + 1;
			}
		}

		// Token: 0x040006DA RID: 1754
		public int amount;
	}
}
