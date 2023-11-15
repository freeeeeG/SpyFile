using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001BB RID: 443
	public class ModFreezeDurationAction : Action
	{
		// Token: 0x06000A1E RID: 2590 RVA: 0x00027A57 File Offset: 0x00025C57
		public override void Activate(GameObject target)
		{
			FreezeSystem.SharedInstance.durationMod.AddMultiplierBonus(this.freezeDurationMutli);
		}

		// Token: 0x04000720 RID: 1824
		[SerializeField]
		private float freezeDurationMutli;
	}
}
