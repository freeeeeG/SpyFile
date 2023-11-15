using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001A0 RID: 416
	public class ApplyBurnAction : Action
	{
		// Token: 0x060009DD RID: 2525 RVA: 0x000273A2 File Offset: 0x000255A2
		public override void Activate(GameObject target)
		{
			BurnSystem.SharedInstance.Burn(target, this.burnDamage);
		}

		// Token: 0x040006FF RID: 1791
		[SerializeField]
		private int burnDamage;
	}
}
