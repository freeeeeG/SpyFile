using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.PerkSystem.Triggers
{
	// Token: 0x02000188 RID: 392
	public class OnAmmoRemainTrigger : Trigger
	{
		// Token: 0x06000986 RID: 2438 RVA: 0x00026A99 File Offset: 0x00024C99
		public override void OnEquip(PlayerController player)
		{
			player.ammo.OnAmmoChanged.AddListener(new UnityAction<int>(this.OnAmmoChanged));
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x00026AB7 File Offset: 0x00024CB7
		public override void OnUnEquip(PlayerController player)
		{
			player.ammo.OnAmmoChanged.RemoveListener(new UnityAction<int>(this.OnAmmoChanged));
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x00026AD5 File Offset: 0x00024CD5
		private void OnAmmoChanged(int a)
		{
			if (a == this.ammoAmount)
			{
				base.RaiseTrigger(PlayerController.Instance.gameObject);
			}
		}

		// Token: 0x040006E6 RID: 1766
		[SerializeField]
		private int ammoAmount;
	}
}
