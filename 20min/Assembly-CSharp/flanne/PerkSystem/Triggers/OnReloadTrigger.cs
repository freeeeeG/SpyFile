using System;
using UnityEngine.Events;

namespace flanne.PerkSystem.Triggers
{
	// Token: 0x02000194 RID: 404
	public class OnReloadTrigger : Trigger
	{
		// Token: 0x060009B6 RID: 2486 RVA: 0x00027008 File Offset: 0x00025208
		public override void OnEquip(PlayerController player)
		{
			PlayerController.Instance.ammo.OnReload.AddListener(new UnityAction(this.OnReload));
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x00027008 File Offset: 0x00025208
		public override void OnUnEquip(PlayerController player)
		{
			PlayerController.Instance.ammo.OnReload.AddListener(new UnityAction(this.OnReload));
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x00026F65 File Offset: 0x00025165
		private void OnReload()
		{
			base.RaiseTrigger(PlayerController.Instance.gameObject);
		}
	}
}
