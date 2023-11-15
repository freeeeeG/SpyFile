using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001B1 RID: 433
	public class HolyShieldCDRAction : Action
	{
		// Token: 0x06000A04 RID: 2564 RVA: 0x00027844 File Offset: 0x00025A44
		public override void Activate(GameObject target)
		{
			PlayerController.Instance.GetComponentInChildren<PreventDamage>().cooldownRate.AddMultiplierBonus(this.additionalCDR);
		}

		// Token: 0x04000713 RID: 1811
		[SerializeField]
		private float additionalCDR;
	}
}
