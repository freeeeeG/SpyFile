using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001A2 RID: 418
	public class ApplyBurnEqualToCurseDamageAction : Action
	{
		// Token: 0x060009E1 RID: 2529 RVA: 0x000273FC File Offset: 0x000255FC
		public override void Activate(GameObject target)
		{
			int burnDamage = Mathf.FloorToInt(PlayerController.Instance.gun.damage * this.damageMultiplier);
			BurnSystem.SharedInstance.Burn(target, burnDamage);
		}

		// Token: 0x04000701 RID: 1793
		[SerializeField]
		private float damageMultiplier = 1f;
	}
}
