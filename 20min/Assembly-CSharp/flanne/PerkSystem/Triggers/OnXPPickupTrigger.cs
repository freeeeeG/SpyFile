using System;
using flanne.Pickups;
using UnityEngine;

namespace flanne.PerkSystem.Triggers
{
	// Token: 0x02000199 RID: 409
	public class OnXPPickupTrigger : Trigger
	{
		// Token: 0x060009CA RID: 2506 RVA: 0x0002719F File Offset: 0x0002539F
		public override void OnEquip(PlayerController player)
		{
			this.AddObserver(new Action<object, object>(this.OnXPPickup), XPPickup.XPPickupEvent);
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x000271B8 File Offset: 0x000253B8
		public override void OnUnEquip(PlayerController player)
		{
			this.RemoveObserver(new Action<object, object>(this.OnXPPickup), XPPickup.XPPickupEvent);
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x000271D1 File Offset: 0x000253D1
		private void OnXPPickup(object sender, object args)
		{
			if (Random.Range(0f, 1f) < this.chanceToActivate)
			{
				base.RaiseTrigger(PlayerController.Instance.gameObject);
			}
		}

		// Token: 0x040006F7 RID: 1783
		[Range(0f, 1f)]
		[SerializeField]
		private float chanceToActivate;
	}
}
