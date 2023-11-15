using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001C2 RID: 450
	public class ModShootingSummonDamageAction : Action
	{
		// Token: 0x06000A2E RID: 2606 RVA: 0x00027D0C File Offset: 0x00025F0C
		public override void Activate(GameObject target)
		{
			foreach (ShootingSummon shootingSummon in PlayerController.Instance.GetComponentsInChildren<ShootingSummon>(true))
			{
				if (shootingSummon.SummonTypeID == this.SummonTypeID)
				{
					shootingSummon.baseDamage += this.baseDamageMod;
				}
			}
		}

		// Token: 0x04000729 RID: 1833
		[SerializeField]
		private string SummonTypeID;

		// Token: 0x0400072A RID: 1834
		[SerializeField]
		private int baseDamageMod;
	}
}
