using System;
using UnityEngine;

namespace flanne.Pickups
{
	// Token: 0x0200017D RID: 381
	public class DevilDealPickup : MonoBehaviour
	{
		// Token: 0x06000962 RID: 2402 RVA: 0x0002671D File Offset: 0x0002491D
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.tag == "Player" || other.tag == "MapBounds")
			{
				this.PostNotification(DevilDealPickup.DevilDealPickupEvent, null);
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x040006D4 RID: 1748
		public static string DevilDealPickupEvent = "DevilDealPickup.DevilDealPickupEvent";
	}
}
