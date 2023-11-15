using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001B0 RID: 432
	public class HealPlayerAction : Action
	{
		// Token: 0x06000A02 RID: 2562 RVA: 0x0002781E File Offset: 0x00025A1E
		public override void Activate(GameObject target)
		{
			PlayerController.Instance.playerHealth.Heal(this.healAmount);
		}

		// Token: 0x04000712 RID: 1810
		[SerializeField]
		private int healAmount = 1;
	}
}
