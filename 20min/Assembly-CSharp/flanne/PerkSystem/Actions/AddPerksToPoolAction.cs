using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x0200019D RID: 413
	public class AddPerksToPoolAction : Action
	{
		// Token: 0x060009D7 RID: 2519 RVA: 0x00027321 File Offset: 0x00025521
		public override void Activate(GameObject target)
		{
			PowerupGenerator.Instance.AddToPool(this.powerupsToAdd, 1);
		}

		// Token: 0x040006FC RID: 1788
		[SerializeField]
		private List<Powerup> powerupsToAdd;
	}
}
