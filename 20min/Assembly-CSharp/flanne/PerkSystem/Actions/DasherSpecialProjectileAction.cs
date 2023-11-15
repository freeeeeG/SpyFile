using System;
using flanne.CharacterPassives;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001AA RID: 426
	public class DasherSpecialProjectileAction : Action
	{
		// Token: 0x060009F4 RID: 2548 RVA: 0x000276F8 File Offset: 0x000258F8
		public override void Activate(GameObject target)
		{
			PlayerController.Instance.GetComponentInChildren<DasherSpecial>().projectiles++;
		}
	}
}
