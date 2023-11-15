using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001A1 RID: 417
	public class ApplyBurnEqualToBulletDamageAction : Action
	{
		// Token: 0x060009DF RID: 2527 RVA: 0x000273B8 File Offset: 0x000255B8
		public override void Activate(GameObject target)
		{
			int burnDamage = Mathf.FloorToInt((float)CurseSystem.Instance.curseDamage * this.damageMultiplier);
			BurnSystem.SharedInstance.Burn(target, burnDamage);
		}

		// Token: 0x04000700 RID: 1792
		[SerializeField]
		private float damageMultiplier = 1f;
	}
}
