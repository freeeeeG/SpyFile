using System;
using flanne.Player;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x0200019F RID: 415
	public class AddToXPMultiplierAction : Action
	{
		// Token: 0x060009DB RID: 2523 RVA: 0x00027386 File Offset: 0x00025586
		public override void Activate(GameObject target)
		{
			PlayerController.Instance.GetComponentInChildren<PlayerXP>().xpMultiplier.AddMultiplierBonus(this.xpMultiplier);
		}

		// Token: 0x040006FE RID: 1790
		[SerializeField]
		private float xpMultiplier;
	}
}
