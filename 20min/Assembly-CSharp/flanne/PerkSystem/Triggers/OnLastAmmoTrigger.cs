using System;
using UnityEngine.Events;

namespace flanne.PerkSystem.Triggers
{
	// Token: 0x02000190 RID: 400
	public class OnLastAmmoTrigger : Trigger
	{
		// Token: 0x060009A6 RID: 2470 RVA: 0x00026ED8 File Offset: 0x000250D8
		public override void OnEquip(PlayerController player)
		{
			player.ammo.OnAmmoChanged.AddListener(new UnityAction<int>(this.OnAmmoChanged));
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x00026EF6 File Offset: 0x000250F6
		public override void OnUnEquip(PlayerController player)
		{
			player.ammo.OnAmmoChanged.RemoveListener(new UnityAction<int>(this.OnAmmoChanged));
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x00026F14 File Offset: 0x00025114
		private void OnAmmoChanged(int ammoAmount)
		{
			if (ammoAmount == 0)
			{
				base.RaiseTrigger(PlayerController.Instance.gameObject);
			}
		}
	}
}
