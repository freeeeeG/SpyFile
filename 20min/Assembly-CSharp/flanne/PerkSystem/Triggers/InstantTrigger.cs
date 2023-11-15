using System;

namespace flanne.PerkSystem.Triggers
{
	// Token: 0x02000187 RID: 391
	public class InstantTrigger : Trigger
	{
		// Token: 0x06000984 RID: 2436 RVA: 0x00026A83 File Offset: 0x00024C83
		public override void OnEquip(PlayerController player)
		{
			base.RaiseTrigger(player.gameObject);
		}
	}
}
