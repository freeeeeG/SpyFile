using System;
using UnityEngine;

namespace flanne.Pickups
{
	// Token: 0x0200017F RID: 383
	public class HaloPickup : MonoBehaviour
	{
		// Token: 0x06000967 RID: 2407 RVA: 0x000267A3 File Offset: 0x000249A3
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.tag == "Player")
			{
				this.PostNotification(HaloPickup.HaloPickupEvent, null);
				SoundEffectSO soundEffectSO = this.soundFX;
				if (soundEffectSO != null)
				{
					soundEffectSO.Play(null);
				}
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x040006D6 RID: 1750
		public static string HaloPickupEvent = "HaloPickup.HaloPickupEvent";

		// Token: 0x040006D7 RID: 1751
		[SerializeField]
		private SoundEffectSO soundFX;
	}
}
