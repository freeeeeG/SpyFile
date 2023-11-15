using System;
using UnityEngine.Events;

namespace flanne.PerkSystem.Triggers
{
	// Token: 0x02000195 RID: 405
	public class OnSHPGainedTrigger : Trigger
	{
		// Token: 0x060009BA RID: 2490 RVA: 0x0002702A File Offset: 0x0002522A
		public override void OnEquip(PlayerController player)
		{
			player.playerHealth.onGainedSHP.AddListener(new UnityAction(this.OnSHPGained));
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x00027048 File Offset: 0x00025248
		public override void OnUnEquip(PlayerController player)
		{
			player.playerHealth.onGainedSHP.RemoveListener(new UnityAction(this.OnSHPGained));
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x00026F65 File Offset: 0x00025165
		private void OnSHPGained()
		{
			base.RaiseTrigger(PlayerController.Instance.gameObject);
		}
	}
}
