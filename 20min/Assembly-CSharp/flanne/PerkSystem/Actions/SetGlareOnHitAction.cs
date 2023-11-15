using System;
using flanne.PowerupSystem;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001CE RID: 462
	public class SetGlareOnHitAction : Action
	{
		// Token: 0x06000A4B RID: 2635 RVA: 0x000280E0 File Offset: 0x000262E0
		public override void Activate(GameObject target)
		{
			AttachVisionDamage[] componentsInChildren = target.GetComponentsInChildren<AttachVisionDamage>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].visionDamage.triggerOnHit = true;
			}
		}
	}
}
