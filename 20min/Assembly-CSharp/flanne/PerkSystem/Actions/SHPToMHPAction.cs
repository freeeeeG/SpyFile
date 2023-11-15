using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001CD RID: 461
	public class SHPToMHPAction : Action
	{
		// Token: 0x06000A49 RID: 2633 RVA: 0x00028098 File Offset: 0x00026298
		public override void Activate(GameObject target)
		{
			PlayerController instance = PlayerController.Instance;
			PlayerHealth playerHealth = instance.playerHealth;
			StatsHolder stats = instance.stats;
			if (playerHealth.shp > 0)
			{
				PlayerHealth playerHealth2 = playerHealth;
				int shp = playerHealth2.shp;
				playerHealth2.shp = shp - 1;
				stats[StatType.MaxHP].AddFlatBonus(1);
			}
		}
	}
}
