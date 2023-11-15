using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001B9 RID: 441
	public class ModCurseDamageAction : Action
	{
		// Token: 0x06000A1A RID: 2586 RVA: 0x00027A24 File Offset: 0x00025C24
		public override void Activate(GameObject target)
		{
			CurseSystem.Instance.curseDamageMultiplier += this.damageMultiMod;
		}

		// Token: 0x0400071E RID: 1822
		[SerializeField]
		private float damageMultiMod;
	}
}
