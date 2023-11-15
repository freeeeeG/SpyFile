using System;
using UnityEngine;

namespace flanne.PerkSystem.Triggers
{
	// Token: 0x0200018C RID: 396
	public class OnFreezeTrigger : Trigger
	{
		// Token: 0x06000996 RID: 2454 RVA: 0x00026C4D File Offset: 0x00024E4D
		public override void OnEquip(PlayerController player)
		{
			this.AddObserver(new Action<object, object>(this.OnFreeze), FreezeSystem.InflictFreezeEvent);
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x00026C66 File Offset: 0x00024E66
		public override void OnUnEquip(PlayerController player)
		{
			this.RemoveObserver(new Action<object, object>(this.OnFreeze), FreezeSystem.InflictFreezeEvent);
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x00026C80 File Offset: 0x00024E80
		private void OnFreeze(object sender, object args)
		{
			GameObject target = args as GameObject;
			base.RaiseTrigger(target);
		}
	}
}
