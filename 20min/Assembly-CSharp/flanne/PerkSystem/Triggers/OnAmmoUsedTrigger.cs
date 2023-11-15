using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.PerkSystem.Triggers
{
	// Token: 0x02000189 RID: 393
	public class OnAmmoUsedTrigger : Trigger
	{
		// Token: 0x0600098A RID: 2442 RVA: 0x00026AF0 File Offset: 0x00024CF0
		public override void OnEquip(PlayerController player)
		{
			player.ammo.OnAmmoChanged.AddListener(new UnityAction<int>(this.OnAmmoChanged));
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x00026B0E File Offset: 0x00024D0E
		public override void OnUnEquip(PlayerController player)
		{
			player.ammo.OnAmmoChanged.RemoveListener(new UnityAction<int>(this.OnAmmoChanged));
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x00026B2C File Offset: 0x00024D2C
		private void OnAmmoChanged(int a)
		{
			if (Mathf.Max(1, PlayerController.Instance.ammo.max) - a == this.ammoUsed)
			{
				base.RaiseTrigger(PlayerController.Instance.gameObject);
			}
		}

		// Token: 0x040006E7 RID: 1767
		[SerializeField]
		private int ammoUsed;
	}
}
