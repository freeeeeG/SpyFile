using System;
using UnityEngine.Events;

namespace flanne.PerkSystem.Triggers
{
	// Token: 0x02000196 RID: 406
	public class OnSHPLostTrigger : Trigger
	{
		// Token: 0x060009BE RID: 2494 RVA: 0x00027066 File Offset: 0x00025266
		public override void OnEquip(PlayerController player)
		{
			player.playerHealth.onLostSHP.AddListener(new UnityAction(this.OnSHPLost));
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x00027084 File Offset: 0x00025284
		public override void OnUnEquip(PlayerController player)
		{
			player.playerHealth.onLostSHP.RemoveListener(new UnityAction(this.OnSHPLost));
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x00026F65 File Offset: 0x00025165
		private void OnSHPLost()
		{
			base.RaiseTrigger(PlayerController.Instance.gameObject);
		}
	}
}
