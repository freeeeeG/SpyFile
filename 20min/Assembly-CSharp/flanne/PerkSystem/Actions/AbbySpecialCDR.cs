using System;
using flanne.CharacterPassives;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x0200019C RID: 412
	public class AbbySpecialCDR : Action
	{
		// Token: 0x060009D5 RID: 2517 RVA: 0x000272D8 File Offset: 0x000254D8
		public override void Activate(GameObject target)
		{
			DumpAmmoPassive componentInChildren = target.GetComponentInChildren<DumpAmmoPassive>();
			if (componentInChildren != null)
			{
				componentInChildren.shotCDMultiplier /= 1f + this.shotCDR;
				return;
			}
			Debug.LogWarning("Abby's special not found.");
		}

		// Token: 0x040006FB RID: 1787
		[SerializeField]
		private float shotCDR;
	}
}
