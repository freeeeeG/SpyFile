using System;
using UnityEngine.Events;

namespace flanne.PerkSystem.Triggers
{
	// Token: 0x02000192 RID: 402
	public class OnPlayerHurtTrigger : Trigger
	{
		// Token: 0x060009AE RID: 2478 RVA: 0x00026F77 File Offset: 0x00025177
		public override void OnEquip(PlayerController player)
		{
			player.playerHealth.onHurt.AddListener(new UnityAction(this.OnPlayerHurt));
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x00026F95 File Offset: 0x00025195
		public override void OnUnEquip(PlayerController player)
		{
			player.playerHealth.onHurt.RemoveListener(new UnityAction(this.OnPlayerHurt));
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x00026F65 File Offset: 0x00025165
		private void OnPlayerHurt()
		{
			base.RaiseTrigger(PlayerController.Instance.gameObject);
		}
	}
}
