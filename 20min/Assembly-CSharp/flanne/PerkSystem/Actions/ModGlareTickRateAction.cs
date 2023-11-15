using System;
using flanne.PowerupSystem;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001BC RID: 444
	public class ModGlareTickRateAction : Action
	{
		// Token: 0x06000A20 RID: 2592 RVA: 0x00027A70 File Offset: 0x00025C70
		public override void Activate(GameObject target)
		{
			AttachVisionDamage[] componentsInChildren = target.GetComponentsInChildren<AttachVisionDamage>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].visionDamage.tickRate.AddMultiplierBonus(this.visionTickSpeedMultiplierBonus);
			}
		}

		// Token: 0x04000721 RID: 1825
		public float visionTickSpeedMultiplierBonus;
	}
}
