using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001AE RID: 430
	public class DoubleBaseNumProjectilesAction : Action
	{
		// Token: 0x060009FE RID: 2558 RVA: 0x000277D4 File Offset: 0x000259D4
		public override void Activate(GameObject target)
		{
			PlayerController instance = PlayerController.Instance;
			instance.stats[StatType.Projectiles].AddFlatBonus(instance.gun.gunData.numOfProjectiles);
		}
	}
}
