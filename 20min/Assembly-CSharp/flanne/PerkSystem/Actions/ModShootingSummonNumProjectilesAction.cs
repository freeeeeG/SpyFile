using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001C3 RID: 451
	public class ModShootingSummonNumProjectilesAction : Action
	{
		// Token: 0x06000A30 RID: 2608 RVA: 0x00027D60 File Offset: 0x00025F60
		public override void Activate(GameObject target)
		{
			foreach (ShootingSummon shootingSummon in PlayerController.Instance.GetComponentsInChildren<ShootingSummon>(true))
			{
				if (shootingSummon.SummonTypeID == this.SummonTypeID)
				{
					shootingSummon.numProjectiles += this.addNumProjectiles;
				}
			}
		}

		// Token: 0x0400072B RID: 1835
		[SerializeField]
		private string SummonTypeID;

		// Token: 0x0400072C RID: 1836
		[SerializeField]
		private int addNumProjectiles;
	}
}
