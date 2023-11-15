using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001B8 RID: 440
	public class ModBurnDurationAction : Action
	{
		// Token: 0x06000A18 RID: 2584 RVA: 0x00027A0D File Offset: 0x00025C0D
		public override void Activate(GameObject target)
		{
			BurnSystem.SharedInstance.burnDurationMultiplier.AddMultiplierBonus(this.burnDurationMulti);
		}

		// Token: 0x0400071D RID: 1821
		[SerializeField]
		private float burnDurationMulti;
	}
}
