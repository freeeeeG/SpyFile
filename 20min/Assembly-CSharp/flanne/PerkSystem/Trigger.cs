using System;
using UnityEngine;

namespace flanne.PerkSystem
{
	// Token: 0x02000185 RID: 389
	public abstract class Trigger
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600097A RID: 2426 RVA: 0x00026994 File Offset: 0x00024B94
		// (remove) Token: 0x0600097B RID: 2427 RVA: 0x000269CC File Offset: 0x00024BCC
		public event EventHandler<GameObject> Triggered;

		// Token: 0x0600097C RID: 2428
		public abstract void OnEquip(PlayerController player);

		// Token: 0x0600097D RID: 2429 RVA: 0x00002F51 File Offset: 0x00001151
		public virtual void OnUnEquip(PlayerController player)
		{
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x00026A01 File Offset: 0x00024C01
		protected void RaiseTrigger(GameObject target)
		{
			EventHandler<GameObject> triggered = this.Triggered;
			if (triggered == null)
			{
				return;
			}
			triggered(this, target);
		}
	}
}
