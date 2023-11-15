using System;
using UnityEngine.Events;

namespace flanne.PerkSystem.Triggers
{
	// Token: 0x02000191 RID: 401
	public class OnPlayerHealTrigger : Trigger
	{
		// Token: 0x060009AA RID: 2474 RVA: 0x00026F29 File Offset: 0x00025129
		public override void OnEquip(PlayerController player)
		{
			player.playerHealth.onHealed.AddListener(new UnityAction(this.OnHeal));
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x00026F47 File Offset: 0x00025147
		public override void OnUnEquip(PlayerController player)
		{
			player.playerHealth.onHealed.RemoveListener(new UnityAction(this.OnHeal));
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x00026F65 File Offset: 0x00025165
		private void OnHeal()
		{
			base.RaiseTrigger(PlayerController.Instance.gameObject);
		}
	}
}
