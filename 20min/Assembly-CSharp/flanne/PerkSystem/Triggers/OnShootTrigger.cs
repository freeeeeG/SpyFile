using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.PerkSystem.Triggers
{
	// Token: 0x02000197 RID: 407
	public class OnShootTrigger : Trigger
	{
		// Token: 0x060009C2 RID: 2498 RVA: 0x000270A2 File Offset: 0x000252A2
		public override void OnEquip(PlayerController player)
		{
			player.gun.OnShoot.AddListener(new UnityAction(this.OnShoot));
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x000270C0 File Offset: 0x000252C0
		public override void OnUnEquip(PlayerController player)
		{
			player.gun.OnShoot.RemoveListener(new UnityAction(this.OnShoot));
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x000270DE File Offset: 0x000252DE
		private void OnShoot()
		{
			this._shotCounter++;
			if (this._shotCounter >= this.shotsToTrigger)
			{
				this._shotCounter = 0;
				base.RaiseTrigger(PlayerController.Instance.gameObject);
			}
		}

		// Token: 0x040006F2 RID: 1778
		[SerializeField]
		private int shotsToTrigger;

		// Token: 0x040006F3 RID: 1779
		[NonSerialized]
		private int _shotCounter;
	}
}
