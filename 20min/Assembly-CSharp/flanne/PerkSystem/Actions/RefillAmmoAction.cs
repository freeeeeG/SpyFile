using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001CB RID: 459
	public class RefillAmmoAction : Action
	{
		// Token: 0x06000A43 RID: 2627 RVA: 0x0002804A File Offset: 0x0002624A
		public override void Activate(GameObject target)
		{
			PlayerController.Instance.ammo.GainAmmo(this.refillAmount);
		}

		// Token: 0x04000743 RID: 1859
		[SerializeField]
		private int refillAmount;
	}
}
