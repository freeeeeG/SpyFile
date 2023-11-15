using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001D9 RID: 473
	public class TripleBaseAmmoAction : Action
	{
		// Token: 0x06000A6B RID: 2667 RVA: 0x000289B0 File Offset: 0x00026BB0
		public override void Activate(GameObject target)
		{
			PlayerController instance = PlayerController.Instance;
			instance.stats[StatType.MaxAmmo].AddFlatBonus(2 * instance.gun.gunData.maxAmmo);
		}
	}
}
